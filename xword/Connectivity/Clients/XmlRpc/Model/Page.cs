using System;
using CookComputing.XmlRpc;

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

        /// <summary>
        /// The wiki syntax of the document.(Since XWiki 2.0.4)
        /// </summary> 
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public String syntaxId;

        /// <summary>
        /// Creates a new Page instance. Initialises the non-parametrized members with the default values.
        /// </summary>
        /// <param name="pageId">The id of the page.</param>
        public Page(String pageId)
        {
            this.id = pageId;            
            this.title = "";
            this.translations = new String[0];
            this.space = "";
            this.url = "";
            this.content = "";
            this.parentId = "";
            this.syntaxId = "";
        }

        /// <summary>
        /// Creates a new Page instance. Initialises the non-parametrized members with the default values.
        /// </summary>
        /// <param name="pageId">The id of the page.</param>
        /// <param name="content">The content of the page.</param>
        public Page(String pageId, String content)
        {
            this.id = pageId;
            this.content = content;
            this.title = "";
            this.translations = new String[0];
            this.space = "";
            this.url = "";
            this.parentId = "";
            this.syntaxId = "";
        }
    }
}