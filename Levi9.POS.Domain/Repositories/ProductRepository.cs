using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Levi9.POS.Domain.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBaseContext _dataBaseContext;

        public ProductRepository(DataBaseContext dataBaseContext)//DI
        {
            _dataBaseContext = dataBaseContext;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _dataBaseContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
