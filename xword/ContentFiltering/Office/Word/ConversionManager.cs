#region LGPL license
/*
 * See the NOTICE file distributed with this work for additional
 * information regarding copyright ownership.
 *
 * This is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this software; if not, write to the Free
 * Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
 * 02110-1301 USA, or see the FSF site: http://www.fsf.org.
 */
#endregion //license

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using XWiki.Html;
using XWiki.Clients;
using XOffice;

namespace XWiki.Office.Word
{
    /// <summary>
    /// Manages the bidirectional conversion.(Web to Word and Word to Web)
    /// </summary>
    public class ConversionManager
    {

        /// <summary>
        /// Contains the new or modified attachments that need to be uploaded.
        /// </summary>
        private List<String> newAttachments;

        private XOfficeCommonSettings settings;

        /// <summary>
        /// Creates a new instance of the ConversionManager class.
        /// </summary>
        /// <param name="serverURL">The url of the server.</param>
        /// <param name="localFolder">The local folder where documents are saved/</param>
        /// <param name="docFullName">The full name of the wiki page.</param>
        /// <param name="localFileName">The local file coresponding to the edited wiki page.</param>
        /// <param name="client">IXWikiClient implementation, handling Client server communication.</param>
        public ConversionManager(XOfficeCommonSettings settings, String serverURL, String localFolder, String docFullName, String localFileName, IXWikiClient client)
        {
            states = new BidirectionalConversionStates(serverURL);
            states.LocalFolder = localFolder;
            states.PageFullName = docFullName;
            states.LocalFileName = localFileName;
            xwikiClient = client;
            this.settings = settings;
            localToWebHtml = new LocalToWebHTML(this);
            webToLocalHtml = new WebToLocalHTML(this);
            newAttachments = new List<string>();
        }

        internal XOfficeCommonSettings AddinSettings
        {
            get { return this.settings; }
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
            states.SetActionState(ConverterActionState.EditingPage);
            return content;
        }

        /// <summary>
        /// Adds an file to the list of files to be attached to the page.
        /// </summary>
        /// <param name="attachmentPath">The path to the attachment to be uploaded.</param>
        public void RegisterForUpload(String attachmentPath)
        {
            String path = HttpUtility.UrlDecode(attachmentPath);
            newAttachments.Add(path);            
        }

        public void UploadAttachments()
        {
            foreach (String filePath in newAttachments)
            {
                xwikiClient.AddAttachment(states.PageFullName, filePath);
            }
            //clean the list for future uploads
            newAttachments.Clear();
        }
    }
}
