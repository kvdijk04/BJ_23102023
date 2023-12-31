﻿using BJ.Application.Service;
using BJ.Application.Ultities;
using BJ.Contract.SubCategory;
using BJ.Contract.Translation.SubCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly ILogger<SubCategoriesController> _logger;
        private readonly ICategoryService _categoryService;
        public SubCategoriesController(ILogger<SubCategoriesController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }
        /// <summary>
        /// Danh sách danh mục con
        /// </summary>
        [HttpGet]

        public async Task<IEnumerable<SubCategoryDto>> GetSizes()
        {

            return await _categoryService.GetSubCategoryDtos();

        }
        /// <summary>
        /// Phân trang danh mục con sản phẩm
        /// </summary>
        [HttpGet("paging")]

        public async Task<PagedViewModel<SubCategoryDto>> GetSubCategories([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _categoryService.GetPagingSubCategory(getListPagingRequest);

        }
        /// <summary>
        /// Tạo mới danh mục con
        /// </summary>
        /// 
        [Authorize]

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateSubCategoryDto createSubCategoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _categoryService.CreateSubCategory(createSubCategoryDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,e.Message);
            }
        }
        /// <summary>
        /// Cập nhật danh mục con
        /// </summary>
        /// 
        [Authorize]

        [HttpPut("{id}")]
        public async Task<IActionResult> EditSubCateogry(int id, [FromForm] UpdateSubCategoryDto updateSubCategoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _categoryService.GetSubCategoryById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                await _categoryService.UpdateSubCategory(id, updateSubCategoryDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Lấy thông tin danh mục con bằng Id
        /// </summary>

        [HttpGet("{id}")]

        public async Task<IActionResult> GetSubCategpryById(int id)
        {
            if (await _categoryService.GetSubCategoryById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _categoryService.GetSubCategoryById(id));

        }
        /// <summary>
        /// Tạo mới danh mục con cho từng sản phẩm
        /// </summary>
        /// 
        [Authorize]

        [HttpPost("subcategoryspecificproduct")]
        public async Task<IActionResult> CreateSubCategorySpecificProduct([FromBody] CreateSubCategorySpecificDto createSubCategorySpecificDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _categoryService.CreateSubCategorySpecific(createSubCategorySpecificDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Lấy thông tin ngôn ngữ của danh mục con bằng Id
        /// </summary>

        [HttpGet("language/{id}")]

        public async Task<IActionResult> GetSubCategoryTranslationById(Guid id)
        {
            if (await _categoryService.GetSubCategoryTransalationById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _categoryService.GetSubCategoryTransalationById(id));

        }

        /// <summary>
        /// Tạo mới danh mục con theo từng ngôn ngữ
        /// </summary>
        /// 

        [HttpPost("language/create")]
        public async Task<IActionResult> CreateTranslate([FromBody] CreateSubCategoryTranslationDto createSubCategoryTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _categoryService.CreateSubCategoryTranslate(createSubCategoryTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Cập nhật danh mục con theo từng ngôn ngữ
        /// </summary>
        /// 

        [HttpPut("{subCatId}/language/{id}")]
        public async Task<IActionResult> UpdateTranslate(int subCatId, Guid id, [FromBody] UpdateSubCategoryTranslationDto updateSubCategoryTranslationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _categoryService.UpdateSubCategoryTranslate(subCatId, id, updateSubCategoryTranslationDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
