using BJ.Application.Ultities;
using BJ.Contract.Category;
using BJ.Contract.SubCategory;
using BJ.Contract.Translation.SubCategory;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface ISubCategoryServiceConnection
    {
        Task<bool> CreateSubCategorySpecific(CreateSubCategorySpecificDto createSubCategorySpecificDto);
        Task<bool> CreateSubCategory(CreateSubCategoryAdminView createSubCategoryAdminView);
        Task<bool> UpdateSubCategory(int id, UpdateSubCategoryAdminView updateSubCategoryAdminView);
        Task<IEnumerable<SubCategoryDto>> GetAllSubCategories();
        public Task<PagedViewModel<SubCategoryDto>> GetPagingSubCategory([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<SubCategoryDto> GetSubCategoryById(int id);

        public Task<SubCategoryTranslationDto> GetSubCategoryTranslationById(Guid id);

        Task<bool> CreateLanguage(CreateSubCategoryTranslationDto createSubCategoryTranslationDto);

        Task<bool> UpdateSubCategoryTranslation(Guid id, UpdateSubCategoryTranslationDto updateSubCategoryTranslationDto);
    }
    public class SubCategoryServiceConnection : BaseApiClient, ISubCategoryServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SubCategoryServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateLanguage(CreateSubCategoryTranslationDto createSubCategoryTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createSubCategoryTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/SubCategories/language", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateSubCategory(CreateSubCategoryAdminView createSubCategoryAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

            if (createSubCategoryAdminView.Image != null)
            {
                byte[] data;
                using (var br = new BinaryReader(createSubCategoryAdminView.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createSubCategoryAdminView.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "createSubCategoryDto.Image", createSubCategoryAdminView.Image.FileName);
            }



            requestContent.Add(new StringContent(string.IsNullOrEmpty(createSubCategoryAdminView.CreateSubCategoryTranslationDto.SubCatName) ? "" : createSubCategoryAdminView.CreateSubCategoryTranslationDto.SubCatName), "createSubCategoryAdminView.CreateSubCategoryTranslationDto.subCatName");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createSubCategoryAdminView.CreateSubCategoryTranslationDto.Description) ? "" : createSubCategoryAdminView.CreateSubCategoryTranslationDto.Description), "createSubCategoryAdminView.CreateSubCategoryTranslationDto.description");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createSubCategoryAdminView.CreateSubCategoryDto.Active.ToString()) ? "" : createSubCategoryAdminView.CreateSubCategoryDto.Active.ToString()), "createSubCategoryAdminView.CreateSubCategoryDto.active");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createSubCategoryAdminView.CreateSubCategoryDto.UserName) ? "" : createSubCategoryAdminView.CreateSubCategoryDto.UserName), "createSubCategoryAdminView.CreateSubCategoryDto.userName");
            ;
            var json = JsonConvert.SerializeObject(createSubCategoryAdminView);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/SubCategories", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateSubCategorySpecific(CreateSubCategorySpecificDto createSubCategorySpecificDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var json = JsonConvert.SerializeObject(createSubCategorySpecificDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/SubCategories/subCategoryspecificproduct", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<SubCategoryDto>> GetAllSubCategories()
        {
            return await GetListAsync<SubCategoryDto>($"/api/SubCategories/");

        }
        public async Task<PagedViewModel<SubCategoryDto>> GetPagingSubCategory([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/SubCategories/paging?PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var subCat = JsonConvert.DeserializeObject<PagedViewModel<SubCategoryDto>>(body);

            return subCat;
        }

        public async Task<SubCategoryDto> GetSubCategoryById(int id)
        {
            return await GetAsync<SubCategoryDto>($"/api/SubCategories/{id}");

        }

        public async Task<SubCategoryTranslationDto> GetSubCategoryTranslationById(Guid id)
        {
            return await GetAsync<SubCategoryTranslationDto>($"/api/SubCategories/language/{id}");
        }

        public async Task<bool> UpdateSubCategory(int id, UpdateSubCategoryAdminView updateSubCategoryAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

            byte[] data;
            if (updateSubCategoryAdminView.Image != null)
            {
                using (var br = new BinaryReader(updateSubCategoryAdminView.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateSubCategoryAdminView.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "updateSubCategoryDto.Image", updateSubCategoryAdminView.Image.FileName);
            }
            if (updateSubCategoryAdminView.Image == null)
            {
                requestContent.Add(new StringContent(string.IsNullOrEmpty(updateSubCategoryAdminView.UpdateSubCategoryDto.ImagePath) ? "" : updateSubCategoryAdminView.UpdateSubCategoryDto.ImagePath.ToString()), "updateSubCategoryAdminView.UpdateSubCategoryDto.imagePath");

            }


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateSubCategoryAdminView.UpdateSubCategoryTranslationDto.SubCatName) ? "" : updateSubCategoryAdminView.UpdateSubCategoryTranslationDto.SubCatName), "updateSubCategoryAdminView.UpdateSubCategoryTranslationDto.subCatName");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateSubCategoryAdminView.UpdateSubCategoryTranslationDto.Description) ? "" : updateSubCategoryAdminView.UpdateSubCategoryTranslationDto.Description), "updateSubCategoryAdminView.UpdateSubCategoryTranslationDto.description");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateSubCategoryAdminView.UpdateSubCategoryDto.Active.ToString()) ? "" : updateSubCategoryAdminView.UpdateSubCategoryDto.Active.ToString()), "updateSubCategoryAdminView.UpdateSubCategoryDto.active");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateSubCategoryAdminView.UpdateSubCategoryDto.UserName) ? "" : updateSubCategoryAdminView.UpdateSubCategoryDto.UserName), "updateSubCategoryAdminView.UpdateSubCategoryDto.userName");

            ;

            var json = JsonConvert.SerializeObject(updateSubCategoryAdminView);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/SubCategories/{id}", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSubCategoryTranslation(Guid id, UpdateSubCategoryTranslationDto updateSubCategoryTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateSubCategoryTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/SubCategories/language/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
