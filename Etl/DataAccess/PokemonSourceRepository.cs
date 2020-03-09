using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Shakespokemon.Etl.Core;
using Shakespokemon.Core;

namespace Shakespokemon.Etl.DataAccess
{
    public class PokemonSourceRepository : IPokemonSourceRepository
    {
        private readonly IPokemonNamesClient _namesClient;
        private readonly IPokemonDescriptionClient _descriptionClient;
        private readonly IShakespeareTranslationClient _translationClient;
        private readonly ILogger<PokemonSourceRepository> _logger;

        public PokemonSourceRepository(IPokemonNamesClient namesClient, IPokemonDescriptionClient descriptionClient, 
            IShakespeareTranslationClient translationClient, ILogger<PokemonSourceRepository> logger)
        {
            _namesClient = namesClient;
            _descriptionClient = descriptionClient;
            _translationClient = translationClient;
            _logger = logger;
        }

        public IEnumerable<Pokemon> GetAll()
        {
            foreach(var name in _namesClient.GetAll())
            {
                if(_descriptionClient.TryGet(name, out var description))
                {
                    var translation = _translationClient.GetTranslation(description);

                    yield return new Pokemon(name, translation);
                }
                else
                {
                    _logger.LogWarning($"Could not find description for Pokémon '{name}'.");
                }
            }
        }
    }
}
