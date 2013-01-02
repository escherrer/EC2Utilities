using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EC2Utilities.Host.WebApp.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "EC2 Utilities";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ServerStartUp()
        {
            return View();
        }
    }
}
