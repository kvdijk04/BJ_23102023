using AutoMapper;
using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.Category;
using BJ.Contract.SubCategory;
using BJ.Contract.Translation.Category;
using BJ.Contract.Translation.SubCategory;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategoryDtos();
        Task<IEnumerable<UserCategoryDto>> GetUserCategoryDtos(string languageId);

        Task<IEnumerable<SubCategoryDto>> GetSubCategoryDtos();
        Task<PagedViewModel<CategoryDto>> GetPagingCategory([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<PagedViewModel<SubCategoryDto>> GetPagingSubCategory([FromQuery] GetListPagingRequest getListPagingRequest);

        Task CreateCategory(CreateCategoryDto createCategoryDto);

        Task CreateSubCategory(CreateSubCategoryDto createSubCategoryDto);

        Task CreateSubCategorySpecific(CreateSubCategorySpecificDto createSubCategorySpecificDto);

        Task UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto);
        Task UpdateSubCategory(int id, UpdateSubCategoryDto updateSubCategoryDto);

        Task<CategoryDto> GetCategoryById(Guid id);
        Task<SubCategoryDto> GetSubCategoryById(int id);

        Task<CategoryTranslationDto> GetCategoryTransalationById(Guid id);

        Task CreateTranslateCategory(CreateCategoryTranslationDto createCategoryTranslationDto);
        Task UpdateTranslateCategory(Guid catId, Guid id, UpdateCategoryTranslationDto updateCategoryTranslationDto);


        Task<SubCategoryTranslationDto> GetSubCategoryTransalationById(Guid id);

        Task CreateSubCategoryTranslate(CreateSubCategoryTranslationDto createSubCategoryTranslationDto);
        Task UpdateSubCategoryTranslate(int subCatId, Guid id, UpdateSubCategoryTranslationDto updateSubCategoryTranslationDto);

        Task<Guid> GetIdOfCategorỵ(string code);

    }
    public class CategoryService : ICategoryService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public CategoryService(BJContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task CreateCategory(CreateCategoryDto createCategoryDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            createCategoryDto.Id = Guid.NewGuid();

            createCategoryDto.Alias = Utilities.SEOUrl(createCategoryDto.CatName);

            var code = _configuration.GetValue<string>("Code:Category");


            var total = await _context.Categories.OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();

            string s = null;
            var codeLimit = _configuration.GetValue<string>("LimitCode");

            if (total.Count == 0) { createCategoryDto.Code = code + codeLimit; }

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

                createCategoryDto.Code = code + s;
            }
            if (createCategoryDto.Image != null)
            {
                string extension = Path.GetExtension(createCategoryDto.Image.FileName);

                string image = Utilities.SEOUrl(createCategoryDto.CatName) + extension;

                createCategoryDto.ImagePath = await Utilities.UploadFile(createCategoryDto.Image, "ImageCategory", image);
            }


            createCategoryDto.DateCreated = DateTime.Now;
            createCategoryDto.DateUpdated = DateTime.Now;
            Category category = _mapper.Map<Category>(createCategoryDto);

            _context.Add(category);

            await _context.SaveChangesAsync();

            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");
            CreateCategoryTranslationDto createCategoryTranslationDto = new()
            {
                Id = Guid.NewGuid(),
                CategoryId = createCategoryDto.Id,
                CatName = createCategoryDto.CatName,
                Alias = Utilities.SEOUrl(createCategoryDto.CatName),
                Description = createCategoryDto.Description,
                MetaDesc = createCategoryDto.MetaDesc,
                LanguageId = defaultLanguage,
            };
            CategoryTranslation categoryTranslation = _mapper.Map<CategoryTranslation>(createCategoryTranslationDto);

            _context.Add(categoryTranslation);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        public async Task CreateSubCategory(CreateSubCategoryDto createSubCategoryDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            createSubCategoryDto.DateCreated = DateTime.Now;

            createSubCategoryDto.DateUpdated = DateTime.Now;

            var code = _configuration.GetValue<string>("Code:SubCategory");

            var total = await _context.SubCategories.OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();

            string s = null;
            var codeLimit = _configuration.GetValue<string>("LimitCode");

            if (total.Count == 0) { createSubCategoryDto.Code = code + codeLimit; }

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

                createSubCategoryDto.Code = code + s;

            }
            if (createSubCategoryDto.Image != null)
            {
                string extension = Path.GetExtension(createSubCategoryDto.Image.FileName);

                string image = Utilities.SEOUrl(createSubCategoryDto.SubCatName) + extension;

                createSubCategoryDto.ImagePath = await Utilities.UploadFile(createSubCategoryDto.Image, "ImageSubCategory", image);
            }



            SubCategory category = _mapper.Map<SubCategory>(createSubCategoryDto);

            _context.Add(category);

            await _context.SaveChangesAsync();

            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            CreateSubCategoryTranslationDto createSubCategoryTranslationDto = new()
            {
                Id = Guid.NewGuid(),
                SubCategoryId = category.Id,
                SubCatName = createSubCategoryDto.SubCatName,
                Description = createSubCategoryDto.Description,
                LanguageId = defaultLanguage,

            };
            SubCategoryTranslation categoryTranslation = _mapper.Map<SubCategoryTranslation>(createSubCategoryTranslationDto);

            _context.Add(categoryTranslation);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        public async Task CreateSubCategorySpecific(CreateSubCategorySpecificDto createSubCategorySpecificDto)
        {
            createSubCategorySpecificDto.Id = Guid.NewGuid();

            SizeSpecificEachProduct sizeSpecificEachProduct = _mapper.Map<SizeSpecificEachProduct>(createSubCategorySpecificDto);

            _context.Add(sizeSpecificEachProduct);
            await _context.SaveChangesAsync();
        }

        public async Task CreateTranslateCategory(CreateCategoryTranslationDto createCategoryTranslationDto)
        {
            createCategoryTranslationDto.Id = Guid.NewGuid();

            createCategoryTranslationDto.Alias = Utilities.SEOUrl(createCategoryTranslationDto.CatName);

            CategoryTranslation transaltecategory = _mapper.Map<CategoryTranslation>(createCategoryTranslationDto);

            _context.Add(transaltecategory);

            await _context.SaveChangesAsync();
        }

        public async Task<CategoryDto> GetCategoryById(Guid id)
        {
            var cat = await _context.Categories.Include(x => x.CategoryTranslations).FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (cat == null) return null;

            return _mapper.Map<CategoryDto>(cat);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoryDtos()
        {
            var category = await _context.Categories.Include(x => x.Products.Where(x => x.BestSeller == true)).AsNoTracking().ToListAsync();
            var categoryDto = _mapper.Map<List<CategoryDto>>(category);

            return categoryDto;
        }

        public async Task<PagedViewModel<CategoryDto>> GetPagingCategory([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:Category"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.Categories.Count() / (double)pageResult);
            var query = _context.Categories.OrderByDescending(x => x.DateCreated).AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.CatName.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new CategoryDto()
                                    {
                                        Id = x.Id,
                                        CatName = x.CatName,
                                        Code = x.Code,
                                        Active = x.Active,
                                        ImagePath = x.ImagePath,
                                    }).ToListAsync();
            var categoryResponse = new PagedViewModel<CategoryDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return categoryResponse;
        }
        public async Task<PagedViewModel<SubCategoryDto>> GetPagingSubCategory([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:SubCategory"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.SubCategories.Count() / (double)pageResult);
            var query = _context.SubCategories.OrderByDescending(x => x.DateCreated).AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.SubCatName.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new SubCategoryDto()
                                    {
                                        Id = x.Id,
                                        SubCatName = x.SubCatName,
                                        Active = x.Active,
                                        ImagePath = x.ImagePath,
                                    }).ToListAsync();
            var subCategoryResponse = new PagedViewModel<SubCategoryDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return subCategoryResponse;
        }

        public async Task<SubCategoryDto> GetSubCategoryById(int id)
        {
            var subCat = await _context.SubCategories.Include(x => x.SubCategoryTranslations).FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (subCat == null) return null;

            return _mapper.Map<SubCategoryDto>(subCat);
        }

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategoryDtos()
        {
            var subCategory = await _context.SubCategories.Where(x => x.Active == true).AsNoTracking().ToListAsync();
            var subCategoryDto = _mapper.Map<List<SubCategoryDto>>(subCategory);

            return subCategoryDto;
        }

        public async Task<CategoryTranslationDto> GetCategoryTransalationById(Guid id)
        {
            var categoryTranslate = await _context.CategoryTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var categoryTranslateDto = _mapper.Map<CategoryTranslationDto>(categoryTranslate);

            return categoryTranslateDto;
        }

        public async Task<IEnumerable<UserCategoryDto>> GetUserCategoryDtos(string languageId)
        {
            if (languageId == null) languageId = _configuration.GetValue<string>("DefaultLanguageId");

            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId into cl
                        from ct in cl.DefaultIfEmpty()
                        where ct.LanguageId == languageId && c.Active == true
                        select new { c, ct };
            var cat = await query.Select(x => new UserCategoryDto()
            {
                CatName = x.ct.CatName,
                Active = x.c.Active,
                ImagePath = x.c.ImagePath,
                Id = x.c.Id,


            }).ToListAsync();

            return cat;
        }

        public async Task UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto)
        {
            var item = await _context.Categories.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                updateCategoryDto.Alias = Utilities.SEOUrl(updateCategoryDto.CatName);

                if (updateCategoryDto.Image != null)
                {
                    string extension = Path.GetExtension(updateCategoryDto.Image.FileName);

                    string image = Utilities.SEOUrl(updateCategoryDto.CatName) + extension;

                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageCategory", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }
                    updateCategoryDto.ImagePath = await Utilities.UploadFile(updateCategoryDto.Image, "ImageCategory", image);

                }



                updateCategoryDto.DateUpdated = DateTime.Now;



                _context.Update(_mapper.Map(updateCategoryDto, item));

                await _context.SaveChangesAsync();

                var culture = _configuration.GetValue<string>("DefaultLanguageId");

                var translate = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId.Equals(id) && x.LanguageId == culture);

                if (translate != null)
                {
                    var updateTranslate = new UpdateCategoryTranslationDto()
                    {
                        CatName = updateCategoryDto.CatName,
                        Description = updateCategoryDto.Description,
                        MetaDesc = updateCategoryDto.MetaDesc,
                        Alias = Utilities.SEOUrl(updateCategoryDto.CatName),

                    };
                    _context.Update(_mapper.Map(updateTranslate, translate));

                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }

        }

        public async Task UpdateSubCategory(int id, UpdateSubCategoryDto updateSubCategoryDto)
        {
            var item = await _context.SubCategories.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                if (updateSubCategoryDto.Image != null)
                {
                    string extension = Path.GetExtension(updateSubCategoryDto.Image.FileName);

                    string image = Utilities.SEOUrl(updateSubCategoryDto.SubCatName) + extension;

                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageSubCategory", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }


                    updateSubCategoryDto.ImagePath = await Utilities.UploadFile(updateSubCategoryDto.Image, "ImageSubCategory", image);
                }


                updateSubCategoryDto.DateUpdated = DateTime.Now;

                _context.Update(_mapper.Map(updateSubCategoryDto, item));

                await _context.SaveChangesAsync();

                var culture = _configuration.GetValue<string>("DefaultLanguageId");

                var translate = await _context.SubCategoryTranslations.FirstOrDefaultAsync(x => x.SubCategoryId.Equals(id) && x.LanguageId == culture);

                if (translate != null)
                {
                    var updateSubCategoryTranslate = new UpdateSubCategoryTranslationDto()
                    {
                        SubCatName = updateSubCategoryDto.SubCatName,
                        Description = updateSubCategoryDto.Description,

                    };
                    _context.Update(_mapper.Map(updateSubCategoryTranslate, translate));

                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }
        }

        public async Task UpdateTranslateCategory(Guid catId, Guid id, UpdateCategoryTranslationDto updateCategoryTranslationDto)
        {
            var item = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.Id.Equals(id));


            if (item != null)
            {

                using var transaction = _context.Database.BeginTransaction();

                updateCategoryTranslationDto.Alias = Utilities.SEOUrl(updateCategoryTranslationDto.CatName);

                _context.Update(_mapper.Map(updateCategoryTranslationDto, item));

                await _context.SaveChangesAsync();

                var culture = _configuration.GetValue<string>("DefaultLanguageId");

                if (item.LanguageId == culture)
                {
                    var category = await _context.Categories.FindAsync(catId);

                    var updateCategoryDto = new UpdateCategoryDto()
                    {
                        Active = category.Active,
                        Alias = updateCategoryTranslationDto.CatName,
                        Description = updateCategoryTranslationDto.Description,
                        ImagePath = category.ImagePath,
                        DateUpdated = DateTime.Now,
                        MetaDesc = updateCategoryTranslationDto.MetaDesc,
                        MetaKey = category.MetaKey,
                        CatName = updateCategoryTranslationDto.CatName,
                    };
                    _context.Update(_mapper.Map(updateCategoryDto, category));

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
        }

        public async Task<SubCategoryTranslationDto> GetSubCategoryTransalationById(Guid id)
        {
            var subCategoryTranslate = await _context.SubCategoryTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var subCategoryTranslateDto = _mapper.Map<SubCategoryTranslationDto>(subCategoryTranslate);

            return subCategoryTranslateDto;
        }

        public async Task CreateSubCategoryTranslate(CreateSubCategoryTranslationDto createSubCategoryTranslationDto)
        {
            createSubCategoryTranslationDto.Id = Guid.NewGuid();


            SubCategoryTranslation transaltecategory = _mapper.Map<SubCategoryTranslation>(createSubCategoryTranslationDto);

            _context.Add(transaltecategory);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubCategoryTranslate(int subCatId, Guid id, UpdateSubCategoryTranslationDto updateSubCategoryTranslationDto)
        {
            var item = await _context.SubCategoryTranslations.FirstOrDefaultAsync(x => x.Id.Equals(id));


            if (item != null)
            {
                using var transaction = _context.Database.BeginTransaction();


                _context.Update(_mapper.Map(updateSubCategoryTranslationDto, item));

                await _context.SaveChangesAsync();
                var culture = _configuration.GetValue<string>("DefaultLanguageId");

                if (item.LanguageId == culture)
                {
                    var subCategory = await _context.SubCategories.FindAsync(subCatId);
                    var updateSubCategoryDto = new UpdateSubCategoryDto()
                    {
                        Active = subCategory.Active,
                        Description = updateSubCategoryTranslationDto.Description,
                        ImagePath = subCategory.ImagePath,
                        DateUpdated = DateTime.Now,
                        SubCatName = updateSubCategoryTranslationDto.SubCatName,
                    };
                    _context.Update(_mapper.Map(updateSubCategoryDto, subCategory));

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
        }

        public async Task<Guid> GetIdOfCategorỵ(string code)
        {
            var r = await _context.Categories.FirstOrDefaultAsync(x => x.Code == code);
            if (r == null) return Guid.Empty;

            return r.Id;
        }
    }
}
