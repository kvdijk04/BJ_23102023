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
    public class UpdateSubCategoryAdminView
    {
        public UpdateSubCategoryDto UpdateSubCategoryDto { get; set; } = new UpdateSubCategoryDto();
        public UpdateSubCategoryTranslationDto UpdateSubCategoryTranslationDto { get; set; } = new UpdateSubCategoryTranslationDto();
        public IFormFile Image {  get; set; }
    }
}
