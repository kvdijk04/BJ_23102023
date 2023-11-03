using BJ.Contract.Translation.News;

namespace BJ.Contract.ViewModel
{
    public class NewsUserViewModel
    {
        public Guid Id { get; set; }

        public string ImagePath { get; set; }

        public bool Active { get; set; }

        public bool Popular { get; set; }

        public bool Home { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Title { get; set; }

        public string ShortDesc { get; set; }
        public string Alias { get; set; }

        public string Description { get; set; }
        public string MetaDesc { get; set; }

        public string MetaKey { get; set; }

        public List<NewsTranslationDto> NewsTranslationDtos { get; set; }
    }
}
