using Shakespokemon.Etl.Core;
using Shakespokemon.Core;
using NUnit.Framework;
using AutoFixture;
using Moq;
using System.Linq;

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
            var sourcePokemonsMock = new Mock<IPokemonSourceRepository>();
            sourcePokemonsMock.Setup(x => x.GetAll()).Returns(pokemons);
            var destinationPokemonsMock = new Mock<IPokemonDestinationRepository>();
            destinationPokemonsMock.Setup(x => x.TryFind(existing.Name, out existing)).Returns(true);
            
            new PokemonLoadService(sourcePokemonsMock.Object, destinationPokemonsMock.Object).Process();

            destinationPokemonsMock.Verify(x => x.Add(It.Is<Pokemon>(i => expected.Contains(i))), Times.Exactly(expected.Count()));
        }
    }
}