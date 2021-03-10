using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsPhotoBlog.Models;

namespace CmsPhotoBlog.ViewModels.Pages
{
    public class SidebarVm
    {
        public SidebarVm()
        {
            
        }

        public SidebarVm(Sidebar row)
        {
            Id = row.Id;
            Body = row.Body;
        }

        public int Id { get; set; }
        [AllowHtml]
        public string Body { get; set; }

    }
}