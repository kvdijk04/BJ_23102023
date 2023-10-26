using BJ.Application.Ultities;
using BJ.Contract.Size;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface ISizeServiceConnection
    {
        public Task<IEnumerable<SizeDto>> GetAllSizes();
        Task<SizeSpecificProductDto> GetSize(Guid productId, int sizeId);
        public Task<PagedViewModel<SizeDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<SizeDto> GetSizeById(int id);
        Task<bool> CreateSize(CreateSizeDto createSizeDto);
        Task<bool> UpdateSize(int id, UpdateSizeDto updateSizeDto);

    }
    public class SizeServiceConnection : BaseApiClient, ISizeServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SizeServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateSize(CreateSizeDto createSizeDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createSizeDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Sizes", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<SizeDto>> GetAllSizes()
        {
            return await GetListAsync<SizeDto>($"/api/Sizes");

        }

        public async Task<PagedViewModel<SizeDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Sizes/paging?PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var size = JsonConvert.DeserializeObject<PagedViewModel<SizeDto>>(body);

            return size;
        }

        public async Task<SizeSpecificProductDto> GetSize(Guid productId, int sizeId)
        {
            return await GetAsync<SizeSpecificProductDto>($"/api/Sizes/{sizeId}/{productId}");

        }

        public async Task<SizeDto> GetSizeById(int id)
        {
            return await GetAsync<SizeDto>($"/api/Sizes/{id}");

        }

        public async Task<bool> UpdateSize(int id, UpdateSizeDto updateSizeDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateSizeDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Sizes/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
