using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Html;

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Removes the head section from an html source.
    /// </summary>
    public class HeadSectionRemover : IHTMLCleaner
    {
        private HtmlUtil htmlUtil;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public HeadSectionRemover()
        {
            htmlUtil = new HtmlUtil();
        }
        #region IHTMLCleaner Members

        /// <summary>
        /// Removes the head section from an html source.
        /// </summary>
        /// <param name="htmlSource">The HTML source.</param>
        /// <returns>The HTML source without the head section.</returns>
        public string Clean(string htmlSource)
        {
            return htmlUtil.RemoveSpecificTagContent(htmlSource, "<head", "</head>");
        }

        #endregion IHTMLCleaner Members
    }
}