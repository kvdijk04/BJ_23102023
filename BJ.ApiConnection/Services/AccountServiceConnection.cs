using BJ.Application.Ultities;
using BJ.Contract.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface IAccountServiceConnection
    {
        public Task<IEnumerable<AccountDto>> GetAllAccounts();
        public Task<IEnumerable<AccountDto>> GetAllAccountsByCatId(Guid catId);

        public Task<PagedViewModel<AccountDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<AccountDto> GetAccountByEmail(string email);

        Task<AccountDto> GetAccountById(Guid id);
        Task<bool> CreateAccount(CreateAccountDto createAccountDto);
        Task<bool> UpdateAccount(Guid id, UpdateAccountDto updateAccountDto);
        Task<bool> ChangePassword(string email, ChangePassword changePassword);

    }
    public class AccountServiceConnection : BaseApiClient, IAccountServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateAccount(CreateAccountDto createAccountDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createAccountDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Accounts", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccounts()
        {
            return await GetListAsync<AccountDto>($"/api/Accounts");

        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsByCatId(Guid catId)
        {
            return await GetListAsync<AccountDto>($"/api/Accounts/catId?catId={catId}");
        }

        public async Task<PagedViewModel<AccountDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Accounts/paging?PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var size = JsonConvert.DeserializeObject<PagedViewModel<AccountDto>>(body);

            return size;
        }

        public async Task<AccountDto> GetAccountById(Guid id)
        {
            return await GetAsync<AccountDto>($"/api/Accounts/{id}");

        }

        public async Task<bool> UpdateAccount(Guid id, UpdateAccountDto updateAccountDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateAccountDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Accounts/{id}", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<AccountDto> GetAccountByEmail(string email)
        {
            return await GetAsync<AccountDto>($"/api/Accounts/email?email={email}");
        }

        public async Task<bool> ChangePassword(string email, ChangePassword changePassword)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(changePassword);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Accounts/changepassword/{email}", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
