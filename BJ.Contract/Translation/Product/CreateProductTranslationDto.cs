namespace BJ.Contract.Translation.Product
{
    public class CreateProductTranslationDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string LanguageId { get; set; }
        public string ProductName { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }

    }
}
