namespace Shakespokemon.Etl.DataAccess
{
    public interface IPokemonDescriptionClient
    {
        string GetDescription(string name);
    }        
}