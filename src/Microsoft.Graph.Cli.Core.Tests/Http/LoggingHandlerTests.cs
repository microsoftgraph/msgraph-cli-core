using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Cli.Core.Http;
using Moq;
using Moq.Protected;
using Xunit;

namespace Microsoft.Graph.Cli.Core.Tests.Http;

public class LoggingHandlerTests
{
    [Fact]
    public async Task Logs_Request_And_Response_Messages_Empty_Response()
    {
        string? request = null;
        string? response = null;
        var calls = 0;
        var loggerObj = SetUpLoggerWithCallBack<LoggingHandler>((s) =>
        {
            if (calls == 0) request = s;
            else if (calls == 1) response = s;
            calls++;
        });
        var handler = new LoggingHandler(loggerObj);
        var mockHandler = new Mock<HttpMessageHandler>();
        var responseMsg = new HttpResponseMessage()
        {
            Content = new StringContent("")
        };
        responseMsg.StatusCode = HttpStatusCode.NoContent;
        mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMsg);
        handler.InnerHandler = mockHandler.Object;
        var client = new HttpClient(handler);
        var message = new HttpRequestMessage(HttpMethod.Get, "http://example.com");

        var responseMessage = await client.SendAsync(message);

        Assert.NotNull(request);
        Assert.NotNull(response);
        Assert.Equal("\nRequest:\n\nGET http://example.com/ HTTP/1.1\n\n\n", request);
        Assert.Equal("\nResponse:\n\nHTTP/1.1 204 No Content\nContent-Type: text/plain; charset=utf-8\nContent-Length: 0\n\n\n", response);
    }

    [Fact]
    public async Task Logs_Request_And_Response_Messages_With_Content()
    {
        string? request = null;
        string? response = null;
        var calls = 0;
        var loggerObj = SetUpLoggerWithCallBack<LoggingHandler>((s) =>
        {
            if (calls == 0) request = s;
            else if (calls == 1) response = s;
            calls++;
        });
        var handler = new LoggingHandler(loggerObj);
        var mockHandler = new Mock<HttpMessageHandler>();
        const string resp = "Response from server";
        var responseMsg = new HttpResponseMessage()
        {
            Content = new StringContent(resp)
        };
        responseMsg.Content.Headers.ContentLength = resp.Length;
        mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMsg);
        handler.InnerHandler = mockHandler.Object;
        var client = new HttpClient(handler);
        var message = new HttpRequestMessage(HttpMethod.Get, "http://example.com");

        var responseMessage = await client.SendAsync(message);

        Assert.NotNull(request);
        Assert.NotNull(response);
        Assert.Equal("\nRequest:\n\nGET http://example.com/ HTTP/1.1\n\n\n", request);
        Assert.Equal("\nResponse:\n\nHTTP/1.1 200 OK\nContent-Type: text/plain; charset=utf-8\nContent-Length: 20\n\nResponse from server\n", response);
    }

    [Fact]
    public async Task Logs_Response_For_Stream_Messages_Without_Content_Length()
    {
        string? request = null;
        string? response = null;
        var calls = 0;
        var loggerObj = SetUpLoggerWithCallBack<LoggingHandler>((s) =>
        {
            if (calls == 0) request = s;
            else if (calls == 1) response = s;
            calls++;
        });
        var handler = new LoggingHandler(loggerObj);
        var mockHandler = new Mock<HttpMessageHandler>();
        var responseMsg = new HttpResponseMessage()
        {
            Content = new StreamContent(Stream.Null)
        };
        responseMsg.Content.Headers.ContentLength = 0;
        responseMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMsg);
        handler.InnerHandler = mockHandler.Object;
        var client = new HttpClient(handler);
        var message = new HttpRequestMessage(HttpMethod.Get, "http://example.com");

        var responseMessage = await client.SendAsync(message);

        Assert.NotNull(request);
        Assert.NotNull(response);
        Assert.Contains("Content-Length: 0", response);
        Assert.Contains("[...<data stream>...]", response);
    }

    [Fact]
    public async Task Logs_Response_Length_For_Stream_Messages_With_Content_Length()
    {
        string? request = null;
        string? response = null;
        var calls = 0;
        var loggerObj = SetUpLoggerWithCallBack<LoggingHandler>((s) =>
        {
            if (calls == 0) request = s;
            else if (calls == 1) response = s;
            calls++;
        });
        var handler = new LoggingHandler(loggerObj);
        var mockHandler = new Mock<HttpMessageHandler>();
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes("Test message"));
        var responseMsg = new HttpResponseMessage()
        {
            Content = new StreamContent(ms)
        };
        responseMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMsg);
        handler.InnerHandler = mockHandler.Object;
        var client = new HttpClient(handler);
        var message = new HttpRequestMessage(HttpMethod.Get, "http://example.com");

        var responseMessage = await client.SendAsync(message);

        Assert.NotNull(request);
        Assert.NotNull(response);
        Assert.Contains("Content-Length: 12", response);
        Assert.Contains("[...<12 byte data stream>...]", response);
    }

    [Fact]
    public async Task Hides_Sensitive_Content_In_Logs()
    {
        string? request = null;
        string? response = null;
        var calls = 0;
        var loggerObj = SetUpLoggerWithCallBack<LoggingHandler>((s) =>
        {
            if (calls == 0) request = s;
            else if (calls == 1) response = s;
            calls++;
        });
        var handler = new LoggingHandler(loggerObj);
        var mockHandler = new Mock<HttpMessageHandler>();
        const string resp = "Response from server";
        var responseMsg = new HttpResponseMessage()
        {
            Content = new StringContent(resp)
        };
        responseMsg.Content.Headers.ContentLength = resp.Length;
        mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMsg);
        handler.InnerHandler = mockHandler.Object;
        var client = new HttpClient(handler);
        var message = new HttpRequestMessage(HttpMethod.Get, "http://example.com");
        message.Headers.Authorization = new AuthenticationHeaderValue("bearer", "Secret Content");

        var responseMessage = await client.SendAsync(message);

        Assert.NotNull(request);
        Assert.NotNull(response);
        Assert.Contains("Authorization: [PROTECTED]", request);
        Assert.DoesNotContain("Secret Content", request);
    }

    [Fact]
    public async Task Skips_Logging_If_Log_Level_Is_Disabled()
    {
        string? request = null;
        string? response = null;
        var calls = 0;
        var loggerObj = SetUpLoggerWithCallBack<LoggingHandler>((s) =>
        {
            if (calls == 0) request = s;
            else if (calls == 1) response = s;
            calls++;
        }, LogLevel.None);

        var handler = new LoggingHandler(loggerObj);
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage());
        handler.InnerHandler = mockHandler.Object;
        var client = new HttpClient(handler);
        var message = new HttpRequestMessage(HttpMethod.Get, "http://example.com");

        var responseMessage = await client.SendAsync(message);

        Assert.Null(request);
        Assert.Null(response);
        Assert.Equal(0, calls);
    }

    private ILogger<T> SetUpLoggerWithCallBack<T>(Action<string> callback, LogLevel logLevel = LogLevel.Trace)
    {
        var logger = new Mock<ILogger<T>>();
        logger.Setup(l => l.IsEnabled(It.IsAny<LogLevel>())).Returns<LogLevel>(ll => ll >= logLevel);

        logger.Setup(l => l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
                ))
                .Callback((IInvocation invocation) =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var state = invocation.Arguments[2];
                    var exception = (Exception)invocation.Arguments[3];
                    var formatter = invocation.Arguments[4];

                    var invokeMethod = formatter.GetType().GetMethod("Invoke");
                    var s = ((string?)invokeMethod?.Invoke(formatter, new[] { state, exception })) ?? string.Empty;
                    callback(s);
                });
        return logger.Object;
    }
}
