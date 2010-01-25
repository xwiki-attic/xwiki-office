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
using XWiki;

namespace ContentFiltering.Office.Word.Filters
{
    public class WebMacrosAdaptorFilter:IDOMFilter
    {
        private ConversionManager manager;
        private Random random = new Random();

        public WebMacrosAdaptorFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        #region IDOMFilter Members

        /// <summary>
        /// Adapts the html source to convert XWiki macros to Word Content Controls.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml dom containing the source.</param>
        public void Filter(ref XmlDocument xmlDoc)
        {
            XmlNode node = xmlDoc;
            ReplaceMacros(ref node, ref xmlDoc);
        }

        #endregion


        /// <summary>
        /// Replaces the macros in a xml node with a Word content control tag.
        /// </summary>
        /// <param name="node">The xml node to be adapted.</param>
        /// <param name="xmlDoc">A refrence to the xml document.</param>
        private void ReplaceMacros(ref XmlNode node, ref XmlDocument xmlDoc)
        {
            int context = 0; //0 - outside macros, 1- inside macro.
            List<List<XmlNode>> macroNodes = new List<List<XmlNode>>();
            List<XmlNode> currentMacroNodes = new List<XmlNode>();
            List<XmlNode> regularNodes = new List<XmlNode>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.NodeType == XmlNodeType.Comment)
                {
                    if (childNode.InnerText.StartsWith("startmacro"))
                    {
                        context = 1;
                        currentMacroNodes = new List<XmlNode>();
                        currentMacroNodes.Add(childNode);
                        macroNodes.Add(currentMacroNodes);
                    }
                    else if (childNode.InnerText.StartsWith("stopmacro"))
                    {
                        context = 0;
                        currentMacroNodes.Add(childNode);
                    }
                }
                else if (childNode.NodeType != XmlNodeType.Document && childNode.NodeType != XmlNodeType.DocumentType)
                {
                    if (context == 0)
                    {
                        regularNodes.Add(childNode);
                    }
                    else
                    {
                        currentMacroNodes.Add(childNode);
                    }
                }
            }
            foreach (List<XmlNode> macroElements in macroNodes)
            {
                if (macroElements.Count > 0)
                {
                    try
                    {
                        String macroContent = "";
                        XmlNode element = GenerateContentControlNode(ref xmlDoc);
                        String id = element.Attributes["ID"].Value;
                        XmlNode parent = macroElements[0].ParentNode;
                        parent.InsertBefore(element, macroElements[0]);
                        foreach (XmlNode n in macroElements)
                        {
                            String s = n.OuterXml;
                            if (n.NamespaceURI.Length > 0)
                            {
                                //Removing inline namespace declaration
                                String ns = " xmlns=\"" + n.NamespaceURI + "\"";
                                s = s.Replace(ns, "");
                            }
                            macroContent += s;
                            parent.RemoveChild(n);
                            element.AppendChild(n);
                        }
                        this.manager.States.Macros.Add(id, macroContent);
                    }
                    catch (XmlException ex)
                    {
                        Log.Exception(ex);
                    }
                }
            }
            foreach (XmlNode n in regularNodes)
            {

                XmlNode clone = n.Clone();
                n.ParentNode.ReplaceChild(clone, n);
                ReplaceMacros(ref clone, ref xmlDoc);
            }
        }



        /// <summary>
        /// Generates a new node instance for the Word Content Control.
        /// </summary>
        /// <param name="xmlDoc">A refence to the xml document.</param>
        /// <returns>The instance of the new node.</returns>
        private XmlNode GenerateContentControlNode(ref XmlDocument xmlDoc)
        {
            //Initialize the node of the content control.
            XmlElement element = xmlDoc.CreateElement("w:Sdt", "urn:schemas-microsoft-com:office:word");
            XmlAttribute sdtLocked = xmlDoc.CreateAttribute("SdtLocked");
            sdtLocked.Value = "t";
            XmlAttribute contentLocked = xmlDoc.CreateAttribute("ContentLocked");
            contentLocked.Value = "t";
            XmlAttribute docPart = xmlDoc.CreateAttribute("DocPart");
            docPart.Value = "DefaultPlaceholder_" + random.Next(9000000, 9999999).ToString();
            XmlAttribute id = xmlDoc.CreateAttribute("ID");
            id.Value = random.Next(9000000, 9999999).ToString();
            element.Attributes.Append(sdtLocked);
            element.Attributes.Append(contentLocked);
            element.Attributes.Append(docPart);
            element.Attributes.Append(id);
            return element;
        }
    }
}
