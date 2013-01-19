using System;
using System.Web.Mvc;
using StructureMap;
using log4net;

namespace EC2Utilities.Host.WebApp.Controllers
{
    public class SharedController : Controller
    {
        public ActionResult Error()
        {
            try
            {
                var logger = ObjectFactory.GetInstance<ILog>();
                Exception exception = Server.GetLastError();

                logger.Fatal("An unhandled exception occurred.", exception);
            }
            catch
            { }

            return View();
        }
    }
}
