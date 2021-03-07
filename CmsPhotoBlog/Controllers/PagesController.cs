using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsPhotoBlog.Models;
using CmsPhotoBlog.ViewModels.Pages;

namespace CmsPhotoBlog.Controllers
{
    public class PagesController : Controller
    {
        // GET: Index/{pages}
        public ActionResult Index(string page = "")
        {
            // ustalamy adres naszej strony
            if (page == "")
                page = "home";

            // deklarujemy pageVm i pageDto
            PageVm model;
            Page dto;

            // sprawdzamy czy nasza strona istnieje
            using (Db db = new Db())
            {
                if (!db.Pages.Any(x => x.Slug.Equals(page)))
                    return RedirectToAction("Index", new {page = ""});
                
            }

            // pobieramy pageDto
            using (Db db = new Db())
            {
                 dto = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }
            // ustawiamy tytul naszej strony
            ViewBag.PageTitle = dto.Title;

            // sprawdzamy czy strona ma pasek boczny
            if (dto.HasSideBar == true)
                ViewBag.Sidebar = "Tak";

            else
                ViewBag.Sidebar = "Nie";
            

            // inicjalizujemy pageVm
            model = new PageVm(dto);

            //zwracamy widok z modelem
            return View(model);
        }
    }
}