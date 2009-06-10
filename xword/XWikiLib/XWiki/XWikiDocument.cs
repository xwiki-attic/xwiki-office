using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XWiki
{
    /// <summary>
    /// Describes a document on the XWiki server.
    /// </summary>
    [Serializable]
    public class XWikiDocument
    {
        /// <summary>
        /// The name of the document.
        /// </summary>
        [XmlAttribute]
        public String name;
        
        /// <summary>
        /// The space name of the document.
        /// </summary>
        [XmlAttribute]
        public String space;

        /// <summary>
        /// Specifies if the document is published on wiki or if it's a local one.
        /// Default is TRUE, since the majority of pages are from the server.
        /// FALSE only for new added (and not published) pages.
        /// </summary>
        public bool published=true;

        /// <summary>
        /// The rendered content of the document.
        /// </summary>
        [NonSerialized]
        protected StringBuilder content;

        /// <summary>
        /// Default constructor, creates a new instance of the XWikiDocument class.
        /// </summary>
        public XWikiDocument()
        {
            name = "";
            space = "";
            content = new StringBuilder();
        }
    }
}
