using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Shared.Services.System.Jwt
{
    public interface IJwtService
    {
        string GenerateToken(List<Claim> claims, int time = 120);
        JwtSecurityToken ValidToken(string token);

	}
}
