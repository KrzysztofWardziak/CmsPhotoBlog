using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CmsPhotoBlog.Models;
using CmsPhotoBlog.ViewModels.Pages;
using Page = CmsPhotoBlog.Models.Page;


namespace CmsPhotoBlog.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Deklaracja listy PageVm
            List<PageVm> pagesList;

            using (Db db = new Db())
            {
                // Inicjalizacja listy 
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVm(x)).ToList();
            }

            // zwracamy strony do widoku 
            return View(pagesList);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVm model)
        {
            // sprawdzanie model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                string slug;

                // Inicjalizacja Page 
                Page dto = new Page();

                // gdy nie mamy adresu strony to przypisujemy tytul
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                // zapobiegamy dodanie takiej samej nazwy strony
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Ten tytuł lub adres strony już istnieje");
                    return View(model);
                }

                dto.Title = model.Title;
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSideBar = model.HasSideBar;
                dto.Sorting = 1000;

                // zapis dto
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            TempData["SM"] = "Dodano nową stronę";

            return RedirectToAction("AddPage");
        }

        // GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            // deklaracja PageVm
            PageVm model;

            using (Db db = new Db())
            {
                // pobranie strony która chcemy edytowac
                Page dto = db.Pages.Find(id);

                // sprawdzamy czy taka strona istnieje
                if (dto == null)
                {
                    return Content("Strona nie istnieje");
                }

                model = new PageVm(dto);
            }

            return View(model);
        }

        // POST: Admin/Pages/EditPage
        [HttpPost]
        public ActionResult EditPage(PageVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                // pobranie id strony 
                int id = model.Id;

                // inicjalizacja slug 
                string slug = "home";

                // pobranie strony do edycji
                Page dto = db.Pages.Find(id);


                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                // sprawdzamy unikalnosc strony, adresu
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) ||
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "Strona lub tytuł już istnieje");
                }

                // modyfikacja dto
                dto.Title = model.Title;
                dto.Slug = slug;
                dto.HasSideBar = model.HasSideBar;
                dto.Body = model.Body;

                // zapis edytowanej strony
                db.SaveChanges();
            }

            // ustawienie komunikatu
            TempData["SM"] = "Strona została edytowana";

            return RedirectToAction("EditPage");
        }

        // GET: Admin/Pages/Details/id
        [HttpGet]
        public ActionResult Details(int id)
        {
            // deklaracja PageVm 
            PageVm model;

            using (Db db = new Db())
            {
                // pobranie strony o id 
                Page dto = db.Pages.Find(id);

                // sprawdzenie czy strona o id istnieje 
                if (dto == null)
                {
                    return Content("Strona nie istnieje");
                }

                // inicjalizacja PageVm 
                model = new PageVm(dto);
            }

            return View(model);
        }

        // GET : Admin/Pages/Delete/id
        public ActionResult Delete(int id)
        {
            using (Db db = new Db())
            {
                // pobranie strony do usuniecia 
                Page dto = db.Pages.Find(id);

                // usuwanie wybranej strony z bazy
                db.Pages.Remove(dto);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // POST : Admin/Pages/ReorderPages
        [HttpPost]
        public ActionResult ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                int count = 1;
                Page dto;

                // sortowanie stron, zapis na bazie
                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();
                    count++;
                }
            }

            return View();
        }

        // GET : Admin/Pages/EdiSideBar
        public ActionResult EditSideBar()
        {
            // Deklaracja sidebarVm 
            SidebarVm model;

            using (Db db = new Db())
            {
                // pobieramy Sidebar 
                Sidebar dto = db.Sidebars.Find(1);

                // Inicjalizacja modelu 
                model = new SidebarVm(dto);
            }

            return View(model);
        }

        // POST : Admin/Pages/EdiSideBar
        [HttpPost]
        public ActionResult EditSideBar(SidebarVm model)
        {

            using (Db db = new Db())
            {
                // pobieramy Sidebar z bazy
                Sidebar dto = db.Sidebars.Find(1);
                
                // modyfikacja Sidebar
                dto.Body = model.Body;

                db.SaveChanges();
            }
            TempData["SM"] = "Zmodyfikowano pasek boczny";
            return RedirectToAction("EditSideBar");
        }
    }
}