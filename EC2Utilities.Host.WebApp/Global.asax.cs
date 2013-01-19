using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using EC2Utilities.Common.Factory;
using NServiceBus;
using StructureMap;
using log4net;

namespace EC2Utilities.Host.WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class Ec2UtilitiesWebApp : HttpApplication
    {
        public static IBus Bus { get; private set; }
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Ec2UtilitiesWebApp));

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            Logger.Info("Application_Start Start.");

            ContainerBootstrapper.BootstrapStructureMap();

            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            StartMessageBus();

            Logger.Info("Application_Start End.");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached && User == null)
            {
                FormsAuthentication.SetAuthCookie("debugger", false);
            }
        }

        private void StartMessageBus()
        {
            SetLoggingLibrary.Log4Net(log4net.Config.XmlConfigurator.Configure);
            
            Bus = Configure.With()
                .DefaultBuilder()
                .XmlSerializer()
                .MsmqTransport()
                .UnicastBus()
                .LoadMessageHandlers()
                .CreateBus()
                .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
        }

        protected void Application_Error()
        {
            try
            {
                var logger = ObjectFactory.GetInstance<ILog>();

                Exception exception = Server.GetLastError() ?? new Exception("Unable to get exception.");

                logger.Fatal("An unhandled exception occurred.", exception);
            }
            catch
            { }
            
            HttpContext ctx = HttpContext.Current;
            ctx.Response.Clear();
            RequestContext rc = ((MvcHandler)ctx.CurrentHandler).RequestContext;
            string controllerName = rc.RouteData.GetRequiredString("controller");
            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
            IController controller = factory.CreateController(rc, controllerName);
            var cc = new ControllerContext(rc, (ControllerBase)controller);

            var viewResult = new ViewResult { ViewName = "Error" };
            viewResult.ExecuteResult(cc);
            ctx.Server.ClearError();
        } 
    }
}

