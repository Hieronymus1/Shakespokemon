

using Microsoft.Extensions.DependencyInjection;
using Shakespokemon.Etl.Core;
using Shakespokemon.Etl.DataAccess;
using Shakespokemon.Etl.DataAccess.Http;

namespace Shakespokemon.Etl.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IPokemonNamesClient, PokeApiClient>()
                .AddSingleton<IPokemonDescriptionClient, PokedexClient>()
                .AddSingleton<IShakespeareTranslationClient, FunTranslationClient>()
                .AddSingleton<IPokemonLoadService, PokemonLoadService>()
                .AddLogging()
                .BuildServiceProvider();

            var service = serviceProvider.GetRequiredService<IPokemonLoadService>();
            service.Process();
        }
    }
}
