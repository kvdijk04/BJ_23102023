using BJ.Contract;
using BJ.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BJ.ApiConnection.Services
{
    public interface IImportExcelServiceConnection
    {
        Task<string> ImportExcel(ImportResponse importResponse, CancellationToken cancellationToken, bool category, bool subCategory, bool size, bool product,
                                bool blog, bool news);
    }
    public class ImportExcelServiceConnection : IImportExcelServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ImportExcelServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> ImportExcel(ImportResponse importResponse, CancellationToken cancellationToken, bool category, bool subCategory, bool size, bool product, bool blog, bool news)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();



            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();
            if (importResponse.File != null)
            {
                byte[] data;
                using (var br = new BinaryReader(importResponse.File.OpenReadStream()))
                {
                    data = br.ReadBytes((int)importResponse.File.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "File", importResponse.File.FileName);
            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(importResponse.Msg) ? "" : importResponse.Msg.ToString()), "msg");

            var json = JsonConvert.SerializeObject(importResponse);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/ImportExcel/import?category={category}&subCategory={subCategory}&size={size}&product={product}&blog={blog}&news={news}", requestContent);
            var token = await response.Content.ReadAsStringAsync();


            return token;
        }
    }
}
