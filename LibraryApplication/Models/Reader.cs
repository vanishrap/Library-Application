using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryApplication.Models
{
    public class Reader
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [RegularExpression(".+\\@.+\\..+",
            ErrorMessage = "Please enter a valid email address")]
        public string EMail { get; set; }
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }
        public List<Book> Books = new List<Book>();
        public List<DateTime> DateBookTaken = new List<DateTime>();
    }

}