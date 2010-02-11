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
using System.IO;
using System.Xml;
using System.Xml.XPath;
using XWiki.Xml;
using System.Collections;
using ContentFiltering.Office.Word;
using ContentFiltering.Office.Word.Filters;
using ContentFiltering.Office.Word.Cleaners;

namespace XWiki.Office.Word
{
    public class LocalToWebHTML : AbstractConverter
    {

        public LocalToWebHTML(ConversionManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Adapts the HTML source from it's local MS Word form in order to be used by the wiki.
        /// </summary>
        /// <param name="content">The initial HTML source.</param>
        /// <returns>The adapted HTML code.</returns>
        public String AdaptSource(String content)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.XmlResolver = null;

            content = new LocalToWebHTMLCleaner(HTML_OPENING_TAG).Clean(content);

            xmlDoc.LoadXml(content);
            //rigister the html filters
            List<IDOMFilter> contentFilters = new List<IDOMFilter>()
            {
                new LocalToWebStyleFilter(manager),
                new StyleRemoverFilter(manager),
                new GrammarAndSpellingErrorsFilter(manager),
                new LocalImageAdaptorFilter(manager),
                new LocalListsAdaptorFilter(manager),
                new LocalMacrosAdaptorFilter(manager),
                new OfficeAttributesRemoverFilter(manager)
            };
            
            
            foreach(IDOMFilter contentFilter in contentFilters)
            {
                contentFilter.Filter(ref xmlDoc);
            }
            
            
            StringBuilder sb = new StringBuilder(xmlDoc.GetIndentedXml());
            sb.Replace(" xmlns=\"\"","");
            return sb.ToString();
        }
     
    }
}
