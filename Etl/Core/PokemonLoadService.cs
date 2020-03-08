namespace Shakespokemon.Etl.Core
{
    public interface IPokemonLoadService
    {
        void Process();
    }

    public class PokemonLoadService : IPokemonLoadService
    {
        private IPokemonSourceRepository _sourceRepository;
        private IPokemonDestinationRepository _destinationRepository;

        public PokemonLoadService(IPokemonSourceRepository sourceRepository, IPokemonDestinationRepository destinationRepository)
        {
            _sourceRepository = sourceRepository;
            _destinationRepository = destinationRepository;
        }

        public void Process()
        {
            var pokemons = _sourceRepository.GetAll();
            foreach(var pokemon in pokemons)
            {
                if(!_destinationRepository.TryFind(pokemon.Name, out var _))
                {
                    _destinationRepository.Add(pokemon);
                }
            }
        }
    }
}