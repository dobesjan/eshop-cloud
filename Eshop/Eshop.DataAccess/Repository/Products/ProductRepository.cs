using Eshop.DataAccess.Context;
using Eshop.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository.Products
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		public ProductRepository(EshopDbContext context) : base(context) { }

		public Product GetWithCategories(int productId)
		{
			return Get(productId, "ProductCategories.Category");
		}

		public IEnumerable<Product> GetAllWithCategories(Expression<Func<Product, bool>>? filter = null, int offset = 0, int limit = 0)
		{
			return GetAll(filter, "ProductCategories.Category", offset, limit);
		}
	}
}
