using BJ.Contract.News;
using BJ.Contract.Translation.News;
using Microsoft.AspNetCore.Http;

namespace BJ.Contract.ViewModel
{
    public class CreateNewsAdminView
    {
        public IFormFile FileUpload { get; set; }

        public CreateNewsDto CreateNews { get; set; }

        public CreateNewsTranslationDto CreateNewsTranslation { get; set; } = new CreateNewsTranslationDto();
    }
}
