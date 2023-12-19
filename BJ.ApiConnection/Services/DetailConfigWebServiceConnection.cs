using BJ.Application.Ultities;
using BJ.Contract;
using BJ.Contract.ConfigWeb;
using BJ.Contract.ConfigWeb.CreateConfigWeb;
using BJ.Contract.ConfigWeb.UpdateConfigWeb;
using BJ.Contract.Translation.Blog;
using BJ.Contract.Translation.ConfigWeb;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface IDetailConfigWebServiceConnection
    {
        public Task<IEnumerable<ConfigWebViewModel>> GetAllDetailConfigWebs(string culture);
        public Task<PagedViewModel<ConfigWebViewModel>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<ConfigWebViewModel> GetDetailConfigWebById(Guid id,string culture);
        Task<ConfigWebViewModel> GetDetailConfigWebByUrl(string url, string culture);

        Task<string> CreateDetailConfigWeb(CreateConfigWebAdminView createDetailConfigWebDto);
        Task<string> UpdateDetailConfigWeb(Guid id, UpdateDetailConfigWebDto updateDetailConfigWebDto);

        public Task<DetailConfigWebTranslationDto> GetDetailConfigWebTranslationnById(Guid id);

        Task<bool> CreateLanguage(CreateDetailConfigWebTranslationDto createDetailConfigWebTranslationDto);

        Task<bool> UpdateDetailConfigWebTranslationn(Guid languageId, UpdateDetailConfigWebTranslationDto updateDetailConfigWebTranslationDto);


    }
    public class DetailConfigWebServiceConnection : BaseApiClient, IDetailConfigWebServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DetailConfigWebServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> CreateDetailConfigWeb(CreateConfigWebAdminView createDetailConfigWebDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createDetailConfigWebDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/DetailConfigWebs", httpContent);

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        public async Task<IEnumerable<ConfigWebViewModel>> GetAllDetailConfigWebs(string culture)
        {
            return await GetListAsync<ConfigWebViewModel>($"/api/DetailConfigWebs?culture={culture}");

        }

        public async Task<IEnumerable<DetailConfigWebDto>> GetAllDetailConfigWebsByCatId(Guid catId)
        {
            return await GetListAsync<DetailConfigWebDto>($"/api/DetailConfigWebs/catId?catId={catId}");
        }

        public async Task<PagedViewModel<ConfigWebViewModel>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/DetailConfigWebs/paging?PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}&categoryId={getListPagingRequest.CategoryId}");

            var body = await response.Content.ReadAsStringAsync();

            var DetailConfigWeb = JsonConvert.DeserializeObject<PagedViewModel<ConfigWebViewModel>>(body);

            return DetailConfigWeb;
        }


        public async Task<ConfigWebViewModel> GetDetailConfigWebById(Guid id, string culture)
        {
            return await GetAsync<ConfigWebViewModel>($"/api/DetailConfigWebs/{id}?culture={culture}");

        }

        public async Task<string> UpdateDetailConfigWeb(Guid id, UpdateDetailConfigWebDto updateDetailConfigWebDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateDetailConfigWebDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/DetailConfigWebs/{id}", httpContent);

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        public async Task<ConfigWebViewModel> GetDetailConfigWebByUrl(string url, string culture)
        {
            return await GetAsync<ConfigWebViewModel>($"/api/DetailConfigWebs/url?url={url}&culture={culture}");
        }

        public async  Task<DetailConfigWebTranslationDto> GetDetailConfigWebTranslationnById(Guid id)
        {
            return await GetAsync<DetailConfigWebTranslationDto>($"/api/DetailConfigWebs/language/{id}");
        }

        public async Task<bool> CreateLanguage(CreateDetailConfigWebTranslationDto createDetailConfigWebTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createDetailConfigWebTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/DetailConfigWebs/language/create", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDetailConfigWebTranslationn(Guid languageId, UpdateDetailConfigWebTranslationDto updateDetailConfigWebTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateDetailConfigWebTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/DetailConfigWebs/language/{languageId}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
