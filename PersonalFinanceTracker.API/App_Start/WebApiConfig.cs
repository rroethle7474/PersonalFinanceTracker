using PersonalFinanceTracker.API.Infrastructure;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using PersonalFinanceTracker.API.Areas.HelpPage;

namespace PersonalFinanceTracker.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Configure global exception handling
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Services.Replace(typeof(IExceptionLogger), new GlobalExceptionLogger());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Enable Help Page
            config.SetDocumentationProvider(new XmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/App_Data/XmlDocument.xml")));
            config.SetHelpPageSampleGenerator(new HelpPageSampleGenerator());
        }
    }
}
