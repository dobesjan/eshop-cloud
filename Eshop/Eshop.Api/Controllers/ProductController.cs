using Eshop.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;

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
			var product = _unitOfWork.ProductRepository.GetWithCategories(id);
			if (product == null)
			{
				return NotFound();
			}

			return Ok(product);
		}

		[HttpGet("GetAll")]
		public IActionResult GetAll(int offset = 0, int limit = 0)
		{
			var products = _unitOfWork.ProductRepository.GetAllWithCategories(offset: offset, limit: limit);
			return Ok(products);
		}

		[HttpGet("GetAllForCategory")]
		public IActionResult GetAllForCategory(int categoryId, int offset = 0, int limit = 0)
		{
			var products = _unitOfWork.ProductRepository.GetAllWithCategories(
				filter: p => p.ProductCategories != null && p.ProductCategories.Any(pc => pc.CategoryId == categoryId),
				offset: offset,
				limit: limit
			);

			return Ok(products);
		}
	}
}
