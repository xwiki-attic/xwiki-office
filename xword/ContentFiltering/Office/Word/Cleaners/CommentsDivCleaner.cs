using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Html;

namespace ContentFiltering.Office.Word.Cleaners
{
    public class CommentsDivCleaner : IHTMLCleaner
    {

        private const String COMMENTS_DIV = "<div style='mso-element:comment-list'>";
        private const String CLOSE_DIV = "</div>";

        #region IHTMLCleaner Members

        public string Clean(string htmlSource)
        {
            int startIndex = htmlSource.IndexOf(COMMENTS_DIV);
            int endIndex = -1;
            //The div does not contain nested divs.
            if (startIndex > 0)
            {
                endIndex = htmlSource.IndexOf(CLOSE_DIV, startIndex);
            }
            if(startIndex > 0 && endIndex > 0)
            {
                htmlSource = htmlSource.Remove(startIndex, endIndex + CLOSE_DIV.Length - startIndex);
            }
            return htmlSource;
        }
        #endregion
    }
}
