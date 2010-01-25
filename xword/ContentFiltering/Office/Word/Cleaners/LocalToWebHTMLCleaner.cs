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

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Main cleaner for <code>LocalToWeb</code>.
    /// </summary>
    public class LocalToWebHTMLCleaner:IHTMLCleaner
    {
        private string htmlOpeningTag;

        public LocalToWebHTMLCleaner(string htmlOpeningTag)
        {
            this.htmlOpeningTag = htmlOpeningTag;
        }

        #region IHTMLCleaner Members

        /// <summary>
        /// Main HTML cleaner for <code>LocalToWeb</code>. It calls other HTML cleaners in a certain order
        /// and returns the cleaned HTML.
        /// </summary>
        /// <param name="content">HTML content.</param>
        /// <returns>Cleaned HTML content.</returns>
        public string Clean(string content)
        {
            String uncleanedContent = new CorrectAttributesCleaner().Clean(content);
            uncleanedContent = new CorrectTagsClosingCleaner("img").Clean(uncleanedContent);
            uncleanedContent = new CorrectTagsClosingCleaner("br").Clean(uncleanedContent);
            content = new TidyHTMLCleaner(true).Clean(uncleanedContent);

            if (content.Length == 0)
            {
                content = uncleanedContent;
            }

            content = new XmlNamespaceDefinitionsReplacer(htmlOpeningTag).Clean(content);
            content = new ListCharsCleaner().Clean(content);
            content = new EmptyParagraphsCleaner().Clean(content);
            content = new NbspBetweenTagsRemover().Clean(content);
            content = new OfficeNameSpacesTagsRemover().Clean(content);
            content = new NbspReplacer().Clean(content);

            return content;
        }

        #endregion IHTMLCleaner Members
    }
}
