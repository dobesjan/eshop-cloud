using Eshop.Admin.Models.Requests.Category;
using Eshop.DataAccess.UnitOfWork;
using Eshop.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Admin.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductController(ILogger<CategoryController> logger, IUnitOfWork unitOfWork)
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

		[HttpPost("Add")]
		public IActionResult Add([FromBody] Product product)
		{
			_unitOfWork.ProductRepository.Add(product);
			_unitOfWork.ProductRepository.Save();
			return Ok();
		}

		[HttpPost("Update")]
		public IActionResult Update([FromBody] Product product)
		{
			_unitOfWork.ProductRepository.Update(product);
			_unitOfWork.ProductRepository.Save();
			return Ok();
		}

		[HttpPost("Delete")]
		public IActionResult Delete([FromBody] Product product)
		{
			_unitOfWork.ProductRepository.Remove(product);
			_unitOfWork.ProductRepository.Save();
			return Ok();
		}
	}
}
