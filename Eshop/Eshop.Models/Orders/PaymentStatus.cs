﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.Orders
{
	public class PaymentStatus : Entity
	{
		[Required]
		public string Name { get; set; }
	}
}
