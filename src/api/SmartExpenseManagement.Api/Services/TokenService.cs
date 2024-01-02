using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SmartExpenseManagement.Api.Repository.Entities;

namespace SmartExpenseManagement.Api.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}

public sealed class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var apiKey = this._configuration.GetSection("ApiKey").ToString() ?? string.Empty;
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(apiKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = credentials,
        };
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claimIdentity = new ClaimsIdentity();
        claimIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
        claimIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

        return claimIdentity;
    }
}
