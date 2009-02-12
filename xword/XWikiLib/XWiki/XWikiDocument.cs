using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XWiki
{
    [Serializable]
    public class XWikiDocument
    {
        [XmlAttribute]
        public String name;
        [XmlAttribute]
        public String space;
        [NonSerialized]
        protected StringBuilder content;
        public XWikiDocument()
        {
            name = "";
            space = "";
            content = new StringBuilder();
        }
    }
}
