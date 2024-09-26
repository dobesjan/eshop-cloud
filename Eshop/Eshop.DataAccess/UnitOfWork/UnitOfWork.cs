using Eshop.DataAccess.Context;
using Eshop.DataAccess.Repository.Orders;
using Eshop.DataAccess.Repository.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly EshopDbContext _context;

		public IOrderRepository OrderRepository { get; }
		public IProductRepository ProductRepository { get; }

		public UnitOfWork(EshopDbContext context)
		{
			_context = context;

			OrderRepository = new OrderRepository(_context);
			ProductRepository = new ProductRepository(_context);
		}
	}
}
