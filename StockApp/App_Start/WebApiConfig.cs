using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;
using Jil;
using System.Net.Http.Formatting;
namespace StockApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.
            
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            // Adding formatter for Json   
         //   config.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("type", "json", new MediaTypeHeaderValue("application/json")));
         //   config.Formatters.Clear(); 
           // var _jilOptions = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: false, includeInherited: true); config.Formatters.Add(new JilFormatter(_jilOptions));
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
