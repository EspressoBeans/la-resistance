using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ElectronicsCalcApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            //NOTE: (Vic Guadalupe) - Because there is an MVC project in the same solution calling this api, NEED TO INCLUDE RouteConfig HERE.  It is NOT in default template for WebApi.
            //                        Secondarily, the RouteConfig statement needs to be AFTER the GlobalConfiguration statement so that the API's config is run first.

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);      
        }
    }
}
