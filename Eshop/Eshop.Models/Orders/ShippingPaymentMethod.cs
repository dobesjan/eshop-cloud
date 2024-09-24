using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.Orders
{
	public class ShippingPaymentMethod
	{
		public int ShippingId { get; set; }
		public Shipping Shipping { get; set; }

		public int PaymentMethodId { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
	}
}
