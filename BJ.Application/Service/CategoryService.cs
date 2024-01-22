using AutoMapper;
using BJ.Application.Helper;
using BJ.Application.Ultities;
using BJ.Contract.Category;
using BJ.Contract.StoreLocation;
using BJ.Contract.SubCategory;
using BJ.Contract.Translation.Blog;
using BJ.Contract.Translation.Category;
using BJ.Contract.Translation.News;
using BJ.Contract.Translation.Product;
using BJ.Contract.Translation.SubCategory;
using BJ.Contract.ViewModel;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace BJ.Application.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategoryDtos();
        Task<IEnumerable<UserCategoryDto>> GetUserCategoryDtos(string languageId);

        Task<IEnumerable<SubCategoryDto>> GetSubCategoryDtos();
        Task<PagedViewModel<CategoryDto>> GetPagingCategory([FromQuery] GetListPagingRequest getListPagingRequest);
        Task<PagedViewModel<SubCategoryDto>> GetPagingSubCategory([FromQuery] GetListPagingRequest getListPagingRequest);

        Task CreateCategory(CreateCategoryAdminView createCategoryAdminView);

        Task CreateSubCategory(CreateSubCategoryAdminView createSubCategoryAdminView);

        Task CreateSubCategorySpecific(CreateSubCategorySpecificDto createSubCategorySpecificDto);

        Task UpdateCategory(Guid id, UpdateCategoryAdminView updateCategoryAdminView);
        Task UpdateSubCategory(int id, UpdateSubCategoryAdminView updateSubCategoryAdminView);

        Task<CategoryDto> GetCategoryById(Guid id);
        Task<SubCategoryDto> GetSubCategoryById(int id);

        Task<CategoryTranslationDto> GetCategoryTranslationById(Guid id);

        Task CreateTranslateCategory(CreateCategoryTranslationDto createCategoryTranslationDto);
        Task UpdateTranslateCategory(Guid id, UpdateCategoryTranslationDto updateCategoryTranslationDto);


        Task<SubCategoryTranslationDto> GetSubCategoryTranslationById(Guid id);

        Task CreateSubCategoryTranslate(CreateSubCategoryTranslationDto createSubCategoryTranslationDto);
        Task UpdateSubCategoryTranslate(Guid id, UpdateSubCategoryTranslationDto updateSubCategoryTranslationDto);

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

        public async Task CreateCategory(CreateCategoryAdminView createCategoryAdminView)
        {
            using var transaction = _context.Database.BeginTransaction();

            createCategoryAdminView.CreateCategoryDto.Id = Guid.NewGuid();

            createCategoryAdminView.CreateCategoryTranslationDto.Alias = Utilities.SEOUrl(createCategoryAdminView.CreateCategoryTranslationDto.CatName);

            var code = _configuration.GetValue<string>("Code:Category");


            var total = await _context.Categories.OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();

            string s = null;
            var codeLimit = _configuration.GetValue<string>("LimitCode");

            if (total.Count == 0) { createCategoryAdminView.CreateCategoryDto.Code = code + codeLimit; }

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

                createCategoryAdminView.CreateCategoryDto.Code = code + s;
            }
            if (createCategoryAdminView.Image != null)
            {
                string extension = Path.GetExtension(createCategoryAdminView.Image.FileName);

                string image = Utilities.SEOUrl(createCategoryAdminView.CreateCategoryTranslationDto.CatName) + extension;

                createCategoryAdminView.CreateCategoryDto.ImagePath = await Utilities.UploadFile(createCategoryAdminView.Image, "ImageCategory", image);
            }

            if (createCategoryAdminView.CreateCategoryDto.Sort == null)
            {
                var currentSort = await _context.Categories.OrderByDescending(x => x.Sort).AsNoTracking().ToListAsync();

                createCategoryAdminView.CreateCategoryDto.Sort = currentSort[0].Sort + 1;
            }

            createCategoryAdminView.CreateCategoryDto.DateCreated = DateTime.Now;
            Category category = _mapper.Map<Category>(createCategoryAdminView.CreateCategoryDto);

            _context.Add(category);

            await _context.SaveChangesAsync(createCategoryAdminView.CreateCategoryDto.UserName);

            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");
            createCategoryAdminView.CreateCategoryTranslationDto.CategoryId = createCategoryAdminView.CreateCategoryDto.Id;
            createCategoryAdminView.CreateCategoryTranslationDto.Alias = Utilities.SEOUrl(createCategoryAdminView.CreateCategoryTranslationDto.Alias);
            createCategoryAdminView.CreateCategoryTranslationDto.LanguageId = defaultLanguage;
            createCategoryAdminView.CreateCategoryTranslationDto.DateCreated = DateTime.Now;

           
            CategoryTranslation categoryTranslation = _mapper.Map<CategoryTranslation>(createCategoryAdminView.CreateCategoryTranslationDto);

            _context.Add(categoryTranslation);

            await _context.SaveChangesAsync(createCategoryAdminView.CreateCategoryDto.UserName);

            await transaction.CommitAsync();
        }

        public async Task CreateSubCategory(CreateSubCategoryAdminView createSubCategoryAdminView)
        {
            using var transaction = _context.Database.BeginTransaction();

            createSubCategoryAdminView.CreateSubCategoryDto.DateCreated = DateTime.Now;

            var code = _configuration.GetValue<string>("Code:SubCategory");

            var total = await _context.SubCategories.OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();

            string s = null;
            var codeLimit = _configuration.GetValue<string>("LimitCode");

            if (total.Count == 0) { createSubCategoryAdminView.CreateSubCategoryDto.Code = code + codeLimit; }

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

                createSubCategoryAdminView.CreateSubCategoryDto.Code = code + s;

            }
            if (createSubCategoryAdminView.Image != null)
            {
                string extension = Path.GetExtension(createSubCategoryAdminView.Image.FileName);

                string image = Utilities.SEOUrl(createSubCategoryAdminView.CreateSubCategoryTranslationDto.SubCatName) + extension;

                createSubCategoryAdminView.CreateSubCategoryDto.ImagePath = await Utilities.UploadFile(createSubCategoryAdminView.Image, "ImageSubCategory", image);
            }



            SubCategory category = _mapper.Map<SubCategory>(createSubCategoryAdminView.CreateSubCategoryDto);

            _context.Add(category);

            await _context.SaveChangesAsync(createSubCategoryAdminView.CreateSubCategoryDto.UserName);

            var defaultLanguage = _configuration.GetValue<string>("DefaultLanguageId");

            createSubCategoryAdminView.CreateSubCategoryTranslationDto.Id = Guid.NewGuid();
            createSubCategoryAdminView.CreateSubCategoryTranslationDto.SubCategoryId = category.Id;
            createSubCategoryAdminView.CreateSubCategoryTranslationDto.DateCreated = DateTime.Now;
            createSubCategoryAdminView.CreateSubCategoryTranslationDto.LanguageId = defaultLanguage;

            SubCategoryTranslation categoryTranslation = _mapper.Map<SubCategoryTranslation>(createSubCategoryAdminView.CreateSubCategoryTranslationDto);

            _context.Add(categoryTranslation);

            await _context.SaveChangesAsync(createSubCategoryAdminView.CreateSubCategoryDto.UserName);

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
            var exist = _context.CategoryTranslations.Any(x => x.CategoryId.Equals(createCategoryTranslationDto.CategoryId) && x.LanguageId == createCategoryTranslationDto.LanguageId);
            if (exist) return;
            createCategoryTranslationDto.Id = Guid.NewGuid();

            createCategoryTranslationDto.Alias = Utilities.SEOUrl(createCategoryTranslationDto.CatName);

            createCategoryTranslationDto.DateCreated = DateTime.Now;
            CategoryTranslation transaltecategory = _mapper.Map<CategoryTranslation>(createCategoryTranslationDto);

            _context.Add(transaltecategory);

            await _context.SaveChangesAsync(createCategoryTranslationDto.UserName);
        }

        public async Task<CategoryDto> GetCategoryById(Guid id)
        {
            var item = await _context.Categories.FindAsync(id);

            if (item == null) return null;

            var culture = _configuration.GetValue<string>("DefaultLanguageId");

            var categoryTranslation = await _context.CategoryTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.LanguageId == culture && x.CategoryId.Equals(id));

            var subCat = new CategoryDto()
            {
                Id = item.Id,
                DateCreated = item.DateCreated,
                Description = categoryTranslation != null ? categoryTranslation.Description : null,
                CatName = categoryTranslation != null ? categoryTranslation.CatName : null,
                Alias = categoryTranslation != null ? categoryTranslation.Alias : null,
                Code = item.Code,
                DateUpdated = item.DateUpdated,
                Active = item.Active,
                ImagePath = item != null ? item.ImagePath : null,
                Sort = item.Sort,
                DateActiveForm = item != null ? item.DateActiveForm : null,
                DateTimeActiveTo = item != null ? item.DateTimeActiveTo : null,
                CategoryTranslationDtos = _mapper.Map<List<CategoryTranslationDto>>
                (await _context.CategoryTranslations.Where(x => x.CategoryId.Equals(id)).Select(x => new CategoryTranslation
                {
                    LanguageId = x.LanguageId,
                    Id = x.Id,
                    CatName = x.CatName

                }).ToListAsync())
            };
            return subCat;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoryDtos()
        {
            var languageId = _configuration.GetValue<string>("DefaultLanguageId");

            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId into cl
                        from ct in cl.DefaultIfEmpty()
                        where ct.LanguageId == languageId
                        select new { c, ct };
            var cat = await query.Select(x => new CategoryDto()
            {
                CatName = x.ct.CatName,
                Active = x.c.Active,
                ImagePath = x.c.ImagePath,
                Id = x.c.Id,


            }).ToListAsync();

            return cat;
        }

        public async Task<PagedViewModel<CategoryDto>> GetPagingCategory([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:Category"));
            }
            var pageResult = getListPagingRequest.PageSize;

            getListPagingRequest.LanguageId = _configuration.GetValue<string>("DefaultLanguageId");

            var query = from c in _context.Categories.OrderBy(x => x.Sort).ThenByDescending(x => x.DateCreated)
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId into cl
                        from ct in cl.DefaultIfEmpty()
                        where ct.LanguageId == getListPagingRequest.LanguageId 
                        select new { c, ct };
            var pageCount = Math.Ceiling(await query.CountAsync() / (double)pageResult);

            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.ct.CatName.Contains(getListPagingRequest.Keyword) || x.c.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new CategoryDto()
                                    {
                                        Id = x.c.Id,
                                        CatName = x.ct.CatName,
                                        Code = x.c.Code,
                                        Active = x.c.Active,
                                        ImagePath = x.c.ImagePath,
                                        Sort = x.c.Sort,
                                        DateActiveForm = x.c.DateActiveForm,
                                        DateTimeActiveTo = x.c.DateTimeActiveTo,
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

            getListPagingRequest.LanguageId = _configuration.GetValue<string>("DefaultLanguageId");

            var query = from sc in _context.SubCategories
                        join sct in _context.SubCategoryTranslations on sc.Id equals sct.SubCategoryId into cl
                        from sct in cl.DefaultIfEmpty()
                        where sct.LanguageId == getListPagingRequest.LanguageId
                        select new { sc, sct };
            var pageCount = Math.Ceiling(await query.CountAsync() / (double)pageResult);

            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.sct.SubCatName.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new SubCategoryDto()
                                    {
                                        Id = x.sc.Id,
                                        SubCatName = x.sct.SubCatName,
                                        Active = x.sc.Active,
                                        ImagePath = x.sc.ImagePath,
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
            var item = await _context.SubCategories.FindAsync(id);

            if(item == null) return null;

            var culture = _configuration.GetValue<string>("DefaultLanguageId");

            var subCategoryTranslation = await _context.SubCategoryTranslations.FirstOrDefaultAsync(x => x.LanguageId == culture && x.SubCategoryId.Equals(id));

            var subCat = new SubCategoryDto()
            {
                Id = item.Id,
                DateCreated = item.DateCreated,
                Description = subCategoryTranslation != null ? subCategoryTranslation.Description : null,
                SubCatName = subCategoryTranslation != null ? subCategoryTranslation.SubCatName : null,
                Code = item.Code,
                DateUpdated = item.DateUpdated,
                Active = item.Active,
                ImagePath = item != null ? item.ImagePath : null,
                SubCategoryTranslationDtos = _mapper.Map<List<SubCategoryTranslationDto>>
                (await _context.SubCategoryTranslations.Where(x => x.SubCategoryId.Equals(id)).Select(x => new SubCategoryTranslationDto
                {
                    LanguageId = x.LanguageId,
                    Id = x.Id,
                    SubCatName = x.SubCatName

                }).ToListAsync())

            };
            return subCat;

        }

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategoryDtos()
        {
            var  languageId = _configuration.GetValue<string>("DefaultLanguageId");

            var query = from sc in _context.SubCategories
                        join sct in _context.SubCategoryTranslations on sc.Id equals sct.SubCategoryId into cl
                        from sct in cl.DefaultIfEmpty()
                        where sct.LanguageId == languageId
                        select new { sc, sct };
            var cat = await query.Select(x => new SubCategoryDto()
            {
                SubCatName = x.sct.SubCatName,
                Active = x.sc.Active,
                ImagePath = x.sc.ImagePath,
                Id = x.sc.Id,
                

            }).ToListAsync();

            return cat;
        }

        public async Task<CategoryTranslationDto> GetCategoryTranslationById(Guid id)
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

        public async Task UpdateCategory(Guid id, UpdateCategoryAdminView updateCategoryAdminView)
        {
            var item = await _context.Categories.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                updateCategoryAdminView.UpdateCategoryTranslationDto.Alias = Utilities.SEOUrl(updateCategoryAdminView.UpdateCategoryTranslationDto.CatName);

                if (updateCategoryAdminView.Image != null)
                {
                    string extension = Path.GetExtension(updateCategoryAdminView.Image.FileName);

                    string image = Utilities.SEOUrl(updateCategoryAdminView.UpdateCategoryTranslationDto.CatName) + extension;

                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageCategory", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }
                    updateCategoryAdminView.UpdateCategory.ImagePath = await Utilities.UploadFile(updateCategoryAdminView.Image, "ImageCategory", image);

                }

                if (updateCategoryAdminView.UpdateCategory.Sort == null)
                {
                    var currentSort = await _context.Categories.OrderByDescending(x => x.Sort).AsNoTracking().ToListAsync();

                    updateCategoryAdminView.UpdateCategory.Sort = currentSort[1].Sort + 1;
                }
                


                updateCategoryAdminView.UpdateCategory.DateUpdated = DateTime.Now;


                _context.Entry(item).CurrentValues.SetValues(updateCategoryAdminView.UpdateCategory);

                await _context.SaveChangesAsync(updateCategoryAdminView.UpdateCategory.UserName);

                var culture = _configuration.GetValue<string>("DefaultLanguageId");

                var translate = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId.Equals(id) && x.LanguageId == culture);

                if (translate != null)
                {
                    updateCategoryAdminView.UpdateCategoryTranslationDto.Alias = Utilities.SEOUrl(updateCategoryAdminView.UpdateCategoryTranslationDto.CatName);

                    updateCategoryAdminView.UpdateCategoryTranslationDto.DateUpdated = DateTime.Now;

                    _context.Entry(translate).CurrentValues.SetValues(updateCategoryAdminView.UpdateCategoryTranslationDto);

                    await _context.SaveChangesAsync(updateCategoryAdminView.UpdateCategory.UserName);
                }

                


                await transaction.CommitAsync();
            }

        }

        public async Task UpdateSubCategory(int id, UpdateSubCategoryAdminView updateSubCategoryAdminView)
        {
            var item = await _context.SubCategories.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                if (updateSubCategoryAdminView.Image != null)
                {
                    string extension = Path.GetExtension(updateSubCategoryAdminView.Image.FileName);

                    string image = Utilities.SEOUrl(updateSubCategoryAdminView.UpdateSubCategoryTranslationDto.SubCatName) + extension;

                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "ImageSubCategory", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }


                    updateSubCategoryAdminView.UpdateSubCategoryDto.ImagePath = await Utilities.UploadFile(updateSubCategoryAdminView.UpdateSubCategoryDto.Image, "ImageSubCategory", image);
                }


                updateSubCategoryAdminView.UpdateSubCategoryDto.DateUpdated = DateTime.Now;

                _context.Entry(item).CurrentValues.SetValues(updateSubCategoryAdminView.UpdateSubCategoryDto);

                await _context.SaveChangesAsync(updateSubCategoryAdminView.UpdateSubCategoryDto.UserName);

                var culture = _configuration.GetValue<string>("DefaultLanguageId");

                var translate = await _context.SubCategoryTranslations.FirstOrDefaultAsync(x => x.SubCategoryId.Equals(id) && x.LanguageId == culture);

                if (translate != null)
                {
                    updateSubCategoryAdminView.UpdateSubCategoryTranslationDto.DateUpdated = DateTime.Now;

                    _context.Entry(translate).CurrentValues.SetValues(updateSubCategoryAdminView.UpdateSubCategoryTranslationDto);

                    await _context.SaveChangesAsync(updateSubCategoryAdminView.UpdateSubCategoryDto.UserName);
                }
                await transaction.CommitAsync();
            }
        }

        public async Task UpdateTranslateCategory(Guid id, UpdateCategoryTranslationDto updateCategoryTranslationDto)
        {
            var item = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.Id.Equals(id));


            if (item != null)
            {

                updateCategoryTranslationDto.Alias = Utilities.SEOUrl(updateCategoryTranslationDto.CatName);

                updateCategoryTranslationDto.DateUpdated = DateTime.Now;

                _context.Entry(item).CurrentValues.SetValues(updateCategoryTranslationDto);

                await _context.SaveChangesAsync(updateCategoryTranslationDto.UserName);

            }
        }

        public async Task<SubCategoryTranslationDto> GetSubCategoryTranslationById(Guid id)
        {
            var subCategoryTranslate = await _context.SubCategoryTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            var subCategoryTranslateDto = _mapper.Map<SubCategoryTranslationDto>(subCategoryTranslate);

            return subCategoryTranslateDto;
        }

        public async Task CreateSubCategoryTranslate(CreateSubCategoryTranslationDto createSubCategoryTranslationDto)
        {
            var exist = _context.SubCategoryTranslations.Any(x => x.SubCategoryId.Equals(createSubCategoryTranslationDto.SubCategoryId) && x.LanguageId == createSubCategoryTranslationDto.LanguageId);
            
            if (exist) return;

            createSubCategoryTranslationDto.Id = Guid.NewGuid();

            createSubCategoryTranslationDto.DateCreated = DateTime.Now;

            SubCategoryTranslation transaltecategory = _mapper.Map<SubCategoryTranslation>(createSubCategoryTranslationDto);

            _context.Add(transaltecategory);

            await _context.SaveChangesAsync(createSubCategoryTranslationDto.UserName);
        }

        public async Task UpdateSubCategoryTranslate(Guid id, UpdateSubCategoryTranslationDto updateSubCategoryTranslationDto)
        {
            var item = await _context.SubCategoryTranslations.FirstOrDefaultAsync(x => x.Id.Equals(id));


            if (item != null)
            {
                updateSubCategoryTranslationDto.DateUpdated = DateTime.Now;

                _context.Entry(item).CurrentValues.SetValues(updateSubCategoryTranslationDto);

                await _context.SaveChangesAsync(updateSubCategoryTranslationDto.UserName);

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
