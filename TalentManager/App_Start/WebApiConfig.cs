using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TalentManager.App_Start;
using TalentManager.Handlers;

namespace TalentManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.MessageHandlers.Add(new TalentManagerHandler());
            //config.MessageHandlers.Add(new CorsPreflightHandler());

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();

            //With this will never send errors to the client
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never;

            //We add our custom exception filter to get thos .net exceptions a convert them in to http exceptions
            config.Filters.Add(new ExceptionFilter());
        }
    }
}
