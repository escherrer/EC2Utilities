using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EC2Utilities.Common.Contract;
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

            IEnumerable<ServerListModel> serverList = instances.Select(x => new ServerListModel(x));
                
            return View(serverList);
        }

        public ActionResult StartServer(string instanceId)
        {
            var instanceManager = ObjectFactory.GetInstance<IInstanceManager>();

            Ec2UtilityInstance ec2UtilityInstance = instanceManager.GetInstance(instanceId);

            var startServerModel = new StartServerModel(ec2UtilityInstance);

            return View(startServerModel);
        }

        [HttpPost]
        public ActionResult StartServer(StartServerModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var instanceManager = ObjectFactory.GetInstance<IInstanceManager>();

                instanceManager.StartUpInstance(model.ServerId);

                return RedirectToAction("ServerStartUp");
            }

            // If we got this far, something failed, redisplay form
            return StartServer(model.ServerId);
        }
    }
}
