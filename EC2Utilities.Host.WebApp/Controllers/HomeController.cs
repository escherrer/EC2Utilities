using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EC2Utilities.Common.Contract;
using EC2Utilities.Common.Contract.Messages;
using EC2Utilities.Common.Exceptions;
using EC2Utilities.Common.Manager;
using EC2Utilities.Host.WebApp.Models;
using NServiceBus;
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
            var serverListContainer = new ServerListModelContainer();
            var instanceManager = ObjectFactory.GetInstance<IInstanceManager>();
            var instances = new List<Ec2UtilityInstance>();

            try
            {
                instances.AddRange(instanceManager.GetInstances());
            }
            catch (ResourceAccessException)
            {
                ModelState.AddModelError("", "An error has occurred while retrieving the list of servers. See server log file for details.");   
            }

            foreach (var ec2UtilityInstance in instances)
            {
                serverListContainer.ServerListModels.Add(new ServerListModel(ec2UtilityInstance));
            }

            foreach (ServerListModel serverListModel in serverListContainer.ServerListModels)
            {
                ServerStartUpStatus startUpStatus = InstanceData.GetServerStartUpStatus(serverListModel.ServerId);

                switch (startUpStatus)
                {
                    case ServerStartUpStatus.Initialized:
                    case ServerStartUpStatus.Starting:
                    case ServerStartUpStatus.Started:
                    case ServerStartUpStatus.IpAssigned:
                        {
                            serverListModel.ServerStatus = "Starting...";
                            break;
                        }
                }
            }

            return View(serverListContainer);
        }

        public ActionResult StartServer(string instanceId)
        {
            var instanceManager = ObjectFactory.GetInstance<IInstanceManager>();
            Ec2UtilityInstance ec2UtilityInstance;

            try
            {
                ec2UtilityInstance = instanceManager.GetInstance(instanceId);
            }
            catch (ResourceAccessException)
            {
                var errMsg = string.Format("An error has occurred while retrieving the detals for instance id {0}. See server log file for details.", instanceId);
                ModelState.AddModelError("", errMsg);
                ec2UtilityInstance = new Ec2UtilityInstance {Status = Ec2UtilityInstanceStatus.Indeterminate};
            }

            var startServerModel = new StartServerModel(ec2UtilityInstance);

            return View(startServerModel);
        }

        [HttpPost]
        public ActionResult StartServer(StartServerModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var command = new StartServerCommand
                {
                    InstanceId = model.ServerId,
                    NotificationEmailAddress = model.EmailAddress
                };

                Ec2UtilitiesWebApp.Bus.Send(command);

                InstanceData.SetStatus(model.ServerId, ServerStartUpStatus.Initialized);

                return RedirectToAction("ServerStartUp");
            }

            // If we got this far, something failed, redisplay form
            return StartServer(model.ServerId);
        }
    }
}
