using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Replaces empty paragraphs with line breaks ('&lt;br/&gt;').
    /// </summary>
    public class EmptyParagraphsCleaner : IHTMLCleaner
    {
        #region IHTMLCleaner Members

        /// <summary>
        /// Replaces empty paragraphs with line breaks ('&lt;br/&gt;').
        /// </summary>
        /// <param name="htmlSource">Initial HTML source.</param>
        /// <returns>Cleaned HTML source (empty paragraphs replaced with line breaks).</returns>
        public string Clean(string htmlSource)
        {
            htmlSource = htmlSource.Replace("<o:p></o:p>", "<br />");
            htmlSource = htmlSource.Replace("<p>&nbsp;</p>", "<br />");
            return htmlSource;
        }

        #endregion IHTMLCleaner Members
    }
}