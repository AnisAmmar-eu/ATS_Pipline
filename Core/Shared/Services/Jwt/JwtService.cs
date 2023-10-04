using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Core.Shared.Services.System.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generate a JWT token based on the claims and time
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="time"></param>
        /// <returns>The created token</returns>
        public string GenerateToken(List<Claim> claims, int time = 120)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            // Generate token with claims and secret key
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(time),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public JwtSecurityToken ValidToken(string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,//bool.Parse(_configuration["JWT:ValidateIssuer"]),
					ValidateAudience = false,//bool.Parse(_configuration["JWT:ValidateAudience"]),
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				return (JwtSecurityToken)validatedToken;
			}
			catch
			{
				throw new UnauthorizedAccessException("Unauthorized");
			}
		}
	}
}
