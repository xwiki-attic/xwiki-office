using System;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains summary information about a XWiki object.
    /// </summary>
    public class XWikiObjectSummary
    {
        /// <summary>
        /// The id of the page contining the object.
        /// </summary>
        public string pageId;

        /// <summary>
        /// The id of the object.
        /// </summary>
        public int id;

        /// <summary>
        /// The version of the page.
        /// </summary>
        public int pageVersion;

        /// <summary>
        /// The name of the class.
        /// </summary>
        public string className;

        /// <summary>
        /// A guid identifing the XWiki object.
        /// </summary>
        public string guid;

        /// <summary>
        /// The minor version of the page.
        /// </summary>
        public int pageMinorVersion;

        /// <summary>
        /// The pretty name of the object, containing the indexed access name. Eg: XWiki.JavaScriptExtension[0]
        /// </summary>
        public string prettyName;
    }
}
