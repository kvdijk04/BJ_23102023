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
    public class UpdateCategoryAdminView
    {
        public UpdateCategoryDto UpdateCategory { get; set; }
        public UpdateCategoryTranslationDto UpdateCategoryTranslationDto { get; set; }
        public IFormFile Image { get; set; }

    }
}
