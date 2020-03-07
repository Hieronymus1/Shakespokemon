using Shakespokemon.Core;
using LiteDB;

namespace Shakespokemon.DataAccess
{
    public class PokemonRepository : IPokemonRepository
    {
        private const string DbName = "Shakespokemon.db";
        private const string CollectionName = "pokemons";

        public void Add(Pokemon item)
        {
            var document = new BsonDocument();
            document["name"] = item.Name;
            document["description"] = item.Description;
            document["utc_creation_date"] = item.UtcCreationDate;

            using(var db = new LiteDatabase(DbName))
            {
                var documents = db.GetCollection(CollectionName);
                documents.EnsureIndex(x => x["name"], true);
                documents.Insert(document);
            }
        }

        public bool TryFind(string name, out Pokemon item)
        {
            item = null;

            using(var db = new LiteDatabase(DbName))
            {
                var documents = db.GetCollection(CollectionName);
                var document = documents.Query().Where(i => i["name"] == name).FirstOrDefault();
                if(document != null)
                {
                    item = new Pokemon(document["name"], document["description"], document["utc_creation_date"]);
                }
                
                return document != null;
            }
        }     

        public void Remove(string name)
        {
            using(var db = new LiteDatabase(DbName))
            {
                var documents = db.GetCollection(CollectionName);
                var document = documents.Query().Where(i => i["name"] == name).FirstOrDefault();
                if(document != null)
                {
                    documents.Delete(document["_id"]);
                }
            }
        }         
    }
}
