using BJ.Contract.Blog;
using BJ.Contract.Translation.Blog;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.ViewModel
{
    public class CreateBlogAdminView
    {
        public IFormFile FileUpload { get; set; }

        public CreateBlogDto CreateBlog { get; set; }

        public CreateBlogTranslationDto CreateBlogTranslation { get; set; } = new CreateBlogTranslationDto();
    }
}
