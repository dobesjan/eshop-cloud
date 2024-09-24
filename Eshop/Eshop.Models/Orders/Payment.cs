using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eshop.Models.Orders
{
	public class Payment : Entity
	{
		public int OrderId { get; set; }

		[ForeignKey(nameof(OrderId))]
		public Order Order { get; set; }

		public int PaymentStatusId { get; set; }

		[ForeignKey(nameof(PaymentStatusId))]
		public PaymentStatus PaymentStatus { get; set; }

		public int PaymentMethodId { get; set; }

		[ForeignKey(nameof(PaymentMethodId))]
		public PaymentMethod PaymentMethod { get; set; }

		public double Cost { get; set; }
		public double CostWithTax { get; set; }

		public override string ToJson()
		{
			object obj = new
			{
				Id = Id,
				OrderId = OrderId,
				Order = Order,
				PaymentStatusId = PaymentStatusId,
				PaymentStatus = PaymentStatus,
				PaymentMethodId = PaymentMethodId,
				PaymentMethod = PaymentMethod,
				Cost = Cost,
				CostWithTax = CostWithTax,
			};

			return JsonSerializer.Serialize(obj);
		}
	}
}
