using AutoMapper;
using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.Category;
using BJ.Contract.Product;
using BJ.Contract.Size;
using BJ.Contract.SubCategory;
using BJ.Contract.Translation.Product;
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
        Task<IEnumerable<UserProductDto>> ClientProductDtos(string languageId);
        Task<PagedViewModel<ViewAllProduct>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<ProductDto> GetProductById(Guid id);
        Task<UserProductDto> GetUserProductById(Guid id, string languageId);
        Task<ProductTranslationDto> GetProductTranslationDto(Guid id);

        Task<IEnumerable<UserProductDto>> GetProductByCatId(Guid catId, string languageId);
        Task CreateProductAdminView(CreateProductAdminView createProductAdminView);
        Task CreateProductTranslate(CreateProductTranslationDto createProductTranslationDto);

        Task UpdateProductAdminView(Guid id, UpdateProductAdminView updateProductAdminView);

        Task UpdateProductTranslate(Guid proId, Guid id, UpdateProductTranslationDto updateProductTranslationDto);

        Task DeleteProductDto(Guid id);

        Task RemoveImage(Guid imageId);

        Task<ProductDto> GetCode(string code);
        Task<ProductUserViewModel> GetProduct(string languageId);
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

        public async Task<IEnumerable<UserProductDto>> ClientProductDtos(string languageId)
        {
            var query = from p in _context.Products.Include(x => x.SubCategorySpecificProducts).ThenInclude(y => y.SubCategory).ThenInclude(z => z.SubCategoryTranslations.Where(x => x.LanguageId == languageId))
                        join c in _context.Categories on p.CategoryId equals c.Id
                        join ct in _context.CategoryTranslations.Where(x => x.LanguageId == languageId) on c.Id equals ct.CategoryId into cd
                        from ct in cd.DefaultIfEmpty()
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId

                        where pt.LanguageId == languageId && p.Active == true  /*sct.LanguageId == languageId*/
                        select new { c, p, pt, ct /*scsp*/ };
            var productDto = await query.Select(x => new UserProductDto()
            {
                Id = x.p.Id,
                Active = x.p.Active,
                CatName = x.ct.CatName,
                BestSeller = x.p.BestSeller,
                CategoryId = x.p.CategoryId,
                Description = x.pt.Description,
                HomeTag = x.p.HomeTag,
                ImagePathCup = x.p.ImagePathCup,
                ImagePathHero = x.p.ImagePathHero,
                ImagePathIngredients = x.p.ImagePathIngredients,
                ProductName = x.pt.ProductName,
                ShortDesc = x.pt.ShortDesc,
                UserSubCategorySpecificProductDto = _mapper.Map<List<UserSubCategorySpecificProductDto>>(x.p.SubCategorySpecificProducts.Where(x => x.Active == true)),

            }).AsNoTracking().ToListAsync();
            return productDto;
        }

        public async Task CreateProductAdminView(CreateProductAdminView createProductAdminView)
        {



            createProductAdminView.CreateProduct.Id = Guid.NewGuid();

            var total = await _context.Products.OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();

            string s = null;

            var code = _configuration.GetValue<string>("Code:Product");

            var codeLimit = _configuration.GetValue<string>("LimitCode");

            if (total.Count == 0) { createProductAdminView.CreateProduct.Code = code + codeLimit; }

            else if (total.Count > 0)
            {
                var x = total[0].Code.Substring(code.Length, codeLimit.Length);

                int k = Convert.ToInt32(total[0].Code.Substring(code.Length, codeLimit.Length)) + 1;
                if (k < 10) s += total[0].Code.Substring(code.Length, codeLimit.Length - 1);
                else if (k < 100)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 2);
                else if (k < 1000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 3);
                else if (k < 10000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 4);
                else if (k < 100000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 5);
                else if (k < 1000000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 6);
                else if (k < 10000000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 7);
                else if (k < 100000000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 8);
                else if (k < 1000000000)
                    s += total[0].Code.Substring(code.Length, codeLimit.Length - 9);
                s += k.ToString();

                createProductAdminView.CreateProduct.Code = code + s;

            }
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
            if (createProductAdminView.CreateProduct.Alias == null)
            {
                createProductAdminView.CreateProduct.Alias = Utilities.SEOUrl(createProductAdminView.CreateProduct.ProductName);
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
            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            CreateProductTranslationDto createProductTranslationDto = new()
            {
                Id = Guid.NewGuid(),
                ProductId = createProductAdminView.CreateProduct.Id,
                ProductName = createProductAdminView.CreateProduct.ProductName,
                Alias = Utilities.SEOUrl(createProductAdminView.CreateProduct.ProductName),
                Description = createProductAdminView.CreateProduct.Description,
                ShortDesc = createProductAdminView.CreateProduct.ShortDesc,
                MetaDesc = createProductAdminView.CreateProduct.MetaDesc,
                MetaKey = createProductAdminView.CreateProduct.MetaKey,
                LanguageId = defaultLanguage,
            };
            ProductTranslation poductTranslation = _mapper.Map<ProductTranslation>(createProductTranslationDto);

            _context.Add(poductTranslation);

            await _context.SaveChangesAsync();


            await transaction.CommitAsync();

            return;
        }

        public async Task CreateProductTranslate(CreateProductTranslationDto createProductTranslationDto)
        {
            createProductTranslationDto.Id = Guid.NewGuid();

            createProductTranslationDto.Alias = Utilities.SEOUrl(createProductTranslationDto.ProductName);

            ProductTranslation transalteProduct = _mapper.Map<ProductTranslation>(createProductTranslationDto);

            _context.Add(transalteProduct);

            await _context.SaveChangesAsync();
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

        public async Task<ProductUserViewModel> GetProduct(string languageId)
        {
            if (languageId == null) languageId = _configuration.GetValue<string>("DefaultLanguageId");

            var queryCat = from c in _context.Categories.OrderByDescending(x => x.DateCreated).ThenByDescending(x => x.CatName)
                           join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId into cl
                           from ct in cl.DefaultIfEmpty()

                           where ct.LanguageId == languageId && c.Active == true
                           select new { c, ct };
            var cat = await queryCat.Select(x => new UserCategoryDto()
            {
                CatName = x.ct.CatName,
                Active = x.c.Active,
                ImagePath = x.c.ImagePath,
                Id = x.c.Id,

            }).AsNoTracking().ToListAsync();

            var queryPro = from p in _context.Products.OrderByDescending(x => x.Category.DateCreated).Include(x => x.SubCategorySpecificProducts).ThenInclude(y => y.SubCategory).ThenInclude(z => z.SubCategoryTranslations.Where(x => x.LanguageId == languageId)).AsNoTracking().AsSplitQuery()
                           join c in _context.Categories on p.CategoryId equals c.Id
                           join ct in _context.CategoryTranslations.Where(x => x.LanguageId == languageId) on c.Id equals ct.CategoryId into cd
                           from ct in cd.DefaultIfEmpty()
                           join pt in _context.ProductTranslations on p.Id equals pt.ProductId

                           where pt.LanguageId == languageId && p.Active == true  /*sct.LanguageId == languageId*/
                           select new { c, p, pt, ct /*scsp*/ };
            var productDto = await queryPro.Select(x => new UserProductDto()
            {
                Id = x.p.Id,
                Active = x.p.Active,
                CategoryId = x.p.CategoryId,
                Description = x.pt.Description,
                HomeTag = x.p.HomeTag,
                Alias = x.pt.Alias,
                ImagePathCup = x.p.ImagePathCup,
                ImagePathHero = x.p.ImagePathHero,
                ImagePathIngredients = x.p.ImagePathIngredients,
                ProductName = x.pt.ProductName,
                UserSubCategorySpecificProductDto = _mapper.Map<List<UserSubCategorySpecificProductDto>>(x.p.SubCategorySpecificProducts.Where(x => x.Active == true)),

            }).AsNoTracking().ToListAsync();

            var a = new ProductUserViewModel
            {
                UserCategoryDtos = cat,
                UserProductDtos = productDto,
            };
            return a;
        }

        public async Task<IEnumerable<UserProductDto>> GetProductByCatId(Guid catId, string languageId)
        {
            //var product = await _context.Products.Include(x => x.Category).Include(x => x.SubCategorySpecificProducts).ThenInclude(x => x.SubCategory).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();
            //if(catId != Guid.Empty && popular == false) { product = product.Where(x => x.CategoryId.Equals(catId)).ToList(); }
            //else { product = product.Where(x => x.BestSeller == popular).ToList(); }
            //var productDto = _mapper.Map<List<UserProductDto>>(product);
            //return productDto;

            var query = from p in _context.Products.Include(x => x.SubCategorySpecificProducts).ThenInclude(y => y.SubCategory).ThenInclude(z => z.SubCategoryTranslations.Where(x => x.LanguageId == languageId))
                        join c in _context.Categories on p.CategoryId equals c.Id
                        join ct in _context.CategoryTranslations.Where(x => x.LanguageId == languageId) on c.Id equals ct.CategoryId into cd
                        from ct in cd.DefaultIfEmpty()
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId

                        where pt.LanguageId == languageId && p.Active == true && p.CategoryId.Equals(catId) /*sct.LanguageId == languageId*/
                        select new { c, p, pt, ct /*scsp*/ };


            var productDto = await query.Select(x => new UserProductDto()
            {
                Id = x.p.Id,
                Active = x.p.Active,
                CatName = x.ct.CatName,
                BestSeller = x.p.BestSeller,
                Alias = x.pt.Alias,
                CategoryId = x.p.CategoryId,
                Description = x.pt.Description,
                HomeTag = x.p.HomeTag,
                ImagePathCup = x.p.ImagePathCup,
                ImagePathHero = x.p.ImagePathHero,
                ImagePathIngredients = x.p.ImagePathIngredients,
                ProductName = x.pt.ProductName,
                ShortDesc = x.pt.ShortDesc,
                UserSubCategorySpecificProductDto = _mapper.Map<List<UserSubCategorySpecificProductDto>>(x.p.SubCategorySpecificProducts.Where(x => x.Active == true)),

            }).AsNoTracking().ToListAsync();

            return productDto;



        }

        public async Task<ProductDto> GetProductById(Guid id)
        {
            var product = await _context.Products.Include(x => x.ProductTranslations).Include(x => x.Category).Include(x => x.SubCategorySpecificProducts).ThenInclude(x => x.SubCategory).Include(x => x.SizeSpecificProducts).ThenInclude(x => x.Size).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetProductDtos()
        {
            var product = await _context.Products.Include(x => x.Category).Include(x => x.SizeSpecificProducts).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();

            var productDto = _mapper.Map<List<ProductDto>>(product);

            return productDto;
        }

        public async Task<ProductTranslationDto> GetProductTranslationDto(Guid id)
        {
            var productTranslate = await _context.ProductTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var productTranslateDto = _mapper.Map<ProductTranslationDto>(productTranslate);

            return productTranslateDto;
        }

        public async Task<UserProductDto> GetUserProductById(Guid id, string languageId)
        {
            //var product = await _context.Products.Include(x => x.Category).Include(x => x.SubCategorySpecificProducts).ThenInclude(x => x.SubCategory).Include(x => x.SizeSpecificProducts).ThenInclude(x => x.Size).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var product = await _context.Products.Include(x => x.SubCategorySpecificProducts).ThenInclude(y => y.SubCategory).ThenInclude(z => z.SubCategoryTranslations.Where(x => x.LanguageId == languageId)).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var productTranslation = await _context.ProductTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId.Equals(id) && x.LanguageId == languageId);
            var size = await _context.SizeSpecificEachProduct.Include(x => x.Size).Where(x => x.ProductId.Equals(id)).AsNoTracking().ToListAsync();

            var productDto = new UserProductDto()
            {
                Id = product.Id,
                Active = product.Active,
                BestSeller = product.BestSeller,
                CategoryId = product.CategoryId,
                Description = productTranslation.Description,
                HomeTag = product.HomeTag,
                Alias = productTranslation.Alias,
                ImagePathCup = product.ImagePathCup,
                ImagePathHero = product.ImagePathHero,
                ImagePathIngredients = product.ImagePathIngredients,
                ProductName = productTranslation.ProductName,
                ShortDesc = productTranslation.ShortDesc,
                UserSubCategorySpecificProductDto = _mapper.Map<List<UserSubCategorySpecificProductDto>>(product.SubCategorySpecificProducts.Where(x => x.Active == true)),
                SizeSpecificProducts = _mapper.Map<List<SizeSpecificProductDto>>(size),

            };

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
                using var transaction = _context.Database.BeginTransaction();

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

                var culture = _configuration.GetValue<string>("DefaultLanguageId");

                var translate = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId.Equals(id) && x.LanguageId == culture);

                if (translate != null)
                {
                    var updateTranslate = new UpdateProductTranslationDto()
                    {
                        ProductName = updateProductAdminView.UpdateProductDto.ProductName,
                        Description = updateProductAdminView.UpdateProductDto.Description,
                        ShortDesc = updateProductAdminView.UpdateProductDto.ShortDesc,
                        MetaDesc = updateProductAdminView.UpdateProductDto.MetaDesc,
                        MetaKey = updateProductAdminView.UpdateProductDto.MetaKey,
                        Alias = Utilities.SEOUrl(updateProductAdminView.UpdateProductDto.ProductName),

                    };
                    _context.Update(_mapper.Map(updateTranslate, translate));

                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }

            return;
        }

        public async Task UpdateProductTranslate(Guid proId, Guid id, UpdateProductTranslationDto updateProductTranslationDto)
        {
            var item = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.Id == id);

            if (item != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                updateProductTranslationDto.Alias = Utilities.SEOUrl(updateProductTranslationDto.ProductName);

                _context.Update(_mapper.Map(updateProductTranslationDto, item));

                await _context.SaveChangesAsync();

                var culture = _configuration.GetValue<string>("DefaultLanguageId");

                if (item.LanguageId == culture)
                {
                    var product = await _context.Products.FindAsync(proId);
                    var updateProductDto = new UpdateProductDto()
                    {
                        Active = product.Active,
                        Alias = updateProductTranslationDto.Alias,
                        BestSeller = product.BestSeller,
                        CategoryId = product.CategoryId,
                        DateModified = DateTime.Now,
                        Description = updateProductTranslationDto.Description,
                        Discount = product.Discount,
                        HomeTag = product.HomeTag,
                        ImagePathCup = product.ImagePathCup,
                        ImagePathHero = product.ImagePathHero,
                        ImagePathIngredients = product.ImagePathIngredients,
                        MetaDesc = updateProductTranslationDto.MetaDesc,
                        MetaKey = updateProductTranslationDto.MetaKey,
                        ProductName = updateProductTranslationDto.ProductName,
                        ShortDesc = updateProductTranslationDto.ShortDesc,

                    };
                    _context.Update(_mapper.Map(updateProductDto, product));

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
        }
    }
}
