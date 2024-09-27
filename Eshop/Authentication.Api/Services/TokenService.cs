using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Api.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public bool ValidateClient(string clientId, string clientSecret)
		{
			var clientsSection = _configuration.GetSection("Clients").GetChildren();

			foreach (var clientSection in clientsSection)
			{
				var configuredClientId = clientSection["ClientId"];
				var configuredClientSecret = clientSection["ClientSecret"];

				if (configuredClientId == clientId && configuredClientSecret == clientSecret)
				{
					return true;
				}
			}
			return false;
		}

		public string GenerateAccessToken(IEnumerable<Claim> claims)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Issuer"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"])),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
