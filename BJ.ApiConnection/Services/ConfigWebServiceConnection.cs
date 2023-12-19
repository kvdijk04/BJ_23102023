using BJ.Application.Ultities;
using BJ.Contract.ConfigWeb;
using BJ.Contract.ConfigWeb.CreateConfigWeb;
using BJ.Contract.ConfigWeb.UpdateConfigWeb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface IConfigWebServiceConnection
    {
        public Task<IEnumerable<ConfigWebDto>> GetAllConfigWebs();
        public Task<PagedViewModel<ConfigWebDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<ConfigWebDto> GetConfigWebById(Guid id);
        Task<bool> CreateConfigWeb(CreateConfigWebDto createConfigWebDto);
        Task<bool> UpdateConfigWeb(Guid id, UpdateConfigWebDto updateConfigWebDto);

    }
    public class ConfigWebServiceConnection : BaseApiClient, IConfigWebServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ConfigWebServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateConfigWeb(CreateConfigWebDto createConfigWebDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createConfigWebDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/ConfigWebs", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ConfigWebDto>> GetAllConfigWebs()
        {
            return await GetListAsync<ConfigWebDto>($"/api/ConfigWebs");

        }

        public async Task<IEnumerable<ConfigWebDto>> GetAllConfigWebsByCatId(Guid catId)
        {
            return await GetListAsync<ConfigWebDto>($"/api/ConfigWebs/catId?catId={catId}");
        }

        public async Task<PagedViewModel<ConfigWebDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/ConfigWebs/paging?PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var ConfigWeb = JsonConvert.DeserializeObject<PagedViewModel<ConfigWebDto>>(body);

            return ConfigWeb;
        }


        public async Task<ConfigWebDto> GetConfigWebById(Guid id)
        {
            return await GetAsync<ConfigWebDto>($"/api/ConfigWebs/{id}");

        }

        public async Task<bool> UpdateConfigWeb(Guid id, UpdateConfigWebDto updateConfigWebDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateConfigWebDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/ConfigWebs/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
