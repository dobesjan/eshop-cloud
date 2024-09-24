using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public override bool Validate()
		{
			if (String.IsNullOrEmpty(Street)) throw new InvalidDataException("Street not provided");
			if (String.IsNullOrEmpty(City)) throw new InvalidDataException("City not provided");
			if (String.IsNullOrEmpty(PostalCode)) throw new InvalidDataException("Postal code not provided");

			return true;
		}
	}
}
