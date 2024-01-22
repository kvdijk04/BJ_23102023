using AutoMapper;
using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.Category;
using BJ.Contract.Product;
using BJ.Contract.Size;
using BJ.Contract.SubCategory;
using BJ.Contract.Translation.Blog;
using BJ.Contract.Translation.Product;
using BJ.Contract.ViewModel;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace BJ.Application.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductDtos();
        Task<PagedViewModel<ViewAllProduct>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<ProductDto> GetProductById(Guid id);
        Task<UserProductDto> GetUserProductById(Guid id, string languageId);
        Task<ProductTranslationDto> GetProductTranslationDto(Guid id);

        Task<IEnumerable<UserProductDto>> GetProductByCatId(string culture, Guid catId);
        Task CreateProductAdminView(CreateProductAdminView createProductAdminView);
        Task CreateProductTranslate(CreateProductTranslationDto createProductTranslationDto);

        Task UpdateProductAdminView(Guid id, UpdateProductAdminView updateProductAdminView);

        Task UpdateProductTranslate(Guid id, UpdateProductTranslationDto updateProductTranslationDto);

        Task DeleteProductDto(Guid id);

        Task RemoveImage(Guid imageId);

        Task<ProductDto> GetCode(string code);
        Task<ProductUserViewModel> GetProduct(string culture);
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

            createProductAdminView.CreateProduct.DateCreated = DateTime.Now;


            using var transaction = _context.Database.BeginTransaction();

            if (createProductAdminView.ImageCup != null)
            {
                string extension = Path.GetExtension(createProductAdminView.ImageCup.FileName);

                string image = Utilities.SEOUrl(createProductAdminView.CreateProductTranslationDto.ProductName) + "_cup" + extension;

                createProductAdminView.CreateProduct.ImagePathCup = await Utilities.UploadFile(createProductAdminView.ImageCup, "ImageProduct", image);

            }
            if (createProductAdminView.ImageHero != null)
            {
                string extension = Path.GetExtension(createProductAdminView.ImageHero.FileName);

                string image = Utilities.SEOUrl(createProductAdminView.CreateProductTranslationDto.ProductName) + "_hero" + extension;

                createProductAdminView.CreateProduct.ImagePathHero = await Utilities.UploadFile(createProductAdminView.ImageHero, "ImageProduct", image);

            }
            if (createProductAdminView.ImageIngredients != null)
            {
                string extension = Path.GetExtension(createProductAdminView.ImageIngredients.FileName);

                string image = Utilities.SEOUrl(createProductAdminView.CreateProductTranslationDto.ProductName) + "_ingredients" + extension;

                createProductAdminView.CreateProduct.ImagePathIngredients = await Utilities.UploadFile(createProductAdminView.ImageIngredients, "ImageProduct", image);

            }
            if (createProductAdminView.CreateProductTranslationDto.Alias == null)
            {
                createProductAdminView.CreateProductTranslationDto.Alias = Utilities.SEOUrl(createProductAdminView.CreateProductTranslationDto.ProductName);
            }
            if (createProductAdminView.CreateProduct.Sort == null)
            {
                var currentSort = await _context.Categories.OrderByDescending(x => x.Sort).AsNoTracking().ToListAsync();

                createProductAdminView.CreateProduct.Sort = currentSort[0].Sort + 1;
            }

            Product product = _mapper.Map<Product>(createProductAdminView.CreateProduct);

            if (_context.ProductTranslations.Any(x => x.ProductId.Equals(createProductAdminView.CreateProduct.Id) && x.ProductName == createProductAdminView.CreateProductTranslationDto.ProductName) == false)
            {

                _context.Add(product);

                await _context.SaveChangesAsync(createProductAdminView.CreateProduct.UserName);

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
                        ActiveNutri = false,
                        ActiveSize = true,
                    };
                    SizeSpecificEachProduct sizeSpecificEachProduct = _mapper.Map<SizeSpecificEachProduct>(createSizeSpecificProductDto);

                    _context.Add(sizeSpecificEachProduct);

                    await _context.SaveChangesAsync(createProductAdminView.CreateProduct.UserName);
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
                        DateCreated = DateTime.Now,
                    };
                    SubCategorySpecificProduct subCategorySpecificProduct = _mapper.Map<SubCategorySpecificProduct>(createSubCategorySpecificDto);

                    _context.Add(subCategorySpecificProduct);

                    await _context.SaveChangesAsync(createProductAdminView.CreateProduct.UserName);
                }
            }
            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            createProductAdminView.CreateProductTranslationDto.Id = Guid.NewGuid();
            createProductAdminView.CreateProductTranslationDto.ProductId = createProductAdminView.CreateProduct.Id;
            createProductAdminView.CreateProductTranslationDto.Alias = Utilities.SEOUrl(createProductAdminView.CreateProductTranslationDto.ProductName);
            createProductAdminView.CreateProductTranslationDto.LanguageId = defaultLanguage;
            createProductAdminView.CreateProductTranslationDto.DateCreated = DateTime.Now;

            ProductTranslation poductTranslation = _mapper.Map<ProductTranslation>(createProductAdminView.CreateProductTranslationDto);

            _context.Add(poductTranslation);

            await _context.SaveChangesAsync(createProductAdminView.CreateProduct.UserName);


            await transaction.CommitAsync();

            return;
        }

        public async Task CreateProductTranslate(CreateProductTranslationDto createProductTranslationDto)
        {
            var exist = _context.ProductTranslations.Any(x => x.ProductId.Equals(createProductTranslationDto.ProductId) && x.LanguageId == createProductTranslationDto.LanguageId);
            if (exist) return;

            createProductTranslationDto.Id = Guid.NewGuid();

            createProductTranslationDto.Alias = Utilities.SEOUrl(createProductTranslationDto.ProductName);

            createProductTranslationDto.DateCreated = DateTime.Now;

            ProductTranslation transalteProduct = _mapper.Map<ProductTranslation>(createProductTranslationDto);

            _context.Add(transalteProduct);

            await _context.SaveChangesAsync(createProductTranslationDto.UserName);
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

            getListPagingRequest.LanguageId = _configuration.GetValue<string>("DefaultLanguageId"); 

            var pageResult = getListPagingRequest.PageSize; 
            
            var query = from p in _context.Products.OrderBy(x => x.Sort).ThenByDescending(x => x.DateCreated)
                        join c in _context.Categories on p.CategoryId equals c.Id
                        join ct in _context.CategoryTranslations.Where(x => x.LanguageId == getListPagingRequest.LanguageId) on c.Id equals ct.CategoryId into cd
                        from ct in cd.DefaultIfEmpty()
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        where pt.LanguageId == getListPagingRequest.LanguageId && p.Id.Equals(pt.ProductId)
                        select new { p, pt,ct };
            var pageCount = Math.Ceiling(await query.CountAsync() / (double)pageResult);

            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.pt.ProductName.Contains(getListPagingRequest.Keyword) || x.p.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new ViewAllProduct()
                                    {
                                        Id = x.p.Id,
                                        ProductName = x.pt.ProductName,
                                        Code = x.p.Code,
                                        Active = x.p.Active,
                                        BestSeller = x.p.BestSeller,
                                        CategoryName = x.ct.CatName,
                                        Sort = x.p.Sort,
                                        DateActiveForm = x.p.DateActiveForm,
                                        DateTimeActiveTo = x.p.DateTimeActiveTo,
                                        ImageIngredients = x.p.ImagePathIngredients,
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

        public async Task<ProductUserViewModel> GetProduct(string culture)
        {
            if (culture == null) culture = _configuration.GetValue<string>("DefaultLanguageId");

            var queryCat = from c in _context.Categories.OrderBy(x => x.Sort).ThenByDescending(x => x.DateCreated)
                           join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId into cl
                           from ct in cl.DefaultIfEmpty()

                           where ct.LanguageId == culture && c.Active == true
                           select new { c, ct };
            var cat = await queryCat.Select(x => new UserCategoryDto()
            {
                CatName = x.ct.CatName,
                Active = x.c.Active,
                ImagePath = x.c.ImagePath,
                Alias = x.ct.Alias,
                Id = x.c.Id,
                DateActiveForm = x.c.DateActiveForm,
                DateTimeActiveTo = x.c.DateTimeActiveTo,
            }).AsNoTracking().ToListAsync();


            var queryPro = from p in _context.Products.OrderBy(x => x.Category.Sort).ThenByDescending(x => x.Category.DateCreated).Include(x => x.SubCategorySpecificProducts).ThenInclude(y => y.SubCategory).ThenInclude(z => z.SubCategoryTranslations.Where(x => x.LanguageId == culture)).AsNoTracking().AsSingleQuery()
                           join c in _context.Categories on p.CategoryId equals c.Id
                           join ct in _context.CategoryTranslations.Where(x => x.LanguageId == culture) on c.Id equals ct.CategoryId into cd
                           from ct in cd.DefaultIfEmpty()
                           join pt in _context.ProductTranslations on p.Id equals pt.ProductId

                           where pt.LanguageId == culture && p.Active == true  /*sct.LanguageId == languageId*/ 
                           select new { c, p, pt, ct /*scsp*/ };

            var productDto = await queryPro.Select(x => new UserProductDto()
            {
                Id = x.p.Id,
                Active = x.p.Active,
                CategoryId = x.p.CategoryId,
                Alias = x.pt.Alias,
                CatName = x.ct.CatName,
                ImagePathCup = x.p.ImagePathCup,
                ImagePathHero = x.p.ImagePathHero,
                ImagePathIngredients = x.p.ImagePathIngredients,
                ProductName = x.pt.ProductName,
                Sort = x.p.Sort,
                DateActiveForm = x.p.DateActiveForm,
                DateCreated = x.p.DateCreated,
                DateTimeActiveTo = x.p.DateTimeActiveTo,
                UserSubCategorySpecificProductDto = _mapper.Map<List<UserSubCategorySpecificProductDto>>(x.p.SubCategorySpecificProducts.Where(x => x.Active == true)),

            }).AsNoTracking().ToListAsync();

            var a = new ProductUserViewModel
            {
                UserCategoryDtos = cat,
                UserProductDtos = productDto,
            };
            return a;
        }

        public async Task<IEnumerable<UserProductDto>> GetProductByCatId(string culture, Guid catId)
        {

            //var product = await _context.Products.Include(x => x.Category).Include(x => x.SubCategorySpecificProducts).ThenInclude(x => x.SubCategory).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();
            //if(catId != Guid.Empty && popular == false) { product = product.Where(x => x.CategoryId.Equals(catId)).ToList(); }
            //else { product = product.Where(x => x.BestSeller == popular).ToList(); }
            //var productDto = _mapper.Map<List<UserProductDto>>(product);
            //return productDto;

            var query = from p in _context.Products.OrderBy(x => x.Sort).ThenByDescending(x => x.DateCreated).Include(x => x.SubCategorySpecificProducts).ThenInclude(y => y.SubCategory).ThenInclude(z => z.SubCategoryTranslations.Where(x => x.LanguageId == culture))
                        join c in _context.Categories on p.CategoryId equals c.Id
                        join ct in _context.CategoryTranslations.Where(x => x.LanguageId == culture) on c.Id equals ct.CategoryId into cd
                        from ct in cd.DefaultIfEmpty()
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId

                        where pt.LanguageId == culture && p.Active == true && p.CategoryId.Equals(catId) /*sct.LanguageId == languageId*/
                        select new { c, p, pt, ct /*scsp*/ };


            var productDto = await query.Select(x => new UserProductDto()
            {
                Id = x.p.Id,
                Active = x.p.Active,
                CatName = x.ct.CatName,
                Alias = x.pt.Alias,
                CategoryId = x.p.CategoryId,
                ImagePathCup = x.p.ImagePathCup,
                ImagePathHero = x.p.ImagePathHero,
                ImagePathIngredients = x.p.ImagePathIngredients,
                ProductName = x.pt.ProductName,
                DateActiveForm = x.p.DateActiveForm,
                DateTimeActiveTo = x.p.DateTimeActiveTo,
                UserSubCategorySpecificProductDto = _mapper.Map<List<UserSubCategorySpecificProductDto>>(x.p.SubCategorySpecificProducts.Where(x => x.Active == true)),

            }).AsNoTracking().ToListAsync();

            return productDto;



        }

        public async Task<ProductDto> GetProductById(Guid id)
        {
            var culture = _configuration.GetValue<string>("DefaultLanguageId");
            var product = await _context.Products.Include(x => x.ProductTranslations).Include(x => x.SubCategorySpecificProducts).ThenInclude(x => x.SubCategory).ThenInclude(x => x.SubCategoryTranslations.Where(x => x.LanguageId == culture)).Include(x => x.SizeSpecificProducts).ThenInclude(x => x.Size).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var catName = await _context.CategoryTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.CategoryId.Equals(product.CategoryId) && x.LanguageId == culture);
            var productTranslate = await _context.ProductTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId.Equals(id) && x.LanguageId == culture);
            ProductDto productDto = new()
            {
                Id = product.Id,
                Active = product.Active,
                BestSeller = product.BestSeller,
                CategoryId = product.CategoryId,
                Code = product.Code,
                DateCreated = product.DateCreated,
                DateModified = product.DateModified,
                Discount = product.Discount,
                ProductName = productTranslate.ProductName,
                Alias = productTranslate.Alias,
                Description = productTranslate.Description,
                ShortDesc = productTranslate.ShortDesc,
                ImagePathCup = product.ImagePathCup,
                ImagePathIngredients = product.ImagePathIngredients,
                ImagePathHero = product.ImagePathHero,
                HomeTag = product.HomeTag,
                MetaDesc = productTranslate.MetaDesc,
                MetaKey = productTranslate.MetaKey,
                CatName = catName.CatName,
                Sort = product.Sort,
                DateActiveForm = product.DateActiveForm,
                DateTimeActiveTo = product.DateTimeActiveTo,
                ProductTranslationDtos = _mapper.Map<List<ProductTranslationDto>>(await _context.ProductTranslations.Where(x => x.ProductId.Equals(id)).Select(x => new ProductTranslation
                {
                    LanguageId = x.LanguageId,
                    Id = x.Id,
                    ProductName = x.ProductName,

                }).ToListAsync()),
                SizeSpecificProducts = _mapper.Map<List<SizeSpecificProductDto>>(product.SizeSpecificProducts.Select(x => new SizeSpecificEachProduct()
                {
                    Id = x.Id,
                    ActiveNutri = x.ActiveNutri,
                    ActiveSize = x.ActiveSize,
                    Cal = x.Cal,
                    Carbonhydrate = x.Carbonhydrate,
                    CarbonhydrateSugar = x.CarbonhydrateSugar,
                    DietaryFibre = x.DietaryFibre,
                    Energy = x.Energy,
                    Fat = x.Fat,
                    FatSaturated = x.FatSaturated,
                    Protein = x.Protein,
                    Sodium = x.Sodium,
                    ProductId = x.ProductId,
                    SizeId = x.SizeId,
                    Size = new Size
                    {
                        Name = x.Size.Name,
                    }
                })),
                SubCategorySpecificProductDtos = _mapper.Map<List<SubCategorySpecificProductDto>>(product.SubCategorySpecificProducts),
            };

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

                    string image = Utilities.SEOUrl(updateProductAdminView.UpdateProductTranslationDto.ProductName) + "_cup" + extension;


                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageProduct", image);

                    var fileExist = File.Exists(imagePath);

                    if (fileExist == true)
                    {
                        File.Delete(imagePath);

                    }

                    updateProductAdminView.UpdateProductDto.ImagePathCup = await Utilities.UploadFile(updateProductAdminView.ImageCup, "ImageProduct", image);

                }
                if (updateProductAdminView.ImageHero != null)
                {

                    string extension = Path.GetExtension(updateProductAdminView.ImageHero.FileName);

                    string image = Utilities.SEOUrl(updateProductAdminView.UpdateProductTranslationDto.ProductName) + "_hero" + extension;

                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageProduct", image);

                    var fileExist = File.Exists(imagePath);

                    if (fileExist == true)
                    {
                        File.Delete(imagePath);

                    }

                    updateProductAdminView.UpdateProductDto.ImagePathHero = await Utilities.UploadFile(updateProductAdminView.ImageHero, "ImageProduct", image);

                }
                if (updateProductAdminView.ImageIngredients != null)
                {
                    string extension = Path.GetExtension(updateProductAdminView.ImageIngredients.FileName);

                    string image = Utilities.SEOUrl(updateProductAdminView.UpdateProductTranslationDto.ProductName) + "_ingredients" + extension;

                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageProduct", image);

                    var fileExist = File.Exists(imagePath);

                    if (fileExist == true)
                    {
                        File.Delete(imagePath);

                    }

                    updateProductAdminView.UpdateProductDto.ImagePathIngredients = await Utilities.UploadFile(updateProductAdminView.ImageIngredients, "ImageProduct", image);

                }
                if (updateProductAdminView.UpdateProductDto.Sort == null)
                {
                    var currentSort = await _context.Categories.OrderByDescending(x => x.Sort).AsNoTracking().ToListAsync();

                    updateProductAdminView.UpdateProductDto.Sort = currentSort[1].Sort + 1;
                }

                updateProductAdminView.UpdateProductTranslationDto.Alias = Utilities.SEOUrl(updateProductAdminView.UpdateProductTranslationDto.ProductName);

                updateProductAdminView.UpdateProductDto.DateModified = DateTime.Now;

               
                _context.Entry(item).CurrentValues.SetValues(updateProductAdminView.UpdateProductDto);

                await _context.SaveChangesAsync(updateProductAdminView.UpdateProductDto.UserName);

                var culture = _configuration.GetValue<string>("DefaultLanguageId");

                var translate = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId.Equals(id) && x.LanguageId == culture);

                if (translate != null)
                {
                    updateProductAdminView.UpdateProductTranslationDto.Alias = Utilities.SEOUrl(updateProductAdminView.UpdateProductTranslationDto.Alias);
                    updateProductAdminView.UpdateProductDto.DateModified = DateTime.Now;
                    _context.Entry(translate).CurrentValues.SetValues(updateProductAdminView.UpdateProductTranslationDto);

                    await _context.SaveChangesAsync(updateProductAdminView.UpdateProductDto.UserName);
                }
                await transaction.CommitAsync();
            }

            return;
        }

        public async Task UpdateProductTranslate(Guid id, UpdateProductTranslationDto updateProductTranslationDto)
        {
            var item = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.Id == id);

            if (item != null)
            {

                updateProductTranslationDto.Alias = Utilities.SEOUrl(updateProductTranslationDto.ProductName);
                
                updateProductTranslationDto.DateUpdated = DateTime.Now;

                _context.Entry(item).CurrentValues.SetValues(updateProductTranslationDto);

                await _context.SaveChangesAsync(updateProductTranslationDto.UserName);


                var culture = _configuration.GetValue<string>("DefaultLanguageId");

            }
        }
    }
}
