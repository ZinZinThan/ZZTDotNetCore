using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ZZTDotNetCore.RestApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ZZTDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdoDotNetController : ControllerBase
    {
        private readonly SqlConnectionStringBuilder sqlConnectionStringBuilder;
        
        public BlogAdoDotNetController()
        {
            sqlConnectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = "DESKTOP-TVK5D53\\SQL2022",
                InitialCatalog = "DotNetCore",
                UserID = "sa",
                Password = "@visible1",

                //DataSource = "DESKTOP-2RCKCTJ\\SQLSERVER",
                //InitialCatalog = "DotNetCore",
                //UserID = "sa",
                //Password = "sasa"
            };
        }

        [HttpGet]
        public IActionResult GetBlogs()
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = "select * from tbl_blog";
            SqlCommand cmd = new SqlCommand(query,connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            List<BlogDataModel> lst = new List<BlogDataModel>();

            foreach (DataRow row in dt.Rows)
            {
                BlogDataModel item = new BlogDataModel();
                item.Blog_Id = Convert.ToInt32( row["Blog_Id"]);
                item.Blog_Title = row["Blog_Title"].ToString();
                item.Blog_Author = row["Blog_Title"].ToString();
                item.Blog_Content = row["Blog_Content"].ToString();
                lst.Add(item);

            }
            BlogListResponseModel model = new BlogListResponseModel
            {
                IsSuccess = true,
                Message = "Success",
                Data =lst
            };
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = "select * from tbl_blog where Blog_Id=@Blog_Id";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Blog_Id", id);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            if (dt.Rows.Count == 0)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                return NotFound(response);
            }

            DataRow row = dt.Rows[0];

            BlogDataModel item = new BlogDataModel();
            item.Blog_Id = Convert.ToInt32(row["Blog_Id"]);
            item.Blog_Title = row["Blog_Title"].ToString();
            item.Blog_Author = row["Blog_Title"].ToString();
            item.Blog_Content = row["Blog_Content"].ToString();

            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateBlog(BlogDataModel blog)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = @"INSERT INTO [dbo].[Tbl_Blog]
           ([Blog_Title]
           ,[Blog_Author]
           ,[Blog_Content])
    VALUES
           (@Blog_Title
            ,@Blog_Author
            ,@Blog_Content)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Blog_Title", blog.Blog_Title);
            cmd.Parameters.AddWithValue("@Blog_Author", blog.Blog_Author);
            cmd.Parameters.AddWithValue("@Blog_Content", blog.Blog_Content);
            int result = cmd.ExecuteNonQuery();

            connection.Close();

            BlogResponseModel model = new BlogResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Saving Successful." : "Saving Failed.",
                Data = blog
            };
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, BlogDataModel blog)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = "select * from tbl_blog where Blog_Id=@Blog_Id";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Blog_Id", id);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            if (dt.Rows.Count == 0)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                return NotFound(response);
            }

            connection.Open();

            query = @"UPDATE [dbo].[Tbl_Blog]
                    SET [Blog_Title] = @Blog_Title
                        ,[Blog_Author] = @Blog_Author
                        ,[Blog_Content] = @Blog_Content
                    WHERE Blog_Id = @Blog_Id";

            cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Blog_Id", id);
            cmd.Parameters.AddWithValue("@Blog_Title", blog.Blog_Title);
            cmd.Parameters.AddWithValue("@Blog_Author", blog.Blog_Author);
            cmd.Parameters.AddWithValue("@Blog_Content", blog.Blog_Content);
            int result = cmd.ExecuteNonQuery();

            connection.Close();

            BlogResponseModel model = new BlogResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Saving Successful." : "Saving Failed.",
                Data = blog
            };
            return Ok(model);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogDataModel blog)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = "select * from tbl_blog where Blog_Id=@Blog_Id";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Blog_Id", id);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            if (dt.Rows.Count == 0)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                return NotFound(response);
            }

            connection.Open();

            SqlCommand command = new SqlCommand();
            string conditions = "";

            if (!string.IsNullOrEmpty(blog.Blog_Title))
            {
                conditions += " [Blog_Title] = @Blog_Title, ";
                command.Parameters.AddWithValue("@Blog_Title", blog.Blog_Title);
            }
            if (!string.IsNullOrEmpty(blog.Blog_Author))
            {
                conditions += " [Blog_Author] = @Blog_Author, ";
                command.Parameters.AddWithValue("@Blog_Author", blog.Blog_Author);
            }
            if (!string.IsNullOrEmpty(blog.Blog_Content))
            {
                conditions += " [Blog_Content] = @Blog_Content, ";
                command.Parameters.AddWithValue("@Blog_Content", blog.Blog_Content);
            }
            if (conditions.Length == 0)
            {
                var response = new { IsSuccess = false, Message = "No data to update." };
                return NotFound(response);
            }

            conditions = conditions.Substring(0, conditions.Length - 2);

            query = $@"UPDATE [dbo].[Tbl_Blog]
                    SET {conditions}
                    WHERE Blog_Id = @Blog_Id";

            command.CommandText = query;
            command.Connection = connection;
            command.Parameters.AddWithValue("@Blog_Id", id);
            int result = command.ExecuteNonQuery();

            connection.Close();

            BlogResponseModel model = new BlogResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Update Successful." : "Updating Failed.",
                Data = blog
            };
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = "select * from tbl_blog where Blog_Id=@Blog_Id";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Blog_Id", id);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            if (dt.Rows.Count == 0)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                return NotFound(response);
            }

            connection.Open();

            query = @"DELETE FROM [dbo].[Tbl_Blog]
                      WHERE Blog_Id = @Blog_Id" ;

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Blog_Id", id);
            int result = command.ExecuteNonQuery();

            connection.Close();
             
            BlogResponseModel model = new BlogResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Deleting Successful." : "Deleting Failed."
            };
            return Ok(model);
        }

    }
}
