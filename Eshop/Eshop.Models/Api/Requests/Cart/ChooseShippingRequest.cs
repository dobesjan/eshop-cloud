using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.Api.Requests.Cart
{
	public class ChooseShippingRequest
	{
		public string UserId { get; set; }
		public int CartId { get; set; }
		public int ShippingId { get; set; }
	}
}
