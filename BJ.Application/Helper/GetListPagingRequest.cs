namespace BJ.Application.Ultities
{
    public class GetListPagingRequest : PagingBase
    {
        public string Keyword { get; set; }

        public Guid[] CategoryId { get; set; }
        public bool Active { get; set; }



    }
}
