namespace BJ.Contract.Product
{
    public class CreateProductDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public int? Price { get; set; }
        public int? Discount { get; set; }
        public int? Sort { get; set; }
        public DateTime? DateActiveForm { get; set; }
        public DateTime? DateTimeActiveTo { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool BestSeller { get; set; }
        public bool HomeTag { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }
        public string ImagePathCup { get; set; }
        public string CodeCategory { get; set; }
        public string ImagePathHero { get; set; }

        public string ImagePathIngredients { get; set; }
        public string UserName { get; set; }

    }
}
