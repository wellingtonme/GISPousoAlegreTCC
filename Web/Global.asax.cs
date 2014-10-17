using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using SharpArch.Web.Mvc.Castle;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Optimization;
using Web.Controllers;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using CommonServiceLocator.WindsorAdapter;
using Web.Utils;
using ApplicationService.AutoMapper;

namespace Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            InitializeServiceLocator();
        }

        protected virtual void InitializeServiceLocator()
        {
            IWindsorContainer container = new WindsorContainer();
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
            container.RegisterControllers(typeof(HomeController).Assembly);
            ComponentRegistrar.AddComponentsTo(container);

            AutoMapperConfig.RegisterMappers();

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }
    }
}