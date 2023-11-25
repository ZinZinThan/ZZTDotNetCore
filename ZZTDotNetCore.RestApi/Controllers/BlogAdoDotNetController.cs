using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ZZTDotNetCore.RestApi.Models;

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
                DataSource = "DESKTOP-2RCKCTJ\\SQLSERVER",
                InitialCatalog = "DotNetCore",
                UserID = "sa",
                Password = "sasa"
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

    }
}
