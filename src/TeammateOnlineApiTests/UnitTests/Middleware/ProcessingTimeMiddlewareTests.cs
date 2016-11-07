using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using TeammateOnlineApi.Database;
using TeammateOnlineApi.Database.Repositories;
using Xunit;
using TeammateOnlineApi.Models;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using TeammateOnlineApi;
using System.Net.Http;
using TeammateOnlineApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace TeammateOnlineApiTests.UnitTests.Middleware
{
    public class ProcessingTimeMiddlewareTests : IDisposable
    {
        private TestServer server;

        private HttpClient client;

        private int sleepTimer = 1000;

        public ProcessingTimeMiddlewareTests()
        {
            var builder = new WebHostBuilder().Configure(
                app => {
                    app.UseProcessingTimeMiddleware();
                    app.Run(async context =>
                    {
                        Thread.Sleep(sleepTimer);
                        await context.Response.WriteAsync("Test response");
                    });
                });

            server = new TestServer(builder);
            client = server.CreateClient();
        }

        [Fact(DisplayName = "ProcessingTimeMiddleware - Check header greater than sleep")]
        public async void AddProcessingTimeHeader_Returns200()
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), "/test-processing-time/");
            var responseMessage = await client.SendAsync(requestMessage);
            var processingTimeHeader = responseMessage.Headers.FirstOrDefault(h => h.Key == "X-ProcessingTime");

            Assert.Equal("X-ProcessingTime", processingTimeHeader.Key);

            var processingTimeValue = Int32.Parse(processingTimeHeader.Value.FirstOrDefault().Replace(" ms", ""));

            Assert.True(processingTimeValue > sleepTimer);
        }

        public void Dispose()
        {
            server.Dispose();
            client.Dispose();
        }
    }
}
