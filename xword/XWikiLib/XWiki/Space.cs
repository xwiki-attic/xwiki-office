using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XWiki
{
    /// <summary>
    /// Describes a space in a XWiki server.
    /// </summary>
    [Serializable]
    public class Space
    {
        /// <summary>
        /// The name of the space.
        /// </summary>
        [XmlAttribute]
        public String name;

        /// <summary>
        /// Specifies if the space should be visible to the user.
        /// </summary>
        [XmlAttribute]
        public bool hidden;
        
        /// <summary>
        /// The list of documents in the space.
        /// </summary>
        public List<XWikiDocument> documents;
        
        /// <summary>
        /// Default constructor. Creates a new instance of the Space class.
        /// </summary>
        public Space()
        {
            name = "";
            hidden = false;
            documents = new List<XWikiDocument>();
        }
    }
}
