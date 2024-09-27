using Eshop.DataAccess.Context;
using Eshop.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository.Orders
{
	public class OrderInvoiceRepository : Repository<OrderInvoice>, IOrderInvoiceRepository
	{
		public OrderInvoiceRepository(EshopDbContext context) : base(context) { }
	}
}
