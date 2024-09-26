using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.Products
{
    public class Category : Entity
    {
		[Required]
		public string Name { get; set; }

		public int? ParentCategoryId { get; set; }

		[ForeignKey(nameof(ParentCategoryId))]
		public Category? ParentCategory { get; set; }

		public bool Enabled { get; set; }

		public List<ProductCategory> ProductCategories { get; set; }

		[NotMapped]
		public List<Category> Children { get; set; } = new List<Category>();
	}
}
