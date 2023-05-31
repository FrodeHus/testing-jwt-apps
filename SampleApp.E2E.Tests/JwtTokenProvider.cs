using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SampleApp.E2E.Tests;

public static class JwtTokenProvider
{
    public static string Issuer { get; } = "Sample_Auth_Server";
    public static SecurityKey SecurityKey { get; } =
        new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());
    public static SigningCredentials SigningCredentials { get; } =
        new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    private static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();

    public static TestJwtToken WithRole(this TestJwtToken token, string roleName)
    {
        token.Claims.Add(new Claim(ClaimTypes.Role, roleName));
        return token;
    }

    public static TestJwtToken WithUserName(this TestJwtToken token, string username)
    {
        token.Claims.Add(new Claim(ClaimTypes.Upn, username));
        return token;
    }

    public static TestJwtToken WithEmail(this TestJwtToken token, string email)
    {
        token.Claims.Add(new Claim(ClaimTypes.Email, email));
        return token;
    }

    public static string Build(this TestJwtToken tokenData)
    {
        var token = new JwtSecurityToken(
            Issuer,
            Issuer,
            tokenData.Claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: SigningCredentials
        );
        return JwtSecurityTokenHandler.WriteToken(token);
    }

    public class TestJwtToken
    {
        public List<Claim> Claims { get; } = new();
    }
}
