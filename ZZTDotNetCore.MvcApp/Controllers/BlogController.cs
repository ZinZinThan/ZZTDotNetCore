using Microsoft.AspNetCore.Mvc;
using ZZTDotNetCore.MvcApp.EFDbContext;
using ZZTDotNetCore.MvcApp.Models;

namespace ZZTDotNetCore.MvcApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        [ActionName("Index")]
        public IActionResult BlogIndex()
        {
            List<BlogDataModel> lst = _context.Blogs.ToList();
            return View("BlogIndex", lst);
        }
    }
}
