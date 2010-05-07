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
    public class GrammarAndSpellingErrorsFilter:IDOMFilter
    {
        private ConversionManager manager;

        public GrammarAndSpellingErrorsFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        #region IDOMFilter Members
        
        /// <summary>
        /// Removes 'class' attribute from text marked as  containing grammar or spelling errors.
        /// (when values are 'gramE' or 'spellE'). Removes 'lang' attribute. Adds a space character
        /// (' ') to the affected text, to make sure words marked as errors are separated.
        /// </summary>
        /// <param name="xmlDoc">A reference to an xml document.</param>
        public void Filter(ref XmlDocument xmlDoc)
        {
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("span");
            bool insertASpace = false;
            XmlNode tempNode = null;
            foreach (XmlNode node in nodes)
            {
                try
                {
                    insertASpace = false;
                    XmlAttribute classAttribute = node.Attributes["class"];
                    if (classAttribute != null)
                    {
                        if (classAttribute.Value.ToLower().Trim().IndexOf("grame") >= 0
                            ||
                            classAttribute.Value.ToLower().Trim().IndexOf("spelle") >= 0)
                        {
                            node.Attributes.Remove(classAttribute);
                            insertASpace = true;
                        }
                    }
                    XmlAttribute langAttribute = node.Attributes["lang"];
                    if (langAttribute != null)
                    {
                        node.Attributes.Remove(langAttribute);
                        insertASpace = true;
                    }

                    if (insertASpace)
                    {
                        if (node.NodeType == XmlNodeType.Element)
                        {
                            tempNode = node.ChildNodes[0];
                        }
                        else
                        {
                            tempNode = node;
                        }
                        if (tempNode != null)
                        {
                            if (tempNode.Value != null)
                            {
                                tempNode.Value += " ";
                            }
                        }
                    }
                }
                catch (NullReferenceException nre)
                {
                    Log.Exception(nre);
                }
            }
        }
        #endregion
    }
}
