using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(Account account)
    {
        var roleClaims = account.AccountRoles.Select(ar => new Claim(ClaimTypes.Role, ar.Role.Name));
        var claims = new List<Claim>(new[]
        {
            new Claim(ClaimTypes.Sid, account.EmployeeId.ToString())
        });
        claims.AddRange(roleClaims);

        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(_configuration.GetValue<double>("JWT:AccessTokenExpirationInMinutes")),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}