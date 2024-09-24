using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.Products
{
	public class Product : Entity
	{
		[Required]
		public string Name { get; set; }

		public bool Enabled { get; set; }

		public bool IsInStock { get; set; }

		public int ViewOrder { get; set; }

		public int BuyLimit { get; set; }

		public double Cost { get; set; }
		public double CostWithTax { get; set; }
		public double CostBefore { get; set; }
	}
}
