using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsPhotoBlog.Models;

namespace CmsPhotoBlog.ViewModels.Blog
{
    public class CategoryVm
    {
        public CategoryVm()
        {
            
        }

        public CategoryVm(Category row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Sorting = row.Sorting;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}