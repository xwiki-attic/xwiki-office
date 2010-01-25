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

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Replaces the opening html tag with a given one.
    /// </summary>
    public class XmlNamespaceDefinitionsReplacer : IHTMLCleaner
    {
        private string newHtmlTag;

        /// <summary>
        /// Creates a new XmlNamespaceDefinitionsReplacer HTML cleaner (pre-DOM filter).
        /// </summary>
        /// <param name="newHtmlTag">Openening html tag that will replace the current one.</param>
        public XmlNamespaceDefinitionsReplacer(string newHtmlTag)
        {
            this.newHtmlTag = newHtmlTag;
        }

        #region IHTMLCleaner Members
        /// <summary>
        /// Replaces the opening html tag with a given one.
        /// </summary>
        /// <param name="htmlCode">The html source.</param>
        /// <param name="newHtmlTag">The new html tag.</param>
        /// <returns>Cleaned HTML source.</returns>
        public string Clean(string htmlCode)
        {
            String oldHtmlTag = GetXmlNamespaceDefinitions(htmlCode);
            if (oldHtmlTag == null)
            {
                if (!htmlCode.Contains("<body"))
                {
                    htmlCode = htmlCode.Insert(0, "<body>");
                    htmlCode = htmlCode.Insert(htmlCode.Length, "</body>");
                }
                htmlCode = htmlCode.Insert(0, newHtmlTag);
                htmlCode = htmlCode.Insert(htmlCode.Length, "</html>");
            }
            else
            {
                htmlCode = htmlCode.Replace(oldHtmlTag, newHtmlTag);
            }
            return htmlCode;
        }

        #endregion IHTMLCleaner Members

        /// <summary>
        /// Gets a string representing the opening html tag with the XML namespace definitions, if any.
        /// </summary>
        /// <param name="htmlCode">The html source to be processed</param>
        /// <returns>a string representing the opening html tag.</returns>
        public String GetXmlNamespaceDefinitions(String htmlCode)
        {
            int startIndex, endIndex;
            startIndex = htmlCode.IndexOf("<html");

            if (startIndex < 0)
            {
                return null;
            }
            else
            {
                endIndex = htmlCode.IndexOf(">", startIndex);
                return htmlCode.Substring(startIndex, endIndex - startIndex + 1);
            }
        }

    }
}