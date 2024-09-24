using Eshop.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eshop.Models.Orders
{
	public class Order : Entity
	{
		public DateTime CreatedDate { get; set; }

		public List<OrderProduct> OrderProducts { get; set; }

		public int? ShippingId { get; set; }

		[ForeignKey(nameof(ShippingId))]
		public Shipping? Shipping { get; set; }

		public int? PaymentId { get; set; }

		[ForeignKey(nameof(PaymentId))]
		public Payment? Payment { get; set; }

		public int BillingContactId { get; set; }

		[ForeignKey(nameof(BillingContactId))]
		public Contact? BillingContact { get; set; }

		public int? DeliveryContactId { get; set; }

		[ForeignKey(nameof(DeliveryContactId))]
		public Contact? DeliveryContact { get; set; }

		public override string ToJson()
		{
			object obj = new
			{
				Id = Id,
				CreatedDate = CreatedDate,
				OrderProducts = OrderProducts != null && OrderProducts.Any() ? OrderProducts.Select(op => op.ToJson()) : null,
				ShippingId = ShippingId,
				Shipping = Shipping,
				BillingContactId = BillingContactId,
				BillingContact = BillingContact,
				DeliveryContactId = DeliveryContactId,
				DeliveryContact = DeliveryContact,
				Payment = Payment != null ? new { PaymentStatus = Payment.PaymentStatus, PaymentMethod = Payment.PaymentMethod, Cost = Payment.Cost, CostWithTax = Payment.CostWithTax } : null,
			};

			return JsonSerializer.Serialize(obj);
		}
	}
}
