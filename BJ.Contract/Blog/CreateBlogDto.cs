namespace BJ.Contract.Blog
{
    public class CreateBlogDto
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public bool Active { get; set; }

        public bool Popular { get; set; }
        public DateTime? DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
