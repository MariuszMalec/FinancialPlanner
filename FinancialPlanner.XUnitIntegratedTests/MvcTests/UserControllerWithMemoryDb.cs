using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace FinancialPlanner.XUnitIntegratedTests.MvcTests
{
    public class UserControllerWithMemoryDb : IClassFixture<TestingWebAppFactory<Program>>
    {
        private HttpClient _client;

        WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>();

        public UserControllerWithMemoryDb(TestingWebAppFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:7293/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        [Theory]
        [InlineData("/Home")]
        [InlineData("/User")]
        [InlineData("/Roles")]
        [InlineData("/User/Index")]
        public async Task Get_EndPointsReturns_StatusOk(string url)
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("/Homes")]
        public async Task Get_EndPointsReturns_NotFound(string url)
        {
            var response = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
