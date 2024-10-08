﻿using Eshop.Utility.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.Users
{
	public class Contact : Entity
	{
		public int? PersonId { get; set; }

		[ForeignKey(nameof(PersonId))]
		public Person? Person { get; set; }

		public int? AddressId { get; set; }

		[ForeignKey(nameof(AddressId))]
		public Address? Address { get; set; }

		public override EshopValidationResult Validate()
		{
			var result = new EshopValidationResult();

			if (Person != null)
			{
				result.MergeValidationResult(Person.Validate());
			}

			if (Address != null)
			{
				result.MergeValidationResult(Address.Validate());
			}

			return result;
		}
	}
}
