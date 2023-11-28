using BJ.Application.Ultities;
using BJ.Contract.StoreLocation;
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
        public Task<IEnumerable<StoreLocationDto>> GetAllStoreLocations();
        public Task<PagedViewModel<StoreLocationDto>> GetPagingStoreLocation([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<StoreLocationDto> GetStoreById(int id);

        Task<bool> CreateStoreLocation(CreateStoreLocationDto createStoreLocationDto);
        Task<bool> UpdateStoreLocation(int id, UpdateStoreLocationDto updateStoreLocationDto);

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

        public async Task<bool> CreateStoreLocation(CreateStoreLocationDto createStoreLocationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

            if (createStoreLocationDto.ImageStore != null)
            {
                byte[] data;

                using (var br = new BinaryReader(createStoreLocationDto.ImageStore.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createStoreLocationDto.ImageStore.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "createStoreLocationDto.ImageStore", createStoreLocationDto.ImageStore.FileName);
            }


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationDto.Address) ? "" : createStoreLocationDto.Address.ToString()), "createStoreLocationDto.Address");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationDto.Name) ? "" : createStoreLocationDto.Name.ToString()), "createStoreLocationDto.Name");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationDto.IconPath) ? "" : createStoreLocationDto.IconPath.ToString()), "createStoreLocationDto.IconPath");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationDto.Closed.ToString()) ? "" : createStoreLocationDto.Closed.ToString()), "createStoreLocationDto.Closed");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationDto.Repaired.ToString()) ? "" : createStoreLocationDto.Repaired.ToString()), "createStoreLocationDto.Repaired");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationDto.City) ? "" : createStoreLocationDto.City), "createStoreLocationDto.City");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationDto.Latitude.ToString()) ? "" : createStoreLocationDto.Latitude.ToString()), "createStoreLocationDto.Latitude");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createStoreLocationDto.Longitude.ToString()) ? "" : createStoreLocationDto.Longitude.ToString()), "createStoreLocationDto.Longitude");

            var json = JsonConvert.SerializeObject(createStoreLocationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/StoreLocations/", requestContent);

            return response.IsSuccessStatusCode;

           
        }

        public async Task<IEnumerable<StoreLocationDto>> GetAllStoreLocations()
        {
            return await GetListAsync<StoreLocationDto>($"/api/StoreLocations");

        }

        public async  Task<PagedViewModel<StoreLocationDto>> GetPagingStoreLocation([FromQuery] GetListPagingRequest getListPagingRequest)
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

        public async  Task<bool> UpdateStoreLocation(int id, UpdateStoreLocationDto updateStoreLocationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

            if (updateStoreLocationDto.ImageStore != null)
            {
                byte[] data;

                using (var br = new BinaryReader(updateStoreLocationDto.ImageStore.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateStoreLocationDto.ImageStore.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "updateStoreLocationDto.ImageStore", updateStoreLocationDto.ImageStore.FileName);
            }
            if(updateStoreLocationDto.ImagePath  != null)
            {
                requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationDto.ImagePath) ? "" : updateStoreLocationDto.ImagePath.ToString()), "updateStoreLocationDto.ImagePath");

            }

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationDto.Address) ? "" : updateStoreLocationDto.Address.ToString()), "updateStoreLocationDto.Address");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationDto.Name) ? "" : updateStoreLocationDto.Name.ToString()), "updateStoreLocationDto.Name");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationDto.IconPath) ? "" : updateStoreLocationDto.IconPath.ToString()), "updateStoreLocationDto.IconPath");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationDto.Closed.ToString()) ? "" : updateStoreLocationDto.Closed.ToString()), "updateStoreLocationDto.Closed");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationDto.Repaired.ToString()) ? "" : updateStoreLocationDto.Repaired.ToString()), "updateStoreLocationDto.Repaired");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationDto.City) ? "" : updateStoreLocationDto.City), "updateStoreLocationDto.City");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationDto.Latitude.ToString()) ? "" : updateStoreLocationDto.Latitude.ToString()), "updateStoreLocationDto.Latitude");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateStoreLocationDto.Longitude.ToString()) ? "" : updateStoreLocationDto.Longitude.ToString()), "updateStoreLocationDto.Longitude");
                
            var json = JsonConvert.SerializeObject(updateStoreLocationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/StoreLocations/{id}", requestContent);

            return response.IsSuccessStatusCode;
        }
    }
}
