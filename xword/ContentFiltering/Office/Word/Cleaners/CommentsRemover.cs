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
using XWiki.Html;

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Removes comments ('&lt;!-- ... &gt;' and '&lt;![ ... ]&gt;') from an html source.
    /// </summary>
    public class CommentsRemover : IHTMLCleaner
    {
        private HtmlUtil htmlUtil;

        public CommentsRemover()
        {
            htmlUtil = new HtmlUtil();
        }
        #region IHTMLCleaner Members

        /// <summary>
        /// Removes comments ('&lt;!-- ... &gt;' and '&lt;![ ... ]&gt;') from an html source.
        /// </summary>
        /// <param name="htmlSource">The HTML source to clean.</param>
        /// <returns>The cleaned HTML source (without comments)</returns>
        public string Clean(string htmlSource)
        {
            string cleanHTML = htmlUtil.RemoveSpecificTagContent(htmlSource, "<!--", "-->");
            return htmlUtil.RemoveSpecificTagContent(cleanHTML, "<![", "]>");
        }

        #endregion IHTMLCleaner Members
    }
}