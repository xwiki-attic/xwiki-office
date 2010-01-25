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
using ContentFiltering.Html;
using ContentFiltering.StyleSheetExtensions;

namespace ContentFiltering.Office.Word.Filters
{
    /// <summary>
    /// Gets the StyleSheetExtensions objects for a page and inserts the CSS in the XmlDocument.
    /// </summary>
    public class WebToLocalStyleFilter:IDOMFilter
    {
        private ConversionManager manager;

        public WebToLocalStyleFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        #region IDOMFilter Members

        public void Filter(ref XmlDocument xmlDoc)
        {
            XmlNode styleNode = GetStyleNode(ref xmlDoc);
            string cssContent = ExtractCSSFromStyleSheetExtensions();
            styleNode.InnerText = cssContent;

            CSSUtil.InlineCSS(ref xmlDoc);
            
        }

        #endregion IDOMFilter Members


        /// <summary>
        /// Gets or creates a style node in the XmlDocument.
        /// </summary>
        /// <param name="xmlDoc">A reference to the <code>XmlDocument</code>.</param>
        /// <returns>Existing or created style node.</returns>
        private XmlNode GetStyleNode(ref XmlDocument xmlDoc)
        {
            XmlNode bodyNode = xmlDoc.GetElementsByTagName("body")[0];
            if (bodyNode == null)
            {
                bodyNode = xmlDoc.CreateElement("body");
                xmlDoc.InsertBefore(bodyNode, xmlDoc.ChildNodes[0]);
            }
            XmlNode headNode = xmlDoc.GetElementsByTagName("head")[0];
            if (headNode == null)
            {
                headNode = xmlDoc.CreateElement("head");
                bodyNode.ParentNode.InsertBefore(headNode, bodyNode);
            }
            XmlNode styleNode = xmlDoc.GetElementsByTagName("style")[0];
            if (styleNode == null)
            {
                styleNode = xmlDoc.CreateElement("style");
                headNode.AppendChild(styleNode);
            }

            return styleNode;
        }


        /// <summary>
        /// Extracts the CSS from StyleSheetExtension objects.
        /// </summary>
        /// <returns></returns>
        private string ExtractCSSFromStyleSheetExtensions()
        {
            SSXManager ssxManager = SSXManager.BuildFromServerPage(manager);
            return ssxManager.PageCSSContent;
        }

    }
}
