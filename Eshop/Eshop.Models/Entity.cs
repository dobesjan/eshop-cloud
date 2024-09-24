using Eshop.Models.Orders;
using Eshop.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models
{
	public class Entity : IEntity
	{
		[Key]
		public int Id { get; set; }

		public virtual string ToJson()
		{
			throw new NotImplementedException();
		}

		public virtual bool Validate()
		{
			return true;
		}
	}
}
