using Eshop.DataAccess.Context;
using Eshop.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository.Orders
{
	public class OrderRepository : Repository<Order>, IOrderRepository
	{
		public OrderRepository(EshopDbContext context) : base(context)
		{
		}
	}
}
