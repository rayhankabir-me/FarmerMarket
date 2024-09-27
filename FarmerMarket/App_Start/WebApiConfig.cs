using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Owin;
using System.Web.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Web.Http.Cors;


namespace FarmerMarket
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Enable CORS
            var cors = new EnableCorsAttribute("http://localhost:3000", "*", "*");
            config.EnableCors(cors);

            // Web API configuration and services

            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter("Bearer"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
