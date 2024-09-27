using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
