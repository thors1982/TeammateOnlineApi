using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using TeammateOnlineApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Net;

namespace TeammateOnlineApiTests.UnitTests.Middleware
{
    public class ThrowExceptionMiddlewareTests : IDisposable
    {
        private TestServer server;

        private HttpClient client;

        private int sleepTimer = 1000;

        public ThrowExceptionMiddlewareTests()
        {
            var builder = new WebHostBuilder().Configure(
                app => {
                    app.UseThrowExceptionMiddleware();
                    app.Run(async context =>
                    {
                        await context.Response.WriteAsync("Test response");
                    });
                });

            server = new TestServer(builder);
            client = server.CreateClient();
        }

        [Fact(DisplayName = "UseThrowExceptionMiddleware - Returns Ok")]
        public async void ReturnsOk()
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), "/test-url");
            var responseMessage = await client.SendAsync(requestMessage);

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Fact(DisplayName = "UseThrowExceptionMiddleware - Returns Unauthorized")]
        public async void ReturnsUnauthorized()
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), "/test-url?throw=401");
            var responseMessage = await client.SendAsync(requestMessage);

            Assert.Equal(HttpStatusCode.Unauthorized, responseMessage.StatusCode);
        }

        [Fact(DisplayName = "UseThrowExceptionMiddleware - Returns Internal Server Error")]
        public async void ReturnsInternalServerError()
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), "/test-url?throw=500");
            var responseMessage = await client.SendAsync(requestMessage);

            Assert.Equal(HttpStatusCode.InternalServerError, responseMessage.StatusCode);
        }

        [Fact(DisplayName = "UseThrowExceptionMiddleware - Throws exception")]
        public void ThrowsException()
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), "/test-url?throw=exception");

            var responseMessage = Assert.ThrowsAsync<Exception>(() => client.SendAsync(requestMessage));
        }

        public void Dispose()
        {
            server.Dispose();
            client.Dispose();
        }
    }
}
