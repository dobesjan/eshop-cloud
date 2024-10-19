using Eshop.Admin.Models.Requests.Category;
using Eshop.DataAccess.UnitOfWork;
using Eshop.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Admin.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ILogger<CategoryController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public CategoryController(ILogger<CategoryController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		[HttpGet]
		public IActionResult Get(int categoryId)
		{
			var category = _unitOfWork.CategoryRepository.GetWithProducts(categoryId);
			if (category == null)
			{
				return NotFound();
			}

			return Ok(category);
		}

		[HttpGet(Name = "All")]
		public IActionResult GetAllCategories()
		{
			var categories = _unitOfWork.CategoryRepository.GetAllWithHierarchy();
			return Ok(categories);
		}

		[HttpPost("Add")]
		public IActionResult Add([FromBody] Category category)
		{
			_unitOfWork.CategoryRepository.Add(category);
			_unitOfWork.CategoryRepository.Save();
			return Ok();
		}

		[HttpPost("Delete")]
		public IActionResult Delete([FromBody] DeleteCategoryRequest request)
		{
			_unitOfWork.CategoryRepository.Remove(request.Category);
			_unitOfWork.CategoryRepository.Save();
			return Ok();
		}
	}
}
