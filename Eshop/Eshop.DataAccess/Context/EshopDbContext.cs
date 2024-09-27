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
		public DbSet<OrderInvoice> OrderInvoices { get; set; }
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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OrderProduct>()
				.HasKey(pc => new { pc.ProductId, pc.OrderId });

			modelBuilder.Entity<OrderProduct>()
				.HasOne(pc => pc.Product)
				.WithMany(p => p.OrderProducts)
				.HasForeignKey(pc => pc.ProductId);

			modelBuilder.Entity<OrderProduct>()
				.HasOne(pc => pc.Order)
				.WithMany(c => c.OrderProducts)
				.HasForeignKey(pc => pc.OrderId);

			modelBuilder.Entity<ProductCategory>()
				.HasKey(pc => new { pc.ProductId, pc.CategoryId });

			modelBuilder.Entity<ProductCategory>()
				.HasOne(pc => pc.Product)
				.WithMany(p => p.ProductCategories)
				.HasForeignKey(pc => pc.ProductId);

			modelBuilder.Entity<ProductCategory>()
				.HasOne(pc => pc.Category)
				.WithMany(c => c.ProductCategories)
				.HasForeignKey(pc => pc.CategoryId);

			modelBuilder.Entity<Category>()
				.HasOne(c => c.ParentCategory)
				.WithMany()
				.HasForeignKey(c => c.ParentCategoryId)
				.OnDelete(DeleteBehavior.Restrict);

			base.OnModelCreating(modelBuilder);
		}
	}
}
