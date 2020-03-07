namespace Shakespokemon.Core
{
    public interface IPokemonRepository
    {
        void Add(Pokemon item);

        bool TryFind(string name, out Pokemon item);
    }
}