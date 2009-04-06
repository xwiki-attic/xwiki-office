using System;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains the attributes of a page.
    /// </summary>
    public struct Page
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
        /// The 'view' url of the page.
        /// </summary>
        public String url;

        /// <summary>
        /// Array containing all available translations for the current page.
        /// The array does not contain the value of the default language of the page.
        /// </summary>
        public String[] translations;

        /// <summary>
        /// The name of the space.
        /// </summary>
        public String space;

        /// <summary>
        /// The id of the page.
        /// </summary>
        public String id;

        /// <summary>
        /// The (non-rendered)content of the page.
        /// </summary>
        public String content;
    }
}