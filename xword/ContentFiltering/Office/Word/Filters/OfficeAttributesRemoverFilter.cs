using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using XWiki.Office.Word;

namespace ContentFiltering.Office.Word.Filters
{
    public class OfficeAttributesRemoverFilter:IDOMFilter
    {
        private ConversionManager manager;

        public OfficeAttributesRemoverFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        #region IDOMFilter Members

        /// <summary>
        /// Deletes all office specific attributes.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        public void Filter(ref XmlDocument xmlDoc)
        {
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            XmlNamespaceManager nsMr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsMr.AddNamespace(String.Empty, "http://www.w3.org/1999/xhtml");
            nsMr.AddNamespace("v", "urn:schemas-microsoft-com:vml");
            nsMr.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
            nsMr.AddNamespace("w", "urn:schemas-microsoft-com:office:word");
            nsMr.AddNamespace("m", "http://schemas.microsoft.com/office/2004/12/omml");

            XPathExpression expression = navigator.Compile("//@v:* | //@o:* | //@w:* | //@m:*");
            XPathNodeIterator xIterator = navigator.Select(expression.Expression, nsMr);
            foreach (XPathNavigator nav in xIterator)
            {
                nav.DeleteSelf();
            }
        }

        #endregion
    }
}
