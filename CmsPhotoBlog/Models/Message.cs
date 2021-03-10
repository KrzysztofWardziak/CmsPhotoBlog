using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsPhotoBlog.Models
{
    [Table("tblMessages")]
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MessageInfo { get; set; }
        public string Date { get; set; }
    }
}