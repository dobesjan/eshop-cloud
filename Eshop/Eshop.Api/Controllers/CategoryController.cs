using Eshop.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public CategoryController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		[HttpGet]
		public IActionResult GetCategory(int id)
		{
			var category = _unitOfWork.CategoryRepository.GetWithProducts(id);
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
	}
}
