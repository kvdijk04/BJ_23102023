namespace BJ.Contract.Translation.ConfigWeb
{
    public class CreateDetailConfigWebTranslationDto
    {
        public Guid Id { get; set; }

        public Guid DetailConfigWebId { get; set; }

        public string LanguageId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public string Url {  get; set; }
    }
}
