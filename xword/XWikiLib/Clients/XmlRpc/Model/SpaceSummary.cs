using System;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains summary data regarding a space.
    /// </summary>
    public struct SpaceSummary
    {
        /// <summary>
        /// The name of the space.
        /// </summary>
        public String name;

        /// <summary>
        /// The key for the space.
        /// </summary>
        public String key;

        /// <summary>
        /// The 'view' url for the space(by default: WebHome page).
        /// </summary>
        public String url;
    }
}