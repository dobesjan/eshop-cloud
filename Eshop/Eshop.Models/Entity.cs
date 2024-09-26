using Eshop.Models.Orders;
using Eshop.Models.Products;
using Eshop.Utility.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eshop.Models
{
    public class Entity : IEntity
	{
		[Key]
		public int Id { get; set; }

		public virtual string ToJson()
		{
			return JsonSerializer.Serialize(this);
		}

		public virtual EshopValidationResult Validate()
		{
			return new EshopValidationResult();
		}
	}
}
