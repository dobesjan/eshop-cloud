using Eshop.Api.Configuration;
using Eshop.DataAccess.UnitOfWork;
using Eshop.Models.Orders;
using Eshop.Models.Products;
using Invoices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace Eshop.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class OrderController : ControllerBase
	{
		private readonly ILogger<OrderController> _logger;
		private readonly IConnectionMultiplexer _redis;
		private readonly IUnitOfWork _unitOfWork;
		private readonly QueuesOptions _queueOptions;

		public OrderController(ILogger<OrderController> logger, IConnectionMultiplexer redis, IUnitOfWork unitOfWork, IOptions<QueuesOptions> queueOptions)
		{
			_logger = logger;
			_redis = redis;
			_unitOfWork = unitOfWork;
			_queueOptions = queueOptions.Value;
		}

		[HttpPost]
		public IActionResult PostOrder([FromBody] Eshop.Models.Orders.Order order)
		{
			var validationResult = order.Validate();
			if (validationResult.Status == Utility.Validation.EshopValidationStatus.FAIL)
			{
				return BadRequest(new { validationResult });
			}

			List<Product> products = order.OrderProducts.Select(op => op.Product).ToList();
			if (!_unitOfWork.ProductRepository.AreStored(products))
			{
				return BadRequest();
			}

			_unitOfWork.OrderRepository.Add(order);
			_unitOfWork.OrderRepository.Save();

			OrderInvoice orderInvoice = new OrderInvoice
			{
				InvoiceNumber = 1,
				IssueDate = DateTime.Now,
				OrderId = order.Id
			};

			var latestInvoice = _unitOfWork.OrderInvoiceRepository.GetAll().OrderByDescending(i => i.InvoiceNumber).FirstOrDefault();
			if (latestInvoice != null)
			{
				orderInvoice.InvoiceNumber = latestInvoice.InvoiceNumber + 1;
			}

			// TODO: GEt seller info
			Invoice invoice = new Invoice
			{
				InvoiceNumber = orderInvoice.InvoiceNumber,
				IssueDate = orderInvoice.IssueDate,
				Seller = "",
				Buyer = order.BillingContact?.Person?.FirstName + " " + order.BillingContact?.Person?.LastName,
				TotalAmount = order.Payment.Cost
			};

			invoice.Items = GetInvoiceItems(invoice, order);

			string json = JsonSerializer.Serialize(invoice);
			var db = _redis.GetDatabase();
			db.ListRightPushAsync(_queueOptions.Invoice, json);

			return Ok();
		}

		[HttpPost]
		public IActionResult PostPayment([FromBody] Eshop.Models.Orders.Payment payment)
		{
			string json = payment.ToJson();
			var db = _redis.GetDatabase();
			db.ListRightPushAsync(_queueOptions.Payment, json);
			return Ok();
		}

		private List<InvoiceItem> GetInvoiceItems(Invoice invoice, Models.Orders.Order order) 
		{
			List<InvoiceItem> items = new List<InvoiceItem>();
			var products = order.OrderProducts.Select(op => op.Product);

			foreach (OrderProduct orderProduct in order.OrderProducts)
			{
				InvoiceItem item = new InvoiceItem
				{
					Description = String.Empty,
					Quantity = orderProduct.Count,
					UnitPrice = orderProduct.Cost
				};

				items.Add(item);
			}

			return items;
		}
	}
}
