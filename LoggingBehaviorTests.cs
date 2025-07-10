using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using TELERADIOLOGY.Application.Behaviors;
using MediatR;

public class DummyRequest : IRequest<string> { }

public class LoggingBehaviorTests
{
    private readonly Mock<ILogger<LoggingBehavior<DummyRequest, string>>> _loggerMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

    public LoggingBehaviorTests()
    {
        _loggerMock = new Mock<ILogger<LoggingBehavior<DummyRequest, string>>>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
    }

    [Fact]
    public async Task Handle_ShouldLogInformation_WhenRequestIsHandledSuccessfully()
    {
       
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "123"),
            new Claim(ClaimTypes.Name, "John Doe")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = user };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
        var behavior = new LoggingBehavior<DummyRequest, string>(_loggerMock.Object, _httpContextAccessorMock.Object);
        var request = new DummyRequest();
        RequestHandlerDelegate<string> next = () => Task.FromResult("Success");
        var result = await behavior.Handle(request, next, CancellationToken.None);
        result.Should().Be("Success");

        _loggerMock.Verify(log => log.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString().Contains("Handling DummyRequest")
                && state.ToString().Contains("123")
            ),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);

        _loggerMock.Verify(log => log.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString().Contains("Handled DummyRequest successfully")
                && state.ToString().Contains("John Doe")
            ),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldLogError_WhenExceptionIsThrown()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "456"),
            new Claim(ClaimTypes.Name, "Jane Smith")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = user };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var behavior = new LoggingBehavior<DummyRequest, string>(_loggerMock.Object, _httpContextAccessorMock.Object);
        var request = new DummyRequest();
        RequestHandlerDelegate<string> next = () => throw new InvalidOperationException("Something failed");
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            behavior.Handle(request, next, CancellationToken.None));

        exception.Message.Should().Be("Something failed");

        _loggerMock.Verify(log => log.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString().Contains("Error handling DummyRequest")
                && state.ToString().Contains("456")
            ),
            It.Is<Exception>(ex => ex.Message == "Something failed"),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldLogAnonymous_WhenUserNotAuthenticated()
    {
        var identity = new ClaimsIdentity(); 
        var user = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = user };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var behavior = new LoggingBehavior<DummyRequest, string>(_loggerMock.Object, _httpContextAccessorMock.Object);
        var request = new DummyRequest();
        RequestHandlerDelegate<string> next = () => Task.FromResult("AnonSuccess");
        var result = await behavior.Handle(request, next, CancellationToken.None);
        result.Should().Be("AnonSuccess");

        _loggerMock.Verify(log => log.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString().Contains("Handling DummyRequest")
                && state.ToString().Contains("Anonymous")
            ),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);
    }
}
