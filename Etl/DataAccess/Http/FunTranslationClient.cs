using System;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Collections.Generic;

namespace Shakespokemon.Etl.DataAccess.Http
{
    internal class FunTranslationClient : IShakespeareTranslationClient
    {
        private readonly Uri Uri = new Uri("https://api.funtranslations.com/translate/shakespeare.json");

        public string GetTranslation(string text)
        {
            using(var client = new HttpClient())
            {
                var textContent = new KeyValuePair<string, string>("text", text);
                using(var content = new FormUrlEncodedContent(new[] { textContent }))
                {
                    var response = client.PostAsync(Uri, content).Result;  
                    response.EnsureSuccessStatusCode();  
                    var json = response.Content.ReadAsStringAsync().Result;  

                    return ParseTranslation(json);
                }
            }
        }

        public static string ParseTranslation(string json)
        {
            var document = JToken.Parse(json);
            var translatedToken = document.SelectToken("$..translated");

            return translatedToken.ToString();
        }
    }
}