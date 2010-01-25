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
using XWiki.Office.Word;

namespace ContentFiltering.Office.Word.Filters
{
    public class LocalListsAdaptorFilter:IDOMFilter
    {
        private ConversionManager manager;

        public LocalListsAdaptorFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        #region IDOMFilter Members
        /// <summary>
        /// Adapts to the lists to a less styled format.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document instance.</param>
        public void Filter(ref XmlDocument xmlDoc)
        {
            XmlNodeList listItems = xmlDoc.GetElementsByTagName("li");
            //Remove the extra paragraph from list items.
            foreach (XmlNode node in listItems)
            {
                if (node.NodeType == XmlNodeType.Element && node.FirstChild.NodeType == XmlNodeType.Element)
                {
                    if (node.FirstChild.Name == "p")
                    {
                        node.InnerXml = node.FirstChild.InnerXml;
                    }
                }
            }
            bool foundExtraLists = false;
            do
            {
                foundExtraLists = RemoveExtraLists(ref xmlDoc);
            } while (foundExtraLists);
            //Remove attributes from list declarations.
            XmlNodeList lists = xmlDoc.GetElementsByTagName("ul");
            foreach (XmlNode node in lists)
            {
                node.Attributes.RemoveAll();
            }
            lists = xmlDoc.GetElementsByTagName("ol");
            foreach (XmlNode node in lists)
            {
                node.Attributes.RemoveAll();
            }
            RemoveDivFromLists(ref xmlDoc, "ul");
            RemoveDivFromLists(ref xmlDoc, "ol");
            MoveChildListToTheirParent(ref xmlDoc);
        }

        #endregion


        /// <summary>
        /// Removes the extra lists Word creates for sublists.
        /// The child 'ul' is moved to the previous sibling.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        private bool RemoveExtraLists(ref XmlDocument xmlDoc)
        {
            bool foundExtraLists = false;
            XmlNodeList listItems = xmlDoc.GetElementsByTagName("li");
            foreach (XmlNode node in listItems)
            {
                //A 'li' with no innerText but with 'ul' or 'ol' children should be moved up
                if (node.NodeType == XmlNodeType.Element && ("" + node.Value).Length < 1)
                {
                    if (node.ChildNodes[0].Name == "ul" || node.ChildNodes[0].Name == "ol")
                    {
                        XmlNode prevListItem = node.PreviousSibling;
                        //XmlNode subList = node.RemoveChild(node.FirstChild);
                        XmlNodeList children = node.ChildNodes;
                        foreach (XmlNode child in children)
                        {
                            prevListItem.AppendChild(child);
                        }
                        //prevListItem.AppendChild(subList);
                        foundExtraLists = true;
                    }
                }
            }
            return foundExtraLists;
        }

        /// <summary>
        /// Remove 'div' elements used for alignment and move their children in the correct position.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        /// <param name="listName">List type/name (like "ol", "ul").</param>
        private void RemoveDivFromLists(ref XmlDocument xmlDoc, string listName)
        {
            XmlNodeList lists = xmlDoc.GetElementsByTagName(listName);
            Dictionary<XmlNode, List<XmlNode>> childrenToMoveUp = new Dictionary<XmlNode, List<XmlNode>>();
            foreach (XmlNode node in lists)
            {
                if (node.Name.ToLower().Trim() == "div")
                {
                    XmlNode prevListItem = node.PreviousSibling;
                    XmlNode parent = node.ParentNode;
                    List<XmlNode> value = new List<XmlNode>();
                    if (prevListItem != null)
                    {
                        if (childrenToMoveUp.ContainsKey(prevListItem))
                        {
                            value = childrenToMoveUp[prevListItem];
                        }
                        value.Add(node);
                        childrenToMoveUp.Add(prevListItem, value);
                    }
                }
            }

            foreach (XmlNode key in childrenToMoveUp.Keys)
            {
                XmlNode prevListItem = (XmlNode)key;
                XmlNode parent = prevListItem.ParentNode;
                List<XmlNode> nodes = childrenToMoveUp[key];
                foreach (XmlNode node in nodes)
                {
                    //take all the children from this div and put them in the right position
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        parent.InsertAfter(child, prevListItem);
                    }
                    //remove the node
                    parent.RemoveChild(node);
                }
            }

        }

        /// <summary>
        /// Move orphan 'ul' and 'ol' elements to their coresponding 'li' parent.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        private void MoveChildListToTheirParent(ref XmlDocument xmlDoc)
        {
            MoveElementsUp(ref xmlDoc, "ul");
            MoveElementsUp(ref xmlDoc, "ol");
        }

        /// <summary>
        /// Move an element from an inner list to it's correct position.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        /// <param name="element">Element type/name (like "ol", "ul").</param>
        private void MoveElementsUp(ref XmlDocument xmlDoc, string element)
        {
            Dictionary<XmlNode, List<XmlNode>> childrenToMoveUp = new Dictionary<XmlNode, List<XmlNode>>();

            XmlNodeList items = xmlDoc.GetElementsByTagName(element.ToLower().Trim());
            foreach (XmlNode node in items)
            {
                XmlNode prevListItem = node.PreviousSibling;
                XmlNode parent = node.ParentNode;
                List<XmlNode> value = new List<XmlNode>();
                if (prevListItem != null)
                {
                    if (prevListItem.Name.ToLower().Trim() == "li")
                    {
                        if (childrenToMoveUp.ContainsKey(prevListItem))
                        {
                            value = childrenToMoveUp[prevListItem];
                        }
                        value.Add(node);
                        childrenToMoveUp.Add(prevListItem, value);
                    }
                }
            }
            foreach (XmlNode key in childrenToMoveUp.Keys)
            {
                XmlNode liParent = (XmlNode)key;
                XmlNode parent = liParent.ParentNode;
                List<XmlNode> nodes = childrenToMoveUp[key];
                foreach (XmlNode node in nodes)
                {
                    parent.RemoveChild(node);
                    liParent.AppendChild(node);
                }
            }
        }

    }
}
