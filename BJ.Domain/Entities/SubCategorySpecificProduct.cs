namespace BJ.Domain.Entities
{
    public class SubCategorySpecificProduct
    {
        public Guid Id { get; set; }
        public int SubCategoryId { get; set; }

        public Guid ProductId { get; set; }
        public bool Active { get; set; }
        public virtual Product Product { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

    }
}
