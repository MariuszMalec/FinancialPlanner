using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
        [InlineData("/User/Index")]
        public async Task Get_Index_EndPointsReturns_Response(string url)
        {
            var response = await _client.GetAsync(url);

            //var content = await response.Content.ReadAsStringAsync();

            //var users = JsonConvert.DeserializeObject<List<UserDto>>(content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
