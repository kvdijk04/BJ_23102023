﻿using Microsoft.AspNetCore.Http;

namespace BJ.Contract.Category
{
    public class CreateCategoryDto
    {
        public Guid Id { get; set; }
        public string CatName { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }
        public string ImagePath { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Code { get; set; }
        public IFormFile Image { get; set; }

    }
}
