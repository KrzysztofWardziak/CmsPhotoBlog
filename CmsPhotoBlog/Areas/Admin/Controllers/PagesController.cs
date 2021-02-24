using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CmsPhotoBlog.Models;
using CmsPhotoBlog.ViewModels.Pages;


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
        public ActionResult AddPage()
        {
            return View();
        }
    }
}