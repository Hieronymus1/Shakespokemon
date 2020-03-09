using System.Collections.Generic;
using Shakespokemon.Etl.Core;
using Shakespokemon.Core;

namespace Shakespokemon.Etl.DataAccess
{
    public class PokemonSourceRepository : IPokemonSourceRepository
    {
        private readonly IPokemonNamesClient _namesClient;
        private readonly IPokemonDescriptionClient _descriptionClient;
        private readonly IShakespeareTranslationClient _translationClient;

        public PokemonSourceRepository(IPokemonNamesClient namesClient, IPokemonDescriptionClient descriptionClient, 
            IShakespeareTranslationClient translationClient)
        {
            _namesClient = namesClient;
            _descriptionClient = descriptionClient;
            _translationClient = translationClient;
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
            }
        }
    }
}
