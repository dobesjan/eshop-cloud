using Eshop.Models.Orders;
using Eshop.Models.Products;
using Eshop.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Context
{
	public class EshopDbContext : DbContext
	{
		#region Orders
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderProduct> OrderProducts { get; set; }
		public DbSet<Payment> Payment { get; set; }
		public DbSet<PaymentMethod> PaymentMethod { get; set; }
		public DbSet<PaymentStatus> PaymentStatus { get; set; }
		public DbSet<Shipping> Shippings { get; set; }
		public DbSet<ShippingPaymentMethod> ShippingPaymentMethods { get; set; }
		#endregion

		#region Products
		public DbSet<Product> Products { get; set; }
		#endregion

		#region Contacts
		public DbSet<Address> Addresses { get; set; }
		public DbSet<Contact> Contacts { get; set; }
		public DbSet<Person> People { get; set; }
		#endregion

		public EshopDbContext(DbContextOptions<EshopDbContext> options) : base(options)
		{

		}
	}
}
