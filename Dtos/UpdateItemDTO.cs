using System.ComponentModel.DataAnnotations;

namespace Catalog.Dtos
{
  public record UpdateItemDto
  {
    public string Name { get; init; }

    [Range(1, 1000)]
    public decimal Price { get; init; }
  }
}
