using BJ.Contract.ConfigWeb;

namespace BJ.Contract.Translation.ConfigWeb
{
    public class DetailConfigWebTranslationDto
    {
        public Guid Id { get; set; }

        public Guid DetailConfigWebId { get; set; }

        public string LanguageId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }
        public DetailConfigWebDto DetailConfigWebDto { get; set; }

        public LanguageDto LanguageDto { get; set; }
        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }
    }
}
