namespace BJ.Contract.ViewModel
{
    public class ConfigProduct
    {
        public Guid ProId { get; set; }
        public Guid CategoryId { get; set; }    

        public string UserName { get; set; }
        public List<int> SubCat { get; set; }

        public List<int> Size { get; set; }
        public List<int> NewSize { get; set; }
    }
}
