using LibraryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LibraryApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
    public static class CurrentUserInfo
    {
        public static Reader Reader = new Reader();// { Name = "Vlad" , EMail = ""};
    }

    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(
        this HtmlHelper html,
        PagingInfo pagingInfo)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag2 = new TagBuilder("button");
                tag2.MergeAttribute("type", "submit");
                tag2.MergeAttribute("name", "button");
                tag2.MergeAttribute("value", i.ToString());
                tag2.InnerHtml = i.ToString();
                tag2.AddCssClass("btn-success"); 
                result.Append(tag2.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}
