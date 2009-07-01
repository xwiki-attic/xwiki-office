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