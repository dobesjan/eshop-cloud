using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.Orders
{
	public class Shipping : Entity
	{
		[Required]
		public string Name { get; set; }

		public bool Enabled { get; set; }

		public List<ShippingPaymentMethod> ShippingPaymentMethod { get; set; }
	}
}
