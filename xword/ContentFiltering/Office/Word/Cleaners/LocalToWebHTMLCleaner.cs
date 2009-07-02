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
