using Invoices.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Service
{
	public class InvoiceService
	{
		private string _storePath;
		private string _queueName;
		private readonly ConnectionMultiplexer _redis;
		private readonly IDatabase _database;

		public InvoiceService(IConfiguration configuration, ConnectionMultiplexer redis) 
		{
			_storePath = configuration["FileStore:Path"];
			_queueName = configuration["Redis:QueueName"];
			_redis = redis;
			_database = redis.GetDatabase();
		}

		public void GenerateInvoice(Invoice invoice)
		{
			string filePath = $@"{_storePath}{invoice.InvoiceNumber}.pdf";

			Document document = new Document();
			PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
			document.Open();

			document.AddTitle("Invoice");
			document.AddSubject("Invoice for the Czech market");
			document.AddKeywords("Invoice, Czech, PDF");
			document.AddAuthor("CzechInvoiceApp");

			document.Add(new Paragraph("INVOICE"));
			document.Add(new Paragraph($"Invoice Number: {invoice.InvoiceNumber}"));
			document.Add(new Paragraph($"Date: {invoice.IssueDate.ToString("dd/MM/yyyy")}"));
			document.Add(new Paragraph($"Seller: {invoice.Seller}"));
			document.Add(new Paragraph($"Buyer: {invoice.Buyer}"));
			document.Add(new Paragraph("Items:"));

			foreach (var item in invoice.Items)
			{
				document.Add(new Paragraph($"{item.Description} - {item.Quantity} x {item.UnitPrice:C} = {item.TotalPrice:C}"));
			}

			document.Add(new Paragraph($"Total: {invoice.TotalAmount:C}"));
			document.Close();

			Console.WriteLine($"Invoice {invoice.InvoiceNumber} generated at {filePath}.");
		}

		public async Task ListenOnInvoices(CancellationTokenSource cancellationTokenSource, CancellationToken cancellationToken)
		{
			try
			{
				// Continuously listen for invoice requests in Redis
				while (true)
				{
					var invoiceRequest = await _database.ListRightPopAsync(_queueName);
					if (!invoiceRequest.IsNullOrEmpty)
					{
						var invoice = JsonConvert.DeserializeObject<Invoice>(invoiceRequest);
						if (invoice != null)
						{
							GenerateInvoice(invoice);
						}
					}

					await Task.Delay(1000, cancellationToken);
				}
			}
			catch (TaskCanceledException)
			{
				Console.WriteLine("Invoice processing loop canceled.");
			}
			finally
			{
				cancellationTokenSource.Dispose();
			}
		}
	}
}
