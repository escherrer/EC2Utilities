using System.Collections.Generic;

namespace EC2Utilities.Host.WebApp.Models
{
    public class ServerListModelContainer
    {
        public ServerListModelContainer()
        {
            ServerListModels = new List<ServerListModel>();
        }
        
        public List<ServerListModel> ServerListModels { get; set; }
    }
}