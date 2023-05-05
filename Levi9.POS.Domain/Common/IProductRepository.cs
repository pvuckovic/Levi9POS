using Levi9.POS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.Common
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);       
    }
}
