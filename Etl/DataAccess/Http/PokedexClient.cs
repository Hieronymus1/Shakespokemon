using System;
using System.Net.Http;
using System.Web;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Shakespokemon.Etl.DataAccess.Http
{
    internal class PokedexClient : IPokemonDescriptionClient
    {
        public string GetDescription(string name)
        {
            using(var client = new HttpClient())
            {
                var url = new Uri($"https://www.pokemon.com/us/pokedex/{name}");;
                var response = client.GetAsync(url).Result;  
                response.EnsureSuccessStatusCode();  
  
                using (var content = response.Content)  
                {  
                    var html = response.Content.ReadAsStringAsync().Result;  

                    return ParseDescription(html);
                }  
            }
        }

        public static string ParseDescription(string html)
        {
            var page = new HtmlDocument();
            page.LoadHtml(html);

            var node = page.DocumentNode.SelectSingleNode("//div[contains(@class, 'version-descriptions')]/p[1]");
            if(node != null)
            {               
                return Sanitize(node.InnerText);
            }

            throw new InvalidOperationException("Could not find element with class 'version-descriptions' in document.");
        }

        private static string Sanitize(string rawText)
        {
            var text = rawText.Replace('\n', ' ');
            text = HttpUtility.HtmlDecode(rawText);
            text = Regex.Replace(text, @"\s+", " "); // Removes extra blank spaces.

            return text.Trim();
        }
    }
}