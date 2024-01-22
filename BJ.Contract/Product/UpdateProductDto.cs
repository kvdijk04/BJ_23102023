namespace BJ.Contract.Product
{
    public class UpdateProductDto
    {
        public int? Discount { get; set; }
        public int? Sort { get; set; }
        public DateTime? DateActiveForm { get; set; }
        public DateTime? DateTimeActiveTo { get; set; }
        public DateTime? DateModified { get; set; }
        public bool BestSeller { get; set; }
        public bool HomeTag { get; set; }
        public bool Active { get; set; }
        public string ImagePathCup { get; set; }

        public string ImagePathHero { get; set; }

        public string ImagePathIngredients { get; set; }
        public string UserName { get; set; }

    }
}
