using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Html;
using XWiki.Clients;

namespace XWiki.Office.Word
{
    /// <summary>
    /// Manages the bidirectional conversion.(Web to Word and Word to Web)
    /// </summary>
    public class ConversionManager
    {
        /// <summary>
        /// Creates a new instance of the ConversionManager class.
        /// </summary>
        /// <param name="serverURL">The url of the server.</param>
        /// <param name="localFolder">The local folder where documents are saved/</param>
        /// <param name="docFullName">The full name of the wiki page.</param>
        /// <param name="localFileName">The local file coresponding to the edited wiki page.</param>
        /// <param name="client">IXWikiClient implementation, handling Client server communication.</param>
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

        /// <summary>
        /// IXWikiClient instance.
        /// </summary>
        private IXWikiClient xwikiClient;

        /// <summary>
        /// Gets or sets the instance for the client.
        /// </summary>
        public IXWikiClient XWikiClient
        {
            get { return xwikiClient; }
            set { xwikiClient = value; }
        }

        /// <summary>
        /// Handler for Word->Web conversion
        /// </summary>
        private LocalToWebHTML localToWebHtml;
        
        /// <summary>
        /// Gets or sets the instance of the Word->Web convertor.
        /// </summary>
        internal LocalToWebHTML LocalToWebHtml
        {
            get { return localToWebHtml; }
            set { localToWebHtml = value; }
        }
        
        /// <summary>
        /// Instaqnce for the Web->Word convertor.
        /// </summary>
        private WebToLocalHTML webToLocalHtml;
        
        /// <summary>
        /// Gets or sets the instance for the Web->Word convertor.
        /// </summary>
        internal WebToLocalHTML WebToLocalHtml
        {
            get { return webToLocalHtml; }
            set { webToLocalHtml = value; }
        }

        /// <summary>
        /// The states of the conversion.
        /// </summary>
        private BidirectionalConversionStates states;

        /// <summary>
        /// Gets or sets the states of the conversion.
        /// </summary>
        public BidirectionalConversionStates States
        {
            get { return states; }
            set { states = value; }
        }

        /// <summary>
        /// Converts a html source from a Web form to Word compatible format.
        /// </summary>
        /// <param name="content">The html to be converted.</param>
        /// <returns>The converted html.</returns>
        public String ConvertFromWebToWord(String content)
        {
            states.SetActionState(ConverterActionState.Downloading);            
            content = webToLocalHtml.AdaptSource(content);
            states.SetActionState(ConverterActionState.EditingPage);
            return content;
        }

        /// <summary>
        /// Converts a html source from local Word format to xhtml usable on web.
        /// </summary>
        /// <param name="content">The html to be converted.</param>
        /// <returns>The new  xhtml.</returns>
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
