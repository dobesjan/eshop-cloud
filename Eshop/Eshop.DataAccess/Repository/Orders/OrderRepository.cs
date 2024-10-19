using Eshop.DataAccess.Context;
using Eshop.Models.Orders;
using Eshop.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository.Orders
{
	public class OrderRepository : Repository<Order>, IOrderRepository
	{
		private readonly string _allProperties = "OrderProducts.Product,Shipping,Shipping.ShippingPaymentMethod,Payment,Payment.PaymentStatus,Payment.PaymentMethod,BillingContact,BillingContact.Person,BillingContact.Address,DeliveryContact,DeliveryContact.Person,DeliveryContact.Address";

		public OrderRepository(EshopDbContext context) : base(context)
		{
		}

		public Order GetCart(string userId)
		{
			return Get(o => o.UserId == userId && !o.IsOrdered, _allProperties);
		}

		public Order GetCart(string userId, int cartId)
		{
			var cart = Get(o => o.UserId == userId && o.Id == cartId, _allProperties);
			if (cart == null)
			{
				throw new InvalidDataException("Cart not found!");
			}

			return cart;
		}

		public Order AddToCart(string userId, int cartId, int productId, int count)
		{
			var cart = GetCart(userId, cartId);

			if (cart.OrderProducts == null)
			{
				cart.OrderProducts = new List<OrderProduct>();
			}

			cart.OrderProducts.Add(new OrderProduct { ProductId = productId, Count = count });

			Update(cart);
			Save();

			return cart;
		}
	}
}
