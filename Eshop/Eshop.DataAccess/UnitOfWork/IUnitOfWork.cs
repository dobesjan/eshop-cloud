using Eshop.DataAccess.Repository.Orders;
using Eshop.DataAccess.Repository.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.UnitOfWork
{
	public interface IUnitOfWork
	{
		public IOrderRepository OrderRepository { get; }

		public IProductRepository ProductRepository { get; }
		public ICategoryRepository CategoryRepository { get; }
	}
}
