using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Gets the inner html of the body.
    /// </summary>
    public class BodyContentExtractor : IHTMLCleaner
    {

        #region IHTMLCleaner Members

        /// <summary>
        /// Gets the content between the opening and closing html tags.
        /// </summary>
        /// <param name="htmlCode">The html source to be </param>
        /// <returns>The inner html of the body.</returns>
        public string Clean(string htmlCode)
        {
            //Delete header & footer
            int startIndex, endIndex;
            startIndex = htmlCode.IndexOf("<body");
            endIndex = htmlCode.IndexOf(">", startIndex);
            htmlCode = htmlCode.Remove(0, endIndex + 1);
            startIndex = htmlCode.IndexOf("</body");
            if (startIndex >= 0)
                htmlCode = htmlCode.Remove(startIndex);
            return htmlCode;
        }

        #endregion IHTMLCleaner Members
    }
}