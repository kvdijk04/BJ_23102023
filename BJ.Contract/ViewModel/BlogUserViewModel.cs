using BJ.Contract.Translation.Blog;

namespace BJ.Contract.ViewModel
{
    public class BlogUserViewModel
    {
        public Guid Id { get; set; }

        public string ImagePath { get; set; }

        public bool Active { get; set; }

        public bool Popular { get; set; }
        public DateTime? DateActiveForm { get; set; }

        public DateTime? DateTimeActiveTo { get; set; }
        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string Title { get; set; }

        public string ShortDesc { get; set; }

        public string Description { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }

        public string MetaKey { get; set; }

        public List<BlogTranslationDto> BlogTranslationDtos { get; set; }
    }
}
