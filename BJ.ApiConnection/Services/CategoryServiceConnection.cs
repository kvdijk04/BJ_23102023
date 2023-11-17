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
        Task<bool> CreateCategory(CreateCategoryDto createCategoryDto);
        Task<bool> UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto);
        public Task<CategoryTranslationDto> GetCategoryTranslationnById(Guid id);

        Task<bool> CreateLanguage(CreateCategoryTranslationDto createCategoryTranslationDto);

        Task<bool> UpdateCategoryTranslationn(Guid catId, Guid id, UpdateCategoryTranslationDto updateCategoryTranslationDto);

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

        public async Task<bool> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();
            if (createCategoryDto.Image != null)
            {
                byte[] data;

                using (var br = new BinaryReader(createCategoryDto.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createCategoryDto.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "createCategoryDto.Image", createCategoryDto.Image.FileName);
            }


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryDto.CatName) ? "" : createCategoryDto.CatName.ToString()), "createCategoryDto.catName");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryDto.Description) ? "" : createCategoryDto.Description.ToString()), "createCategoryDto.description");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryDto.Active.ToString()) ? "" : createCategoryDto.Active.ToString()), "createCategoryDto.active");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryDto.Alias) ? "" : createCategoryDto.Alias.ToString()), "createCategoryDto.alias");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryDto.MetaDesc.ToString()) ? "" : createCategoryDto.MetaDesc.ToString()), "createCategoryDto.metaDesc");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createCategoryDto.MetaKey.ToString()) ? "" : createCategoryDto.MetaKey.ToString()), "createCategoryDto.metaKey");

            var json = JsonConvert.SerializeObject(createCategoryDto);

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

            var response = await client.PostAsync($"/api/Categories/language/create", httpContent);

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

        public async Task<CategoryTranslationDto> GetCategoryTranslationnById(Guid id)
        {
            return await GetAsync<CategoryTranslationDto>($"/api/Categories/language/{id}/detail");
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

        public async Task<bool> UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

            byte[] data;

            if (updateCategoryDto.Image != null)
            {
                using (var br = new BinaryReader(updateCategoryDto.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateCategoryDto.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "updateCategoryDto.Image", updateCategoryDto.Image.FileName);
            }

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryDto.ImagePath) ? "" : updateCategoryDto.ImagePath.ToString()), "updateCategoryDto.imagePath");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryDto.CatName) ? "" : updateCategoryDto.CatName.ToString()), "updateCategoryDto.catName");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryDto.Description) ? "" : updateCategoryDto.Description.ToString()), "updateCategoryDto.description");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryDto.Active.ToString()) ? "" : updateCategoryDto.Active.ToString()), "updateCategoryDto.active");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryDto.Alias) ? "" : updateCategoryDto.Alias.ToString()), "updateCategoryDto.alias");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryDto.MetaDesc.ToString()) ? "" : updateCategoryDto.MetaDesc.ToString()), "updateCategoryDto.metaDesc");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateCategoryDto.MetaKey.ToString()) ? "" : updateCategoryDto.MetaKey.ToString()), "updateCategoryDto.metaKey");

            var json = JsonConvert.SerializeObject(updateCategoryDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Categories/{id}", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryTranslationn(Guid catId, Guid id, UpdateCategoryTranslationDto updateCategoryTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateCategoryTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Categories/{catId}/language/{id}/update", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
