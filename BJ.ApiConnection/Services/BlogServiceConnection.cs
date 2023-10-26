using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.Blog;
using BJ.Contract.Translation.Blog;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace BJ.ApiConnection.Services
{
    public interface IBlogServiceConnection
    {
        public Task<IEnumerable<BlogUserViewModel>> GetAllBlogs(string culture, bool popular);
        public Task<PagedViewModel<BlogDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<BlogUserViewModel> GetBlogById(Guid id, string culture);
        Task<bool> CreateBlog(CreateBlogAdminView createBlogAdminView);
        Task<bool> UpdateBlog(Guid id, string culture, UpdateBlogAdminView updateBlogAdminView);

        public Task<BlogTranslationDto> GetBlogTranslationnById(Guid id);

        Task<bool> CreateLanguage(CreateBlogTranslationDto createBlogTranslationDto);

        Task<bool> UpdateBlogTranslationn(Guid languageId, UpdateBlogTranslationDto updateBlogTranslationDto);

    }
    public class BlogServiceConnection : BaseApiClient, IBlogServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BlogServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateBlog(CreateBlogAdminView createBlogAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            createBlogAdminView.CreateBlogTranslation.Alias = Utilities.SEOUrl(createBlogAdminView.CreateBlogTranslation.Title);


            var requestContent = new MultipartFormDataContent();

            if (createBlogAdminView.FileUpload != null)
            {
                byte[] data;
                using (var br = new BinaryReader(createBlogAdminView.FileUpload.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createBlogAdminView.FileUpload.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new(data);

                requestContent.Add(bytes, "createBlogAdminView.FileUpload", createBlogAdminView.FileUpload.FileName);
            }

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createBlogAdminView.CreateBlogTranslation.Title) ? "" : createBlogAdminView.CreateBlogTranslation.Title.ToString()), "createBlogAdminView.CreateBlogTranslation.Title");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createBlogAdminView.CreateBlogTranslation.ShortDesc) ? "" : createBlogAdminView.CreateBlogTranslation.ShortDesc.ToString()), "createBlogAdminView.CreateBlogTranslation.shortDesc");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createBlogAdminView.CreateBlogTranslation.Description) ? "" : createBlogAdminView.CreateBlogTranslation.Description.ToString()), "createBlogAdminView.CreateBlogTranslation.description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createBlogAdminView.CreateBlog.DateUpdated.ToString()) ? "" : createBlogAdminView.CreateBlog.DateUpdated.ToString()), "createBlogAdminView.CreateBlog.DateUpdated");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createBlogAdminView.CreateBlog.Active.ToString()) ? "" : createBlogAdminView.CreateBlog.Active.ToString()), "createBlogAdminView.CreateBlog.active");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createBlogAdminView.CreateBlog.Popular.ToString()) ? "" : createBlogAdminView.CreateBlog.Popular.ToString()), "createBlogAdminView.CreateBlog.popular");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createBlogAdminView.CreateBlog.DateCreated.ToString()) ? "" : createBlogAdminView.CreateBlog.DateCreated.ToString()), "createBlogAdminView.CreateBlog.DateCreated");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createBlogAdminView.CreateBlogTranslation.Alias) ? "" : createBlogAdminView.CreateBlogTranslation.Alias.ToString()), "createBlogAdminView.CreateBlogTranslation.alias");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createBlogAdminView.CreateBlogTranslation.MetaDesc.ToString()) ? "" : createBlogAdminView.CreateBlogTranslation.MetaDesc.ToString()), "createBlogAdminView.CreateBlogTranslation.metaDesc");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createBlogAdminView.CreateBlogTranslation.MetaKey.ToString()) ? "" : createBlogAdminView.CreateBlogTranslation.MetaKey.ToString()), "createBlogAdminView.CreateBlogTranslation.metaKey");

            var response = await client.PostAsync($"/api/Blogs/", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<BlogUserViewModel>> GetAllBlogs(string culture, bool popular)
        {
            return await GetListAsync<BlogUserViewModel>($"/api/Blogs?culture={culture}&popular={popular}");

        }

        public async Task<PagedViewModel<BlogDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Blogs/paging?PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageBlog={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var size = JsonConvert.DeserializeObject<PagedViewModel<BlogDto>>(body);

            return size;
        }


        public async Task<BlogUserViewModel> GetBlogById(Guid id, string culture)
        {
            return await GetAsync<BlogUserViewModel>($"/api/Blogs/{id}/language/{culture}");

        }

        public async Task<bool> UpdateBlog(Guid id,string culture, UpdateBlogAdminView updateBlogAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

            if (updateBlogAdminView.FileUpload != null)
            {
                byte[] data;
                using (var br = new BinaryReader(updateBlogAdminView.FileUpload.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateBlogAdminView.FileUpload.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new(data);

                requestContent.Add(bytes, "updateBlogAdminView.FileUpload", updateBlogAdminView.FileUpload.FileName);
            }
            if(updateBlogAdminView.FileUpload == null) 
            {
                requestContent.Add(new StringContent(string.IsNullOrEmpty(updateBlogAdminView.UpdateBlog.ImagePath) ? "" : updateBlogAdminView.UpdateBlog.ImagePath.ToString()), "updateBlogAdminView.UpdateBlog.ImagePath");

            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateBlogAdminView.UpdateBlogTranslation.Title) ? "" : updateBlogAdminView.UpdateBlogTranslation.Title.ToString()), "updateBlogAdminView.UpdateBlogTranslation.Title");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateBlogAdminView.UpdateBlogTranslation.ShortDesc) ? "" : updateBlogAdminView.UpdateBlogTranslation.ShortDesc.ToString()), "updateBlogAdminView.UpdateBlogTranslation.shortDesc");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateBlogAdminView.UpdateBlogTranslation.Description) ? "" : updateBlogAdminView.UpdateBlogTranslation.Description.ToString()), "updateBlogAdminView.UpdateBlogTranslation.description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateBlogAdminView.UpdateBlog.DateUpdated.ToString()) ? "" : updateBlogAdminView.UpdateBlog.DateUpdated.ToString()), "updateBlogAdminView.UpdateBlog.DateUpdated");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateBlogAdminView.UpdateBlog.Active.ToString()) ? "" : updateBlogAdminView.UpdateBlog.Active.ToString()), "updateBlogAdminView.UpdateBlog.active");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateBlogAdminView.UpdateBlog.Popular.ToString()) ? "" : updateBlogAdminView.UpdateBlog.Popular.ToString()), "updateBlogAdminView.UpdateBlog.popular");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateBlogAdminView.UpdateBlogTranslation.MetaDesc.ToString()) ? "" : updateBlogAdminView.UpdateBlogTranslation.MetaDesc.ToString()), "updateBlogAdminView.UpdateBlogTranslation.metaDesc");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateBlogAdminView.UpdateBlogTranslation.MetaKey.ToString()) ? "" : updateBlogAdminView.UpdateBlogTranslation.MetaKey.ToString()), "updateBlogAdminView.UpdateBlogTranslation.metaKey");

            var response = await client.PutAsync($"/api/Blogs/{id}?culture={culture}", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<BlogTranslationDto> GetBlogTranslationnById(Guid id)
        {
            return await GetAsync<BlogTranslationDto>($"/api/Blogs/language/{id}/detail");
        }

        public async Task<bool> CreateLanguage(CreateBlogTranslationDto createBlogTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createBlogTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Blogs/language/create", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateBlogTranslationn(Guid languageId, UpdateBlogTranslationDto updateBlogTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateBlogTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Blogs/language/{languageId}/update", httpContent);

            return response.IsSuccessStatusCode;
        }


    }
}
