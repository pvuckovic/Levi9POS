using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

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
        public async Task<bool> DoesProductExist(int productId, string name)
        {
            var product = await _dataBaseContext.Products.FirstOrDefaultAsync(c => c.Id == productId && c.Name == name);
            return product != null;
        }
    }
}
