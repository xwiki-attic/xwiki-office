using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Office.Word;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;
using ContentFiltering.Html;

namespace ContentFiltering.Office.Word.Filters
{
    /// <summary>
    /// Extracts the CSS inline styles, optimizes CSS and adds CSS classes in the head section for current page.
    /// </summary>
    public class LocalToWebStyleFilter : IDOMFilter
    {
        private int counter = 0;
        private Hashtable cssClasses;
        private ConversionManager manager;

        public LocalToWebStyleFilter(ConversionManager manager)
        {
            this.manager = manager;
            this.cssClasses = new Hashtable();
            this.counter = 0;
        }

        #region IDOMFilter Members

        /// <summary>
        /// Extracts the inline styles, optimizes CSS and adds CSS classes in the head section for current page
        /// </summary>
        /// <param name="xmlDoc">A reference to a XmlDocument instance.</param>
        public void Filter(ref System.Xml.XmlDocument xmlDoc)
        {
            XmlNode body = xmlDoc.GetElementsByTagName("body")[0];
            XmlNode head = xmlDoc.GetElementsByTagName("head")[0];
            if (head == null)
            {
                head = xmlDoc.CreateNode(XmlNodeType.Element, "head", xmlDoc.NamespaceURI);
                body.ParentNode.InsertBefore(head, body);
            }

            //step1: inline CSS for existing CSS classes and ids, for better manipulation at step2 and step3
            CSSUtil.InlineCSS(ref xmlDoc);

            //step2: convert all inlined CSS to CSS classes 
            //(including, but not limited to, those generated at step1)
            body = CSSUtil.ConvertInlineStylesToCssClasses(body, ref xmlDoc, ref counter, ref cssClasses);

            //step3: optimize CSS by grouping selectors with the same properties
            cssClasses = CSSUtil.GroupCSSSelectors(cssClasses);

            InsertCssClassesInHeader(ref head, ref xmlDoc);
        }

        #endregion IDOMFilter Members


        /// <summary>
        /// Inserts the CSS from the cssClasses in the style node of the head section,
        /// creating the style node if neccessary.
        /// </summary>
        /// <param name="headNode">A reference to the head node of the document.</param>
        /// <param name="xmlDoc">A reference to the <code>XmlDocument</code>.</param>
        private void InsertCssClassesInHeader(ref XmlNode headNode, ref XmlDocument xmlDoc)
        {
            XmlNode styleNode = xmlDoc.CreateNode(XmlNodeType.Element, "style", xmlDoc.NamespaceURI);

            string value = "";

            foreach (Object key in cssClasses.Keys)
            {
                value += Environment.NewLine;
                value += (string)key;
                value += "{";
                value += cssClasses[key];
                value += "}";
            }

            XmlNode styleNodeContent = xmlDoc.CreateNode(XmlNodeType.Text, "#text", xmlDoc.NamespaceURI);
            styleNodeContent.Value = value;

            styleNode.AppendChild(styleNodeContent);
            headNode.AppendChild(styleNode);
        }
    }

}
