using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using EC2Utilities.Common.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EC2Utilities.Test
{
    [TestClass]
    public class ServerStartEmailConfigSectionTester
    {
        [TestMethod]
        public void Loads_Section()
        {
            var section = (ServerStartEmailConfigSection)ConfigurationManager.GetSection("ServerStartEmail");

            Assert.IsNotNull(section, "Failed to load config section");

            IEnumerable<string> result = section.InstanceItems.Cast<EmailAddressElement>().Select(x => x.EmailAddress);
        
            Assert.IsNotNull(result, "Could not get email address elements. ");
            Assert.AreEqual(1, result.Count(), "Did not load correct amount of email addresses.");
            Assert.AreEqual("test@test.com", result.Single(), "Did not load configured email address.");
        }
    }
}
