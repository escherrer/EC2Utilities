using System.Configuration;

namespace EC2Utilities.Common.Config
{
    [ConfigurationCollection(typeof(EmailAddressElement))]
    public class EmailAddressCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EmailAddressElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EmailAddressElement)(element)).EmailAddress;
        }

        public EmailAddressElement this[int idx]
        {
            get
            {
                return (EmailAddressElement)BaseGet(idx);
            }
        }
    }
}