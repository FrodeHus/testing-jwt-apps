using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace SampleApp.E2E.Tests;

public static class JwtTokenProvider
{
    public static string Issuer { get; } = "Sample_Auth_Server";
    public static SecurityKey SecurityKey { get; } =
        new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());
    public static SigningCredentials SigningCredentials { get; } =
        new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    internal static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
}
