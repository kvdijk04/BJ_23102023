using BJ.Contract.Category;
using BJ.Contract.Translation.Category;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.ViewModel
{
    public class CreateCategoryAdminView
    {
        public CreateCategoryDto CreateCategoryDto { get; set; } = new CreateCategoryDto();
        public  CreateCategoryTranslationDto CreateCategoryTranslationDto { get;set; } = new CreateCategoryTranslationDto();
        public IFormFile Image { get;set; }
    }
}
