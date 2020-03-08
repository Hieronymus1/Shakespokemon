using NUnit.Framework;
using System.IO;
using Shakespokemon.Etl.DataAccess.Http;

namespace Shakespokemon.Etl.Tests.DataAccess.Http
{
    public class PokedexClientTests
    {
        [Test]
        public void GivenHtml_WhenParseDescription_ThenReturnsDescription()
        {
            var html = File.ReadAllText("PokedexPokemon.html");
            var expected = "Charizard flies around the sky in search of powerful opponents. It breathes fire of such great heat that it melts anything. However, it never turns its fiery breath on any opponent weaker than itself.";

            var actual = PokedexClient.ParseDescription(html);

            Assert.AreEqual(expected, actual);
        }

        [Test]        
        public void GivenPokemonName_WhenGetDescription_ThenReturnsExpected()
        {
            var sut = new PokedexClient();
            var expected = "Squirtle's shell is not merely used for protection. The shell's rounded shape and the grooves on its surface help minimize resistance in water, enabling this Pokémon to swim at high speeds.";

            var actual = sut.GetDescription("squirtle");

            Assert.AreEqual(expected, actual);
        }
    }
}