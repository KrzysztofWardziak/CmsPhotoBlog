using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsPhotoBlog.Application.ViewModels.Pages
{
    public class PageVm
    {
        public PageVm()
        {
            
        }

        public PageVm(Page row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSideBar = row.HasSideBar;
        }

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Body { get; set; }
        public int Sorting { get; set; }
        public byte HasSideBar { get; set; }
    }
}
