using Eshop.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.Orders
{
    public class OrderInvoice : Entity
    {
		public int InvoiceNumber { get; set; }
		public DateTime IssueDate { get; set; }
		public int OrderId { get; set; }
		[ForeignKey(nameof(OrderId))]
		public Order? Order { get; set; }
	}
}
