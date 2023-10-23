namespace BJ.Contract.SubCategory
{
    public class CreateSubCategorySpecificDto
    {
        public Guid Id { get; set; }
        public int SubCategoryId { get; set; }

        public Guid ProductId { get; set; }
        public bool Active { get; set; }

    }
}
