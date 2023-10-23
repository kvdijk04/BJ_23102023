using BJ.Contract.Size;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface IConfigProductServiceConnection
    {
        Task<bool> CreateSizeSpecificProduct(CreateSizeSpecificProductDto createSizeSpecificProductDto);
        Task<bool> UpdateSpecificProduct(Guid id, UpdateSizeSpecificProductDto updateSizeSpecificProductDto);
        Task<bool> CreateConfigProduct(ConfigProduct configProduct);

    }
    public class ConfigProductServiceConnection : IConfigProductServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ConfigProductServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateConfigProduct(ConfigProduct configProduct)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(configProduct);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/ConfigProducts/product/subcategory/sizespecific/nutrition", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateSizeSpecificProduct(CreateSizeSpecificProductDto createSizeSpecificProductDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createSizeSpecificProductDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/ConfigProducts/sizespecific/nutrition", httpContent);

            return response.IsSuccessStatusCode;

        }

        public async Task<bool> UpdateSpecificProduct(Guid id, UpdateSizeSpecificProductDto updateSizeSpecificProductDto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(updateSizeSpecificProductDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/ConfigProducts/sizespecific/{id}/nutrition", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
