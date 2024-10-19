using Eshop.DataAccess.UnitOfWork;
using Eshop.Models.Api.Requests;
using Eshop.Utility.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Eshop.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class CartController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public CartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		[HttpGet]
		public IActionResult GetCart(string userId)
		{
			var cart = _unitOfWork.OrderRepository.GetCart(userId);
			return Ok(cart);
		}

		[HttpPost("Add")]
		public IActionResult AddToCart([FromBody] AddToCartRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var product = _unitOfWork.ProductRepository.Get(request.ProductId);
			if (product == null)
			{
				return BadRequest(new { Error = "Product not found!" });
			}

			try
			{
				var cart = _unitOfWork.OrderRepository.AddToCart(request.UserId, request.CartId, request.ProductId, request.Count);
				return Ok(cart);
			}
			catch (InvalidDataException ex)
			{
				return BadRequest(new { Error = ex.Message });
			}
		}

		[HttpGet("Order")]
		public IActionResult Order(string userId, int cartId)
		{
			try
			{
				var cart = _unitOfWork.OrderRepository.GetCart(userId, cartId);
				var result = cart.Validate();

				if (result.Status == EshopValidationStatus.FAIL)
				{
					return BadRequest(result);
				}

				cart.IsOrdered = true;
				_unitOfWork.OrderRepository.Update(cart);
				_unitOfWork.OrderRepository.Save();

				return Ok(cart);
			}
			catch (InvalidDataException ex)
			{
				return BadRequest(new { Error = ex.Message });
			}
		}
	}
}
