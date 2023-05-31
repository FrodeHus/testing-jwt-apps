using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;

namespace SampleApp.E2E.Tests;

public class SampleTests : EndToEndTestCase
{
    [Fact]
    public async Task Should_Allow_All_RegisteredUsers()
    {
        var token = new JwtTokenProvider.TestJwtToken()
            .WithRole("User")
            .WithUserName("testuser")
            .Build();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync("/");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Allow_Admins()
    {
        var token = new JwtTokenProvider.TestJwtToken()
            .WithRole("Admin")
            .WithUserName("testuser")
            .Build();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync("/admin");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Reject_Non_Admins()
    {
        var token = new JwtTokenProvider.TestJwtToken()
            .WithRole("User")
            .WithUserName("testuser")
            .Build();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync("/admin");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
