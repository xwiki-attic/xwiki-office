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
using ContentFiltering.Office.Word.Filters;
using XWiki.Office.Word;

namespace ContentFiltering.Office.Word.Filters
{
    public class ParentDivAttributeRemoverFilter : IDOMFilter
    {
        private ConversionManager manager;
        private const String DEFAULT_ATTRIBUTE_VALUE = "Section1";

        public ParentDivAttributeRemoverFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Deletes the Word introduced attribute from the parent div
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml dom.</param>
        public void Filter(ref System.Xml.XmlDocument xmlDoc)
        {
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("div");
            foreach (XmlNode node in nodes)
            {
                if (node.ParentNode.Name == "body")
                {
                    XmlAttribute attribute = node.Attributes["class"];
                    if (attribute != null && attribute.Value == DEFAULT_ATTRIBUTE_VALUE)
                    {
                        node.Attributes.Remove(attribute);
                        break;
                    }
                }
            }            
        }
    }
}
