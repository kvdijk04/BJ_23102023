using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.News;
using BJ.Contract.Translation.News;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface INewsServiceConnection
    {
        public Task<IEnumerable<NewsUserViewModel>> GetNewsAtHome(string culture);
        public Task<IEnumerable<NewsUserViewModel>> GetNews(string culture, bool popular, bool promotion);

        public Task<PagedViewModel<NewsDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<PagedViewModel<NewsUserViewModel>> GetPagingNews([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<PagedViewModel<NewsUserViewModel>> GetPagingPromotion([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<NewsUserViewModel> GetNewsById(Guid id, string culture);
        Task<bool> CreateNews(CreateNewsAdminView createNewsAdminView);
        Task<bool> UpdateNews(Guid id, string culture, UpdateNewsAdminView updateNewsAdminView);

        public Task<NewsTranslationDto> GetNewsTranslationnById(Guid id);

        Task<bool> CreateLanguage(CreateNewsTranslationDto createNewsTranslationDto);

        Task<bool> UpdateNewsTranslationn(Guid languageId, UpdateNewsTranslationDto updateNewsTranslationDto);

    }
    public class NewsServiceConnection : BaseApiClient, INewsServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NewsServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateNews(CreateNewsAdminView createNewsAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            createNewsAdminView.CreateNewsTranslation.Alias = Utilities.SEOUrl(createNewsAdminView.CreateNewsTranslation.Title);


            var requestContent = new MultipartFormDataContent();

            if (createNewsAdminView.FileUpload != null)
            {
                byte[] data;
                using (var br = new BinaryReader(createNewsAdminView.FileUpload.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createNewsAdminView.FileUpload.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new(data);

                requestContent.Add(bytes, "createNewsAdminView.FileUpload", createNewsAdminView.FileUpload.FileName);
            }

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createNewsAdminView.CreateNewsTranslation.Title) ? "" : createNewsAdminView.CreateNewsTranslation.Title.ToString()), "createNewsAdminView.CreateNewsTranslation.Title");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createNewsAdminView.CreateNewsTranslation.ShortDesc) ? "" : createNewsAdminView.CreateNewsTranslation.ShortDesc.ToString()), "createNewsAdminView.CreateNewsTranslation.shortDesc");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createNewsAdminView.CreateNewsTranslation.Description) ? "" : createNewsAdminView.CreateNewsTranslation.Description.ToString()), "createNewsAdminView.CreateNewsTranslation.description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createNewsAdminView.CreateNews.DateUpdated.ToString()) ? "" : createNewsAdminView.CreateNews.DateUpdated.ToString()), "createNewsAdminView.CreateNews.DateUpdated");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createNewsAdminView.CreateNews.Active.ToString()) ? "" : createNewsAdminView.CreateNews.Active.ToString()), "createNewsAdminView.CreateNews.active");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createNewsAdminView.CreateNews.Popular.ToString()) ? "" : createNewsAdminView.CreateNews.Popular.ToString()), "createNewsAdminView.CreateNews.popular");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createNewsAdminView.CreateNews.Home.ToString()) ? "" : createNewsAdminView.CreateNews.Home.ToString()), "createNewsAdminView.CreateNews.Home");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createNewsAdminView.CreateNews.Promotion.ToString()) ? "" : createNewsAdminView.CreateNews.Promotion.ToString()), "createNewsAdminView.CreateNews.promotion");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createNewsAdminView.CreateNews.DateCreated.ToString()) ? "" : createNewsAdminView.CreateNews.DateCreated.ToString()), "createNewsAdminView.CreateNews.DateCreated");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createNewsAdminView.CreateNewsTranslation.Alias) ? "" : createNewsAdminView.CreateNewsTranslation.Alias.ToString()), "createNewsAdminView.CreateNewsTranslation.alias");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createNewsAdminView.CreateNewsTranslation.MetaDesc.ToString()) ? "" : createNewsAdminView.CreateNewsTranslation.MetaDesc.ToString()), "createNewsAdminView.CreateNewsTranslation.metaDesc");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createNewsAdminView.CreateNewsTranslation.MetaKey.ToString()) ? "" : createNewsAdminView.CreateNewsTranslation.MetaKey.ToString()), "createNewsAdminView.CreateNewsTranslation.metaKey");

            var response = await client.PostAsync($"/api/Newss/", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<PagedViewModel<NewsDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Newss/paging?PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageNews={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var news = JsonConvert.DeserializeObject<PagedViewModel<NewsDto>>(body);

            return news;
        }


        public async Task<NewsUserViewModel> GetNewsById(Guid id, string culture)
        {
            return await GetAsync<NewsUserViewModel>($"/api/Newss/{id}/language/{culture}");

        }

        public async Task<bool> UpdateNews(Guid id, string culture, UpdateNewsAdminView updateNewsAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

            if (updateNewsAdminView.FileUpload != null)
            {
                byte[] data;
                using (var br = new BinaryReader(updateNewsAdminView.FileUpload.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateNewsAdminView.FileUpload.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new(data);

                requestContent.Add(bytes, "updateNewsAdminView.FileUpload", updateNewsAdminView.FileUpload.FileName);
            }
            if (updateNewsAdminView.FileUpload == null)
            {
                requestContent.Add(new StringContent(string.IsNullOrEmpty(updateNewsAdminView.UpdateNews.ImagePath) ? "" : updateNewsAdminView.UpdateNews.ImagePath.ToString()), "updateNewsAdminView.UpdateNews.ImagePath");

            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateNewsAdminView.UpdateNewsTranslation.Title) ? "" : updateNewsAdminView.UpdateNewsTranslation.Title.ToString()), "updateNewsAdminView.UpdateNewsTranslation.Title");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateNewsAdminView.UpdateNewsTranslation.ShortDesc) ? "" : updateNewsAdminView.UpdateNewsTranslation.ShortDesc.ToString()), "updateNewsAdminView.UpdateNewsTranslation.shortDesc");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateNewsAdminView.UpdateNewsTranslation.Description) ? "" : updateNewsAdminView.UpdateNewsTranslation.Description.ToString()), "updateNewsAdminView.UpdateNewsTranslation.description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateNewsAdminView.UpdateNews.DateUpdated.ToString()) ? "" : updateNewsAdminView.UpdateNews.DateUpdated.ToString()), "updateNewsAdminView.UpdateNews.DateUpdated");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateNewsAdminView.UpdateNews.Active.ToString()) ? "" : updateNewsAdminView.UpdateNews.Active.ToString()), "updateNewsAdminView.UpdateNews.active");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateNewsAdminView.UpdateNews.Popular.ToString()) ? "" : updateNewsAdminView.UpdateNews.Popular.ToString()), "updateNewsAdminView.UpdateNews.popular");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateNewsAdminView.UpdateNews.Home.ToString()) ? "" : updateNewsAdminView.UpdateNews.Home.ToString()), "updateNewsAdminView.UpdateNews.Home");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateNewsAdminView.UpdateNews.Promotion.ToString()) ? "" : updateNewsAdminView.UpdateNews.Promotion.ToString()), "updateNewsAdminView.UpdateNews.promotion");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateNewsAdminView.UpdateNewsTranslation.MetaDesc.ToString()) ? "" : updateNewsAdminView.UpdateNewsTranslation.MetaDesc.ToString()), "updateNewsAdminView.UpdateNewsTranslation.metaDesc");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateNewsAdminView.UpdateNewsTranslation.MetaKey.ToString()) ? "" : updateNewsAdminView.UpdateNewsTranslation.MetaKey.ToString()), "updateNewsAdminView.UpdateNewsTranslation.metaKey");

            var response = await client.PutAsync($"/api/Newss/{id}?culture={culture}", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<NewsTranslationDto> GetNewsTranslationnById(Guid id)
        {
            return await GetAsync<NewsTranslationDto>($"/api/Newss/language/{id}");
        }

        public async Task<bool> CreateLanguage(CreateNewsTranslationDto createNewsTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createNewsTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Newss/language/create", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateNewsTranslationn(Guid languageId, UpdateNewsTranslationDto updateNewsTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateNewsTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Newss/language/{languageId}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<NewsUserViewModel>> GetNewsAtHome(string culture)
        {
            return await GetListAsync<NewsUserViewModel>($"/api/Newss/homepage?culture={culture}");
        }

        public async Task<IEnumerable<NewsUserViewModel>> GetNews(string culture, bool popular, bool promotion)
        {
            return await GetListAsync<NewsUserViewModel>($"/api/Newss?culture={culture}&popular={popular}&promotion={promotion}");
        }

        public async Task<PagedViewModel<NewsUserViewModel>> GetPagingNews([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Newss/newspaging?LanguageId={getListPagingRequest.LanguageId}&PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageNews={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var news = JsonConvert.DeserializeObject<PagedViewModel<NewsUserViewModel>>(body);

            return news;
        }

        public async Task<PagedViewModel<NewsUserViewModel>> GetPagingPromotion([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Newss/promotionpaging?LanguageId={getListPagingRequest.LanguageId}&PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageNews={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var news = JsonConvert.DeserializeObject<PagedViewModel<NewsUserViewModel>>(body);

            return news;
        }
    }
}
