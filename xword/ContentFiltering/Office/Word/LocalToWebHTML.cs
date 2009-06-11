using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using XWiki.Xml;
using System.Collections;
using ContentFiltering.Office.Word;
using ContentFiltering.Office.Word.Filters;

namespace XWiki.Office.Word
{
    public class LocalToWebHTML : AbstractConverter
    {

        public LocalToWebHTML(ConversionManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Adapts the HTML source from it's local MS Word form in order to be used by the wiki.
        /// </summary>
        /// <param name="content">The initial HTML source.</param>
        /// <returns>The adapted HTML code.</returns>
        public String AdaptSource(String content)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.XmlResolver = null;
            String uncleanedContent = htmlUtil.CorrectAttributes(content);
            uncleanedContent = htmlUtil.CorrectTagsClosing(uncleanedContent, "img");
            uncleanedContent = htmlUtil.CorrectTagsClosing(uncleanedContent, "br");
            content = htmlUtil.CleanHTML(uncleanedContent, true);
            if (content.Length == 0)
            {
                content = uncleanedContent;
            }
            //content = htmlUtil.RemoveOfficeNameSpacesTags(content);
            //content = htmlUtil.ReplaceBody(content, "<body>");
            content = htmlUtil.ReplaceXmlNamespaceDefinitions(content, HTML_OPENING_TAG);
            content = content.Replace('·','o');
            content = content.Replace('§', 'o');//"·"; "o"; "§";
            //Removing &nbsp; from Word and Tidy output
            content = content.Replace("<o:p></o:p>", "<br />");
            content = content.Replace("<p>&nbsp;</p>", "<br />");
            content = content.Replace(">&nbsp;<", "><");
            content = content.Replace("<o:p>", "");
            content = content.Replace("</o:p>", "");
            content = content.Replace("&nbsp;", " ");
            xmlDoc.LoadXml(content);

            List<IDOMFilter> contentFilters = new List<IDOMFilter>()
            {
                new StyleRemoverFilter(manager),
                new GrammarAndSpellingErrorsFilter(manager),
                new LocalImageAdaptorFilter(manager),
                new LocalListsAdaptorFilter(manager),
                new LocalMacrosAdaptorFilter(manager),
                new OfficeAttributesRemoverFilter(manager)
            };
            
            
            foreach(IDOMFilter contentFilter in contentFilters)
            {
                contentFilter.Filter(ref xmlDoc);
            }
            
            
            StringBuilder sb = new StringBuilder(xmlDoc.GetIndentedXml());
            sb.Replace(" xmlns=\"\"","");
            return sb.ToString();
        }
     
    }
}
