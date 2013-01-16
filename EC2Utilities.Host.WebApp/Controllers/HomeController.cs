using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EC2Utilities.Common.Contract;
using EC2Utilities.Common.Contract.Messages;
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
            var instanceManager = ObjectFactory.GetInstance<IInstanceManager>();
            var instances = instanceManager.GetInstances();

            List<ServerListModel> serverList = instances.Select(x => new ServerListModel(x)).ToList();

            foreach (ServerListModel serverListModel in serverList)
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
