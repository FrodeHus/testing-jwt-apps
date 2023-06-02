using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using FluentAssertions;

namespace SampleApp.E2E.Tests;

public class SampleTests : EndToEndTestCase
{
    [Fact]
    public async Task Should_Reject_Unauthenticated_Requests()
    {
        var response = await Client.GetAsync("/");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Allow_Operators_To_Retrieve_Secrets()
    {
        var token = JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
            new JwtSecurityToken(
                JwtTokenProvider.Issuer,
                JwtTokenProvider.Issuer,
                new List<Claim> { new(ClaimTypes.Role, "Operator"), },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: JwtTokenProvider.SigningCredentials
            )
        );

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync("/admin");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Allow_All_RegisteredUsers()
    {
        var token = new TestJwtToken().WithRole("User").WithUserName("testuser").Build();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync("/");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("Operator")]
    public async Task Should_Allow_Power_Users(string roleName)
    {
        var response = await Client
            .WithJwtBearerToken(token => token.WithRole(roleName))
            .GetAsync("/admin");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Reject_Non_Admins()
    {
        var token = new TestJwtToken().WithRole("User").WithUserName("testuser").Build();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync("/admin");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
