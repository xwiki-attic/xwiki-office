using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Removes the tags that are in the office namespaces.
    /// </summary>
    public class OfficeNameSpacesTagsRemover : IHTMLCleaner
    {
        #region IHTMLCleaner Members

        /// <summary>
        /// Removes the tags that are in the office namespaces.
        /// </summary>
        /// <param name="content">The original content.</param>
        /// <returns>The cleaned content.</returns>
        public string Clean(string htmlSource)
        {
            bool foundTags = false;
            int startIndex = 0;
            int endIndex = 0;
            do
            {
                foundTags = false;
                startIndex = htmlSource.IndexOf("<o:", startIndex);
                if (startIndex >= 0)
                {
                    endIndex = htmlSource.IndexOf("</o:", startIndex);
                    if (endIndex >= 0)
                    {
                        endIndex = htmlSource.IndexOf(">", endIndex + 1);
                        htmlSource = htmlSource.Remove(startIndex, endIndex - startIndex + 1);
                    }
                    foundTags = true;
                    startIndex = endIndex - (endIndex - startIndex + 1);
                }
            } while (foundTags);
            return htmlSource;
        }

        #endregion IHTMLCleaner Members
    }
}