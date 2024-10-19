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

var invoiceService = host.Services.GetRequiredService<InvoiceService>();

Console.WriteLine("Connected to Redis.");

CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
CancellationToken cancellationToken = cancellationTokenSource.Token;

await invoiceService.ListenOnInvoices(cancellationTokenSource, cancellationToken);
