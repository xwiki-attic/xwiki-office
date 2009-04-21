using System;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains information about a version of a page.
    /// </summary>
    public struct PageHistorySummary
    {
        public int version;

        public int minorVersion;

        public DateTime modified;

        public string modifier;

        public String id;
    }
}