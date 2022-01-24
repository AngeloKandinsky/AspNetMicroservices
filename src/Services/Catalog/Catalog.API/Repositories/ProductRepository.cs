using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task CreateProduct(Product product)
        {
            await this._context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(y => y.Id, id);
            var DeleteResult = await _context
               .Products
               .DeleteOneAsync(filter);

            return DeleteResult.IsAcknowledged && DeleteResult.DeletedCount > 0;

        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context
                .Products
                .FindSync<Product>(p => p.Id == id)
                .FirstOrDefaultAsync();
                
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(y => y.Category, categoryName);
            return await _context
               .Products
               .FindSync<Product>(filter)
               .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(y => y.Name, name);
            return await _context
               .Products
               .FindSync<Product>(filter)
               .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
               return await _context
               .Products
               .FindSync<Product>(p => true)
               .ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var UpdateResult = await _context
               .Products
               .ReplaceOneAsync(filter: y => y.Id == product.Id, replacement: product);

            return UpdateResult.IsAcknowledged && UpdateResult.ModifiedCount > 0;
        }
    }
}
