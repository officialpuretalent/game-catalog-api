using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Catalog.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Catalog.Repositories
{
  public class MongoDbItemsRepository : IItemsRepository
  {
    private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
    private readonly IMongoCollection<Item> itemsCollection;
    private const string DatabaseName = "catalogDb";
    private const string CollectionName = "items";

    public MongoDbItemsRepository(IMongoClient mongoClient)
    {
      IMongoDatabase database = mongoClient.GetDatabase(DatabaseName);
      itemsCollection = database.GetCollection<Item>(CollectionName);
    }

    public async Task CreateItemAsync(Item item)
    {
      await itemsCollection.InsertOneAsync(item);
    }

    public async Task DeleteItemAsync(Guid id)
    {
      var filter = filterBuilder.Eq(existingItem => existingItem.Id, id);
      await itemsCollection.DeleteOneAsync(filter);
    }

    public async Task<Item> GetItemAsync(Guid id)
    {
      var filter = filterBuilder.Eq(item => item.Id, id);

      return await itemsCollection.Find(filter)
        .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
      return await itemsCollection.Find(new BsonDocument())
        .ToListAsync();
    }
    
    public async Task UpdateItemAsync(Item item)
    {
      var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
      await itemsCollection.ReplaceOneAsync(filter, item);
    }
  }
}