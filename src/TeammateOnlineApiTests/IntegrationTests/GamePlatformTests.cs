using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TeammateOnlineApi;
using Xunit;
using Newtonsoft.Json;

namespace TeammateOnlineApiTests.IntegrationTests
{
    public class GamePlatformTests
    {
        private readonly TestServer server;
        private readonly HttpClient client;

        public GamePlatformTests()
        {
            var currentPath = PlatformServices.Default.Application.ApplicationBasePath;
            var applicationPath = Path.GetFullPath(Path.Combine(currentPath, "../../../../TeammateOnlineApi"));

            server = new TestServer(new WebHostBuilder().UseContentRoot(applicationPath).UseEnvironment("Testing").UseStartup<Startup>());
            client = server.CreateClient();
        }

        [Fact(DisplayName = "Integration Test: Get Game Platform Collection", Skip = "Not fully working yet")]
        public async Task GetCollection()
        {
            var request = "/api/gameplatforms";
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();

            var responseString = response.Content.ReadAsStringAsync();

            Assert.Equal(null, responseString.Exception);

            //var test = JsonConvert.DeserializeObject<List<GamePlatformTests>>(responseString.Result);
        }
    }
}
