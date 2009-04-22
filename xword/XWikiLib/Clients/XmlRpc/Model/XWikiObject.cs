using System;
using CookComputing.XmlRpc;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains information about a XWiki object.
    /// </summary>
    public class XWikiObject : XWikiObjectSummary
    {
        /// <summary>
        /// A dictionary mapping the object's properties and values.
        /// </summary>
        [XmlRpcMember("propertyToValueMap")]
        public XmlRpcStruct objectDictionary;
    }
}
