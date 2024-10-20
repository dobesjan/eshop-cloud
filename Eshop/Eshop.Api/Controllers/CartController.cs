using Eshop.DataAccess.UnitOfWork;
using Eshop.Models.Api.Requests.Cart;
using Eshop.Models.Orders;
using Eshop.Models.Api.Responses;
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

		[HttpGet]
		public IActionResult CreateCart(string userId)
		{
			//TODO: Validate user existence

			var cart = new Order
			{
				UserId = userId
			};

			cart = _unitOfWork.OrderRepository.Add(cart);

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
				return BadRequest(new ErrorResponse { Error = "Product not found!" });
			}

			try
			{
				var cart = _unitOfWork.OrderRepository.AddToCart(request.UserId, request.CartId, request.ProductId, request.Count);
				return Ok(cart);
			}
			catch (InvalidDataException ex)
			{
				return BadRequest(new ErrorResponse { Error = ex.Message });
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

				// TODO: Send request for invoice generation

				return Ok(cart);
			}
			catch (InvalidDataException ex)
			{
				return BadRequest(new ErrorResponse { Error = ex.Message });
			}
		}

		[HttpPost("Shipping")]
		public IActionResult ChooseShipping([FromBody] ChooseShippingRequest request)
		{
			try
			{
				var cart = _unitOfWork.OrderRepository.GetCart(request.UserId, request.CartId);
				var shipping = _unitOfWork.ShippingRepository.Get(request.ShippingId);

				if (shipping == null)
				{
					return BadRequest(new { Error = "Shipping method not found!" });
				}

				cart.ShippingId = request.ShippingId;
				_unitOfWork.OrderRepository.Update(cart);
				_unitOfWork.OrderRepository.Save();

				return Ok(cart);
			}
			catch (InvalidDataException ex)
			{
				return BadRequest(new ErrorResponse { Error = ex.Message });
			}
		}

		[HttpPost("Payment")]
		public IActionResult ChoosePayment([FromBody] ChoosePaymentRequest request)
		{
			try
			{
				var cart = _unitOfWork.OrderRepository.GetCart(request.UserId, request.CartId);
				
				var paymentMethod = _unitOfWork.PaymentMethodRepository.Get(request.PaymentMethodId);

				if (paymentMethod == null)
				{
					return BadRequest(new { Error = "Wrong payment method" });
				}

				var payment = new Payment
				{
					OrderId = cart.Id,
					PaymentMethodId = paymentMethod.Id,
					PaymentStatusId = 1,
					Cost = cart.CalculateCost(),
					CostWithTax = cart.CalculateCostWithTax()
				};

				cart.Payment = payment;
				
				_unitOfWork.OrderRepository.Update(cart);
				_unitOfWork.OrderRepository.Save();

				return Ok(cart);
			}
			catch (InvalidDataException ex)
			{
				return BadRequest(new ErrorResponse { Error = ex.Message });
			}
		}
	}
}
