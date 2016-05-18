using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Postal;
namespace LibraryApplication.Models
{
    public class NotificationEmail : Email
    {
            public string To { get; set; }
            public string UserName { get; set; }
            public string MessageText { get; set; }
    }
}