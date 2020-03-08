using System;
using System.Collections.Generic;
using Shakespokemon.Core;
using Newtonsoft.Json.Linq;

namespace Shakespokemon.Etl.DataAccess.Http
{
    internal class PokeApiClient : IPokemonNamesClient
    {
        public IEnumerable<string> GetAll()
        {
            throw new NotImplementedException();
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