using BJ.Application.Helper;
using BJ.Application.Service;
using BJ.Contract;
using BJ.Contract.Category;
using BJ.Contract.Product;
using BJ.Contract.Size;
using BJ.Contract.SubCategory;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportExcelController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ISizeService _sizeService;
        private readonly IProductService _productService;
        private readonly IBlogService _blogService;
        private readonly INewsService _newsService;
        public ImportExcelController(ICategoryService categoryService, ISizeService sizeService, IProductService productService, IBlogService blogService, INewsService newsService)
        {
            _categoryService = categoryService;
            _sizeService = sizeService;
            _productService = productService;
            _blogService = blogService;
            _newsService = newsService;
        }
        [SecurityRole(AuthorizeRole.AdminRole)]
        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        public async Task<string> Import([FromForm] ImportResponse importResponse, bool category, bool subCategory, bool size, bool product,
                                bool blog, bool news, CancellationToken cancellationToken)
        {

            if (importResponse.File == null || importResponse.File.Length <= 0)
            {
                return importResponse.Msg = "Không tìm thấy File";
            }

            if (!Path.GetExtension(importResponse.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return importResponse.Msg = "File không đúng định dạng .xlsx";
            }
            if (category == true)
            {

                var createCategoryDtos = new List<CreateCategoryDto>();
                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {

                            createCategoryDtos.Add(new CreateCategoryDto
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                Id = Guid.NewGuid(),
                                //CatName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                //Description = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                //Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 3].Value.ToString().Trim())),
                                //MetaDesc = worksheet.Cells[row, 4].Value.ToString().Trim(),
                                //MetaKey = worksheet.Cells[row, 5].Value.ToString().Trim(),

                            });
                        }
                    }
                }
                foreach (var item in createCategoryDtos)
                {
                    //await _categoryService.CreateCategory(item);
                }
            }
            if (subCategory == true)
            {

                var createSubCategoryDtos = new List<CreateSubCategoryDto>();
                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {

                            createSubCategoryDtos.Add(new CreateSubCategoryDto
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                //SubCatName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                //Description = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 3].Value.ToString().Trim())),

                            });
                        }
                    }
                }
                foreach (var item in createSubCategoryDtos)
                {
                    //await _categoryService.CreateSubCategory(item);
                }
            }
            if (size == true)
            {

                var createSizeDtos = new List<CreateSizeDto>();
                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[2];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {

                            createSizeDtos.Add(new CreateSizeDto
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Price = Convert.ToInt32(worksheet.Cells[row, 2].Value.ToString().Trim()),
                                Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 3].Value.ToString().Trim())),

                            });
                        }
                    }
                }
                foreach (var item in createSizeDtos)
                {
                    await _sizeService.CreateSize(item);
                }
            }
            if (product == true)
            {

                var createProductDto = new List<CreateProductAdminView>();
                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[3];

                        var rowCount = worksheet.Dimension.Rows;

                        int[] sizeSpecificProduct = null;

                        int[] subCatSpecificProduct = null;


                        for (int row = 2; row <= rowCount; row++)
                        {
                            sizeSpecificProduct = Array.ConvertAll(worksheet.Cells[row, 11].Value.ToString().Trim().Split(","), s => int.Parse(s));

                            subCatSpecificProduct = Array.ConvertAll(worksheet.Cells[row, 12].Value.ToString().Trim().Split(","), s => int.Parse(s));

                            createProductDto.Add(new CreateProductAdminView
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                CreateProduct = new CreateProductDto()
                                {
                                    Id = Guid.NewGuid(),
                                    //ProductName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                    CodeCategory = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                    Discount = Convert.ToInt32(worksheet.Cells[row, 3].Value.ToString().Trim()),
                                    HomeTag = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 4].Value.ToString().Trim())),
                                    BestSeller = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 5].Value.ToString().Trim())),
                                    Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 6].Value.ToString().Trim())),
                                    //ShortDesc = worksheet.Cells[row, 7].Value.ToString().Trim(),
                                    //Description = worksheet.Cells[row, 8].Value.ToString().Trim(),
                                    //MetaDesc = worksheet.Cells[row, 9].Value.ToString().Trim(),
                                    //MetaKey = worksheet.Cells[row, 10].Value.ToString().Trim(),
                                },
                                Size = sizeSpecificProduct,
                                SubCat = subCatSpecificProduct,

                            });


                        }
                    }
                }
                foreach (var item in createProductDto)
                {
                    var catId = await _categoryService.GetIdOfCategorỵ(item.CreateProduct.CodeCategory);

                    if (catId == Guid.Empty) return importResponse.Msg = $"Không tìm thấy mã loại {item.CreateProduct.CodeCategory}";

                    item.CreateProduct.CategoryId = catId;
                    await _productService.CreateProductAdminView(item);
                }
            }
            if (blog == true)
            {

                var createBlogAdminViewsDtos = new List<CreateBlogAdminView>();
                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[4];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {

                            createBlogAdminViewsDtos.Add(new CreateBlogAdminView
                            {
                                CreateBlog = new Contract.Blog.CreateBlogDto()
                                {
                                    Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 4].Value.ToString().Trim())),
                                    Popular = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 5].Value.ToString().Trim())),

                                },
                                CreateBlogTranslation = new Contract.Translation.Blog.CreateBlogTranslationDto()
                                {
                                    Title = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                    ShortDesc = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                    Description = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                    MetaDesc = worksheet.Cells[row, 6].Value.ToString().Trim(),
                                    MetaKey = worksheet.Cells[row, 7].Value.ToString().Trim(),
                                },

                            });
                        }
                    }
                }
                foreach (var item in createBlogAdminViewsDtos)
                {
                    await _blogService.CreateBlog(item);
                }
            }
            if (news == true)
            {

                var createNewsAdminViewsDtos = new List<CreateNewsAdminView>();
                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[5];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {

                            createNewsAdminViewsDtos.Add(new CreateNewsAdminView
                            {
                                CreateNews = new Contract.News.CreateNewsDto()
                                {
                                    Home = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 4].Value.ToString().Trim())),
                                    Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 5].Value.ToString().Trim())),
                                    Popular = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 6].Value.ToString().Trim())),

                                },
                                CreateNewsTranslation = new Contract.Translation.News.CreateNewsTranslationDto
                                {
                                    Title = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                    ShortDesc = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                    Description = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                    MetaDesc = worksheet.Cells[row, 7].Value.ToString().Trim(),
                                    MetaKey = worksheet.Cells[row, 8].Value.ToString().Trim(),
                                },

                            });
                        }
                    }
                }
                foreach (var item in createNewsAdminViewsDtos)
                {
                    await _newsService.CreateNews(item);
                }
            }
            return importResponse.Msg = "Import Success";

        }
    }
}
