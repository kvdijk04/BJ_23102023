namespace BJ.Contract.ConfigWeb
{
    public class ConfigWebDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<DetailConfigWebDto> DetailConfigWebDto { get; set; }
    }
}
