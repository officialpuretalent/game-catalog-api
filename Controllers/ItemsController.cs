using System;
using System.Linq;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Controllers
{
  [ApiController]
  [Route("items")]
  public class ItemsController : ControllerBase
  {
    private readonly IItemsRepository _repository;

    public ItemsController(IItemsRepository repository)
    {
      _repository = repository;
    }

    // GET /items
    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetItems()
    {
      var items = (await _repository.GetItemsAsync())
        .Select(item => item.AsDTO());

      return items;
    }

    // GET /items/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItem(Guid id)
    {
      var item = await _repository.GetItemAsync(id);

      if (item == null)
        return NotFound();

      return Ok(item.AsDTO());
    }

    // POST /items
    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto itemDTO)
    {
      Item item = new()
      {
        Id = Guid.NewGuid(),
        Name = itemDTO.Name,
        Price = itemDTO.Price,
        CreatedDate = DateTimeOffset.UtcNow
      };

      await _repository.CreateItemAsync(item);

      return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDTO());
    }

    // PUT /item/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateItem(Guid id, UpdateItemDto itemDto)
    {
      var existingItem = await _repository.GetItemAsync(id);

      if (existingItem is null)
        return NotFound();

      Item updatedItem = existingItem with
      {
        Name = string.IsNullOrEmpty(itemDto.Name) ? existingItem.Name : itemDto.Name,
        Price = itemDto.Price != default ? itemDto.Price : existingItem.Price
      };

      await _repository.UpdateItemAsync(updatedItem);

      return NoContent();
    }

    // DELETE /item/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItem(Guid id)
    {
      var existingItem = await _repository.GetItemAsync(id);

      if (existingItem is null)
        return NotFound();

      await _repository.DeleteItemAsync(id);

      return NoContent();
    }
  }
}
