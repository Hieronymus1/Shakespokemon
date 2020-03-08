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
    }
}