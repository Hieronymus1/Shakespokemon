using NUnit.Framework;
using Shakespokemon.Core;
using Shakespokemon.DataAccess;

namespace Shakespokemon.Tests
{
    public class PokemonRepositoryTests
    {
        [Test]
        public void GivenPokemon_WhenAddAndTryFind_ThenFindsPokemon()
        {
            var sut = new PokemonRepository();
            var expected = new Pokemon("Foo", "Bar");
            sut.Remove(expected.Name); // Required or else second run fails because of existing db file.

            Assert.IsFalse(sut.TryFind(expected.Name, out var actual));

            sut.Add(expected);
            Assert.IsTrue(sut.TryFind(expected.Name, out actual));
            Assert.AreEqual(expected, actual);
        }
    }
}