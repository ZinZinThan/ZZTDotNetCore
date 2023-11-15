using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZTDotNetCore.ConsoleApp.Models;

namespace ZZTDotNetCore.ConsoleApp.DapperExamples
{
    public class DapperExample
    {
        private readonly SqlConnectionStringBuilder sqlConnectionStringBuilder;

        public DapperExample()
        {
            sqlConnectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = "DESKTOP-TVK5D53\\SQL2022",
                InitialCatalog = "DotNetCore",
                UserID = "sa",
                Password="@visible1"
            };
        }

        public void Run()
        {
            //Read();
            //Edit(6);
            //Create("test 13","test 14","test 15");
            //Update(6, "test 13", "test 14", "test 15");
            //Delete(1003);
        }

        private void Read()
        {
            #region Read/Retrieve

            string query = "select * from tbl_blog";
            using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            //List<dynamic> lst = db.Query(query).ToList();
            List<BlogDataModel> lst = db.Query<BlogDataModel>(query).ToList();  

            foreach ( var item in lst ) 
            {
                Console.WriteLine(item.Blog_Id);
                Console.WriteLine(item.Blog_Title);
                Console.WriteLine(item.Blog_Author);
                Console.WriteLine(item.Blog_Content);
            }

            #endregion
        }

        private void Edit(int id)
        {
            #region Edit

            BlogDataModel blog = new BlogDataModel 
            { 
                Blog_Id = id
            };  

            string query = "select * from tbl_blog where Blog_Id=@Blog_Id";
            using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            //BlogDataModel? item = db.Query<BlogDataModel>(query,new { Blog_Id = id }).FirstOrDefault();
            BlogDataModel? item = db.Query<BlogDataModel>(query, blog ).FirstOrDefault();
            
            if (item is null)
            {
                Console.WriteLine("No data found!");
                return;
            }

            Console.WriteLine(item.Blog_Id);
            Console.WriteLine(item.Blog_Title);
            Console.WriteLine(item.Blog_Author);
            Console.WriteLine(item.Blog_Content);
            
            #endregion
        }

        private void Create(string title,string author,string content)
        {
            #region Create

            BlogDataModel blog = new BlogDataModel
            {
                Blog_Title = title,
                Blog_Author = author,
                Blog_Content = content
            };

            string query = @"INSERT INTO [dbo].[Tbl_Blog]
           ([Blog_Title]
           ,[Blog_Author]
           ,[Blog_Content])
    VALUES
           (@Blog_Title
            ,@Blog_Author
            ,@Blog_Content)";

            using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(query, blog);

            string message = result > 0 ? "Saving Successful." : "Saving Failed.";
            Console.WriteLine(message);

            #endregion
        }

        private void Update(int id, string title, string author, string content)
        {
            #region Update

            BlogDataModel blog = new BlogDataModel
            {
                Blog_Id = id,
                Blog_Title = title,
                Blog_Author = author,
                Blog_Content = content
            };

            string query = @"UPDATE [dbo].[Tbl_Blog]
     SET [Blog_Title] = @Blog_Title
        ,[Blog_Author] = @Blog_Author
        ,[Blog_Content] = @Blog_Content
     WHERE Blog_Id = @Blog_Id";

            using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(query, blog);

            string message = result > 0 ? "Update Successful." : "Update Failed.";
            Console.WriteLine(message);

            #endregion
        }

        private void Delete(int id)
        {
            #region Create

            BlogDataModel blog = new BlogDataModel
            {
                Blog_Id = id
            };

            string query = @"DELETE FROM [dbo].[Tbl_Blog]
        WHERE Blog_Id = @Blog_Id";

            using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(query, blog);

            string message = result > 0 ? "Delete Successful." : "Delete Failed.";
            Console.WriteLine(message);

            #endregion
        }
    }
}
