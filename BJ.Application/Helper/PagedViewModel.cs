
namespace BJ.Application.Ultities
{
    public class PagedViewModel<T> : PagingBase
    {
        public IEnumerable<T> Items { get; set; }

    }
}
