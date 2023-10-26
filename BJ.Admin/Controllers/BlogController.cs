using BJ.ApiConnection.Services;
using BJ.Application.Ultities;
using BJ.Contract.Blog;
using BJ.Contract.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Admin.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IBlogServiceConnection _blogServiceConnection;
        public BlogController(ILogger<BlogController> logger, IBlogServiceConnection blogServiceConnection)
        {
            _logger = logger;
            _blogServiceConnection = blogServiceConnection;
        }
        [Route("/tat-ca-blog.html")]
        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1)
        {
            if (keyword != null) ViewBag.Keyword = keyword;

            //var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("/dang-nhap.html");
            //}
            var request = new GetListPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,

            };
            var r = await _blogServiceConnection.GetPaging(request);

            //ViewBag.Keyword = keyword;
            return View(r);
        }
        //[Route("/chi-tiet-blog/{id}")]
        //[HttpGet]
        //public async Task<IActionResult> Detail(int id)
        //{
        //    var r = await _blogServiceConnection.GetBlogById(id);
        //    return View(r);
        //}
        [HttpGet]
        [Route("/tao-moi-blog.html")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-blog.html")]

        public async Task<IActionResult> Create([FromForm] CreateBlogAdminView createBlogAdminView)
        {
            var a = await _blogServiceConnection.CreateBlog(createBlogAdminView);

            return Redirect("/tao-moi-blog.html");
        }
        //[HttpGet]
        //[Route("/cap-nhat-blog/{id}")]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var item = await _blogServiceConnection.GetBlogById(id);
        //    UpdateBlogDto updateBlogDto = new()
        //    {
        //        Active = item.Active,
        //        Name = item.Name,
        //        Price = item.Price,

        //    };
        //    ViewBag.BlogId = id;
        //    return View(updateBlogDto);
        //}
        //[HttpPost]
        //[Route("/cap-nhat-blog/{id}")]

        //public async Task<IActionResult> Edit(int id, UpdateBlogDto updateBlogDto)
        //{
        //    var a = await _blogServiceConnection.UpdateBlog(id, updateBlogDto);

        //    return Redirect("/tat-ca-blog.html");
        //}
    }
}
