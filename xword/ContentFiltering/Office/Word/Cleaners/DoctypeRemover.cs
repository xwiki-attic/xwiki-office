using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Removes the doctype declaration from a given html code.
    /// </summary>
    public class DoctypeRemover : IHTMLCleaner
    {
        #region IHTMLCleaner Members

        /// <summary>
        /// Removes the doctype declaration from a given html code.
        /// </summary>
        /// <param name="htmlCode">The original html code.</param>
        /// <returns>The modified html code.</returns>
        public string Clean(string htmlCode)
        {
            int startIndex, endIndex;
            startIndex = htmlCode.IndexOf("<!DOCTYPE");
            endIndex = htmlCode.IndexOf(">", startIndex);
            return htmlCode.Remove(startIndex, endIndex - startIndex);
        }

        #endregion IHTMLCleaner Members

    }
}