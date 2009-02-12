using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Html;
using XWiki.Clients;

namespace XWiki.Office.Word
{
    public class ConversionManager
    {
        public ConversionManager(String serverURL, String localFolder, String docFullName, String localFileName, IXWikiClient client)
        {
            states = new BidirectionalConversionStates(serverURL);
            states.LocalFolder = localFolder;
            states.PageFullName = docFullName;
            states.LocalFileName = localFileName;
            xwikiClient = client;
            localToWebHtml = new LocalToWebHTML(this);
            webToLocalHtml = new WebToLocalHTML(this);
        }

        private IXWikiClient xwikiClient;

        public IXWikiClient XWikiClient
        {
            get { return xwikiClient; }
            set { xwikiClient = value; }
        }

        private LocalToWebHTML localToWebHtml;

        internal LocalToWebHTML LocalToWebHtml
        {
            get { return localToWebHtml; }
            set { localToWebHtml = value; }
        }
        private WebToLocalHTML webToLocalHtml;

        internal WebToLocalHTML WebToLocalHtml
        {
            get { return webToLocalHtml; }
            set { webToLocalHtml = value; }
        }

        private BidirectionalConversionStates states;

        public BidirectionalConversionStates States
        {
            get { return states; }
            set { states = value; }
        }

        public String ConvertFromWebToWord(String content)
        {
            states.SetActionState(ConverterActionState.Downloading);            
            content = webToLocalHtml.AdaptSource(content);
            states.SetActionState(ConverterActionState.EditingPage);
            return content;
        }

        public String ConvertFromWordToWeb(String content)
        {
            states.SetActionState(ConverterActionState.Uploading);
            content = localToWebHtml.AdaptSource(content);
            String oldContent = content;
            //The indentation does not work if the file is to complex or very large.
            //In this case the content will be stored without indentation.
            content = new HtmlUtil().IndentContent(content);
            if (content == "")
            {
                content = oldContent;
            }
            states.SetActionState(ConverterActionState.EditingPage);
            return content;
        }
    }
}
