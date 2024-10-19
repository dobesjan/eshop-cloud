using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Admin.Models.Requests.Category
{
	public class DeleteCategoryRequest
	{
		public Eshop.Models.Products.Category Category { get; set; }
	}
}
