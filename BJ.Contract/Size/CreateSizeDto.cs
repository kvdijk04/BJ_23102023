namespace BJ.Contract.Size
{
    public class CreateSizeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Price { get; set; }
        public bool Active { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}
