namespace BJ.Contract.Translation.Category
{
    public class CreateCategoryTranslationDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string LanguageId { get; set; }
        public string CatName { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }
    }
}
