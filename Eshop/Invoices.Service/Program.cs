using Invoices.Models;
using Invoices.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StackExchange.Redis;

var host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((context, services) =>
	{
		var configuration = context.Configuration;

		var redisConnectionString = configuration["Redis:ConnectionString"];
		var redisQueueName = configuration["Redis:QueueName"];

		services.AddSingleton<InvoiceService>();
		services.AddSingleton(ConnectionMultiplexer.Connect(redisConnectionString));
		services.AddSingleton(redisQueueName);
	})
	.Build();

var redis = host.Services.GetRequiredService<ConnectionMultiplexer>();
var db = redis.GetDatabase();
var queueName = host.Services.GetRequiredService<string>();
var invoiceService = host.Services.GetRequiredService<InvoiceService>();

Console.WriteLine("Connected to Redis.");

CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
CancellationToken cancellationToken = cancellationTokenSource.Token;

try
{
	// Continuously listen for invoice requests in Redis
	while (true)
	{
		var invoiceRequest = await db.ListRightPopAsync(queueName);
		if (!invoiceRequest.IsNullOrEmpty)
		{
			var invoice = JsonConvert.DeserializeObject<Invoice>(invoiceRequest);
			if (invoice != null)
			{
				invoiceService.GenerateInvoice(invoice);
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
