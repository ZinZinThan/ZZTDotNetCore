using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZTDotNetCore.ConsoleApp.Models
{
    [Table("Tbl_Blog")] // Define Table Name
    public class BlogDataModel
    {
        [Key] //Define Primary Key 
        public int Blog_Id { get; set; }
        //[Column("BlogTitle")]  
        public string Blog_Title { get; set; }
        public string Blog_Author { get; set; }
        public string Blog_Content { get; set; }
    }
}
