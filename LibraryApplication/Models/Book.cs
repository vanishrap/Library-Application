using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryApplication.Models
{
    public class Book
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter book name")]
        public string Name { get; set; }

        public int Quantity = 0;
        public List<Author> Authors = new List<Author>();
        public List<Reader> Readers = new List<Reader>();
        public List<DateTime> DateBookTaken = new List<DateTime>();
    }

    public class BooksViewModel
    {
        public List<Book> Books = new List<Book>();
        public PagingInfo PagingInfo = new PagingInfo();
    }
}