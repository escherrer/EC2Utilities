using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC2Utilities.Common.Contract;
using EC2Utilities.Host.WebApp.Models;

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
            var serverList = new List<ServerListModel>();

            serverList.Add(new ServerListModel
            {
                ServerId = "1",
                ServerName = "1",
                ServerStatus = "Stopped"
            });

            serverList.Add(new ServerListModel
            {
                ServerId = "2",
                ServerName = "2",
                ServerStatus = "Stopped"
            });

            serverList.Add(new ServerListModel
            {
                ServerId = "3",
                ServerName = "3",
                ServerStatus = "Running"
            });

            return View(serverList);
        }
    }
}
