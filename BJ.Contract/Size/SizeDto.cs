namespace BJ.Contract.Size
{
    public class SizeDto
    {
        public int Id { get; set; }
        public Guid CategoryId { get; set; }

        public string Name { get; set; }
        public string Note { get; set; }

        public int? Price { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<SizeSpecificProductDto> SizeSpecificProducts { get; set; } = new HashSet<SizeSpecificProductDto>();
    }
}
