namespace BJ.Application.Ultities
{
    public class GetListPagingRequest : PagingBase
    {
        public string Keyword { get; set; }
        public string LanguageId { get;set; }
        public bool Popular {  get; set; }
        public Guid CategoryId { get; set; }
        public bool Active { get; set; }



    }
}
