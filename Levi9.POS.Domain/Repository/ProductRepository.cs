using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DBContext;
using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.Repository
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
