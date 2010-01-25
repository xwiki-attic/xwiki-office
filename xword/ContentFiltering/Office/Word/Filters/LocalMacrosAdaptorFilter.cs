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
    public class LocalMacrosAdaptorFilter:IDOMFilter
    {
        private ConversionManager manager;

        public LocalMacrosAdaptorFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        #region IDOMFilter Members

        /// <summary>
        /// Replaces the read-only Word content controls with XWiki macro markup.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document instance.</param>
        public void Filter(ref XmlDocument xmlDoc)
        {
            XmlNodeList macroNodes = xmlDoc.GetElementsByTagName("Sdt", "urn:schemas-microsoft-com:office:word");
            XmlDocumentFragment docFrag = xmlDoc.CreateDocumentFragment();
            Dictionary<String, String> macros = this.manager.States.Macros;
            //We use a new list because the XmlNodeList will break when operationg with its' elements.
            List<XmlNode> nodeList = new List<XmlNode>();
            foreach (XmlNode node in macroNodes)
            {
                nodeList.Add(node);
            }
            foreach (XmlNode node in nodeList)
            {
                try
                {
                    String id = node.Attributes["ID"].Value;
                    if (macros.ContainsKey(id))
                    {
                        String content = macros[id];
                        docFrag.InnerXml = content;
                        node.ParentNode.ReplaceChild(docFrag, node);
                    }
                }
                catch (NullReferenceException nre)
                {
                    Log.Exception(nre);
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
            }
        }

        #endregion
    }
}
