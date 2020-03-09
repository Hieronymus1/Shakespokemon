using System.Collections.Generic;

namespace Shakespokemon.Etl.DataAccess
{
    public interface IPokemonNamesClient
    {
        public string[] GetAll();
    }       
}