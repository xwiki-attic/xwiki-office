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
    public class EmptySpanRemoverFilter : IDOMFilter
    {
        private ConversionManager manager;
        
        public EmptySpanRemoverFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Deletes the the extra span elments introduced by Word
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml dom.</param>
        public void Filter(ref System.Xml.XmlDocument xmlDoc)
        {
            List<XmlNode> extraNodes = new List<XmlNode>();
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("span");
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes.Count == 0)
                {
                    //copy the child elements to the parent
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        node.ParentNode.AppendChild(childNode.CloneNode(true));
                    }
                }
                extraNodes.Add(node);                                              
            }
            foreach (XmlNode node in extraNodes)
            {
                XmlNode parent = node.ParentNode;
                parent.RemoveChild(node);
            }
        }
    }
}