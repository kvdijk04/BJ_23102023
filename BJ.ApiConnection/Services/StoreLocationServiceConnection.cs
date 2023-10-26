using BJ.Contract.StoreLocation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface IStoreLocationServiceConnection
    {
        public Task<IEnumerable<StoreLocationDto>> GetAllStoreLocations();
        Task<bool> CreateStoreLocation(CreateStoreLocationDto createStoreLocationDto);

    }
    public class StoreLocationServiceConnection : BaseApiClient, IStoreLocationServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoreLocationServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateStoreLocation(CreateStoreLocationDto createStoreLocationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createStoreLocationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/StoreLocations", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<StoreLocationDto>> GetAllStoreLocations()
        {
            return await GetListAsync<StoreLocationDto>($"/api/StoreLocations");

        }

    }
}
