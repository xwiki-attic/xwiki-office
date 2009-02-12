using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XWiki
{
    [Serializable]
    public class Space
    {
        [XmlAttribute]
        public String name;
        [XmlAttribute]
        public bool hidden;
        public List<XWikiDocument> documents;
        public Space()
        {
            name = "";
            hidden = false;
            documents = new List<XWikiDocument>();
        }
    }
}
