using BJ.Contract.Blog;
using BJ.Contract.Translation.Blog;
using Microsoft.AspNetCore.Http;

namespace BJ.Contract.ViewModel
{
    public class UpdateBlogAdminView
    {
        public IFormFile FileUpload { get; set; }

        public UpdateBlogDto UpdateBlog { get; set; }

        public UpdateBlogTranslationDto UpdateBlogTranslation { get; set; }
    }
}
