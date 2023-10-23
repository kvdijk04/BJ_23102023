using BJ.Application.Ultities;
using BJ.Contract.Product;
using BJ.Contract.SubCategory;
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
        Task<bool> CreateSubCategory(CreateSubCategoryDto createSubCategoryDto);
        Task<bool> UpdateSubCategory(int id, UpdateSubCategoryDto updateSubCategoryDto);
        Task<IEnumerable<SubCategoryDto>> GetAllSubCategories();
        public Task<PagedViewModel<SubCategoryDto>> GetPagingSubCategory([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<SubCategoryDto> GetSubCategoryById(int id);
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



        public async Task<bool> CreateSubCategory(CreateSubCategoryDto createSubCategoryDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

            byte[] data;
            using (var br = new BinaryReader(createSubCategoryDto.Image.OpenReadStream()))
            {
                data = br.ReadBytes((int)createSubCategoryDto.Image.OpenReadStream().Length);
            }
            ByteArrayContent bytes = new ByteArrayContent(data);

            requestContent.Add(bytes, "createSubCategoryDto.Image", createSubCategoryDto.Image.FileName);


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createSubCategoryDto.SubCatName) ? "" : createSubCategoryDto.SubCatName.ToString()), "createSubCategoryDto.subCatName");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createSubCategoryDto.Description) ? "" : createSubCategoryDto.Description.ToString()), "createSubCategoryDto.description");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createSubCategoryDto.Active.ToString()) ? "" : createSubCategoryDto.Active.ToString()), "createSubCategoryDto.active");

            ;

            var json = JsonConvert.SerializeObject(createSubCategoryDto);

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

        public async Task<bool> UpdateSubCategory(int id, UpdateSubCategoryDto updateSubCategoryDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

            byte[] data;
            if (updateSubCategoryDto.Image != null)
            {
                using (var br = new BinaryReader(updateSubCategoryDto.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateSubCategoryDto.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "updateSubCategoryDto.Image", updateSubCategoryDto.Image.FileName);
            }

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateSubCategoryDto.ImagePath) ? "" : updateSubCategoryDto.ImagePath.ToString()), "updateSubCategoryDto.imagePath");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateSubCategoryDto.SubCatName) ? "" : updateSubCategoryDto.SubCatName.ToString()), "updateSubCategoryDto.subCatName");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateSubCategoryDto.Description) ? "" : updateSubCategoryDto.Description.ToString()), "updateSubCategoryDto.description");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateSubCategoryDto.Active.ToString()) ? "" : updateSubCategoryDto.Active.ToString()), "updateSubCategoryDto.active");

            ;

            var json = JsonConvert.SerializeObject(updateSubCategoryDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/SubCategories/{id}", requestContent);

            return response.IsSuccessStatusCode;
        }
    }
}
