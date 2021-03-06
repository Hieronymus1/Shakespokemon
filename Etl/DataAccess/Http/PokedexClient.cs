using System;
using System.Net;
using System.Net.Http;
using System.Web;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Shakespokemon.Core;

namespace Shakespokemon.Etl.DataAccess.Http
{
    internal class PokedexClient : IPokemonDescriptionClient
    {
        public bool TryGet(string name, out string description)
        {
            Argument.IsNotNullOrWhitespace(name, nameof(name));

            description = null;

            using(var client = new HttpClient())
            {
                var url = new Uri($"https://www.pokemon.com/us/pokedex/{name}");;
                var response = client.GetAsync(url).Result; 
                if(response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }  
  
                response.EnsureSuccessStatusCode();
                using (var content = response.Content)  
                {  
                    var html = response.Content.ReadAsStringAsync().Result;  
                    if(TryParse(html, out description))
                    {
                        return true;
                    }
                }  
            }

            return false;
        }

        public static bool TryParse(string html, out string description)
        {
            description = null;

            var page = new HtmlDocument();
            page.LoadHtml(html);

            var node = page.DocumentNode.SelectSingleNode("//div[contains(@class, 'version-descriptions')]/p[1]");
            if(node != null)
            {               
                description = Sanitize(node.InnerText);
            }

            return description != null;
        }

        private static string Sanitize(string rawText)
        {
            var text = HttpUtility.HtmlDecode(rawText);
            text = Regex.Replace(text, @"\s+", " ").Trim(); // Removes extra blank spaces.

            return text;
        }
    }
}