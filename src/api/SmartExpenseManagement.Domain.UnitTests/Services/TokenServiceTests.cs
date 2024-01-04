using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Configuration;
using Moq;
using SmartExpenseManagement.Domain.Entities;
using SmartExpenseManagement.Domain.Services;

namespace SmartExpenseManagement.Domain.UnitTests.Services;

public sealed class TokenServiceTests
{
    private readonly ITokenService _sut;
    private readonly Fixture _fixture;

    public TokenServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string> { { "ApiKey", Guid.NewGuid().ToString() }, };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(initialData: inMemorySettings).Build();
        _sut = new TokenService(configuration);
        _fixture = new Fixture();
    }

    [Fact]
    [Trait(nameof(ITokenService.GenerateToken), "When is a valid user should return a valid token")]
    public void GenerateToken_WhenIsValidUser_ShouldReturnValidToken()
    {
        // Arrange
        User user = _fixture.Create<User>();

        // Act
        var token = _sut.GenerateToken(user);

        // Assert
        using (new AssertionScope())
        {
            token.Should().NotBeNull();
        }
    }
}

