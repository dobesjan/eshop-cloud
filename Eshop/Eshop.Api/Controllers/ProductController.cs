using Eshop.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Eshop.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class ProductController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		[HttpGet]
		public IActionResult GetProduct(int id)
		{
			var product = _unitOfWork.ProductRepository.Get(id);
			if (product == null)
			{
				return NotFound();
			}

			return Ok(product);
		}
	}
}
