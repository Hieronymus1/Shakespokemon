namespace Shakespokemon.Api.Core
{
    using Shakespokemon.Core;

    public interface IPokemonRepository
    {
        bool TryFind(string name, out Pokemon item);
    }
}