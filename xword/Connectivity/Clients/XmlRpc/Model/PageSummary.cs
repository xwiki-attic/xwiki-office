using System;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains summary data about a page.
    /// </summary>
    public struct PageSummary
    {
        /// <summary>
        /// The id of the parent page.
        /// </summary>
        public String parentId;

        /// <summary>
        /// The title of the page.
        /// </summary>
        public String title;

        /// <summary>
        /// The url of the page.
        /// </summary>
        public String url;

        /// <summary>
        /// The available translations for the current page.
        /// The array does not contain the default language of the page.
        /// </summary>
        public String[] translations;

        /// <summary>
        /// The name of the page.
        /// </summary>
        public String space;

        /// <summary>
        /// The id of the page.
        /// </summary>
        public String id;
    }
}