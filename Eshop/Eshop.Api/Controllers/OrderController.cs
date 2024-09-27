using Eshop.DataAccess.UnitOfWork;
using Eshop.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

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

		public OrderController(ILogger<OrderController> logger, IConnectionMultiplexer redis, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_redis = redis;
			_unitOfWork = unitOfWork;
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
