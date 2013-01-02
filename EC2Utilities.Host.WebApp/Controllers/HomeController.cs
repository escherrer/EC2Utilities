using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EC2Utilities.Common.Manager;
using EC2Utilities.Host.WebApp.Models;
using StructureMap;

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
            var instanceManager = ObjectFactory.GetInstance<IInstanceManager>();
            var instances = instanceManager.GetInstances();

            IEnumerable<ServerListModel> serverList = instances.Select(x => new ServerListModel { ServerId = x.InstanceId, ServerName = x.InstanceName, ServerStatus = x.Status.ToString() });
                
            return View(serverList);
        }
    }
}
