using LibraryApplication.DataAccessObjects;
using LibraryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LibraryApplication.Controllers
{
    public class UserController : Controller
    {
        UserDAO dao = new UserDAO();
        public ActionResult MyBooks()
        {
            if (CurrentUserInfo.Reader.Name != "Administrator" && 
                CurrentUserInfo.Reader.EMail != "Administrator" && CurrentUserInfo.Reader.Name != null)
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }


        public ActionResult TakeABook(int id)
        {
            if (CurrentUserInfo.Reader.Name != "Administrator" &&  
                CurrentUserInfo.Reader.EMail != "Administrator" && CurrentUserInfo.Reader.Name != null)
            {
                Book b = dao.GetBooks()
               .FirstOrDefault(p => p.ID == id);
                dao.TakingABook(b);
                string authorsstring = "";
                foreach (Author a in b.Authors)
                {
                    authorsstring += a.Name + ", ";
                }

                var email = new NotificationEmail
                {
                    To = CurrentUserInfo.Reader.EMail,
                    UserName = CurrentUserInfo.Reader.Name,
                    MessageText = "You took the " + b.Name + " by " + authorsstring.Remove(authorsstring.Length - 2) + " in our library!"
                };

                
                email.Send();
                return View(b);
            }
            else return RedirectToAction("Index", "Home");
        }


    }
}