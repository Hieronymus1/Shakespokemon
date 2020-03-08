namespace Shakespokemon.Etl.Core
{
    public interface IPokemonLoadService
    {
        void Process();
    }

    public class PokemonLoadService : IPokemonLoadService
    {
        private IPokemonSourceRepository _source;
        private IPokemonDestinationRepository _destination;

        public PokemonLoadService(IPokemonSourceRepository source, IPokemonDestinationRepository destination)
        {
            _source = source;
            _destination = destination;
        }

        public void Process()
        {
            var pokemons = _source.GetAll();
            foreach(var pokemon in pokemons)
            {
                if(!_destination.TryFind(pokemon.Name, out var _))
                {
                    _destination.Add(pokemon);
                }
            }
        }
    }
}