﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;
using ZZTDotNetCore.MinimalApi;
using ZZTDotNetCore.MinimalApi.Models;

namespace ZZTDotNetCore.MinimalApi.Features.Blog;

public static class BlogService
{
	public static void AddBlogService(this IEndpointRouteBuilder app)
	{
		#region Get

		app.MapGet("/blog/{pageNo}/{pageSize}", async ([FromServices] AppDbContext db,[FromServices] ILogger<Program> _logger, int pageNo, int pageSize) =>
		{
			var lst = await db.Blogs
			.AsNoTracking()
			.Skip((pageNo - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();
			_logger.LogInformation("Blog List => " + JsonSerializer.Serialize(lst));

			return lst;
		})
		.WithName("GetBlogs")
		.WithOpenApi();

		#endregion

		#region Post

		app.MapPost("/blog", async ([FromServices] AppDbContext db,[FromServices] ILogger<Program> _logger, BlogDataModel blog) =>
		{
            _logger.LogInformation("User input => {@blog}", blog);
            await db.Blogs.AddAsync(blog);
			int result = await db.SaveChangesAsync();
            string message = result > 0 ? "Saving Successful." : "Saving Failed.";

			BlogResponseModel model = new BlogResponseModel
			{
				IsSuccess = result > 0,
				Message = message,
                Data = blog
			};

            _logger.LogInformation("Blog Create => " + JsonSerializer.Serialize(model));

            return Results.Ok(model);
		})
		.WithName("CreateBlog")
		.WithOpenApi();

		#endregion

		#region Put

		app.MapPut("/blog/{id}", async ([FromServices] AppDbContext db, [FromServices] ILogger < Program > _logger, int id, BlogDataModel blog) =>
		{
            _logger.LogInformation("User update data => " + JsonSerializer.Serialize(blog));
            var item = await db.Blogs.FirstOrDefaultAsync(x => x.Blog_Id == id);
			if (item is null)
			{
				return Results.NotFound(new
				{
					IsSuccess = false,
					Message = "No data found."
				});
			}
            item.Blog_Title = blog.Blog_Title;
			item.Blog_Author = blog.Blog_Author;
			item.Blog_Content = blog.Blog_Content;
            var result = db.SaveChanges();
			BlogResponseModel model = new BlogResponseModel
			{
				IsSuccess = result > 0,
				Message = result > 0 ? "Update Successful." : "Updating Failed.",
				Data = item
			};
			_logger.LogInformation("Blog Update => " + JsonSerializer.Serialize(model));
			return Results.Ok(model);
		})
		.WithName("UpdateBlog")
		.WithOpenApi();

		#endregion

		#region Patch

		app.MapPatch("/blog/{id}", ([FromServices] AppDbContext db, int id, BlogDataModel blog) =>
		{
			var item = db.Blogs.FirstOrDefault(x => x.Blog_Id == id);

			if (item is null)
			{
				var response = new { IsSuccess = false, Message = "No data found." };
				return Results.NotFound(response);
			}

			if (!string.IsNullOrEmpty(blog.Blog_Title))
			{
				item.Blog_Title = blog.Blog_Title;
			}
			if (!string.IsNullOrEmpty(blog.Blog_Author))
			{
				item.Blog_Author = blog.Blog_Author;
			}
			if (!string.IsNullOrEmpty(blog.Blog_Content))
			{
				item.Blog_Content = blog.Blog_Content;
			}

			var result = db.SaveChanges();
			BlogResponseModel model = new BlogResponseModel
			{
				IsSuccess = result > 0,
				Message = result > 0 ? "Updating Successful." : "Updating Failed.",
				Data = item
			};
			return Results.Ok(model);
		})
		.WithName("PatchBlog")
		.WithOpenApi();

		#endregion

		#region Delete

		app.MapDelete("/blog/{id}", ([FromServices] AppDbContext db,[FromServices] ILogger<Program> _logger, int id) =>
		{
			_logger.LogInformation("User delete data ID => {@id}", id);
			var item = db.Blogs.FirstOrDefault(x => x.Blog_Id == id);
			if (item is null)
			{
				var response = new { IsSuccess = false, Message = "No data found." };
				return Results.NotFound(response);
			}

			db.Blogs.Remove(item);
			var result = db.SaveChanges();
			BlogResponseModel model = new BlogResponseModel
			{
				IsSuccess = result > 0,
				Message = result > 0 ? "Deleting Successful." : "Deleting Failed."
			};
            _logger.LogInformation("Blog Delete => {@model}", model);
            return Results.Ok(model);
		})
		.WithName("DeleteBlog")
		.WithOpenApi();

		#endregion
	}
}