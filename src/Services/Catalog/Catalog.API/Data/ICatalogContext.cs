using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
  interface ICatalogContext
  {
    IMongoCollection<Product> Products { get; }
  }
}
