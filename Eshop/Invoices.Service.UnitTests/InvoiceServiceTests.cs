using Invoices.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Invoices.Service.UnitTests
{
	public class InvoiceServiceTests
	{
		private readonly InvoiceService _invoiceService;
		private readonly Mock<IConfiguration> _mockConfiguration;
		private readonly Mock<IConnectionMultiplexer> _mockRedis;
		private readonly Mock<IDatabase> _mockDatabase;

		public InvoiceServiceTests()
		{
			_mockConfiguration = new Mock<IConfiguration>();
			_mockConfiguration.Setup(c => c["FileStore:Path"]).Returns(@"C:\Invoices\");
			_mockConfiguration.Setup(c => c["Redis:QueueName"]).Returns("invoicesQueue");

			_mockRedis = new Mock<IConnectionMultiplexer>();
			_mockDatabase = new Mock<IDatabase>();
			_mockRedis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_mockDatabase.Object);

			_invoiceService = new InvoiceService(_mockConfiguration.Object, _mockRedis.Object);
		}

		[Fact]
		public void GenerateInvoice_ShouldCreatePDF()
		{
			var invoice = new Invoice
			{
				InvoiceNumber = 1,
				IssueDate = DateTime.Now,
				Seller = "Test Seller",
				Buyer = "Test Buyer",
				Items = new List<InvoiceItem>
		{
			new InvoiceItem { Description = "Item 1", Quantity = 2, UnitPrice = 50 },
			new InvoiceItem { Description = "Item 2", Quantity = 1, UnitPrice = 75 }
		},
				TotalAmount = 175
			};

			string expectedFilePath = @"C:\Invoices\1.pdf";

			_invoiceService.GenerateInvoice(invoice);

			Assert.True(File.Exists(expectedFilePath), "Invoice PDF was not generated.");

			Task.Delay(200).Wait();

			if (File.Exists(expectedFilePath))
			{
				File.Delete(expectedFilePath);
			}
		}

		[Fact]
		public async Task ListenOnInvoices_ShouldProcessInvoiceFromQueue()
		{
			var invoice = new Invoice
			{
				InvoiceNumber = 2,
				IssueDate = DateTime.Now,
				Seller = "Test Seller",
				Buyer = "Test Buyer",
				Items = new List<InvoiceItem>
				{
					new InvoiceItem { Description = "Item 1", Quantity = 2, UnitPrice = 50 },
				},
				TotalAmount = 100
			};

			string invoiceJson = JsonConvert.SerializeObject(invoice);
			_mockDatabase.Setup(db => db.ListRightPopAsync("invoicesQueue", It.IsAny<CommandFlags>()))
						 .ReturnsAsync((RedisValue)invoiceJson);

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken token = cts.Token;

			var task = _invoiceService.ListenOnInvoices(cts, token);
			await Task.Delay(2000);
			cts.Cancel();
			await task;

			string expectedFilePath = @"C:\Invoices\2.pdf";
			Assert.True(File.Exists(expectedFilePath), "Invoice was not processed from the queue.");
			if (File.Exists(expectedFilePath))
			{
				File.Delete(expectedFilePath);
			}
		}
	}
}