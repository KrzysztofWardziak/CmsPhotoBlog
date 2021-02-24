using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmsPhotoBlog.Models;

namespace CmsPhotoBlog.ViewModels.Pages
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
        [Display(Name = "Tytuł strony")]
        public string Title { get; set; }
        [Display(Name = "Adres strony")]
        public string Slug { get; set; }
        [Required]
        [Display(Name = "Zawartość strony")]
        public string Body { get; set; }
        public int Sorting { get; set; }
        [Display(Name = "Pasek boczny")]
        public bool HasSideBar { get; set; }
    }
}
