namespace BJ.Contract.Translation.Blog
{
    public class CreateBlogTranslationDto
    {
        public Guid Id { get; set; }

        public Guid BlogId { get; set; }

        public string LanguageId { get; set; }

        public string Title { get; set; }

        public string ShortDesc { get; set; }

        public string Description { get; set; }

        public string Alias { get; set; }

        public string MetaDesc { get; set; }

        public string MetaKey { get; set; }
    }
}
