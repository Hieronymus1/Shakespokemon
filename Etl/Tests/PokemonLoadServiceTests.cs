using Shakespokemon.Etl.Core;
using Shakespokemon.Core;
using NUnit.Framework;
using AutoFixture;
using Moq;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Shakespokemon.Etl.Tests
{
    public class PokemonLoadServiceTests
    {
        [Test]
        public void GivenSourcePokemons_WhenProcess_ThenAddsMissingPokemonsToDestinationPokemons()
        {
            var fixture = new Fixture();
            var pokemons = fixture.Create<Pokemon[]>();
            var existing = pokemons.First();
            var expected = pokemons.Where(i => i.Name != existing.Name);
            var sourceMock = new Mock<IPokemonSourceRepository>();
            sourceMock.Setup(x => x.GetAll()).Returns(pokemons);
            var destinationMock = new Mock<IPokemonDestinationRepository>();
            destinationMock.Setup(x => x.TryFind(existing.Name, out existing)).Returns(true);
            
            new PokemonLoadService(sourceMock.Object, destinationMock.Object, new Mock<ILogger<PokemonLoadService>>().Object).Process();

            destinationMock.Verify(x => x.Add(It.Is<Pokemon>(i => expected.Contains(i))), Times.Exactly(expected.Count()));
        }
    }
}