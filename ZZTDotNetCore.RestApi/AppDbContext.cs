using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZTDotNetCore.RestApi.Models;

namespace ZZTDotNetCore.RestApi
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var sqlConnectionStringBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = "DESKTOP-TVK5D53\\SQL2022",
                    InitialCatalog = "DotNetCore",
                    UserID = "sa",
                    Password = "@visible1",
                    TrustServerCertificate = true

                    //DataSource = "DESKTOP-2RCKCTJ\\SQLSERVER",
                    //InitialCatalog = "DotNetCore",
                    //UserID = "sa",
                    //Password = "sasa",
                    //TrustServerCertificate = true
                };
                optionsBuilder.UseSqlServer(sqlConnectionStringBuilder.ConnectionString);
            }
        }

        public DbSet<BlogDataModel> Blogs { get; set; }
    }
}
