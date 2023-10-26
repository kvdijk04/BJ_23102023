using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.Translation.Blog
{
    public class UpdateBlogTranslationDto
    {
        public string Title { get; set; }

        public string ShortDesc { get; set; }

        public string Description { get; set; }
        public string Alias { get; set; }

        public string MetaDesc { get; set; }

        public string MetaKey { get; set; }
    }
}
