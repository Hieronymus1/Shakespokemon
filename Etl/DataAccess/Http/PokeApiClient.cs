using System;
using System.Collections.Generic;
using Shakespokemon.Core;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Shakespokemon.Etl.DataAccess.Http
{
    internal class PokeApiClient : IPokemonNamesClient
    {
        private readonly static Uri PokemonUri = new Uri("https://pokeapi.co/api/v2/pokemon/");

        public string[] GetAll()
        {
            return GetNames(PokemonUri);
        }

        private string[] GetNames(Uri url)
        {
            var items = new List<string>();

            while(url != null)
            {
                string json;
                using(var client = new HttpClient())
                {
                    var response = client.GetAsync(url).Result;  
                    response.EnsureSuccessStatusCode();
                    using (var content = response.Content)  
                    {  
                        json = response.Content.ReadAsStringAsync().Result;  
                    } 
                }

                var page = ParsePokemonsPage(json);
                items.AddRange(page.Names);
            
                url = page.NextPage;
            }
            
            return items.ToArray();
        }

        public class PokemonsPage
        {
            public PokemonsPage(string[] names, Uri nextPage)
            {
                Argument.IsNotNull(names, nameof(names));

                Names = names;
                NextPage = nextPage;
            }

            public string[] Names { get; }

            public Uri NextPage { get; }
        }

        internal static PokemonsPage ParsePokemonsPage(string json)
        {
            var document = JToken.Parse(json);

            var nameTokens = document.SelectTokens("$..name");
            var names = new List<string>();
            foreach(var token in nameTokens.Values())
            {
                names.Add(token.ToString());
            }

            var nextPageToken = document.SelectToken("$.next");
            var nextPage = nextPageToken.ToString();
            var nextPageUri = !string.IsNullOrEmpty(nextPage) ? new Uri(nextPage) : null;

            return new PokemonsPage(names.ToArray(), nextPageUri);
        }
    }
}