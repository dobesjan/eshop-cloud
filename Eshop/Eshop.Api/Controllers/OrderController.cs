using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Eshop.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly ILogger<OrderController> _logger;
		private readonly IConnectionMultiplexer _redis;

		public OrderController(ILogger<OrderController> logger, IConnectionMultiplexer redis)
		{
			_logger = logger;
			_redis = redis;
		}

		[HttpPost]
		public IActionResult PostOrder([FromBody] Eshop.Models.Orders.Order order)
		{
			var validationResult = order.Validate();
			if (validationResult.Status == Utility.Validation.EshopValidationStatus.FAIL)
			{
				return BadRequest(new { validationResult });
			}

			string json = order.ToJson();
			var db = _redis.GetDatabase();
			db.ListRightPushAsync("orders", json);
			return Ok();
		}

		[HttpPost]
		public IActionResult PostPayment([FromBody] Eshop.Models.Orders.Payment payment)
		{
			string json = payment.ToJson();
			var db = _redis.GetDatabase();
			db.ListRightPushAsync("payments", json);
			return Ok();
		}
	}
}
