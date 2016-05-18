using LibraryApplication.DataAccessObjects;
using LibraryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryApplication.Controllers
{


    public class HomeController : Controller
    {

        
        DefaultDAO dao = new DefaultDAO();
        public List<Book> Books;
        int PageSize = 10;


        public ActionResult ReaderBooksTable(int id)
        {
            Reader r = dao.GetAllReaders().Where(x => x.ID == id).SingleOrDefault();
            r.Books = dao.GetReaderBooks(r);
            r.Books = r.Books.OrderBy(b => b.DateBookTaken.ToString()).ToList();
            return PartialView(r);
        }


        public ActionResult BooksTable(bool sorting, bool availability, string button = "0") //sorting true = by name, false = by author
        {

            PagingInfo pimodel = new PagingInfo();
            pimodel.Filtering[0] = availability;
            pimodel.Filtering[1] = sorting;
            if (button != "0")
            {
                pimodel.CurrentPage = int.Parse(button);
            }
            if (pimodel.Filtering[0])
            {
                Books = dao.GetBooks();
            }
            else
            {
                Books = dao.GetBooks(false);
            }
            pimodel.ItemsPerPage = PageSize;
            pimodel.TotalItems = Books.Count();
            BooksViewModel bvmodel = new BooksViewModel();
            bvmodel.PagingInfo = pimodel;

            if (pimodel.Filtering[1])
                bvmodel.Books = Books.OrderBy(b => b.Name).ToList();

            else bvmodel.Books = Books.OrderBy(b => b.Authors[0].Name).ToList();

            bvmodel.Books = bvmodel.Books
                .Skip((pimodel.CurrentPage - 1) * pimodel.ItemsPerPage)
                .Take(pimodel.ItemsPerPage).ToList();     

            return PartialView(bvmodel);
        }


        public ActionResult Index(PagingInfo model)
        {
                Books = dao.GetBooks(false);
                model.ItemsPerPage = PageSize;
                model.TotalItems = Books.Count();
                return View((object)model);

        }


        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(Reader R)
        {
            List<Reader> readers = dao.GetAllReaders();
            var isInBase = readers.Select(m => m).Where(m => m.EMail == R.EMail && m.Name == R.Name).ToList();
            if (isInBase.Count != 0)
            {
                CurrentUserInfo.Reader = isInBase[0];
                return RedirectToAction("Index");
            }
            else
            {

                return View(R);
            }
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Reader R)
        {
            if (ModelState.IsValid)
            {
                dao.ReaderSignUp(R);
                CurrentUserInfo.Reader = R;
                return RedirectToAction("Index");
            }
            else return View();
        }

        public ActionResult SignOut()
        {
            CurrentUserInfo.Reader = new Reader();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserBooksTable()
        {
            if (CurrentUserInfo.Reader.Name != "Administrator" &&
                CurrentUserInfo.Reader.EMail != "Administrator" && CurrentUserInfo.Reader.Name != null)
            {
                CurrentUserInfo.Reader.Books = dao.GetReaderBooks(CurrentUserInfo.Reader);
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}