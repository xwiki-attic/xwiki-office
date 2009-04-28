using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using XWiki.XmlRpc;

namespace XWiki.Clients
{
    /// <summary>
    /// Interacts with a XWiki server via XML-RPC
    /// </summary>
    /// <remarks>The client side xml-rpc is done using a XMlRPC.net(CookComputing)generated proxy.</remarks>
    public class XWikiXMLRPCClient : IXWikiClient
    {
        private bool isLoggedIn;
        private Encoding encoding;

        private string serverUrl;
        private string username;
        private string password;
        private string token;

        IXWikiProxy proxy;

        private const string XML_RPC_PATH = "/xwiki/xmlrpc";
        
        /// <summary>
        /// XML-RPC implementation of IXWikiCLient.
        /// Provides access to the main features of XWiki using the XML-RPC API.
        /// </summary>
        /// <param name="serverUrl">The url of the server.</param>
        /// <param name="username">The username used to authenticate.</param>
        /// <param name="password">The passowrd used to authenticate.</param>
        public XWikiXMLRPCClient(String serverUrl, String username, String password)
        {
            this.serverUrl = serverUrl;
            this.username = username;
            this.password = password;
            proxy = XmlRpcProxyGen.Create<IXWikiProxy>();
            proxy.Url = this.serverUrl + XML_RPC_PATH;
            try
            {
                token = proxy.Login(this.username, this.password);
                isLoggedIn = true;
            }
            catch (Exception)
            {
                isLoggedIn = false;
            }
        }

        #region IXWikiClient Members

        /// <summary>
        /// Specifies if the current user is logged in.
        /// </summary>
        public bool LoggedIn
        {
            get
            {
                return isLoggedIn;
            }
        }
              
        /// <summary>
        /// Specifies if the current encoding from the XWiki server.
        /// </summary>
        public Encoding ServerEncoding
        {
            get
            {
                return encoding;
            }
        }

        /// <summary>
        /// Authenticates the user to the server.
        /// </summary>
        /// <returns>True if the operation succedes. False if the operation fails.</returns>
        public bool Login(string username, string password)
        {
            try
            {
                token = proxy.Login(username, password);
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        /// <summary>
        /// Gets the spaces of a wiki.
        /// </summary>
        /// <returns>A list containing the wiki spaces names.</returns>
        public List<string> GetSpaces()
        {
            SpaceSummary[] spaces = proxy.GetSpaces(token);
            List<string> spacesNames = new List<string>();
            foreach (SpaceSummary space in spaces)
            {
                spacesNames.Add(space.name);
            }
            return spacesNames;
        }

        /// <summary>
        /// Gets the pages names of a wiki space.
        /// </summary>
        /// <param name="spaceName">The name of the wiki space.</param>
        /// <returns>A list with the full names of the documents in a wiki space.</returns>
        public List<string> GetPages(string spaceName)
        {
            PageSummary[] pages = proxy.GetPages(token, spaceName);
            List<string> pagesNames = new List<string>();
            foreach (PageSummary ps in pages)
            {
                pagesNames.Add(ps.id);
            }
            return pagesNames;
        }

        public bool SavePageHTML(string docName, string content, string syntax)
        {
            throw new NotImplementedException();
        }

        public bool AddAttachment(string docName, string filePath)
        {
            throw new NotImplementedException();
        }

        public bool AddAttachment(string docName, string fileName, System.IO.Stream content)
        {
            throw new NotImplementedException();
        }

        public int AddObject(string docName, string ClassName, System.Collections.Specialized.NameValueCollection fieldsValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the names of the attached files of a wiki page.
        /// </summary>
        /// <param name="docFullName">The full name of the wiki page.</param>
        /// <returns>A list containing the names of the attachments of a wiki page.</returns>
        public List<string> GetDocumentAttachmentList(string docFullName)
        {
            Attachment[] attachments = proxy.GetAttachments(token, docFullName);
            List<string> attachmentsNames = new List<string>();
            foreach (Attachment attachment in attachments)
            {
                attachmentsNames.Add(attachment.fileName);
            }
            return attachmentsNames;
        }

        public string GetAttachmentURL(string docFullName, string attachmentName)
        {
            throw new NotImplementedException();
        }

        public byte[] GetAttachmentContent(string pageName, string FileName)
        {
            throw new NotImplementedException();
        }

        public byte[] GetAttachmentContent(string URL)
        {
            throw new NotImplementedException();
        }

        public bool AddAttachment(string space, string page, string filePath)
        {
            throw new NotImplementedException();
        }

        public bool AddAttachmentAsync(string docName, string filePath)
        {
            throw new NotImplementedException();
        }

        public bool AddAttachmentAsync(string space, string page, string filePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the rendered content of the specified page.
        /// </summary>
        /// <param name="pageFullName">Wiki page name - SpaceName.PageName</param>
        /// <returns>The rendered content of the page.</returns>
        public string GetRenderedPageContent(string pageFullName)
        {
            Page page = proxy.GetPage(token, pageFullName);
            String renderedContent = proxy.RenderContent(token, page.space, page.id, page.content);
            return renderedContent;
        }

        /// <summary>
        /// Gets the rendered content of a page.
        /// </summary>
        /// <param name="pageFullName">The full name of the wiki page.</param>
        /// <returns>The rendered content of the page.</returns>
        public string GetRenderedPageContent(string space, string page)
        {
            String pageFullName = space + "." + page;
            return GetRenderedPageContent(pageFullName);
        }

        public string GetURL(string documentFullName, string xwikiAction)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
