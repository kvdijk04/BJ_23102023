using BJ.Contract.VisitorCounter;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface IVisitorCounterServiceConnection
    {

        Task<VisitorCounterDto> GetVisitorCounter();

        Task<bool> UpdateVisitorCounter(UpdateVisitorCounterDto updateVisitorCounterDto);

    }
    public class VisitorCounterServiceConnection : BaseApiClient, IVisitorCounterServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VisitorCounterServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<VisitorCounterDto> GetVisitorCounter()
        {
            return await GetAsync<VisitorCounterDto>($"/api/VisitorCounters");

        }



        public async Task<bool> UpdateVisitorCounter(UpdateVisitorCounterDto updateVisitorCounterDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateVisitorCounterDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/VisitorCounters", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
