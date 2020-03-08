using System.Collections.Generic;
using Shakespokemon.Core;

namespace Shakespokemon.Etl.Core
{
    public interface IPokemonSourceRepository
    {
        IEnumerable<Pokemon> GetAll();
    }
}