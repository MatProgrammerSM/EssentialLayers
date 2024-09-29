using EssentialLayers.Helpers.Extension;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EssentialLayers.Services.Token
{
	internal class TokenService : ITokenService
	{
		public string GenerateToken(
			string sub, string secretKey, string issuer,
			string audience, string algorithm, DateTime expiration, Claim[] extraClaims = null
		)
		{
			List<Claim> claims =
			[
				new Claim(JwtRegisteredClaimNames.Sub, sub),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			];

			if (extraClaims != null)
			{
				foreach (Claim claim in extraClaims)
				{
					if (claims.NotAny(x => x.Type == claim.Type)) claims.Add(claim);
				}
			}

			SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(secretKey));
			SigningCredentials credentials = new(symmetricSecurityKey, algorithm);

			JwtSecurityToken token = new(
				issuer: issuer,
				audience: audience,
				claims: claims, expires: expiration,
				signingCredentials: credentials
			);

			string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			return tokenString;
		}
	}
}