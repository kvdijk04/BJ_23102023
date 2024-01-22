﻿using BJ.Contract.Translation.Blog;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BJ.Contract.Blog
{
    public class BlogDto
    {
        public Guid Id { get; set; }

        public string ImagePath { get; set; }
        public bool Active { get; set; }
        public string Title { get; set; }
        public bool Popular { get; set; }
        public string Code { get; set; }
        public DateTime? DateActiveForm { get; set; }

        public DateTime? DateTimeActiveTo { get; set; }
        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public List<BlogTranslationDto> BlogTranslationDtos { get; set; }

    }
}
