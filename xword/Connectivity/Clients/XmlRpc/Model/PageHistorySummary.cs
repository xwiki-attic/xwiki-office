using System;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains information about a version of a page.
    /// </summary>
    public struct PageHistorySummary
    {
        /// <summary>
        /// The version of the page.
        /// </summary>
        public int version;

        /// <summary>
        /// The minor version of the page.
        /// </summary>
        public int minorVersion;

        /// <summary>
        /// The date and time of the modification.
        /// </summary>
        public DateTime modified;

        /// <summary>
        /// The username that made the modification.
        /// </summary>
        public string modifier;

        /// <summary>
        /// The id of the page version.
        /// </summary>
        public String id;
    }
}