using Eshop.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository.Orders
{
	public interface IOrderRepository : IRepository<Order>
	{
		Order GetCart(string userId);
		Order GetCart(string userId, int cartId);
		Order AddToCart(string userId, int cartId, int productId, int count);
	}
}
