using BJ.Contract.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace BJ.ApiConnection.Services
{
    public interface ILoginServiceConnection
    {
        Task<string> Login(LoginDto loginDto);
        Task<AccountDto> GetByEmail(string email);

    }
    public class LoginServiceConnection : BaseApiClient, ILoginServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AccountDto> GetByEmail(string email)
        {
            return await GetAsync<AccountDto>($"/api/Accounts/email?email={email}");
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var json = JsonConvert.SerializeObject(loginDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");


            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var response = await client.PostAsync("api/Accounts/login", httpContent);

            var token = await response.Content.ReadAsStringAsync();

            return token;
        }
    }
}
