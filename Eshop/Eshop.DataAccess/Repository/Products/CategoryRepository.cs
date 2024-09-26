using Eshop.DataAccess.Context;
using Eshop.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository.Products
{
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		public CategoryRepository(EshopDbContext context) : base(context) { }

		public Category GetWithProducts(int id)
		{
			return Get(id, includeProperties: "ProductCategories.Product,ParentCategory");
		}

		public IEnumerable<Category> GetAllWithHierarchy()
		{
			return GetAll(includeProperties: "ParentCategory");
		}
	}
}
