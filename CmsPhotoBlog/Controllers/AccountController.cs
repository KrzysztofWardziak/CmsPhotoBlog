using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CmsPhotoBlog.Models;
using CmsPhotoBlog.Models.Account;
using CmsPhotoBlog.ViewModels.Account;

namespace CmsPhotoBlog.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/Account/Login");
        }
        // GET: Account/Login
        public ActionResult Login()
        {
            // sprawdzaenie czy uzytkownik nie jest juz zalogowany
            string userName = User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
                return RedirectToAction("user-profile");

            // zwracamy widok 
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(LoginUserVm model)
        {
            // sprawdzenie model state
            if (!ModelState.IsValid)
                return View(model);

            // sprawdzamy uzytkownika
            bool isValid = false;
            using (Db db = new Db())
            {
                if (db.Users.Any(x => x.UserName.Equals(model.UserName) && x.Password.Equals(model.Password)))
                    isValid = true;
            }

            if (!isValid)
            {
                ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika lub hasło.");
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.UserName, model.RememberMe));
            }
        }

        // GET: Account/CreateAccount
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        // POST: Account/CreateAccount
        [HttpPost]
        public ActionResult CreateAccount(UserVm model)
        {
            // sprawdzanie model state
            if (!ModelState.IsValid)
            {
                return View("CreateAccount", model);
            }

            // sprawdzenie hasła
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Hasła do siebie nie pasują.");
                return View("CreateAccount", model);
            }

            using (Db db = new Db())
            {
                // sprawdzenie czy nazwa uzytkownika jest unikalna 
                if (db.Users.Any(x => x.UserName.Equals(model.UserName)))
                {
                    ModelState.AddModelError("", "Nazwa użytkownika " + model.UserName + " już jest zajęta.");
                    model.UserName = "";
                    return View("CreateAccount", model);
                }

                // tworzenie uzytkownika
                User userDto = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    UserName = model.UserName,
                    Password = model.Password
                };

                //dodanie uzytkownika i zapis na bazie
                db.Users.Add(userDto);
                db.SaveChanges();

                // dodanie roli dla uzytkownika
                UserRole userRoleDto = new UserRole()
                {
                    UserId = userDto.Id,
                    RoleId = 2
                };

                // dodanie roli i zapis na bazie
                db.UserRoles.Add(userRoleDto);
                db.SaveChanges();
            }

            // TempData komunikat 
            TempData["SM"] = "Jesteś teraz zarejestrowany i możesz się zalogować.";

            return Redirect("~/Account/Login");
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return Redirect("~/Account/Login");
        }

        // GET: Account/UserNavPartial
        public ActionResult UserNavPartial()
        {
            // pobieramy username
            string username = User.Identity.Name;

            // deklarujemy model
            UserNavPartialVm model;

            using (Db db = new Db())
            {
                // pobieramy uzytkownika
                User dto = db.Users.FirstOrDefault(x => x.UserName == username);

                model = new UserNavPartialVm()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                };
            }

            return PartialView(model);
        }
        
        // GET: Account/UserProfile
        public ActionResult UserProfile()
        {
            // pobieramy nazwe uzytkownika
            string username = User.Identity.Name;

            // deklarujemy model
            UserProfileVm model;

            using (Db db = new Db())
            {
                // pobieramy uzytkownika
                User dto = db.Users.FirstOrDefault(x => x.UserName == username);

                model = new UserProfileVm(dto);
            }
            return View(model);
        }

        // POST: Account/UserProfile
        [HttpPost]
        public ActionResult UserProfile(UserProfileVm model)
        {
            // sprawdzamy modelState
            if (!ModelState.IsValid)
                return View(model);

            // sprawdzamy hasla
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Hasła nie pasują do siebie.");
                    return View(model);
                }
            }

            using (Db db = new Db())
            {
                // pobieramy nazwe uzytkownika
                string username = User.Identity.Name;

                // czy nazwa uzytkownika jest unikalna
                if (db.Users.Where(x => x.Id != model.Id).Any(x => x.UserName == username))
                {
                    ModelState.AddModelError("", "Nazwa użytkownika " + model.UserName + " zajęta");
                    model.UserName = "";
                    return View(model);
                }

                // edycja dto
                Models.User dto = db.Users.Find(model.Id);
                dto.FirstName = model.FirstName;
                dto.LastName = model.LastName;
                dto.EmailAddress = model.EmailAddress;
                dto.UserName = model.UserName;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    dto.Password = model.Password;
                }

                // zapis na bazie
                db.SaveChanges();

            }
            
            // ustawianie komunikatu
            TempData["SM"] = "Edytowano profil.";

            return Redirect("~/Account/UserProfile");
        }

    }
}