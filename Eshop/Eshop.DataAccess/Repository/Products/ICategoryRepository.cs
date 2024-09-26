using Eshop.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository.Products
{
	public interface ICategoryRepository : IRepository<Category>
	{
		Category GetWithProducts(int id);
		IEnumerable<Category> GetAllWithHierarchy();
	}
}
