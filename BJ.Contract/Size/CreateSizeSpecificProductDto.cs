namespace BJ.Contract.Size
{
    public class CreateSizeSpecificProductDto
    {
        public Guid Id { get; set; }
        public int SizeId { get; set; }
        public Guid ProductId { get; set; }
        public string Energy { get; set; }
        public string Cal { get; set; }
        public string Fat { get; set; }

        public string Carbonhydrate { get; set; }

        public string DietaryFibre { get; set; }

        public string Protein { get; set; }

        public string FatSaturated { get; set; }

        public string CarbonhydrateSugar { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Sodium { get; set; }
        public bool ActiveSize { get; set; }

        public bool ActiveNutri { get; set; }
        public string UserName { get;set; }
    }
}
