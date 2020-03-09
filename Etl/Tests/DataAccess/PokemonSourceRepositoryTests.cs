using System.Linq;
using NUnit.Framework;
using Moq;
using Shakespokemon.Etl.DataAccess;
using AutoFixture;
using Microsoft.Extensions.Logging;

namespace Shakespokemon.Etl.Tests.DataAccess
{
    public class PokemonSourceRepositoryTests
    {
        [Test]
        public void GivenPokemonNamesAndDescriptionAndShakespeareTranslationClients_WhenGetAll_ThenReturnsPokemons()
        {
            var expectedNames = new Fixture().Create<string[]>();
            var namesClientMock = new Mock<IPokemonNamesClient>();
            namesClientMock.Setup(x => x.GetAll()).Returns(expectedNames);
            var descriptionClientMock = new Mock<IPokemonDescriptionClient>();
            foreach(var name in expectedNames)
            {
                var description = $"{name} description";
                descriptionClientMock.Setup(x => x.TryGet(name, out description)).Returns(true);
            }
                
            var translationClientMock = new Mock<IShakespeareTranslationClient>();
            translationClientMock.Setup(x => x.GetTranslation(It.IsAny<string>()))
                .Returns<string>(text => $"{text} translation");

            var sut = new PokemonSourceRepository(namesClientMock.Object, descriptionClientMock.Object, 
                translationClientMock.Object, new Mock<ILogger<PokemonSourceRepository>>().Object);

            var actualItems = sut.GetAll().ToArray();

            Assert.AreEqual(expectedNames.Length, actualItems.Count());
            for(int i = 0; i < actualItems.Count(); i++)
            {
                Assert.AreEqual(expectedNames[i], actualItems[i].Name);
                Assert.AreEqual($"{expectedNames[i]} description translation", actualItems[i].Description);
            }
        }
    }
}