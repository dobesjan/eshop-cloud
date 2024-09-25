using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Utility.Validation;

namespace Eshop.Models.Users
{
    public class Address : Entity
	{
		public int CustomerId { get; set; }

		[Required]
		public string Street { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string PostalCode { get; set; }

		public override EshopValidationResult Validate()
		{
			var result = new EshopValidationResult();

			if (String.IsNullOrEmpty(Street))
			{
				result.AddErrorMessage("Street not provided");
			}

			if (String.IsNullOrEmpty(City))
			{
				result.AddErrorMessage("City not provided");
			}

			if (String.IsNullOrEmpty(PostalCode))
			{
				result.AddErrorMessage("Postal code not provided");
			}

			return result;
		}
	}
}
