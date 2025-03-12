using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text.Json;
using UKParliament.CodeTest.Web.Middlewares;
using Xunit;

namespace UKParliament.CodeTest.Tests.Middleware;
public class GlobalExceptionMiddlewareTests
{
    private readonly Mock<ILogger<GlobalExceptionMiddleware>> _loggerMock;
    private readonly GlobalExceptionMiddleware _middleware;

    public GlobalExceptionMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<GlobalExceptionMiddleware>>();
        _middleware = new GlobalExceptionMiddleware(next: async (innerHttpContext) =>
        {
            throw new Exception("Test Exception");
        }, _loggerMock.Object);
    }

    [Fact]
    public async Task Middleware_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var responseStream = new MemoryStream();
        context.Response.Body = responseStream;

        // Act
        await _middleware.Invoke(context);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var jsonResponse = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(jsonResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        // Assert
        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.NotNull(problemDetails);
        Assert.Equal("Internal Server Error", problemDetails!.Title);
        Assert.Equal("An unexpected error occurred while processing your request. Please try again later.", problemDetails.Detail);
    }
}
