using StackExchange.Redis;

var redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
var db = redis.GetDatabase();
var redisKey = "orders";

Console.WriteLine($"Listening for {redisKey}...");
while (true)
{
	// Wait for messages in the "messages" list
	var message = await db.ListLeftPopAsync(redisKey);
	if (!message.IsNullOrEmpty)
	{
		Console.WriteLine($"Processing {redisKey}: {message}");
		// Process the message here (e.g., save to database, log, etc.)
	}

	// Sleep for a short time to avoid busy-waiting
	await Task.Delay(1000);
}