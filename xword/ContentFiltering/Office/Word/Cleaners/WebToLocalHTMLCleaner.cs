using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Main HTML cleaner for <code>WebToLocal</code>.
    /// </summary>
    public class WebToLocalHTMLCleaner:IHTMLCleaner
    {
        private string htmlOpeningTag;

        public WebToLocalHTMLCleaner(string htmlOpeningTag)
        {
            this.htmlOpeningTag = htmlOpeningTag;
        }


        #region IHTMLCleaner Members

        public string Clean(string content)
        {
            content = new OfficeNameSpacesTagsRemover().Clean(content);
            content = new TidyHTMLCleaner(false).Clean(content);
            content = new XmlNamespaceDefinitionsReplacer(htmlOpeningTag).Clean(content);
            content = new EmptyParagraphsCleaner().Clean(content);
            content = new NbspBetweenTagsRemover().Clean(content);
            content = new NbspReplacer().Clean(content);

            return content;
        }

        #endregion IHTMLCleaner Members
    }
}
