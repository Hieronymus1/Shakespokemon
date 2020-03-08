using NUnit.Framework;
using System.IO;
using Shakespokemon.Etl.DataAccess.Http;

namespace Shakespokemon.Etl.Tests.DataAccess.Http
{
    public class FunTranslationClientTests
    {
        [Test]
        public void GivenJson_WhenParseTranslation_ThenReturnsExpected()
        {
            var json = File.ReadAllText("FunTranslationShakespeare.json");
            var expected = "Thee did giveth mr. Tim a hearty meal, but unfortunately what he did doth englut did maketh him kicketh the bucket.";

            var actual = FunTranslationClient.ParseTranslation(json);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test, Ignore("Avoid API calls limit by IP")]
        public void GivenText_WhenGetDescription_ThenReturnsTranslation()
        {
            var sut = new FunTranslationClient();
            var expected = "Charizard flies 'round the sky in search of powerful opponents. 't breathes fire of such most wondrous heat yond 't melts aught. However,  't nev'r turns its fiery breath on any opponentweaker than itself.";

            var text = "Charizard flies around the sky in search of powerful opponents. It breathes fire of such great heat that it melts anything. However, it never turns its fiery breath on any opponentweaker than itself.";
            var actual = sut.GetTranslation(text);

            Assert.AreEqual(actual, expected);
        }
    }
}