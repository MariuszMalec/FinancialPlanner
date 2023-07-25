using FinancialPlanner.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using Moq;
using System.Net;

namespace FinancialPlanner.XUnitIntegratedTests.MvcTests
{
    public class UserControllerTests : IClassFixture<FinancialWebAppFactory<Program>>
    {
        private HttpClient _client;

        WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>();

        public UserControllerTests(FinancialWebAppFactory<Program> factory)
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
        [InlineData("/Transactions")]
        [InlineData("/TransactionUser")]
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

        [Theory]
        [InlineData("/User/Details/a200b9f3-d139-4fa6-982f-0ad4f388b485")]
        public async Task Get_Details_EndPointsReturns_StatusBadRequest(string url)
        {
            var response = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }   
    }
}
