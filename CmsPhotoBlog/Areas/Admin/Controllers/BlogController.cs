using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using CmsPhotoBlog.Models;
using CmsPhotoBlog.ViewModels.Blog;
using PagedList;

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

        // POST: Admin/Blog/ReorderCategories
        [HttpPost]
        public ActionResult ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                // inicjalizacja licznika
                int count = 1;

                // deklaracja dto
                Category dto;

                // sortowanie kategorii 
                foreach (var catId in id)
                {
                    dto = db.Categories.Find(catId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }
            return View();
        }

        // GET: Admin/Blog/DeleteCategory
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {
                // pobieramy kategorie o podanym id
                Category dto = db.Categories.Find(id);

                // usuwamy kategorie 
                db.Categories.Remove(dto);
                db.SaveChanges();

            }

            return RedirectToAction("Categories");
        }

        // POST: Admin/Blog/RenameCategory
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            using (Db db = new Db())
            {
                // sprawdzenie czy kategoria jest unikalna
                if (db.Categories.Any(x => x.Name == newCatName))
                    return "tytulzajety";

                // pobieramy kategorie
                Category dto = db.Categories.Find(id);

                // edycja kategorii
                dto.Name = newCatName;
                dto.Slug = newCatName.Replace(" ", "-").ToLower();

                // zapis na bazie
                db.SaveChanges();
            }

            return "Ok";
        }

        // GET: Admin/Blog/AddBlog
        public ActionResult AddBlog()
        {
            // inicjalizacja modelu 
            BlogDetailsVm model = new BlogDetailsVm();

            // pobieramy liste kategorii
            using (Db db = new Db())
            {
                model.Categories= new SelectList(db.Categories.ToList(), "Id", "Name");
            }

            return View(model);
        }

        // POST: Admin/Blog/AddBlog
        [HttpPost]
        public ActionResult AddBlog(BlogDetailsVm model, HttpPostedFileBase file)
        {
            // sprawdzamy model state
            if (!ModelState.IsValid)
            {
                using (Db db = new Db())
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    return View(model);
                }
            }
            // sprawdzamy czy nazwa bloga jest unikalna
            using (Db db = new Db())
            {
                if (db.BlogDetailses.Any(x => x.Name == model.Name))
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "Ta nazwa bloga jest już zajęta.");
                    return View(model);
                }
            }
            // deklaracja blog id
            int id;

            // edycja bloga i zapis na bazie
            using (Db db = new Db())
            {
                var date = model.Date;
                date = DateTime.Now.ToString("D").ToString();
                BlogDetails blog = new BlogDetails();
                blog.Name = model.Name;
                blog.Title = model.Title;
                blog.Slug = model.Name.Replace(" ", "-").ToLower();
                blog.Description = model.Description;
                blog.CategoryId = model.CategoryId;
                blog.ImageName = model.ImageName;

                Category catDto = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                blog.CategoryName = catDto.Name;
                blog.Date = date;

                db.BlogDetailses.Add(blog);
                db.SaveChanges();

                // pobranie id dodanego bloga
                id = blog.Id;
            }
            // ustawiamy komunikat TempData
            TempData["SM"] = "Dodano blog";

            #region Upload Image
            // Utworzenie potrzebnej struktury katalogow 
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Blogs");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Blogs\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Blogs\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Blogs\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Blogs\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);
            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);
            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);
            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);
            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            if (file != null && file.ContentLength > 0)
            {
                // sprawdzenie rozszerzenia pliku czy to jest obrazek
                string ext = file.ContentType.ToLower();
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/xpng" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "Obraz nie został przesłany - nieprawidłowe rozszerzenie obrazu.");
                        return View(model);
                    }
                }

                // inicjalizacja nazwy obrazka
                string imageName = file.FileName;

                // zapis nazwy obrazka do bazy
                using (Db db = new Db())
                {
                    BlogDetails dto = db.BlogDetailses.Find(id);
                    dto.ImageName = imageName;
                    db.SaveChanges();
                }

                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                // zapisujemy oryginalny obrazek 
                file.SaveAs(path);
                
                // zapis miniaturki
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }
            #endregion

            return RedirectToAction("AddBlog");
        }

        // GET: Admin/Blog/AllBlogs
        [Route("wpisy")]
        public ActionResult AllBlogs(int? page, int? catId)
        {
            // deklaracja listy blogow 
            List<BlogDetailsVm> listOfBlogVm;

            // ustawianie numeru strony
            var pageNumber = page ?? 1;

            using (Db db = new Db())
            {
                // inicjalizacja listy blogow 
                listOfBlogVm = db.BlogDetailses.ToArray()
                    .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                    .Select(x => new BlogDetailsVm(x))
                    .ToList();

                // lista kategorii do dropDownList 
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

                // ustawiamy wybrana kategorie 
                ViewBag.SelectedCat = catId.ToString();
            }

            // ustawienie stronnicowania
            var onePageOfBlogs = listOfBlogVm.ToPagedList(pageNumber, 3);
            ViewBag.OnePageOfBlogs = onePageOfBlogs;

            // zwracamy widok z lista blogow

            return View(listOfBlogVm);
        }

        // GET: Admin/Blog/EditBlog
        public ActionResult EditBlog(int id)
        {
            // deklaracja blogVm
            BlogDetailsVm model;
            using (Db db = new Db())
            {
                // pobieramy blog do edycji
                 BlogDetails dto = db.BlogDetailses.Find(id);

                // sprawdzenie czy produkt istnieje
                if (dto == null)
                {
                    return Content("Ten wpis nie istnieje");
                }
                // inicjalizacja modelu 
                model = new BlogDetailsVm(dto);

                // lista kategorii
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

                // ustawiamy zdjecia
                model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Blogs/" + id + "/Gallery/Thumbs"))
                                                 .Select(fn => Path.GetFileName(fn));

            }
            return View(model);
        }

        // POST: Admin/Blog/EditBlog
        [HttpPost]
        public ActionResult EditBlog(BlogDetailsVm model, HttpPostedFileBase file)
        {
            // pobieramy id bloga
            int id = model.Id;

            // pobranie kategorii dla listy rozwijanej
            using (Db db = new Db())
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }

            // ustawiamy zdjecia
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Blogs/" + id + "/Gallery/Thumbs"))
                .Select(fn => Path.GetFileName(fn));

            // sprawdzamy modelState
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // sprawdzamy unikalnosc nazwy bloga 
            using (Db db = new Db())
            {
                if (db.BlogDetailses.Where(x => x.Id != id).Any(x => x.Name == model.Name))
                {
                    ModelState.AddModelError("", "Ta nazwa bloga jest już zajęta.");
                    return View(model);
                }
            }

            // edycja bloga i zapis na bazie
            using (Db db = new Db())
            {
                var mDate = model.ModifiedDate;
                mDate = DateTime.Now.ToString("D").ToString();
                BlogDetails dto = db.BlogDetailses.Find(id);
                dto.Name = model.Name;
                dto.Slug = model.Name.Replace(" ", "-").ToLower();
                dto.Title = model.Title;
                dto.Description = model.Description;
                dto.ImageName = model.ImageName;
                dto.CategoryId = model.CategoryId;

                Category catDto = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                dto.CategoryName = catDto.Name;
                dto.ModifiedDate = mDate;
                db.SaveChanges();
            }
            // ustawiamy zmienna TempData
            TempData["SM"] = "Edytowano blog";

            #region Image Upload

            // sprawdzamy czy jest plik do zaladowania
            if (file != null && file.ContentLength > 0)
            {
                // sprawdzamy rozszerzenie pliku czy to jest obrazek
                string ext = file.ContentType.ToLower();
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/xpng" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "Obraz nie został przesłany - nieprawidłowe rozszerzenie obrazu.");
                        return View(model);
                    }
                }

                // Utworzenie potrzebnej struktury katalogow 
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "Blogs\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Blogs\\" + id.ToString() + "\\Thumbs");

                // usuwamy stare pliki z katalogow
                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);

                foreach (var file2 in di1.GetFiles())
                    file2.Delete();

                foreach(var file3 in di2.GetFiles())
                    file3.Delete();
                
                // zapisujemy nazwe obrazka na bazie
                string imageName = file.FileName;

                using (Db db = new Db())
                {
                    BlogDetails dto = db.BlogDetailses.Find(id);
                    dto.ImageName = imageName;
                    db.SaveChanges();
                }

                // zapis nowych plikow
                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);

                file.SaveAs(path);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);

            }

            #endregion


            return RedirectToAction("EditBlog");
        }

        // GET: Admin/Blog/DeleteBlog
        public ActionResult DeleteBlog(int id)
        {
            // usuniecie bloga z bazy
            using (Db db = new Db())
            {
                BlogDetails dto = db.BlogDetailses.Find(id);
                db.BlogDetailses.Remove(dto);
                db.SaveChanges();
            }
            // usuniecie folderu blogu z plikami
            var orginalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            var pathString = Path.Combine(orginalDirectory.ToString(), "Blogs\\" + id.ToString());

            if (Directory.Exists(pathString))
                    Directory.Delete(pathString, true);

            return RedirectToAction("AllBlogs");
        }

        // POST: Admin/Blog/SaveGalleryImages
        [HttpPost]
        public ActionResult SaveGalleryImages(int id)
        {
            // petla po obrazkach
            foreach (string fileName in Request.Files)
            {
                // inicjalizacja 
                HttpPostedFileBase file = Request.Files[fileName];

                // sprawdzamy czy mam plik i czy nie jest pusty
                if (file != null && file.ContentLength > 0)
                {
                    // Utworzenie potrzebnej struktury katalogów
                    var originalDirectory =
                        new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                    string pathString1 = Path.Combine(originalDirectory.ToString(),
                        "Blogs\\" + id.ToString() + "\\Gallery");
                    string pathString2 = Path.Combine(originalDirectory.ToString(),
                        "Blogs\\" + id.ToString() + "\\Gallery\\Thumbs");

                    var path = string.Format("{0}\\{1}", pathString1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, file.FileName);

                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
                }
            }

            return View();
        }

        // POST: Admin/Blog/DeleteImage
        [HttpPost]
        public void DeleteImage(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~/Images/Uploads/Blogs/" + id.ToString() + "/Gallery/" + imageName);
            string fullPath2 = Request.MapPath("~/Images/Uploads/Blogs/" + id.ToString() + "/Gallery/Thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1))
                System.IO.File.Delete(fullPath1);

            if (System.IO.File.Exists(fullPath2))
                System.IO.File.Delete(fullPath2);
        }
    }
}