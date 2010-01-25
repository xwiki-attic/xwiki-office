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
