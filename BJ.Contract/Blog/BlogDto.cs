using BJ.Contract.Translation.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.Blog
{
    public class BlogDto
    {
        public Guid Id { get; set; }

        public string ImagePath { get; set; }
        public bool Active { get; set; }

        public bool Popular { get; set; }
        public DateTime? DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public List<BlogTranslationDto> BlogTranslationDtos { get; set; }
    }
}
