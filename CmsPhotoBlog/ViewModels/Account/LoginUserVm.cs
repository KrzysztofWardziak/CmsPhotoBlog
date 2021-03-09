using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CmsPhotoBlog.ViewModels.Account
{
    public class LoginUserVm
    {
        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}