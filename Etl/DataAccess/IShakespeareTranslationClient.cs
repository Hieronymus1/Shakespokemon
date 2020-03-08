namespace Shakespokemon.Etl.DataAccess
{
    public interface IShakespeareTranslationClient
    {
        string GetTranslation(string text);
    }    
}