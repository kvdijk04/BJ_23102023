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
        public SiteMapController(IProductServiceConnection productServiceConnection, INewsServiceConnection newsServiceConnection, IBlogServiceConnection blogServiceConnection, IConfiguration configuration, ILanguageServiceConnection languageServiceConnection)
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
        [Route("sitemap.xml")]
        public async Task<IActionResult> Index()
        {
            string baseUrl = GetHost();
            List<string> ls = new List<string>();
            string pathname = null;
            ls.Add(baseUrl + "/vi");
            //thêm các danh sách sitemap
            ls.Add(baseUrl + "/vi/ve-boost-juice");
            ls.Add(baseUrl + "/vi/lien-he");
            ls.Add(baseUrl + "/vi/thuc-uong");
            ls.Add(baseUrl + "/vi/cua-hang");
            ls.Add(baseUrl + "/vi/vibe");
            ls.Add(baseUrl + "/vi/song-khoe");
            ls.Add(baseUrl + "/vi/tin-tuc");
            ls.Add(baseUrl + "/vi/khuyen-mai");

            ls.Add(baseUrl + "/en");
            ls.Add(baseUrl + "/en/about");
            ls.Add(baseUrl + "/en/contact");
            ls.Add(baseUrl + "/en/drinks");
            ls.Add(baseUrl + "/en/stores");
            ls.Add(baseUrl + "/en/vibe");
            ls.Add(baseUrl + "/en/promotion");
            ls.Add(baseUrl + "/en/wellbeing");
            ls.Add(baseUrl + "/en/news");


            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<?xml version=\'1.0\' encoding=\'UTF-8\'?>");
            stringBuilder.AppendLine("<urlset xmlns =\'http://www.sitemaps.org/schemas/sitemap/0.9' xmlns:content=\"http://www.google.com/schemas/sitemap-content/1.0\" xmlns:image=\"http://www.google.com/schemas/sitemap-image/1.1\">");
            var allLanguage = await _languageServiceConnection.GetAllLanguages();

            foreach (var language in allLanguage.Select(x => x.Id))
            {


                var listPromotion = await _newsServiceConnection.GetNews(language, false, true);
                foreach (var item in listPromotion)
                {
                    if (language == "vi") { pathname = "khuyen-mai"; } else { pathname = "promotion"; };
                    ls.Add(baseUrl + "/" + language + "/" + pathname + "/" + item.Id + "/" + item.Alias);

                }

                var listBlog = await _blogServiceConnection.GetBlogsPopular(language, false);
                foreach (var item in listBlog)
                {
                    if (language == "vi") { pathname = "song-khoe"; } else { pathname = "wellbeing"; };
                    ls.Add(baseUrl + "/" + language + "/" + pathname + "/" + item.Id + "/" + item.Alias);

                }

                var listNews = await _newsServiceConnection.GetNews(language, false, false);
                foreach (var item in listNews)
                {
                    if (language == "vi") { pathname = "tin-tuc"; } else { pathname = "news"; };
                    ls.Add(baseUrl + "/" + language + "/" + pathname + "/" + item.Id + "/" + item.Alias);

                }
            }


            foreach (var item in ls)
            {
                string link = "<loc>" + item + "</loc>";
                stringBuilder.AppendLine("<url>");
                stringBuilder.AppendLine(link);
                stringBuilder.AppendLine("<lastmod>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz") + "</lastmod>");
                stringBuilder.AppendLine("</url>");
            }

            stringBuilder.AppendLine("</urlset>");
            return Content(stringBuilder.ToString(), "application/xml", Encoding.UTF8);
        }
        //[Route("/product.xml")]
        //public async Task<IActionResult> SiteMapProduct()
        //{
        //    string baseUrl = _configuration.GetValue<string>("CurrentDomain");
        //    var allLanguage = await _languageServiceConnection.GetAllLanguages();
        //    var sitemapBuilder = new SitemapBuilder();
        //    foreach (var language in allLanguage.Select(x => x.Id))
        //    {
        //        string pathname = null;
        //        if (language == "vi") { pathname = "thuc-uong"; } else { pathname = "drinks"; };
        //        var listProduct = await _productServiceConnection.GetAllUserProduct(language);
        //        foreach (var item in listProduct.UserProductDtos)
        //        {
        //            sitemapBuilder.AddUrl(GetHost() + "/" + language + "/" + pathname + "/" + item.Alias, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Monthly, priority: 0.9);

        //        }
        //    }
        //    string xml = sitemapBuilder.ToString();

        //    return Content(xml, "text/xml");
        //}
        //[Route("/promotion.xml")]
        //public async Task<IActionResult> SiteMapPromotion()
        //{
        //    string baseUrl = _configuration.GetValue<string>("CurrentDomain");
        //    var allLanguage = await _languageServiceConnection.GetAllLanguages();
        //    var sitemapBuilder = new SitemapBuilder();
        //    foreach (var language in allLanguage.Select(x => x.Id))
        //    {
        //        string pathname = null;
        //        if (language == "vi") { pathname = "khuyen-mai"; } else { pathname = "promotion"; };

        //        var listPromotion = await _newsServiceConnection.GetNews(language,false,true);
        //        foreach (var item in listPromotion)
        //        {
        //            sitemapBuilder.AddUrl(GetHost() + "/" + language + "/" + pathname + "/" + item.Id + "/" + item.Alias + ".html", modified: DateTime.Now, changeFrequency: ChangeFrequency.Monthly, priority: 0.9);

        //        }
        //    }
        //    string xml = sitemapBuilder.ToString();
        //    return Content(xml, "text/xml");
        //}
        //[Route("/blog.xml")]
        //public async Task<IActionResult> SiteMapBlog()
        //{
        //    string baseUrl = _configuration.GetValue<string>("CurrentDomain");
        //    var allLanguage = await _languageServiceConnection.GetAllLanguages();
        //    var sitemapBuilder = new SitemapBuilder();
        //    sitemapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);
        //    foreach (var language in allLanguage.Select(x => x.Id))
        //    {
        //        string pathname = null;
        //        if (language == "vi") { pathname = "song-khoe"; } else { pathname = "wellbeing"; };
        //        var listPromotion = await _blogServiceConnection.GetAllBlogs(language, false);
        //        foreach (var item in listPromotion)
        //        {
        //            sitemapBuilder.AddUrl(GetHost() + "/" + language + "/" + pathname + "/" + item.Id + "/" + item.Alias + ".html", modified: DateTime.Now, changeFrequency: ChangeFrequency.Yearly, priority: 0.9);

        //        }
        //    }
        //    string xml = sitemapBuilder.ToString();
        //    return Content(xml, "text/xml");
        //}
        //[Route("/news.xml")]
        //public async Task<IActionResult> SiteMapNews()
        //{
        //    string baseUrl = _configuration.GetValue<string>("CurrentDomain");
        //    var allLanguage = await _languageServiceConnection.GetAllLanguages();
        //    var sitemapBuilder = new SitemapBuilder();
        //    sitemapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);
        //    foreach (var language in allLanguage.Select(x => x.Id))
        //    {
        //        string pathname = null;
        //        if (language == "vi") { pathname = "tin-tuc"; } else { pathname = "news"; };

        //        var listPromotion = await _newsServiceConnection.GetNews(language, false,false);
        //        foreach (var item in listPromotion)
        //        {
        //            sitemapBuilder.AddUrl(GetHost() + "/" + language + "/" + pathname + "/" + item.Id + "/" + item.Alias + ".html", modified: DateTime.Now, changeFrequency: ChangeFrequency.Weekly, priority: 0.9);

        //        }
        //    }
        //    string xml = sitemapBuilder.ToString();
        //    return Content(xml, "text/xml");
        //}
    }
}
