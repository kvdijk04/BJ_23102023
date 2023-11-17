using BJ.ApiConnection.Services;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BJ.App.Controllers
{
    public class SiteMapController : Controller
    {
        private readonly IProductServiceConnection _productServiceConnection;
        private readonly INewsServiceConnection _newsServiceConnection;
        private readonly IBlogServiceConnection _blogServiceConnection;
        private readonly IConfiguration _configuration;
        private readonly ILanguageServiceConnection _languageServiceConnection;
        public SiteMapController(IProductServiceConnection productServiceConnection,INewsServiceConnection newsServiceConnection, IBlogServiceConnection blogServiceConnection, IConfiguration configuration, ILanguageServiceConnection languageServiceConnection)
        {
            _productServiceConnection = productServiceConnection;
            _newsServiceConnection = newsServiceConnection;
            _blogServiceConnection = blogServiceConnection;
            _configuration = configuration;
            _languageServiceConnection = languageServiceConnection;
        }
        public string GetHost()
        {
            return $"{(Request.IsHttps ? "https" : "http")}://{Request.Host.ToString()}";
        }
        [Route("Sitemap.xml")]
        public IActionResult Index()
        {
            string baseUrl = GetHost();
            List<string> ls = new List<string>();
            //thêm các danh sách sitemap
            ls.Add(baseUrl + "/Sitemap-product.xml");
            ls.Add(baseUrl + "/Sitemap-posts.xml");
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<?xml version=\'1.0\' encoding=\'UTF-8\'?>");
            stringBuilder.AppendLine("<sitemapindex xmlns =\'http://www.sitemaps.org/schemas/sitemap/0.9'>");
            foreach (var item in ls)
            {
                string link = "<loc>" + item + "</loc>";
                stringBuilder.AppendLine("<sitemap>");
                stringBuilder.AppendLine(link);
                stringBuilder.AppendLine("<lastmod>" + DateTime.Now.ToString("MMMM-dd-yyyy HH:mm:ss tt") + "</lastmod>");
                stringBuilder.AppendLine("</sitemap>");
            }
            stringBuilder.AppendLine("</sitemapindex>");
            return Content(stringBuilder.ToString(), "text/xml", Encoding.UTF8);
        }
        [Route("/Sitemap-product.xml")]
        public async Task<IActionResult> SiteMapProduct()
        {
            string baseUrl = _configuration.GetValue<string>("CurrentDomain");
            var allLanguage = await _languageServiceConnection.GetAllLanguages();
            var sitemapBuilder = new SitemapBuilder();
            sitemapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);
            foreach (var language in allLanguage.Select(x => x.Id))
            {
                var listProduct = await _productServiceConnection.GetAllUserProduct(language);
                foreach(var item in listProduct.UserProductDtos)
                {
                    sitemapBuilder.AddUrl(GetHost() + "/" + language + "/" + item.Alias , modified: DateTime.UtcNow, changeFrequency: null, priority: 0.9);

                }
            }
            string xml = sitemapBuilder.ToString();
            return Content(xml, "text/xml");
        }
        //[Route("/Sitemap-posts.xml")]
        //public IActionResult SiteMapPost()
        //{
        //    var lsproduct = _context.Posts.Where(x => x.Published == true).ToList();
        //    var sitemapBuilder = new SitemapBuilder();
        //    sitemapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);
        //    foreach (var p in lsproduct)
        //    {
        //        sitemapBuilder.AddUrl(GetHost() + p.Alias, modified: DateTime.UtcNow, changeFrequency: null, priority: 0.8);
        //    }
        //    string xml = sitemapBuilder.ToString();
        //    return Content(xml, "text/xml");
        //}
    }
}
