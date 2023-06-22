using System;
using Catalog.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Catalog.Repositories
{
  public interface IItemsRepository
  {
    Task<Item> GetItemAsync(Guid id);
    Task<IEnumerable<Item>> GetItemsAsync();
    Task CreateItemAsync(Item item);
    Task UpdateItemAsync(Item item);
    Task DeleteItemAsync(Guid id);
  }
}