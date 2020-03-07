using Microsoft.AspNetCore.Mvc;
using Shakespokemon.Api.Core;
using Shakespokemon.Core;

namespace Shakespokemon.Api.Host
{
    public class PokemonController : Controller
    {
        private IPokemonRepository _repository;

        public PokemonController(IPokemonRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns a Shakespearean description of the requested Pokemon.
        /// </summary>
        /// <param name="name">The name of the Pokemon.</param>
        [HttpGet("pokemon/{name}")]
        public IActionResult Get(string name)
        {
            if(_repository.TryFind(name, out var item))
            {
                return Ok(item); 
            }
            
            return NotFound($"Could not find pokemon with name '{name}'.");
        }        
    }
}
