using NUnit.Framework;
using System.IO;
using Shakespokemon.Etl.DataAccess.Http;

namespace Shakespokemon.Etl.Tests.DataAccess.Http
{
    public class PokeApiClientTests
    {
        [TestCase("PokeApiPokemonsPage.json", 20, "https://pokeapi.co/api/v2/pokemon/?offset=20&limit=20")]
        [TestCase("PokeApiPokemonsLastPage.json", 4, null)]
        public void GivenJson_WhenParsePokemonsPage_ThenReturnsExpected(string file, int expectedCount, string expectedNextPage)
        {
            var json = File.ReadAllText(file);

            var actual = PokeApiClient.ParsePokemonsPage(json);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedCount, actual.Names.Length);
            Assert.AreEqual(expectedNextPage, actual.NextPage?.ToString());
        }       
    }
}