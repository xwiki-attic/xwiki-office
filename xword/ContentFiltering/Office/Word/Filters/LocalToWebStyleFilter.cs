#region LGPL license
/*
 * See the NOTICE file distributed with this work for additional
 * information regarding copyright ownership.
 *
 * This is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this software; if not, write to the Free
 * Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
 * 02110-1301 USA, or see the FSF site: http://www.fsf.org.
 */
#endregion //license

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

            //step2: convert all inlined style to CSS classes 
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

            XmlNode styleNode = null;
            if (xmlDoc.GetElementsByTagName("style") != null)
            {
                styleNode = xmlDoc.GetElementsByTagName("style")[0];
            }
            if (styleNode == null)
            {
                styleNode = xmlDoc.CreateNode(XmlNodeType.Element, "style", xmlDoc.NamespaceURI);
                headNode.AppendChild(styleNode);
            }

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
