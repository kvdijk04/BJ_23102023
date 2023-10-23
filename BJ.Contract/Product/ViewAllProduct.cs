namespace BJ.Contract.Product
{
    public class ViewAllProduct
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string ImageIngredients { get; set; }
        public bool BestSeller { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }
    }
}
