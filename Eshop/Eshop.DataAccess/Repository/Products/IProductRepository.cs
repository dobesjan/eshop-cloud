using Eshop.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository.Products
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetWithCategories(int productId);
        IEnumerable<Product> GetAllWithCategories(Expression<Func<Product, bool>>? filter = null, int offset = 0, int limit = 0);
    }
}
