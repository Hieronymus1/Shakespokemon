using Shakespokemon.Core;

namespace Shakespokemon.Etl.Core
{
    public interface IPokemonDestinationRepository : IPokemonRepository
    {
        void Add(Pokemon item);
    }
}