using BJ.Contract.News;

namespace BJ.Contract.Translation.News
{
    public class NewsTranslationDto
    {
        public Guid Id { get; set; }

        public Guid NewsId { get; set; }

        public string LanguageId { get; set; }

        public string Title { get; set; }

        public string ShortDesc { get; set; }
        public string Alias { get; set; }

        public string Description { get; set; }
        public string MetaDesc { get; set; }

        public string MetaKey { get; set; }
        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }
        public LanguageDto Language { get; set; }

        public NewsDto NewsDto { get; set; }
    }
}
