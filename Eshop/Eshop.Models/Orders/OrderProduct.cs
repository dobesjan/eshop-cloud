using Eshop.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eshop.Models.Orders
{
	public class OrderProduct : Entity
	{
		public int ProductId { get; set; }

		public Product Product { get; set; }

		public int OrderId { get; set; }

		public Order Order { get; set; }

		private int _count;
		public int Count
		{
			get => _count;
			set
			{
				_count = value;
				RecalculateCosts();
			}
		}

		public double Cost { get; set; }
		public double CostWithTax { get; set; }
		public double CostBefore { get; set; }

		public override string ToJson()
		{
			object obj = new
			{
				Product = Product.ToJson(),
				Count = Count,
				Cost = Cost,
				CostWithTax = CostWithTax,
				CostBefore = CostBefore,
			};

			return JsonSerializer.Serialize(obj);
		}

		private void RecalculateCosts()
		{
			if (Product != null)
			{
				Cost = Product.Cost * Count;
				CostWithTax = Product.CostWithTax * Count;
				CostBefore = Product.CostBefore * Count;
			}
		}
	}
}
