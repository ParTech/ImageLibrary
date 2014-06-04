using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Facilities.FactorySupport;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ParTech.ImageLibrary.Core.Installers;
using ParTech.ImageLibrary.Website.Plumbing;
using WebMatrix.WebData;

namespace ParTech.ImageLibrary.Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            WebSecurity.InitializeDatabaseConnection("imagedatabaseSecurity", "UserProfile", "Id", "UserName", autoCreateTables: true);

            InitializeDependencyInjection();
        }
        
        protected void Application_End()
        {
            Container.Dispose();
        }

        public static IWindsorContainer Container;

        private static void InitializeDependencyInjection()
        {
            Container = new WindsorContainer();
            Container.AddFacility<FactorySupportFacility>();
            Container.Kernel.Resolver.AddSubResolver(new ArrayResolver(Container.Kernel));
            
            Container.Install(
                new CoreInstaller(),
                FromAssembly.This()
            );

            var controllerFactory = new WindsorControllerFactory(Container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}