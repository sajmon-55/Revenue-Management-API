using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication.JWT;

public class JwtAccessTokenService(IOptions<JwtOptions> options) : IAccessTokenService
{
    private readonly JwtOptions _jwtOptions = options.Value;
    
    public string GenerateAccessToken(AccessTokenDescriptor accessTokenDescriptor)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, accessTokenDescriptor.UserId),
            new Claim(ClaimTypes.Name, accessTokenDescriptor.Login),
            new Claim(ClaimTypes.Role, accessTokenDescriptor.Role)
        };
        
        var symmertricSecurityKey = _jwtOptions.SymmetricSecurityKey
            ?? throw new ArgumentException("Missing JWT:SymmetricSecurityKey");
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmertricSecurityKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingCredentials,
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(securityTokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
