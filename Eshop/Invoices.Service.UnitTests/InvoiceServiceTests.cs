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
			// Mock the IConfiguration
			_mockConfiguration = new Mock<IConfiguration>();
			_mockConfiguration.Setup(c => c["FileStore:Path"]).Returns(@"C:\Invoices\");
			_mockConfiguration.Setup(c => c["Redis:QueueName"]).Returns("invoicesQueue");

			// Mock the Redis connection
			_mockRedis = new Mock<IConnectionMultiplexer>();
			_mockDatabase = new Mock<IDatabase>();
			_mockRedis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_mockDatabase.Object);

			// Initialize the InvoiceService with mocked dependencies
			_invoiceService = new InvoiceService(_mockConfiguration.Object, _mockRedis.Object);
		}

		[Fact]
		public void GenerateInvoice_ShouldCreatePDF()
		{
			// Arrange
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

			// Act
			_invoiceService.GenerateInvoice(invoice);

			// Assert
			Assert.True(File.Exists(expectedFilePath), "Invoice PDF was not generated.");

			// Small delay to ensure file release
			Task.Delay(200).Wait();

			// Clean up the generated file after the test
			if (File.Exists(expectedFilePath))
			{
				// Ensure the file can be accessed by waiting for the disposal
				File.Delete(expectedFilePath);
			}
		}

		[Fact]
		public async Task ListenOnInvoices_ShouldProcessInvoiceFromQueue()
		{
			// Arrange
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

			// Act
			var task = _invoiceService.ListenOnInvoices(cts, token);
			await Task.Delay(2000); // Simulate waiting for the task to process
			cts.Cancel(); // Cancel after some time to stop the loop
			await task; // Await the completion of the task

			// Assert
			string expectedFilePath = @"C:\Invoices\2.pdf";
			Assert.True(File.Exists(expectedFilePath), "Invoice was not processed from the queue.");
			if (File.Exists(expectedFilePath))
			{
				File.Delete(expectedFilePath);
			}
		}
	}
}