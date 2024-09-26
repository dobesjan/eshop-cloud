using Eshop.DataAccess.Context;
using Eshop.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository.Products
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		public ProductRepository(EshopDbContext context) : base(context) { }
	}
}
