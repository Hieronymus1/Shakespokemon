using System;
using Newtonsoft.Json.Linq;

namespace Shakespokemon.Etl.DataAccess.Http
{
    internal class FunTranslationClient : IShakespeareTranslationClient
    {
        public string GetTranslation(string text)
        {
            // Post text for translation: https://api.funtranslations.com/translate/shakespeare.json

            throw new NotImplementedException();
        }

        public static string ParseTranslation(string json)
        {
            var document = JToken.Parse(json);
            var translatedToken = document.SelectToken("$..translated");

            return translatedToken.ToString();
        }
    }
}