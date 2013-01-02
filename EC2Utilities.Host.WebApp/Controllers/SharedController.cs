using System;
using System.Web.Mvc;
using NLog;
using StructureMap;

namespace EC2Utilities.Host.WebApp.Controllers
{
    public class SharedController : Controller
    {
        public ActionResult Error()
        {
            try
            {
                var logger = ObjectFactory.GetInstance<Logger>();
                Exception exception = Server.GetLastError();

                logger.FatalException("An unhandled exception occurred.", exception);
            }
            catch
            { }

            return View();
        }
    }
}
