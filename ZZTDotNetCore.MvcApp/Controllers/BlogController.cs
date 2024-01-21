﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ZZTDotNetCore.MvcApp.EFDbContext;
using ZZTDotNetCore.MvcApp.Models;

namespace ZZTDotNetCore.MvcApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BlogController> _logger;
        public BlogController(AppDbContext context, ILogger<BlogController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [ActionName("Index")]
        public IActionResult BlogIndex()
        {
            List<BlogDataModel> lst = _context.Blogs.ToList();
            _logger.LogInformation("Index page visited");
            return View("BlogIndex", lst);
        }

		[ActionName("List")]
		public async Task<IActionResult> BlogList(int pageNo = 1, int pageSize =5)
		{
            _logger.LogInformation("Page no =>{@pageNo}, Page Size =>{@pageSize}",pageNo,pageSize);
            BlogDataResponseModel model = new BlogDataResponseModel();

			List<BlogDataModel> lst = _context.Blogs.AsNoTracking()
                .Skip((pageNo-1) * pageSize)
                .Take(pageSize)
                .ToList();
            _logger.LogInformation("Blog response list => "+ JsonSerializer.Serialize(lst));
            int rowCount = await _context.Blogs.CountAsync();
            int pageCount = rowCount / pageSize;
            if( rowCount % pageSize > 0)
            {
                pageCount++;
            }

            model.Blogs = lst;
            model.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, "/blog/list");

			return View("BlogList", model);
		}

		[ActionName("Create")]
		public IActionResult BlogCreate()
		{
            _logger.LogInformation("Create page visited");
            return View("BlogCreate");
		}

		[HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> BlogSave(BlogDataModel reqModel)
        {
            _logger.LogInformation("User input =>" + JsonSerializer.Serialize(reqModel));
            await _context.Blogs.AddAsync(reqModel);
            var result = await _context.SaveChangesAsync();
            string message = result > 0 ? "Saving Successful." : " Saving Failed.";
            TempData["Message"] = message;
            TempData["IsSuccess"] = result > 0;
            _logger.LogInformation("Create Blog =>" + JsonSerializer.Serialize(message));
            return Redirect("/blog");
        }

        [ActionName("Edit")]
        public async Task<IActionResult> BlogEdit(int id)
        {
            _logger.LogInformation("User ID =>{id}",id);
            if (!await _context.Blogs.AsNoTracking().AnyAsync(x=> x.Blog_Id == id))
            {
                TempData["Message"] = "No data found.";
                TempData["IsSuccess"] = false;
                _logger.LogError("No data found!");
                return Redirect("/blog");
            }

            var blog = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(x => x.Blog_Id == id);
            if (blog is null)
            {
                TempData["Message"] = "No data found.";
                TempData["IsSuccess"] = false;
                return Redirect("/blog");
            }
            _logger.LogInformation(JsonSerializer.Serialize(blog));
            return View("BlogEdit", blog);
        }

        [HttpPost]
        [ActionName("Update")]
        public async Task<IActionResult> BlogUpdate(int id, BlogDataModel reqModel)
        {
            _logger.LogInformation("User update data =>" + JsonSerializer.Serialize(reqModel));
            if (!await _context.Blogs.AsNoTracking().AnyAsync(x => x.Blog_Id == id))
            {
                TempData["Message"] = "No data found.";
                TempData["IsSuccess"] = false;
                return Redirect("/blog");
            }

            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Blog_Id == id);
            if (blog is null)
            {
                TempData["Message"] = "No data found.";
                TempData["IsSuccess"] = false;
                return Redirect("/blog");
            }

            blog.Blog_Title = reqModel.Blog_Title;
            blog.Blog_Author = reqModel.Blog_Author;
            blog.Blog_Content = reqModel.Blog_Content;

            _context.Blogs.Entry(blog).State = EntityState.Modified;
            int result = _context.SaveChanges();

            string message = result > 0 ? "Updating Successful." : "Updating Failed.";
            TempData["Message"] = message;
            TempData["IsSuccess"] = result > 0;
            _logger.LogInformation("Blog Update =>" + JsonSerializer.Serialize(message));
            return Redirect("/blog");
        }

        [ActionName("Delete")]
        public async Task<IActionResult> BlogDelete(int id)
        {
            _logger.LogInformation("Delete ID =>{id}", id );
            if (!await _context.Blogs.AsNoTracking().AnyAsync(x => x.Blog_Id == id))
            {
                TempData["Message"] = "No data found.";
                TempData["IsSuccess"] = false;
                return Redirect("/blog");
            }

            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Blog_Id == id);
            if (blog is null)
            {
                TempData["Message"] = "No data found.";
                TempData["IsSuccess"] = false;
                return Redirect("/blog");
            }

            _context.Remove(blog);
            int result = _context.SaveChanges();

            string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";
            TempData["Message"] = message;
            TempData["IsSuccess"] = result > 0;
            _logger.LogInformation("Delete Blog =>" + JsonSerializer.Serialize(message));
            return Redirect("/blog");
        }
    }
}
