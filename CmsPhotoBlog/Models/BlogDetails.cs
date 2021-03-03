using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsPhotoBlog.Models
{
    [Table("tblBlogDetails")]
    public class BlogDetails
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string ImageName { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string ModifiedDate { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}