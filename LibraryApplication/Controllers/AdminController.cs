using LibraryApplication.DataAccessObjects;
using LibraryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LibraryApplication.Controllers
{
    public class AdminController : Controller
    {
        AdminDAO dao = new AdminDAO();
        List<Book> Books = new List<Book>();

        [HttpGet]
        public ActionResult ChangeQuantity(int id)
        {
            if (CurrentUserInfo.Reader.Name == "Administrator" && CurrentUserInfo.Reader.EMail == "Administrator")
            {
                Book b = dao.GetBooks(false)
                   .FirstOrDefault(p => p.ID == id);
                return View(b);
            }
            else return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult ChangeQuantity(Book model)
        {
            dao.UpdateBookQuantity(model, model.Quantity);
            return RedirectToAction("Index","Home");
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (CurrentUserInfo.Reader.Name == "Administrator" && CurrentUserInfo.Reader.EMail == "Administrator")
            {
                Book product = dao.GetBooks(false)
                .FirstOrDefault(p => p.ID == id);
                return View(product);
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Delete(Book model)
        {
            dao.DeleteBook(model);
            return RedirectToAction("Index", "Home");
        }


        public ActionResult History(int id)
        {
            if (CurrentUserInfo.Reader.Name == "Administrator" && CurrentUserInfo.Reader.EMail == "Administrator")
            {
                Book b = dao.GetBooks(false)
                .FirstOrDefault(p => p.ID == id);
                dao.GetBookHistory(b);
                return View(b);
            }
            else return RedirectToAction("Index", "Home");
        }


        public ActionResult Create()
        {
            if (CurrentUserInfo.Reader.Name == "Administrator" && CurrentUserInfo.Reader.EMail == "Administrator")
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Create(string Authors, string BookTitle, int Quantity = 0)
        {
            if (Authors != "" && BookTitle != "")
            {
                try
                {
                    dao.InsertBook(Authors, BookTitle, Quantity);
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    return View(new { Authors, BookTitle, Quantity });
                }
            }
            else return View();
        }


        public ActionResult ReadersInfo()
        {
            if (CurrentUserInfo.Reader.Name == "Administrator" && CurrentUserInfo.Reader.EMail == "Administrator")
            {
                List<Reader> readers = dao.GetAllReaders();
                readers.RemoveAt(1);
                return View(readers);
            }
            else return RedirectToAction("Index", "Home");
        }

        public ActionResult ReaderHistory(int id)
        {
            Reader reader = dao.GetAllReaders()
            .FirstOrDefault(p => p.ID == id);
            reader.Books = dao.GetReaderBooks(reader);
            return View(reader);
        }

    }
}