using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsPhotoBlog.Models;
using CmsPhotoBlog.ViewModels.Blog;

namespace CmsPhotoBlog.Areas.Admin.Controllers
{
    public class BlogController : Controller
    {
        // GET: Admin/Blog/Categories
        public ActionResult Categories()
        {
            // deklaracja listy kategorii do wyswietlenia
            List<CategoryVm> categoryVmList;

            using (Db db = new Db())
            {
                categoryVmList = db.Categories
                    .ToArray()
                    .OrderBy(x => x.Sorting)
                    .Select(x => new CategoryVm(x))
                    .ToList();
            }

            return View(categoryVmList);
        }

        // POST: Admin/Blog/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            // Deklaracja id 
            string id;

            using (Db db= new Db())
            {
                // sprawdzanie czy nazwa kategorii jest unikalna 
                if (db.Categories.Any(x => x.Name == catName))
                        return "tytulzajety";
                
                // Inicjalizacja Categori dto 
                Category dto = new Category();
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 1000;

                // zapis do bazy
                db.Categories.Add(dto);
                db.SaveChanges();

                // pobieramy id 
                id = dto.Id.ToString();
            }

            return id;
        }
    }
}