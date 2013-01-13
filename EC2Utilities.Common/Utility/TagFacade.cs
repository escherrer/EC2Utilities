using System.Collections.Generic;
using System.Linq;
using Amazon.EC2.Model;

namespace EC2Utilities.Common.Utility
{
    public static class TagCollectionExtensions
    {
        public static string GetTagValueByKey(this List<Tag> tagContainer, string key)
        {
            Tag matchingTag = tagContainer.SingleOrDefault(x => x.Key == key);

            if (null != matchingTag)
            {
                return matchingTag.Value;
            }
            else
            {
                return null;
            }
        }
    }
}
