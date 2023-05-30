# Testing JWT apps

[![.NET](https://github.com/FrodeHus/testing-jwt-apps/actions/workflows/dotnet.yml/badge.svg)](https://github.com/FrodeHus/testing-jwt-apps/actions/workflows/dotnet.yml)

This is a repo to demonstrate how to easily write E2E tests for apps that have JWT authentication enabled.

Often, this is a pain point for developers, as they need to write a lot of boilerplate code to get the JWT token, and then use it in the tests.
Or they need to:

- write a lot of code to mock the authentication
- use an existing service and deal with secret handling etc

## How this works in broad strokes

All tests that uses authentication inherits from the `EndToEndTestCase` class. This class configures `JwtBearerOptions` to use a randomly generated signing key, and then uses the `JwtSecurityTokenHandler` to generate a token. This token is then used in the `Authorization` header of the request.

It also have a convenience class `TestJwtToken` that provides a fluent interface to adding claims such as `role` and `upn` to the token.

By doing this, we can write tests like this:

```csharp
[Fact]
public async Task Should_Allow_Admin_To_Retrieve_Secret()
{
    // Arrange
    var token = TestJwtToken
        .WithRole("admin")
        .Build();

    // Act
    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var response = await Client.GetAsync("/api/secret");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
```
