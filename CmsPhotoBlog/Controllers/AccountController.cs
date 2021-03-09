using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsPhotoBlog.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: Account/CreateAccount
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }
    }
}