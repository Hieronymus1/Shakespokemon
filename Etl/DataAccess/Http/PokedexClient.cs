using System;
using System.Linq;
using HtmlAgilityPack;

namespace Shakespokemon.Etl.DataAccess.Http
{
    internal class PokedexClient : IPokemonDescriptionClient
    {
        public string GetDescription(string name)
        {
            // Get pokemon description by name: https://www.pokemon.com/us/pokedex/{name}
                
            throw new NotImplementedException();
        }

        public static string ParseDescription(string html)
        {
            var page = new HtmlDocument();
            page.LoadHtml(html);

            var node = page.DocumentNode.SelectSingleNode("//p[contains(@class, 'pokemon-story__body')]/span[1]");
            if(node != null)
            {
                return node.InnerText.Trim();
            }

            throw new InvalidOperationException("Could not find element with class 'pokemon-story__body' in document.");
        }
    }
}