namespace Shakespokemon.Core
{
    public interface IPokemonRepository
    {
        bool TryFind(string name, out Pokemon item);
    }
}