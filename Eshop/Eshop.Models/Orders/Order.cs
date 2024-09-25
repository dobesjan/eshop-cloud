using Eshop.Models.Users;
using Eshop.Utility.Validation;
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
		public DateTime? CreatedDate { get; set; }

		public List<OrderProduct> OrderProducts { get; set; }

		public int? ShippingId { get; set; }

		[ForeignKey(nameof(ShippingId))]
		public Shipping? Shipping { get; set; }

		public int? PaymentId { get; set; }

		[ForeignKey(nameof(PaymentId))]
		public Payment? Payment { get; set; }

		public int? BillingContactId { get; set; }

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

		public override EshopValidationResult Validate()
		{
			var result = new EshopValidationResult();

			if (OrderProducts == null)
			{
				result.AddErrorMessage("There are not any ordered products!");
			}

			if (OrderProducts != null && !OrderProducts.Any())
			{
				result.AddErrorMessage("There are not any ordered products!");
			}

			if (!ShippingId.HasValue)
			{
				result.AddErrorMessage("Shipping not provided!");
			}

			if (Payment == null)
			{
				result.AddErrorMessage("Payment not provided!");
			}

			if (BillingContact == null && !BillingContactId.HasValue)
			{
				result.AddErrorMessage("Billing contact not provided!");
			}

			if (BillingContact != null)
			{
				if (BillingContact.Person == null && !BillingContact.PersonId.HasValue)
				{
					result.AddErrorMessage("Personal information not provided!");
				}
                else if (BillingContact.Person != null)
                {
					result.MergeValidationResult(BillingContact.Person.Validate());
                }

				if (BillingContact.Address == null && !BillingContact.AddressId.HasValue)
				{
					result.AddErrorMessage("Address not provided!");
				}
				else if (BillingContact.Address != null)
				{
					result.MergeValidationResult(BillingContact.Address.Validate());
				}
			}

			return result;
		}
	}
}
