using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LibraryApplication.Models
{
    public class PagingInfo
    {
        public int TotalItems = 1;
        public int ItemsPerPage = 1;
        public int CurrentPage = 1;
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }

        public bool[] Filtering = new bool[] { true, true };

    }
}
