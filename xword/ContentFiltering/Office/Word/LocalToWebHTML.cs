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
using ContentFiltering.Office.Word.Cleaners;

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

            content = new LocalToWebHTMLCleaner(HTML_OPENING_TAG).Clean(content);

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
