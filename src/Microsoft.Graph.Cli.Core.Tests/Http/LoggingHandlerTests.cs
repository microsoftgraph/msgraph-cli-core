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
        var loggerObj = new TestLogger<LoggingHandler>();
        var handler = new LoggingHandler(loggerObj);
        var mockHandler = new Mock<HttpMessageHandler>();
        var responseMsg = new HttpResponseMessage
        {
            Content = new StringContent(""),
            StatusCode = HttpStatusCode.NoContent
        };
        responseMsg.Content.Headers.ContentLength = 0;
        mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMsg);
        handler.InnerHandler = mockHandler.Object;
        var client = new HttpClient(handler);
        var message = new HttpRequestMessage(HttpMethod.Get, "http://example.com");

        var responseMessage = await client.SendAsync(message);

        Assert.Equal(2, loggerObj.Messages.Count);
        Assert.Equal("\nRequest:\n\nGET http://example.com/ HTTP/1.1\n\n\n", loggerObj.Messages[0]);
        Assert.Equal("\nResponse:\n\nHTTP/1.1 204 No Content\nContent-Type: text/plain; charset=utf-8\nContent-Length: 0\n\n\n", loggerObj.Messages[1]);
    }

    [Fact]
    public async Task Logs_Request_And_Response_Messages_With_Content()
    {
        var loggerObj = new TestLogger<LoggingHandler>();
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

        Assert.Equal(2, loggerObj.Messages.Count);
        Assert.Equal("\nRequest:\n\nGET http://example.com/ HTTP/1.1\n\n\n", loggerObj.Messages[0]);
        Assert.Equal("\nResponse:\n\nHTTP/1.1 200 OK\nContent-Type: text/plain; charset=utf-8\nContent-Length: 20\n\nResponse from server\n", loggerObj.Messages[1]);
    }

    [Fact]
    public async Task Logs_Response_For_Stream_Messages_Without_Content_Length()
    {
        var loggerObj = new TestLogger<LoggingHandler>();
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

        Assert.Equal(2, loggerObj.Messages.Count);
        Assert.Contains("Content-Length: 0", loggerObj.Messages[1]);
        Assert.Contains("[...<data stream>...]", loggerObj.Messages[1]);
    }

    [Fact]
    public async Task Logs_Response_Length_For_Stream_Messages_With_Content_Length()
    {
        var loggerObj = new TestLogger<LoggingHandler>();
        var handler = new LoggingHandler(loggerObj);
        var mockHandler = new Mock<HttpMessageHandler>();
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes("Test message"));
        var responseMsg = new HttpResponseMessage()
        {
            Content = new StreamContent(ms),
        };
        responseMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        responseMsg.Content.Headers.ContentLength = ms.Length;

        mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMsg);
        handler.InnerHandler = mockHandler.Object;
        var client = new HttpClient(handler);
        var message = new HttpRequestMessage(HttpMethod.Get, "http://example.com");

        var responseMessage = await client.SendAsync(message);

        Assert.Equal(2, loggerObj.Messages.Count);
        Assert.Contains("Content-Length: 12", loggerObj.Messages[1]);
        Assert.Contains("[...<12 byte data stream>...]", loggerObj.Messages[1]);
    }

    [Fact]
    public async Task Hides_Sensitive_Content_In_Logs()
    {
        var loggerObj = new TestLogger<LoggingHandler>();
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

        Assert.Equal(2, loggerObj.Messages.Count);
        Assert.Contains("Authorization: [PROTECTED]", loggerObj.Messages[0]);
        Assert.DoesNotContain("Secret Content", loggerObj.Messages[0]);
    }

    [Fact]
    public async Task Skips_Logging_If_Log_Level_Is_Disabled()
    {
        var loggerObj = new TestLogger<LoggingHandler>();
        loggerObj.SetLevel(LogLevel.None);
        var handler = new LoggingHandler(loggerObj);
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage());
        handler.InnerHandler = mockHandler.Object;
        var client = new HttpClient(handler);
        var message = new HttpRequestMessage(HttpMethod.Get, "http://example.com");

        var responseMessage = await client.SendAsync(message);

        Assert.Empty(loggerObj.Messages);
    }
}
