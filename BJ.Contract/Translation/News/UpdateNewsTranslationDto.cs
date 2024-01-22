namespace BJ.Contract.Translation.News
{
    public class UpdateNewsTranslationDto
    {
        public string Title { get; set; }

        public string ShortDesc { get; set; }
        public string Alias { get; set; }

        public string Description { get; set; }
        public string MetaDesc { get; set; }

        public string MetaKey { get; set; }
        public DateTime? DateUpdated { get; set; }

        public string UserName { get; set; }
    }
}
