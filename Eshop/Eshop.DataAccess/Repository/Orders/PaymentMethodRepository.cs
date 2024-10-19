using Eshop.DataAccess.Context;
using Eshop.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository.Orders
{
	public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
	{
		public PaymentMethodRepository(EshopDbContext context) : base(context) { }
	}
}
