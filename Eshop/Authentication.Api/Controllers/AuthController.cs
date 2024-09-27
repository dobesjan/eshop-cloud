using Authentication.Api.Services;
using Authentication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Authentication.Api.Controllers
{
	[ApiController]
	[Route("/")]
	public class AuthController : ControllerBase
	{
		private readonly ITokenService _tokenService;

		public AuthController(ITokenService tokenService)
		{
			_tokenService = tokenService;
		}

		[HttpPost("Token")]
		public IActionResult GetToken([FromBody] ClientCredentials request)
		{
			// Validate client credentials
			if (!_tokenService.ValidateClient(request.ClientId, request.ClientSecret))
			{
				return Unauthorized("Unauthorized");
			}

			// Generate access token
			var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, request.ClientId)
		};

			var accessToken = _tokenService.GenerateAccessToken(claims);

			return Ok(new AuthToken { AccessToken = accessToken });
		}
	}
}
