using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.ServiceModel.Activation; 
using System.ServiceModel; 
using System.Net;
using System.ServiceModel.Web;

namespace Interface
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteTable.Routes.Add(new ServiceRoute("api", new WebServiceHostFactory(), typeof(Api)));
        }
    }
}
