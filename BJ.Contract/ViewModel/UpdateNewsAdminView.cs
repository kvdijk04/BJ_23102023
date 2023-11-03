using BJ.Contract.News;
using BJ.Contract.Translation.News;
using Microsoft.AspNetCore.Http;

namespace BJ.Contract.ViewModel
{
    public class UpdateNewsAdminView
    {
        public IFormFile FileUpload { get; set; }

        public UpdateNewsDto UpdateNews { get; set; }

        public UpdateNewsTranslationDto UpdateNewsTranslation { get; set; }
    }
}
