using System.Security.Claims;

namespace Authentication.Api.Services
{
	public interface ITokenService
	{
		bool ValidateClient(string clientId, string clientSecret);
		string GenerateAccessToken(IEnumerable<Claim> claims);
	}
}
