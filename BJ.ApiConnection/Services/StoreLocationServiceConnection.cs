using BJ.Application.Ultities;
using BJ.Contract.Size;
using BJ.Contract.StoreLocation;
using BJ.Contract.Translation.Store;
using BJ.Contract.ViewModel;
using BJ.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface IStoreLocationServiceConnection
    {
        public Task<IEnumerable<StoreLocationDto>> GetAllStoreLocations(string languageId);
        public Task<PagedViewModel<StoreLocationDto>> GetPagingStoreLocation([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<StoreLocationDto> GetStoreById(int id);
        Task<StoreLocationOpenHourDto> GetStoreTimeLineById(int id);

        Task<bool> CreateStoreLocation(CreateStoreLocationAdminView createStoreLocationAdminView);
        Task<bool> UpdateStoreLocation(int id, UpdateStoreLocationAdminView updateStoreLocationAdminView);
        Task<bool> CreateStoreLocationTimeLine(CreateStoreLocationOpenHourDto createStoreLocationTimeLineDto);
        Task<bool> UpdateStoreLocationTimeLine(int id, UpdateStoreLocationOpenHourDto updateStoreLocationTimeLineDto);
        public Task<StoreLocationTranslationDto> GetStoreLocationTranslationById(Guid id);
        Task<bool> CreateLanguage(CreateStoreLocationTranslationDto createStoreLocationTranslationDto);

        Task<bool> UpdateStoreLocationTranslation(Guid id, UpdateStoreLocationTranslationDto updateStoreLocationTranslationDto);
    }
    public class StoreLocationServiceConnection : BaseApiClient, IStoreLocationServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoreLocationServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateStoreLocation(CreateStoreLocationAdminView createStoreLocationAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

            if (createStoreLocationAdminView.ImageStore != null)
            {
                byte[] data;

                using (var br = new BinaryReader(createStoreLocationAdminView.ImageStore.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createStoreLocationAdminView.ImageStore.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "createStoreLocationAdminView.ImageStore", createStoreLocationAdminView.ImageStore.FileName);
            }


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationAdminView.CreateStoreLocationTranslation.Address) ? "" : createStoreLocationAdminView.CreateStoreLocationTranslation.Address), "createStoreLocationAdminView.CreateStoreLocationTranslation.Address");
            
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationAdminView.CreateStoreLocationTranslation.Name) ? "" : createStoreLocationAdminView.CreateStoreLocationTranslation.Name), "createStoreLocationAdminView.CreateStoreLocationTranslation.Name");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationAdminView.CreateStoreLocationTranslation.City) ? "" : createStoreLocationAdminView.CreateStoreLocationTranslation.City), "createStoreLocationAdminView.CreateStoreLocationTranslation.City");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationAdminView.CreateStoreLocationDto.IconPath) ? "" : createStoreLocationAdminView.CreateStoreLocationDto.IconPath), "createStoreLocationAdminView.CreateStoreLocationDto.IconPath");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationAdminView.CreateStoreLocationDto.Closed.ToString()) ? "" : createStoreLocationAdminView.CreateStoreLocationDto.Closed.ToString()), "createStoreLocationAdminView.CreateStoreLocationDto.Closed");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationAdminView.CreateStoreLocationDto.Repaired.ToString()) ? "" : createStoreLocationAdminView.CreateStoreLocationDto.Repaired.ToString()), "createStoreLocationAdminView.CreateStoreLocationDto.Repaired");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationAdminView.CreateStoreLocationDto.Latitude.ToString()) ? "" : createStoreLocationAdminView.CreateStoreLocationDto.Latitude.ToString()), "createStoreLocationAdminView.CreateStoreLocationDto.Latitude");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationAdminView.CreateStoreLocationDto.Longitude.ToString()) ? "" : createStoreLocationAdminView.CreateStoreLocationDto.Longitude.ToString()), "createStoreLocationAdminView.CreateStoreLocationDto.Longitude");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationAdminView.CreateStoreLocationDto.Sort.ToString()) ? "" : createStoreLocationAdminView.CreateStoreLocationDto.Sort.ToString()), "createStoreLocationAdminView.CreateStoreLocationDto.Sort");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationAdminView.CreateStoreLocationDto.OpeningSoon.ToString()) ? "" : createStoreLocationAdminView.CreateStoreLocationDto.OpeningSoon.ToString()), "createStoreLocationAdminView.CreateStoreLocationDto.OpeningSoon");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationAdminView.CreateStoreLocationDto.UserName) ? "" : createStoreLocationAdminView.CreateStoreLocationDto.UserName), "createStoreLocationAdminView.CreateStoreLocationDto.UserName");

            var json = JsonConvert.SerializeObject(createStoreLocationAdminView);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/StoreLocations/", requestContent);

            return response.IsSuccessStatusCode;


        }

        public async Task<bool> CreateStoreLocationTimeLine(CreateStoreLocationOpenHourDto createStoreLocationTimeLineDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createStoreLocationTimeLineDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/StoreLocations/openinghours", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<StoreLocationDto>> GetAllStoreLocations(string languageId)
        {
            return await GetListAsync<StoreLocationDto>($"/api/StoreLocations?languageId={languageId}");

        }

        public async Task<PagedViewModel<StoreLocationDto>> GetPagingStoreLocation([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/StoreLocations/paging?PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var cat = JsonConvert.DeserializeObject<PagedViewModel<StoreLocationDto>>(body);

            return cat;
        }

        public async Task<StoreLocationDto> GetStoreById(int id)
        {
            return await GetAsync<StoreLocationDto>($"/api/StoreLocations/{id}");
        }

        public async Task<StoreLocationOpenHourDto> GetStoreTimeLineById(int id)
        {
            return await GetAsync<StoreLocationOpenHourDto>($"/api/StoreLocations/openinghours/{id}");
        }

        public async Task<bool> UpdateStoreLocation(int id, UpdateStoreLocationAdminView updateStoreLocationAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

            if (updateStoreLocationAdminView.ImageStore != null)
            {
                byte[] data;

                using (var br = new BinaryReader(updateStoreLocationAdminView.ImageStore.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateStoreLocationAdminView.ImageStore.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "updateStoreLocationAdminView.ImageStore", updateStoreLocationAdminView.ImageStore.FileName);
            }
            if (updateStoreLocationAdminView.UpdateStoreLocationDto.ImagePath != null)
            {
                requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationAdminView.UpdateStoreLocationDto.ImagePath) ? "" : updateStoreLocationAdminView.UpdateStoreLocationDto.ImagePath.ToString()), "updateStoreLocationAdminView.UpdateStoreLocationDto.ImagePath");

            }

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationAdminView.UpdateStoreLocationTranslationDto.Address) ? "" : updateStoreLocationAdminView.UpdateStoreLocationTranslationDto.Address), "updateStoreLocationAdminView.UpdateStoreLocationTranslationDto.Address");
            
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationAdminView.UpdateStoreLocationTranslationDto.Name) ? "" : updateStoreLocationAdminView.UpdateStoreLocationTranslationDto.Name), "updateStoreLocationAdminView.UpdateStoreLocationTranslationDto.Name");
            
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationAdminView.UpdateStoreLocationTranslationDto.City) ? "" : updateStoreLocationAdminView.UpdateStoreLocationTranslationDto.City), "updateStoreLocationAdminView.UpdateStoreLocationTranslationDto.City");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationAdminView.UpdateStoreLocationDto.IconPath) ? "" : updateStoreLocationAdminView.UpdateStoreLocationDto.IconPath), "updateStoreLocationAdminView.UpdateStoreLocationDto.IconPath");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationAdminView.UpdateStoreLocationDto.Closed.ToString()) ? "" : updateStoreLocationAdminView.UpdateStoreLocationDto.Closed.ToString()), "updateStoreLocationAdminView.UpdateStoreLocationDto.Closed");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationAdminView.UpdateStoreLocationDto.Repaired.ToString()) ? "" : updateStoreLocationAdminView.UpdateStoreLocationDto.Repaired.ToString()), "updateStoreLocationAdminView.UpdateStoreLocationDto.Repaired");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationAdminView.UpdateStoreLocationDto.Latitude.ToString()) ? "" : updateStoreLocationAdminView.UpdateStoreLocationDto.Latitude.ToString()), "updateStoreLocationAdminView.UpdateStoreLocationDto.Latitude");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationAdminView.UpdateStoreLocationDto.Longitude.ToString()) ? "" : updateStoreLocationAdminView.UpdateStoreLocationDto.Longitude.ToString()), "updateStoreLocationAdminView.UpdateStoreLocationDto.Longitude");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationAdminView.UpdateStoreLocationDto.Sort.ToString()) ? "" : updateStoreLocationAdminView.UpdateStoreLocationDto.Sort.ToString()), "updateStoreLocationAdminView.UpdateStoreLocationDto.Sort");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationAdminView.UpdateStoreLocationDto.OpeningSoon.ToString()) ? "" : updateStoreLocationAdminView.UpdateStoreLocationDto.OpeningSoon.ToString()), "updateStoreLocationAdminView.UpdateStoreLocationDto.OpeningSoon");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationAdminView.UpdateStoreLocationDto.UserName) ? "" : updateStoreLocationAdminView.UpdateStoreLocationDto.UserName), "updateStoreLocationAdminView.UpdateStoreLocationDto.UserName");

            var json = JsonConvert.SerializeObject(updateStoreLocationAdminView);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/StoreLocations/{id}", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateStoreLocationTimeLine(int id, UpdateStoreLocationOpenHourDto updateStoreLocationTimeLineDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateStoreLocationTimeLineDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/StoreLocations/openinghours/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateLanguage(CreateStoreLocationTranslationDto createStoreLocationTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createStoreLocationTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/StoreLocations/language", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateStoreLocationTranslation(Guid id, UpdateStoreLocationTranslationDto updateStoreLocationTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateStoreLocationTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/StoreLocations/language/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<StoreLocationTranslationDto> GetStoreLocationTranslationById(Guid id)
        {
            return await GetAsync<StoreLocationTranslationDto>($"/api/StoreLocations/language/{id}");
        }
    }
}
