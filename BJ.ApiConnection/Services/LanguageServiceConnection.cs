using BJ.Contract.Translation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface ILanguageServiceConnection
    {
        public Task<IEnumerable<LanguageDto>> GetAllLanguages();
        Task<LanguageDto> GetLanguageById(int id);
        Task<bool> CreateLanguage(CreateLanguageDto createLanguageDto);
        Task<bool> UpdateLanguage(int id, UpdateLanguageDto updateLanguageDto);

    }
    public class LanguageServiceConnection : BaseApiClient, ILanguageServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LanguageServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateLanguage(CreateLanguageDto createLanguageDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createLanguageDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Languages", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<LanguageDto>> GetAllLanguages()
        {
            return await GetListAsync<LanguageDto>($"/api/Languages");

        }



        public async Task<LanguageDto> GetLanguageById(int id)
        {
            return await GetAsync<LanguageDto>($"/api/Languages/{id}");

        }

        public async Task<bool> UpdateLanguage(int id, UpdateLanguageDto updateLanguageDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateLanguageDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Languages/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
