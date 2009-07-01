using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Removes the &amp;nbsp; between tags.
    /// </summary>
    public class NbspBetweenTagsRemover:IHTMLCleaner
    {
        #region IHTMLCleaner Members

        /// <summary>
        /// Removes the &amp;nbsp; between tags.
        /// </summary>
        /// <param name="htmlSource">Initial HTML source.</param>
        /// <returns>Cleaned HTML.</returns>
        public string Clean(string htmlSource)
        {
            return htmlSource.Replace(">&nbsp;<", "><");
        }

        #endregion IHTMLCleaner Members
    }
}