namespace BJ.Contract.SubCategory
{
    public class UpdateSubCategorySpecificProduct
    {
        public int SubCategoryId { get; set; }

        public Guid ProductId { get; set; }
        public bool Active { get; set; }

    }
}
