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

namespace ContentFiltering.Office.Word.Filters
{
    /// <summary>
    /// Removes all empty nested elements from the output html
    /// </summary>
    public class EmptyNestedElementsFilter : IDOMFilter
    {
        private ConversionManager manager;

        public EmptyNestedElementsFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Cleans up the <code>XmlDocument</code> by removing nested and empty elements.
        /// </summary>
        /// <param name="xmlDoc">A reference to the XmlDocument</param>
        public void Filter(ref XmlDocument xmlDoc)
        {
            XmlNode body = xmlDoc.GetElementsByTagName("body")[0];
            XmlNodeList bodyChildNodes = body.ChildNodes;

            Dictionary<string, List<string>> nestedNodesToProtect = new Dictionary<string, List<string>>();
            nestedNodesToProtect.Add("b", new List<string>() { "u","i","span"});
            nestedNodesToProtect.Add("u", new List<string>() { "b", "i","span"});
            nestedNodesToProtect.Add("i", new List<string>() { "b", "u","span"});
            nestedNodesToProtect.Add("span", new List<string>() { "b", "u", "i"});
            nestedNodesToProtect.Add("p", new List<string>() {"span","b","u","i" });
            nestedNodesToProtect.Add("h1", new List<string>() { "span", "p", "div", "b", "u", "i" });
            nestedNodesToProtect.Add("h2", new List<string>() { "span", "p", "div", "b", "u", "i" });
            nestedNodesToProtect.Add("h3", new List<string>() { "span", "p", "div", "b", "u", "i" });
            nestedNodesToProtect.Add("ol", new List<string>() { "li","ol","ul" });
            nestedNodesToProtect.Add("ul", new List<string>() { "li", "ol", "ul" });
            nestedNodesToProtect.Add("table",new List<string>(){"tr","td","thead","tbody"});
            nestedNodesToProtect.Add("tr",new List<string>(){"td"});
            nestedNodesToProtect.Add("td", new List<string>() { "p", "span", "div", "b", "u", "i", "ol", "ul" });

            foreach (XmlNode node in bodyChildNodes)
            {
                RemoveNestedElements(ref xmlDoc, node, nestedNodesToProtect);
            }
            List<string> emptyNodesToProtect = new List<string>() { "br", "img", "hr" };            
            RemoveEmptyElements(ref xmlDoc, emptyNodesToProtect);
        }

        /// <summary>
        /// Removes nested empty elements from the specified node and it's children, if node and children
        /// are not in the rules from <code>nodesToProtect</code> <code>Dictionary</code>.
        /// </summary>
        /// <param name="xmlDoc">A reference to an XmlDocument</param>
        /// <param name="nodeToProcess">Node to process.</param>
        /// <param name="nodesToProtect">Nodes not to alter.</param>
        private void RemoveNestedElements(ref XmlDocument xmlDoc,XmlNode nodeToProcess,Dictionary<string,List<string>> nodesToProtect)
        {
            if (nodeToProcess.NodeType == XmlNodeType.Element
                && nodeToProcess.ChildNodes.Count > 0)
            {

                XmlNodeList childNodes = nodeToProcess.ChildNodes;
                foreach (XmlNode child in childNodes)
                {
                    RemoveNestedElements(ref xmlDoc, child,nodesToProtect);
                }

                if (nodeToProcess.FirstChild.NodeType == XmlNodeType.Element
                    && nodeToProcess.Value == null)
                {
                    XmlNode parentNode = nodeToProcess.ParentNode;
                    XmlNode afterNode = nodeToProcess;
                    List<XmlNode> nodesToMove = new List<XmlNode>();
                    bool readyForRemoval=true;
                    foreach (string protectedParent in nodesToProtect.Keys)
                    {
                        if (nodeToProcess.Name == protectedParent.Trim().ToLower() && readyForRemoval)
                        {
                            foreach (XmlNode child in childNodes)
                            {
                                foreach (string protectedChild in nodesToProtect[protectedParent])
                                {
                                    if (protectedChild.Trim().ToLower() == child.Name)
                                    {
                                        readyForRemoval = false;
                                        break;
                                    }
                                }
                                if (!readyForRemoval)
                                {
                                    break;
                                }
                            }
                        }
                        if (!readyForRemoval)
                        {
                            break;
                        }
                    }

                    if (readyForRemoval)
                    {
                        foreach (XmlNode child in childNodes)
                        {
                            nodesToMove.Add(child);
                        }
                        foreach (XmlNode nodeToMove in nodesToMove)
                        {
                            afterNode = parentNode.InsertAfter(nodeToProcess.RemoveChild(nodeToMove), afterNode);
                        }

                        parentNode.RemoveChild(nodeToProcess);
                    }
                }
            }
        }

        /// <summary>
        /// Removes empty elements if not in <code>nodesToProtect</code> list.
        /// An element is considered to be empty if it has no children and no innerText.
        /// Empty paragraphs are transformed to breaks.
        /// </summary>
        /// <param name="xmlDoc">A reference to the XmlDocument</param>
        /// <param name="nodesToProtect">A list of strings containing nodes to protect from erasing(like br, img, hr).</param>
        private void RemoveEmptyElements(ref XmlDocument xmlDoc, List<string> nodesToProtect)
        {
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("*");
            List<XmlNode> nodesToDelete = new List<XmlNode>();
            List<XmlNode> paragraphsToAlter = new List<XmlNode>();
            foreach (XmlNode node in nodes)
            {
                if (node.NodeType == XmlNodeType.Element
                    && node.Value==null
                    && node.ChildNodes.Count==0)
                {
                    bool preventDelete=false;
                    foreach (string nodeName in nodesToProtect)
                    {
                        if (nodeName.ToLower().Trim() == node.Name)
                        {
                            preventDelete = true;
                            break;
                        }
                    }

                    if (node.Name == "p")
                    {
                        preventDelete = true;
                        paragraphsToAlter.Add(node);
                    }
                    if (!preventDelete)
                    {
                        nodesToDelete.Add(node);
                    }
                }
            }
            foreach (XmlNode node in nodesToDelete)
            {
                node.ParentNode.RemoveChild(node);
            }
            foreach (XmlNode node in paragraphsToAlter)
            {
                node.ParentNode.ReplaceChild(xmlDoc.CreateElement("br"), node);
            }
        }
    }
}