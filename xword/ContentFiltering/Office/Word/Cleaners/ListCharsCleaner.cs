using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Replaces some characters used by MS Word for bullet lists with 'o' characters.
    /// </summary>
    public class ListCharsCleaner : IHTMLCleaner
    {
        #region IHTMLCleaner Members

        /// <summary>
        /// Replaces some characters used by MS Word for bullet lists (like <code>&amp;middot;</code>)
        /// with 'o' characters.
        /// </summary>
        /// <param name="htmlSource">Initial HTML source.</param>
        /// <returns>Cleaned HTML source.</returns>
        public string Clean(string htmlSource)
        {
            htmlSource = htmlSource.Replace('·', 'o');
            htmlSource = htmlSource.Replace('§', 'o');
            return htmlSource;
        }

        #endregion  IHTMLCleaner Members
    }
}