using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsPhotoBlog.Models;
using CmsPhotoBlog.ViewModels.Blog;

namespace CmsPhotoBlog.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            // deklarujemy categoryVmList
            List<CategoryVm> categoryVmList;

            // inicjalizacja listy
            using (Db db = new Db())
            {
                categoryVmList = db.Categories
                    .ToArray()
                    .OrderBy(x => x.Sorting)
                    .Select(x => new CategoryVm(x))
                    .ToList();
            }

            // zwracamy partial z lista
            return PartialView(categoryVmList);
        }

        // GET: Blog/category/name
        public ActionResult Category(string name)
        {
            // deklaracja productVmList 
            List<BlogDetailsVm> blogVmList;

            using (Db db = new Db())
            {
                Category categoryDto = db.Categories.Where(x => x.Slug == name).FirstOrDefault();
                int catId = categoryDto.Id;

                // inicjalizacja listy blogow
                blogVmList = db.BlogDetailses
                    .ToArray()
                    .Where(x => x.CategoryId == catId)
                    .Select(x => new BlogDetailsVm(x))
                    .ToList();

                // pobieramy nazwe kategori
                var blogCat = db.BlogDetailses.Where(x => x.CategoryId == catId).FirstOrDefault();
                ViewBag.CategoryName = blogCat.CategoryName;
            }
            // zwracamy widok z lista blogow z danej kategorii
            return View(blogVmList);
        }

        // GET: Blog/blog-szczegoly/name
        [ActionName("blog-szczegoly")]
        public ActionResult BlogDetails(string name)
        {
            // deklaracja blogDto i blogVm
            BlogDetailsVm model;
            BlogDetails dto;

            // inicjalizacja blog id
            int id = 0;

            using (Db db = new Db())
            {
                // sprawdzamy czy blog istnieje
                if (!db.BlogDetailses.Any(x => x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Blog");
                }

                // inicjalizacja blogDto
                dto = db.BlogDetailses.Where(x => x.Slug == name).FirstOrDefault();

                // pobranie id 
                id = dto.Id;

                // inicjalizacja modelu 
                model = new BlogDetailsVm(dto);
            }

            // pobieramy galerie obrazków
            model.GalleryImages =
                Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Blogs/" + id + "/Gallery/Thumbs"))
                    .Select(fn => Path.GetFileName(fn));

            // zwracamy widok z modelem
            return View("BlogDetails", model);
        }
    }
}