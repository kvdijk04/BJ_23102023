using BJ.Contract.Translation.ConfigWeb;

namespace BJ.Contract.ConfigWeb
{
    public class DetailConfigWebDto
    {
        public Guid Id { get; set; }

        public Guid ConfigWebId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
        public bool Active { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }

        public ConfigWebDto ConfigWebDto { get; set; }

        public List<DetailConfigWebTranslationDto> DetailConfigWebTranslationDtos { get; set; }
    }
}
