using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsPhotoBlog.Models;

namespace CmsPhotoBlog.ViewModels.Blog
{
    public class BlogDetailsVm
    {
        public BlogDetailsVm()
        {
            
        }

        public BlogDetailsVm(BlogDetails row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Description = row.Description;
            CategoryName = row.CategoryName;
            CategoryId = row.CategoryId;
            ImageName = row.ImageName;
            Date = row.Date;
            ModifiedDate = row.ModifiedDate;
            Name = row.Name;
        }

        public int Id { get; set; }
        [Display(Name = "Tytuł")]
        [Required]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Display(Name = "Opis")]
        [Required]
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string ImageName { get; set; }
        [Display(Name = "Data")]
        public string Date { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name= "Data modyfikacji")]
        public string ModifiedDate { get; set; }


        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}