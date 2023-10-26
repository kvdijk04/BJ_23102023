namespace BJ.Contract.Blog
{
    public class UpdateBlogDto
    {
        public string ImagePath { get; set; }
        public bool Active { get; set; }

        public bool Popular { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
