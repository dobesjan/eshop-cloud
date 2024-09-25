using Eshop.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.Users
{
	public class Person : Entity
	{
		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		public string? Email { get; set; }

		public string? PhoneNumber { get; set; }

		public override EshopValidationResult Validate()
		{
			var result = new EshopValidationResult();

			if (String.IsNullOrEmpty(FirstName)) result.AddErrorMessage("First name not provided");
			if (String.IsNullOrEmpty(LastName)) result.AddErrorMessage("Last name not provided");
			if (String.IsNullOrEmpty(Email)) result.AddErrorMessage("Email name not provided");
			if (String.IsNullOrEmpty(PhoneNumber)) result.AddErrorMessage("Phone number name not provided");

			return result;
		}
	}
}
