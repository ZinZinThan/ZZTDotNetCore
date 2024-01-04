using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZTDotNetCore.AtmWebApp.Models;

namespace ZZTDotNetCore.AtmWebApp.EFDbContext
{
    public class AppDbContext : DbContext
    {
		public AppDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<AtmDataModel> AtmDatas { get; set; }
    }
}
