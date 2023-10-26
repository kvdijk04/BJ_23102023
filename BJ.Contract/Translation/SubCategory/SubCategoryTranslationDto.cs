﻿using BJ.Contract.Category;
using BJ.Contract.SubCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.Translation.SubCategory
{
    public class SubCategoryTranslationDto
    {
        public Guid Id { get; set; }
        public string LanguageId { get; set; }

        public int SubCategoryId { get; set; }
        public string SubCatName { get; set; }
        public string Description { get; set; }
        public SubCategoryDto SubCategoryDto { get; set; }
        public LanguageDto Language { get; set; }
    }
}
