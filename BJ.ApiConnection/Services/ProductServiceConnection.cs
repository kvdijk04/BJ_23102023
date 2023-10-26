using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.Category;
using BJ.Contract.Product;
using BJ.Contract.Size;
using BJ.Contract.Translation.Product;
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
    public interface IProductServiceConnection
    {
        public Task<PagedViewModel<ViewAllProduct>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<bool> CreateProduct(CreateProductAdminView createProductAdminView);

        Task<bool> UpdateProduct(Guid id, UpdateProductAdminView updateProductAdminView);


        public Task<ProductDto> GetProductById(Guid id);

        public Task<IEnumerable<UserProductDto>> GetAllProductByCatId(Guid catId,bool popular, string culture);
        public Task<ProductUserViewModel> GetAllUserProduct(string cultrue);
        public Task<UserProductDto> GetUserProductById(Guid id,string culture);

        public Task<ProductTranslationDto> GetProductTranslationnById(Guid id);

        Task<bool> CreateLanguage(CreateProductTranslationDto createProductTranslationDto);

        Task<bool> UpdateProductTranslationn(Guid proId, Guid id, UpdateProductTranslationDto updateProductTranslationDto);
    }
    public class ProductServiceConnection : BaseApiClient, IProductServiceConnection
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductServiceConnection(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateLanguage(CreateProductTranslationDto createProductTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(createProductTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Products/language", httpContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateProduct(CreateProductAdminView createProductAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            createProductAdminView.CreateProduct.Alias = Utilities.SEOUrl(createProductAdminView.CreateProduct.ProductName);


            var requestContent = new MultipartFormDataContent();

            if (createProductAdminView.ImageCup != null)
            {
                byte[] data;
                using (var br = new BinaryReader(createProductAdminView.ImageCup.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createProductAdminView.ImageCup.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new(data);

                requestContent.Add(bytes, "createProductAdminView.imageCup", createProductAdminView.ImageCup.FileName);
            }
            if (createProductAdminView.ImageHero != null)
            {
                byte[] data;
                using (var br = new BinaryReader(createProductAdminView.ImageHero.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createProductAdminView.ImageHero.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new(data);

                requestContent.Add(bytes, "createProductAdminView.imageHero", createProductAdminView.ImageHero.FileName);
            }
            if (createProductAdminView.ImageIngredients != null)
            {
                byte[] data;
                using (var br = new BinaryReader(createProductAdminView.ImageIngredients.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createProductAdminView.ImageIngredients.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new(data);

                requestContent.Add(bytes, "createProductAdminView.imageIngredients", createProductAdminView.ImageIngredients.FileName);
            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.ProductName) ? "" : createProductAdminView.CreateProduct.ProductName.ToString()), "createProductAdminView.CreateProduct.productName");
            if (createProductAdminView.Size != null)
            {
                for (int i = 0; i < createProductAdminView.Size.Length; i++)
                {
                    requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.Size[i].ToString()) ? "" : createProductAdminView.Size[i].ToString()), "createProductAdminView.size");

                }
            }
            if (createProductAdminView.SubCat != null)
            {
                for (int i = 0; i < createProductAdminView.SubCat.Length; i++)
                {
                    requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.SubCat[i].ToString()) ? "" : createProductAdminView.SubCat[i].ToString()), "createProductAdminView.subCat");

                }
            }




            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.ShortDesc) ? "" : createProductAdminView.CreateProduct.ShortDesc.ToString()), "createProductAdminView.CreateProduct.shortDesc");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.Description) ? "" : createProductAdminView.CreateProduct.Description.ToString()), "createProductAdminView.CreateProduct.description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.Discount.ToString()) ? "" : createProductAdminView.CreateProduct.Discount.ToString()), "createProductAdminView.CreateProduct.discount");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.DateModified.ToString()) ? "" : createProductAdminView.CreateProduct.DateModified.ToString()), "createProductAdminView.CreateProduct.dateModified");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.Active.ToString()) ? "" : createProductAdminView.CreateProduct.Active.ToString()), "createProductAdminView.CreateProduct.active");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.BestSeller.ToString()) ? "" : createProductAdminView.CreateProduct.BestSeller.ToString()), "createProductAdminView.CreateProduct.bestSeller");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.HomeTag.ToString()) ? "" : createProductAdminView.CreateProduct.HomeTag.ToString()), "createProductAdminView.CreateProduct.homeTag");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.CategoryId.ToString()) ? "" : createProductAdminView.CreateProduct.CategoryId.ToString()), "createProductAdminView.CreateProduct.categoryId");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.DateCreated.ToString()) ? "" : createProductAdminView.CreateProduct.DateCreated.ToString()), "createProductAdminView.CreateProduct.dateCreated");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.Alias) ? "" : createProductAdminView.CreateProduct.Alias.ToString()), "createProductAdminView.CreateProduct.alias");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.MetaDesc.ToString()) ? "" : createProductAdminView.CreateProduct.MetaDesc.ToString()), "createProductAdminView.CreateProduct.metaDesc");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createProductAdminView.CreateProduct.MetaKey.ToString()) ? "" : createProductAdminView.CreateProduct.MetaKey.ToString()), "createProductAdminView.CreateProduct.metaKey");

            var response = await client.PostAsync($"/api/Products/", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<UserProductDto>> GetAllProductByCatId(Guid catId, bool popular,string culture)
        {
            return await GetListAsync<UserProductDto>($"/api/Products/category/filter?catId={catId}&popular={popular}&languageId={culture}");
        }

        public async Task<ProductUserViewModel> GetAllUserProduct(string cultrue)
        {
            return await GetAsync<ProductUserViewModel>($"/api/Products/userpage?languageId={cultrue}");
        }

        public async Task<PagedViewModel<ViewAllProduct>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Products/paging?PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var products = JsonConvert.DeserializeObject<PagedViewModel<ViewAllProduct>>(body);

            return products;
        }

        public async Task<ProductDto> GetProductById(Guid id)
        {
            return await GetAsync<ProductDto>($"/api/Products/{id}");

        }

        public async Task<ProductTranslationDto> GetProductTranslationnById(Guid id)
        {
            return await GetAsync<ProductTranslationDto>($"/api/Products/language/{id}/detail");
        }

        public async Task<UserProductDto> GetUserProductById(Guid id, string culture)
        {
            return await GetAsync<UserProductDto>($"/api/Products/userpage/{id}/{culture}");
        }

        public async Task<bool> UpdateProduct(Guid id, UpdateProductAdminView updateProductAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            updateProductAdminView.UpdateProductDto.Alias = Utilities.SEOUrl(updateProductAdminView.UpdateProductDto.ProductName);


            var requestContent = new MultipartFormDataContent();

            //foreach(var item in createProductAdminView.ImagesToUpload)
            //{
            //    byte[] data;
            //    using (var br = new BinaryReader(item.OpenReadStream()))
            //    {
            //        data = br.ReadBytes((int)item.OpenReadStream().Length);
            //    }
            //    ByteArrayContent bytes = new ByteArrayContent(data);

            //    requestContent.Add(bytes, "createProductAdminView.ImagesToUpload", item.FileName);



            //}
            if (updateProductAdminView.ImageCup != null)
            {
                byte[] data;
                using (var br = new BinaryReader(updateProductAdminView.ImageCup.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateProductAdminView.ImageCup.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new(data);

                requestContent.Add(bytes, "updateProductAdminView.imageCup", updateProductAdminView.ImageCup.FileName);
            }
            if (updateProductAdminView.ImageHero != null)
            {
                byte[] data;
                using (var br = new BinaryReader(updateProductAdminView.ImageHero.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateProductAdminView.ImageHero.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new(data);

                requestContent.Add(bytes, "updateProductAdminView.imageHero", updateProductAdminView.ImageHero.FileName);
            }
            if (updateProductAdminView.ImageIngredients != null)
            {
                byte[] data;
                using (var br = new BinaryReader(updateProductAdminView.ImageIngredients.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateProductAdminView.ImageIngredients.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new(data);

                requestContent.Add(bytes, "updateProductAdminView.imageIngredients", updateProductAdminView.ImageIngredients.FileName);
            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.ProductName) ? "" : updateProductAdminView.UpdateProductDto.ProductName.ToString()), "updateProductAdminView.UpdateProductDto.productName");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.ShortDesc) ? "" : updateProductAdminView.UpdateProductDto.ShortDesc.ToString()), "updateProductAdminView.UpdateProductDto.shortDesc");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.ImagePathCup) ? "" : updateProductAdminView.UpdateProductDto.ImagePathCup.ToString()), "updateProductAdminView.UpdateProductDto.imagePathCup");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.ImagePathHero) ? "" : updateProductAdminView.UpdateProductDto.ImagePathHero.ToString()), "updateProductAdminView.UpdateProductDto.imagePathHero");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.ImagePathIngredients) ? "" : updateProductAdminView.UpdateProductDto.ImagePathIngredients.ToString()), "updateProductAdminView.UpdateProductDto.imagePathIngredients");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.Description) ? "" : updateProductAdminView.UpdateProductDto.Description.ToString()), "updateProductAdminView.UpdateProductDto.description");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.Discount.ToString()) ? "" : updateProductAdminView.UpdateProductDto.Discount.ToString()), "updateProductAdminView.UpdateProductDto.discount");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.DateModified.ToString()) ? "" : updateProductAdminView.UpdateProductDto.DateModified.ToString().ToString()), "updateProductAdminView.UpdateProductDto.productName");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.Active.ToString()) ? "" : updateProductAdminView.UpdateProductDto.Active.ToString().ToString()), "updateProductAdminView.UpdateProductDto.active");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.BestSeller.ToString()) ? "" : updateProductAdminView.UpdateProductDto.BestSeller.ToString()), "updateProductAdminView.UpdateProductDto.bestSeller");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.HomeTag.ToString()) ? "" : updateProductAdminView.UpdateProductDto.HomeTag.ToString()), "updateProductAdminView.UpdateProductDto.homeTag");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.CategoryId.ToString()) ? "" : updateProductAdminView.UpdateProductDto.CategoryId.ToString()), "updateProductAdminView.UpdateProductDto.categoryId");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.Alias) ? "" : updateProductAdminView.UpdateProductDto.Alias.ToString()), "updateProductAdminView.UpdateProductDto.alias");

            //requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.Code.ToString()) ? "" : updateProductAdminView.UpdateProductDto.Code.ToString()), "updateProductAdminView.UpdateProductDto.code");
            //requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.Discount.ToString()) ? "" : updateProductAdminView.UpdateProductDto.Discount.ToString()), "updateProductAdminView.UpdateProductDto.discount");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.MetaDesc.ToString()) ? "" : updateProductAdminView.UpdateProductDto.MetaDesc.ToString()), "updateProductAdminView.UpdateProductDto.metaDesc");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateProductAdminView.UpdateProductDto.MetaKey.ToString()) ? "" : updateProductAdminView.UpdateProductDto.MetaKey.ToString()), "updateProductAdminView.UpdateProductDto.metaKey");

            var response = await client.PutAsync($"/api/Products/{id}", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async  Task<bool> UpdateProductTranslationn(Guid proId, Guid id, UpdateProductTranslationDto updateProductTranslationDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(updateProductTranslationDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Products/{proId}/language/{id}/update", httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
