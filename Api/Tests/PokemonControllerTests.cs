using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using Shakespokemon.Core;
using Shakespokemon.Api.Core;
using Shakespokemon.Api.Host;
using Newtonsoft.Json;

namespace Shakespokemon.Tests
{
    public class PokemonControllerTests
    {
        private Mock<IPokemonRepository> _repositoryMock;
        private HttpClient _client;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _repositoryMock = new Mock<IPokemonRepository>();

            var factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services => 
                    {
                        services.AddSingleton(_repositoryMock.Object);
                    });
                });

            _client = factory.CreateClient();
        }

        [Test]
        public void GivenExistingPokemon_WhenGetByName_ThenReturnsPokemon()
        {
            var expected = new Pokemon("Foo", "Bar");
            _repositoryMock.Setup(x => x.TryFind(expected.Name, out expected)).Returns(true);

            var response = _client.GetAsync($"pokemon/{expected.Name}").Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var json = response.Content.ReadAsStringAsync().Result;
            var actual = JsonConvert.DeserializeObject<PokemonDto>(json);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
        }

        [Test]
        public void GivenMissingPokemon_WhenGetByName_ThenReturnsNotFound()
        {
            Pokemon @null;
            _repositoryMock.Setup(x => x.TryFind(It.IsAny<string>(), out @null)).Returns(false);

            var response = _client.GetAsync($"pokemon/foo").Result;
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}