using BJ.Contract.Blog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.Translation.Blog
{
    public class BlogTranslationDto
    {
        public Guid Id { get; set; }

        public Guid BlogId { get; set; }

        public string LanguageId { get; set; }

        public string Title { get; set; }

        public string ShortDesc { get; set; }

        public string Description { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }

        public string MetaKey { get; set; }

        public LanguageDto LanguageDto { get; set; }

        public BlogDto BlogDto { get; set; }
    }
}
