using Microsoft.Extensions.Logging;

namespace Shakespokemon.Etl.Core
{
    public interface IPokemonLoadService
    {
        /// <summary>
        /// Extract Pokemons and transform their description in a Shakespearean style before to load the results in local database.
        /// </summary>
        void Process();
    }

    public class PokemonLoadService : IPokemonLoadService
    {
        private IPokemonSourceRepository _source;
        private IPokemonDestinationRepository _destination;
        private ILogger<PokemonLoadService> _logger;

        public PokemonLoadService(IPokemonSourceRepository source, IPokemonDestinationRepository destination,
            ILogger<PokemonLoadService> logger)
        {
            _source = source;
            _destination = destination;
            _logger = logger;
        }
        
        public void Process()
        {
            int addedCount = 0;

            var pokemons = _source.GetAll();
            foreach(var pokemon in pokemons)
            {
                if(!_destination.TryFind(pokemon.Name, out var _))
                {                    
                    _destination.Add(pokemon);
                    addedCount++;
                }
            }

            _logger.LogInformation($"Added {addedCount} new Pokémons.");
        }
    }
}