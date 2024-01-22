using BJ.Contract.Translation.News;

namespace BJ.Contract.News
{
    public class NewsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string ImagePath { get; set; }

        public bool Active { get; set; }

        public bool Popular { get; set; }
        public bool Promotion { get; set; }

        public bool Home { get; set; }
        public string Code { get; set; }

        public DateTime? DateActiveForm { get; set; }

        public DateTime? DateTimeActiveTo { get; set; }
        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public List<NewsTranslationDto> NewsTranslationsDto { get; set; }
    }
}
