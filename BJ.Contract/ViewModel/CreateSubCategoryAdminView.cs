using BJ.Contract.SubCategory;
using BJ.Contract.Translation.SubCategory;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.ViewModel
{
    public class CreateSubCategoryAdminView
    {
        public CreateSubCategoryDto CreateSubCategoryDto { get; set; } = new CreateSubCategoryDto();
        public CreateSubCategoryTranslationDto CreateSubCategoryTranslationDto { get; set;} = new CreateSubCategoryTranslationDto();

        public IFormFile Image { get; set; }
    }
}
