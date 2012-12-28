using System.Configuration;

namespace EC2Utilities.Common.Config
{
    [ConfigurationCollection(typeof(InstanceElement))]
    public class InstancesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new InstanceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((InstanceElement)(element)).Instance;
        }

        public InstanceElement this[int idx]
        {
            get
            {
                return (InstanceElement)BaseGet(idx);
            }
        }
    }
}