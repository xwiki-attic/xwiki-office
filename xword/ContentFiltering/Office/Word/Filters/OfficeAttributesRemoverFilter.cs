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
    public class OfficeAttributesRemoverFilter:IDOMFilter
    {
        private ConversionManager manager;

        public OfficeAttributesRemoverFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        #region IDOMFilter Members

        /// <summary>
        /// Deletes all office specific attributes.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        public void Filter(ref XmlDocument xmlDoc)
        {
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            XmlNamespaceManager nsMr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsMr.AddNamespace(String.Empty, "http://www.w3.org/1999/xhtml");
            nsMr.AddNamespace("v", "urn:schemas-microsoft-com:vml");
            nsMr.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
            nsMr.AddNamespace("w", "urn:schemas-microsoft-com:office:word");
            nsMr.AddNamespace("m", "http://schemas.microsoft.com/office/2004/12/omml");

            XPathExpression expression = navigator.Compile("//@v:* | //@o:* | //@w:* | //@m:*");
            XPathNodeIterator xIterator = navigator.Select(expression.Expression, nsMr);
            foreach (XPathNavigator nav in xIterator)
            {
                nav.DeleteSelf();
            }
        }

        #endregion
    }
}
