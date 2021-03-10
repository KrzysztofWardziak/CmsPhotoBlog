using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CmsPhotoBlog.Models;

namespace CmsPhotoBlog.ViewModels.Blog
{
    public class MessageInfoVm
    {
        public MessageInfoVm()
        {
            
        }

        public MessageInfoVm(Message row)
        {
            Id = row.Id;
            Name = row.Name;
            Email = row.Email;
            MessageInfo = row.MessageInfo;
            Date = row.Date;
        }

        public int Id { get; set; }
        [Display(Name = "Imię i nazwisko")]
        public string Name { get; set; }
        [Display(Name = "Adres E-mail")]
        public string Email { get; set; }
        [Display(Name = "Treść wiadomości")]
        public string MessageInfo { get; set; }
        public string Date { get; set; }
    }
}