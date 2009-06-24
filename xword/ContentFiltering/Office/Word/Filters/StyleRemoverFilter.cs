using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using XWiki.Office.Word;

namespace ContentFiltering.Office.Word.Filters
{
    public class StyleRemoverFilter:IDOMFilter
    {
        private ConversionManager manager;

        public StyleRemoverFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        #region IDOMFilter Members
        /// <summary>
        /// Deletes the style attributes from the Word generated content.
        /// </summary>
        /// <param name="xmlDoc">A refrence to the xml document.</param>
        public void Filter(ref XmlDocument xmlDoc)
        {
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            XPathExpression expression = navigator.Compile("//@style");
            XPathNodeIterator xIterator = navigator.Select(expression);
            foreach (XPathNavigator nav in xIterator)
            {
                nav.DeleteSelf();
            }
            expression = navigator.Compile("//@class");
            xIterator = navigator.Select(expression);
            foreach (XPathNavigator nav in xIterator)
            {
                if (nav.Value == "MsoNormal" || nav.Value == "MsoNormalTable" || nav.Value == "MsoTableGrid"||nav.Value=="MsoNoSpacing")
                {
                    nav.DeleteSelf();
                }
            }
            expression = navigator.Compile("//td[@valign]");
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("td");
            XmlAttribute colspanAttribute, rowspanAttribute;
            foreach (XmlNode node in nodes)
            {
                //XmlAttribute valign = node.Attributes["valign"];
                //node.Attributes.Remove(valign);

                //get colspan and rowspan values
                colspanAttribute = node.Attributes["colspan"];
                rowspanAttribute = node.Attributes["rowspan"];
                //remove all valid and invalid attributes
                node.Attributes.RemoveAll();
                //put back the colspan and rowspan attributes
                if (colspanAttribute != null)
                {
                    node.Attributes.Append(colspanAttribute);
                }
                if (rowspanAttribute != null)
                {
                    node.Attributes.Append(rowspanAttribute);
                }
            }
        }

        #endregion
    }
}
