using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace Levi9.POS.Domain.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBaseContext _dataBaseContext;
        public ProductRepository(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _dataBaseContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<Product>> SearchProductsAsync(int page, string name, string orderBy, string direction)
        {
            IQueryable<Product> query = _dataBaseContext.Products;

            // Filter by name
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));
            }

            // Order by
            switch (orderBy?.ToLower())
            {
                case "name":
                    query = direction?.ToLower() == "dsc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                    break;
                case "id":
                    query = direction?.ToLower() == "dsc" ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id);
                    break;
                case "globalid":
                    query = direction?.ToLower() == "dsc" ? query.OrderByDescending(p => p.GlobalId) : query.OrderBy(p => p.GlobalId);
                    break;
                case "availablequantity":
                    query = direction?.ToLower() == "dsc" ? query.OrderByDescending(p => p.AvailableQuantity) : query.OrderBy(p => p.AvailableQuantity);
                    break;
                default:
                    query = direction?.ToLower() == "dsc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                    break;
            }

            // Paginate
            int pageSize = 10;
            int skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);

            return await query.ToListAsync();
        }
        public async Task<Product> InsertProductAsync(Product product)
        {
            await _dataBaseContext.Products.AddAsync(product);
            await _dataBaseContext.SaveChangesAsync();
            return product;
        }
        public async Task<Product> UpdateProductAsync(Product product)
        {
            _dataBaseContext.Products.Update(product);
            await _dataBaseContext.SaveChangesAsync();
            return product;
        }
        public async Task<Product> GetProductByGlobalIdAsync(Guid globalId)
        {
            return await _dataBaseContext.Products.FirstOrDefaultAsync(p => p.GlobalId == globalId);
        }
    }
}
