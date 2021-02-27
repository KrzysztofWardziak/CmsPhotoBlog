﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsPhotoBlog.Models
{
    [Table("tblSidebar")]
    public class Sidebar
    {
        [Key]
        public int Id { get; set; }
        public string Body { get; set; }
    }
}