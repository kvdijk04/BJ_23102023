using AutoMapper;
using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.Product;
using BJ.Contract.Size;
using BJ.Contract.SubCategory;
using BJ.Contract.ViewModel;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BJ.Application.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductDtos();
        Task<IEnumerable<UserProductDto>> ClientProductDtos();
        Task<PagedViewModel<ViewAllProduct>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<ProductDto> GetProductById(Guid id);
        Task<UserProductDto> GetUserProductById(Guid id);

        Task<IEnumerable<UserProductDto>> GetProductByCatId(Guid catId, bool popular);
        Task CreateProductAdminView(CreateProductAdminView createProductAdminView);

        Task UpdateProductAdminView(Guid id, UpdateProductAdminView updateProductAdminView);


        Task DeleteProductDto(Guid id);

        Task RemoveImage(Guid imageId);

        Task<ProductDto> GetCode(string code);
    }
    public class ProductService : IProductService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public ProductService(BJContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<UserProductDto>> ClientProductDtos()
        {
            var product = await _context.Products.Include(x => x.Category).Include(x => x.SubCategorySpecificProducts).ThenInclude(x => x.SubCategory).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();

            var productDto = _mapper.Map<List<UserProductDto>>(product);

            return productDto;
        }

        public async Task CreateProductAdminView(CreateProductAdminView createProductAdminView)
        {
            var code = _configuration.GetValue<string>("Code:Product");

            createProductAdminView.CreateProduct.Id = Guid.NewGuid();

            createProductAdminView.CreateProduct.Code = code + Utilities.GenerateStringDateTime();

            createProductAdminView.CreateProduct.DateModified = DateTime.Now;

            createProductAdminView.CreateProduct.DateCreated = DateTime.Now;


            using var transaction = _context.Database.BeginTransaction();

            if (createProductAdminView.ImageCup != null)
            {
                string extension = Path.GetExtension(createProductAdminView.ImageCup.FileName);

                string image = Utilities.SEOUrl(createProductAdminView.CreateProduct.ProductName) + "_cup" + extension;

                createProductAdminView.CreateProduct.ImagePathCup = await Utilities.UploadFile(createProductAdminView.ImageCup, "ImageProduct", image);

            }
            if (createProductAdminView.ImageHero != null)
            {
                string extension = Path.GetExtension(createProductAdminView.ImageHero.FileName);

                string image = Utilities.SEOUrl(createProductAdminView.CreateProduct.ProductName) + "_hero" + extension;

                createProductAdminView.CreateProduct.ImagePathHero = await Utilities.UploadFile(createProductAdminView.ImageHero, "ImageProduct", image);

            }
            if (createProductAdminView.ImageIngredients != null)
            {
                string extension = Path.GetExtension(createProductAdminView.ImageIngredients.FileName);

                string image = Utilities.SEOUrl(createProductAdminView.CreateProduct.ProductName) + "_ingredients" + extension;

                createProductAdminView.CreateProduct.ImagePathIngredients = await Utilities.UploadFile(createProductAdminView.ImageIngredients, "ImageProduct", image);

            }
            Product product = _mapper.Map<Product>(createProductAdminView.CreateProduct);

            if (_context.Products.Any(x => x.ProductName == createProductAdminView.CreateProduct.ProductName) == false)
            {

                _context.Add(product);

                await _context.SaveChangesAsync();

            }
            if (createProductAdminView.Size != null)
            {
                for (int i = 0; i < createProductAdminView.Size.Length; i++)
                {
                    CreateSizeSpecificProductDto createSizeSpecificProductDto = new()
                    {
                        Id = Guid.NewGuid(),
                        SizeId = createProductAdminView.Size[i],
                        ProductId = createProductAdminView.CreateProduct.Id,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        ActiveNutri = false,
                        ActiveSize = true,
                    };
                    SizeSpecificEachProduct sizeSpecificEachProduct = _mapper.Map<SizeSpecificEachProduct>(createSizeSpecificProductDto);

                    _context.Add(sizeSpecificEachProduct);

                    await _context.SaveChangesAsync();
                }
            }
            if (createProductAdminView.SubCat != null)
            {
                for (int i = 0; i < createProductAdminView.SubCat.Length; i++)
                {
                    CreateSubCategorySpecificDto createSubCategorySpecificDto = new()
                    {
                        Id = Guid.NewGuid(),
                        SubCategoryId = createProductAdminView.SubCat[i],
                        ProductId = createProductAdminView.CreateProduct.Id,
                        Active = true,
                    };
                    SubCategorySpecificProduct subCategorySpecificProduct = _mapper.Map<SubCategorySpecificProduct>(createSubCategorySpecificDto);

                    _context.Add(subCategorySpecificProduct);

                    await _context.SaveChangesAsync();
                }
            }


            await transaction.CommitAsync();

            return;
        }

        public Task DeleteProductDto(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDto> GetCode(string code)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Code.Equals(code));

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<PagedViewModel<ViewAllProduct>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:Product"));
            }
            var pageResult = getListPagingRequest.PageSize; var pageCount = Math.Ceiling(_context.Products.Count() / (double)pageResult);
            var query = _context.Products.Include(x => x.Category).Include(x => x.SizeSpecificProducts).OrderByDescending(x => x.DateCreated).AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.ProductName.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            //if (getListPagingRequest.CategoryId != null)
            //{
            //    query = query.Where(x => getListPagingRequest.CategoryId.Contains(x.CategoryId));
            //    pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            //}
            //if (getListPagingRequest.Active == true)
            //{
            //    query = query.Where(x => x.Active == false);
            //    pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            //}

            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new ViewAllProduct()
                                    {
                                        Id = x.Id,
                                        ProductName = x.ProductName,
                                        Code = x.Code,
                                        Active = x.Active,
                                        BestSeller = x.BestSeller,
                                        CategoryName = x.Category.CatName,
                                        ImageIngredients = x.ImagePathIngredients,
                                        //SizeSpecificProducts = new List<SizeSpecificProductDto>(_mapper.Map<List<SizeSpecificProductDto>>(x.SizeSpecificProducts)),
                                    }).ToListAsync();
            var productResponse = new PagedViewModel<ViewAllProduct>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return productResponse;
        }

        public async Task<IEnumerable<UserProductDto>> GetProductByCatId(Guid catId, bool popular)
        {
            var product = await _context.Products.Include(x => x.Category).Include(x => x.SubCategorySpecificProducts).ThenInclude(x => x.SubCategory).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();
            if(catId != Guid.Empty && popular == false) { product = product.Where(x => x.CategoryId.Equals(catId)).ToList(); }
            else { product = product.Where(x => x.BestSeller == popular).ToList(); }
            var productDto = _mapper.Map<List<UserProductDto>>(product);
            return productDto;
        }

        public async Task<ProductDto> GetProductById(Guid id)
        {
            var product = await _context.Products.Include(x => x.Category).Include(x => x.SubCategorySpecificProducts).ThenInclude(x => x.SubCategory).Include(x => x.SizeSpecificProducts).ThenInclude(x => x.Size).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetProductDtos()
        {
            var product = await _context.Products.Include(x => x.Category).Include(x => x.SizeSpecificProducts).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();

            var productDto = _mapper.Map<List<ProductDto>>(product);

            return productDto;
        }

        public async Task<UserProductDto> GetUserProductById(Guid id)
        {
            var product = await _context.Products.Include(x => x.Category).Include(x => x.SubCategorySpecificProducts).ThenInclude(x => x.SubCategory).Include(x => x.SizeSpecificProducts).ThenInclude(x => x.Size).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var size = await _context.SizeSpecificEachProduct.Include(x => x.Size).Where(x => x.ProductId.Equals(id)).AsNoTracking().ToListAsync();
            
            var productDto = _mapper.Map<UserProductDto>(product);
            productDto.SizeSpecificProducts = _mapper.Map<List<SizeSpecificProductDto>>(size);

            return productDto;
        }

        public Task RemoveImage(Guid imageId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProductAdminView(Guid id, UpdateProductAdminView updateProductAdminView)
        {
            var item = await _context.Products.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {

                if (updateProductAdminView.ImageCup != null)
                {
                    string extension = Path.GetExtension(updateProductAdminView.ImageCup.FileName);

                    string image = Utilities.SEOUrl(updateProductAdminView.UpdateProductDto.ProductName) + "_cup" + extension;


                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageProduct", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }

                    updateProductAdminView.UpdateProductDto.ImagePathCup = await Utilities.UploadFile(updateProductAdminView.ImageCup, "ImageProduct", image);

                }
                if (updateProductAdminView.ImageHero != null)
                {

                    string extension = Path.GetExtension(updateProductAdminView.ImageHero.FileName);

                    string image = Utilities.SEOUrl(updateProductAdminView.UpdateProductDto.ProductName) + "_hero" + extension;

                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageProduct", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }

                    updateProductAdminView.UpdateProductDto.ImagePathHero = await Utilities.UploadFile(updateProductAdminView.ImageHero, "ImageProduct", image);

                }
                if (updateProductAdminView.ImageIngredients != null)
                {
                    string extension = Path.GetExtension(updateProductAdminView.ImageIngredients.FileName);

                    string image = Utilities.SEOUrl(updateProductAdminView.UpdateProductDto.ProductName) + "_ingredients" + extension;

                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageProduct", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }

                    updateProductAdminView.UpdateProductDto.ImagePathIngredients = await Utilities.UploadFile(updateProductAdminView.ImageIngredients, "ImageProduct", image);

                }
                updateProductAdminView.UpdateProductDto.Alias = Utilities.SEOUrl(updateProductAdminView.UpdateProductDto.ProductName);

                updateProductAdminView.UpdateProductDto.DateModified = DateTime.Now;

                _context.Products.Update(_mapper.Map(updateProductAdminView.UpdateProductDto, item));

                await _context.SaveChangesAsync();
            }

            return;
        }
    }
}
