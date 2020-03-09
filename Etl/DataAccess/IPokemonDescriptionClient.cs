namespace Shakespokemon.Etl.DataAccess
{
    public interface IPokemonDescriptionClient
    {
        bool TryGet(string name, out string description);
    }        
}