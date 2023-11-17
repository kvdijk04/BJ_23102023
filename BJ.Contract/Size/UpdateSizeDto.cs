namespace BJ.Contract.Size
{
    public class UpdateSizeDto
    {
        public string Name { get; set; }
        public string Note { get; set; }

        public int? Price { get; set; }
        public DateTime? Updated { get; set; }
        public bool Active { get; set; }

    }
}
