namespace BJ.Domain.Entities
{
    public class News
    {
        public Guid Id { get; set; }

        public string ImagePath { get; set; }

        public bool Active { get; set; }

        public bool Popular { get; set; }

        public bool Home { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public List<NewsTranslation> NewsTranslations { get; set; }

    }
}
