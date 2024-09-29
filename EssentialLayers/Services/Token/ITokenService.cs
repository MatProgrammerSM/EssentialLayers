using System;
using System.Security.Claims;

namespace EssentialLayers.Services.Token
{
	public interface ITokenService
	{
		string GenerateToken(
			string sub, string secretKey, string issuer,
			string audience, string algorithm, DateTime expiration, Claim[] extraClaims = null
		);
	}
}