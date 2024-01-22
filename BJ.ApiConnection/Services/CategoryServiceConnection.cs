using BJ.Application.Ultities;
using BJ.Contract.Category;
using BJ.Contract.Translation.Category;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface ICategoryServiceConnection
    {
        public Task<IEnumerable<CategoryDto>> GetAllCategories();
        public Task<IEnumerable<UserCategoryDto>> GetAllUserCategories(string culture);

        public Task<PagedViewModel<CategoryDto>> GetPagingCategory([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<CategoryDto> GetCategoryById(Guid id);
        Task<bool> CreateCategory(CreateCategoryAdminView createCategoryAdminView);
        Task<bool> UpdateCategory(Guid id, UpdateCategoryAdminView updateCategoryAdminView);
        public Task<CategoryTranslationDto> GetCategoryTranslationById(Guid id);

        Task<bool> CreateLanguage(CreateCategoryTranslationDto createCategoryTranslationDto);

        Task<bool> UpdateCategoryTranslation(Guid id, UpdateCategoryTranslationDto updateCategoryTranslationDto);

    }
    public class CategoryServiceConnection : BaseApiClient, ICategoryServiceConnection
    {


        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CategoryServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateCategory(CreateCategoryAdminView createCategoryAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();
            if (createCategoryAdminView.Image != null)
            {
                byte[] data;

                using (var br = new BinaryReader(createCategoryAdminView.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createCategoryAdminView.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "createCategoryAdminView.Image", createCategoryAdminView.Image.FileName);
            }


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryAdminView.CreateCategoryTranslationDto.CatName) ? "" : createCategoryAdminView.CreateCategoryTranslationDto.CatName), "createCategoryAdminView.CreateCategoryTranslationDto.catName");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryAdminView.CreateCategoryTranslationDto.Description) ? "" : createCategoryAdminView.CreateCategoryTranslationDto.Description), "createCategoryAdminView.CreateCategoryTranslationDto.description");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryAdminView.CreateCategoryDto.Active.ToString()) ? "" : createCategoryAdminView.CreateCategoryDto.Active.ToString()), "createCategoryAdminView.CreateCategoryDto.active");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryAdminView.CreateCategoryDto.Sort.ToString()) ? "" : createCategoryAdminView.CreateCategoryDto.Sort.ToString()), "createCategoryAdminView.CreateCategoryDto.sort");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryAdminView.CreateCategoryTranslationDto.Alias) ? "" : createCategoryAdminView.CreateCategoryTranslationDto.Alias.ToString()), "createCategoryAdminView.CreateCategoryTranslationDto.alias");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryAdminView.CreateCategoryDto.DateActiveForm.ToString()) ? "" : createCategoryAdminView.CreateCategoryDto.DateActiveForm.ToString()), "createCategoryAdminView.CreateCategoryDto.DateActiveForm");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryAdminView.CreateCategoryDto.DateTimeActiveTo.ToString()) ? "" : createCategoryAdminView.CreateCategoryDto.DateTimeActiveTo.ToString()), "createCategoryAdminView.CreateCategoryDto.DateTimeActiveTo");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryAdminView.CreateCategoryDto.UserName) ? "" : createCategoryAdminView.CreateCategoryDto.UserName), "createCategoryAdminView.CreateCategoryDto.userName");

            var json = JsonConvert.SerializeObject(createCategoryAdminView);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Categories/", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateLanguage(CreateCategoryTranslationDto createCategoryTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createCategoryTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Categories/language", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {

            return await GetListAsync<CategoryDto>("/api/Categories");

        }

        public async Task<IEnumerable<UserCategoryDto>> GetAllUserCategories(string culture)
        {
            return await GetListAsync<UserCategoryDto>($"/api/Categories/userpage?languageId={culture}");
        }

        public async Task<CategoryDto> GetCategoryById(Guid id)
        {
            return await GetAsync<CategoryDto>($"/api/Categories/{id}");

        }

        public async Task<CategoryTranslationDto> GetCategoryTranslationById(Guid id)
        {
            return await GetAsync<CategoryTranslationDto>($"/api/Categories/language/{id}");
        }

        public async Task<PagedViewModel<CategoryDto>> GetPagingCategory([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Categories/paging?PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var cat = JsonConvert.DeserializeObject<PagedViewModel<CategoryDto>>(body);

            return cat;
        }

        public async Task<bool> UpdateCategory(Guid id, UpdateCategoryAdminView updateCategoryAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

            byte[] data;

            if (updateCategoryAdminView.Image != null)
            {
                using (var br = new BinaryReader(updateCategoryAdminView.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateCategoryAdminView.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "updateCategoryAdminView.UpdateCategory.Image", updateCategoryAdminView.Image.FileName);
            }
            if (updateCategoryAdminView.Image == null)
            {
                requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryAdminView.UpdateCategory.ImagePath) ? "" : updateCategoryAdminView.UpdateCategory.ImagePath), "updateCategoryAdminView.UpdateCategory.ImagePath");

            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryAdminView.UpdateCategoryTranslationDto.CatName) ? "" : updateCategoryAdminView.UpdateCategoryTranslationDto.CatName), "updateCategoryAdminView.UpdateCategoryTranslationDto.catName");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryAdminView.UpdateCategoryTranslationDto.Description) ? "" : updateCategoryAdminView.UpdateCategoryTranslationDto.Description), "updateCategoryAdminView.UpdateCategoryTranslationDto.description");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryAdminView.UpdateCategory.Active.ToString()) ? "" : updateCategoryAdminView.UpdateCategory.Active.ToString()), "updateCategoryAdminView.UpdateCategory.active");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryAdminView.UpdateCategoryTranslationDto.Alias) ? "" : updateCategoryAdminView.UpdateCategoryTranslationDto.Alias), "updateCategoryAdminView.UpdateCategoryTranslationDto.alias");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryAdminView.UpdateCategory.Sort.ToString()) ? "" : updateCategoryAdminView.UpdateCategory.Sort.ToString()), "updateCategoryAdminView.UpdateCategory.sort");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryAdminView.UpdateCategory.DateActiveForm.ToString()) ? "" : updateCategoryAdminView.UpdateCategory.DateActiveForm.ToString()), "updateCategoryAdminView.UpdateCategory.dateActiveForm");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryAdminView.UpdateCategory.DateTimeActiveTo.ToString()) ? "" : updateCategoryAdminView.UpdateCategory.DateTimeActiveTo.ToString()), "updateCategoryAdminView.UpdateCategory.dateTimeActiveTo");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryAdminView.UpdateCategory.UserName) ? "" : updateCategoryAdminView.UpdateCategory.UserName), "updateCategoryAdminView.UpdateCategory.userName");

            var json = JsonConvert.SerializeObject(updateCategoryAdminView);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Categories/{id}", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryTranslation(Guid id, UpdateCategoryTranslationDto updateCategoryTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateCategoryTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Categories/language/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
