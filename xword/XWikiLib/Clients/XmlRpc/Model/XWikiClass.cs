using System;
using CookComputing.XmlRpc;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains information about a XWiki class.
    /// </summary>
    public struct XWikiClass
    {
        /// <summary>
        /// The id of the class
        /// </summary>
        public string id;

        /// <summary>
        /// The propertyToAttributesMap translated into a .net dictionary.
        /// The keys are the properties names, while the values are also dictionaries describing the class property.
        /// </summary>
        [XmlRpcMember("propertyToAttributesMap")]
        public XmlRpcStruct classDictionary;

        /// <summary>
        /// Gets the properties names for the XWiki class.
        /// </summary>
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public String[] Properties
        {
            get
            {
                String[] properties = new string[classDictionary.Count];
                int index = 0;
                foreach (String key in classDictionary.Keys)
                {
                    properties[index] = key;
                    index++;
                }
                return properties;
            }
        }
    }
}