using System.Web;
using System.Web.Mvc;

namespace StockApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            filters.Add(new HandleErrorAttribute());
        }
    }
}