using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
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
        //Default value for encoding
        private Encoding encoding = Encoding.GetEncoding("ISO-8859-1");

        private string serverUrl;
        private string username;
        private string password;
        private string token;

        private const string separator = ".";
        IXWikiProxy proxy;

        private const string XML_RPC_PATH = "/xwiki/xmlrpc";
        
        /// <summary>
        /// XML-RPC implementation of IXWikiCLient.
        /// Provides access to the main features of XWiki using the XML-RPC API.
        /// </summary>
        /// <param name="serverUrl">The url of the server.</param>
        /// <param name="username">The username used to authenticate.</param>
        /// <param name="password">The password used to authenticate.</param>
        internal XWikiXMLRPCClient(String serverUrl, String username, String password)
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

        /// <summary>
        /// Gets or sets the base URL of the server.
        /// </summary>
        public String ServerURL
        {
            get
            {
                return serverUrl;
            }
            set
            {
                serverUrl = value;
            }
        }


        /// <summary>
        /// Attaches a file to a page on the server.
        /// </summary>
        /// <param name="attachmentInfo">AttachmentInfo instance containing data about the attachment.</param>
        private void AttachFile(object attachmentInfo)
        {
            AttachmentInfo info = (AttachmentInfo)attachmentInfo;
            string filePath = info.filePath;
            string docName = info.pageId;
            FileInfo fi = new FileInfo(filePath);
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] buffer = new byte[fi.Length];
            fs.Read(buffer, 0, (int)fi.Length);
            fs.Close();
            Attachment att = new Attachment(docName);
            att.fileName = Path.GetFileName(filePath);
            proxy.AddAttachment(token, 0, att, buffer);
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
        /// Specifies the protocol used by the client to communicate with the server.
        /// </summary>
        public XWikiClientType ClientType
        {
            get
            {
                return XWikiClientType.XML_RPC;
            }
        }

        /// <summary>
        /// Gets the spaces of a wiki.
        /// </summary>
        /// <returns>A list containing the wiki spaces names.</returns>
        public List<string> GetSpacesNames()
        {
            SpaceSummary[] spaces = proxy.GetSpaces(token);
            List<string> spacesNames = new List<string>();
            foreach (SpaceSummary space in spaces)
            {
                spacesNames.Add(space.key);
            }
            return spacesNames;
        }

        /// <summary>
        /// Gets the pages names of a wiki space.
        /// </summary>
        /// <param name="spaceName">The name of the wiki space.</param>
        /// <returns>A list with the full names of the documents in a wiki space.</returns>
        public List<string> GetPagesNames(string spaceName)
        {
            PageSummary[] pages = proxy.GetPages(token, spaceName);
            List<string> pagesNames = new List<string>();
            foreach (PageSummary ps in pages)
            {
                String name = ps.id.Remove(0, ps.space.Length + separator.Length);
                pagesNames.Add(name);
            }
            return pagesNames;
        }

        /// <summary>
        /// Sets the html content of a page and then saves the page.
        /// </summary>
        /// <param name="docName">Wiki page name - SpaceName.PageName</param>
        /// <param name="content">The html source to be saved.</param>
        /// <param name="syntax">The syntax to which the source will be converted.</param>
        /// <permission>Requires programming rights for wiki page services.</permission>
        /// <returns>The result of the operation(Success - true/Failure - false).</returns>
        public bool SavePageHTML(string docName, string content, string syntax)
        {
            Page page = new Page(docName, content);
            try
            {
                proxy.StorePage(token, page);
                return true;
            }
            catch (XmlRpcFaultException e)
            {
                Log.Exception(e);
                return false;
            }
            catch (XmlRpcException ex)
            {
                Log.Exception(ex);
                return false;
            }
        }

        /// <summary>
        /// Adds a file as an attachment to a wiki page.
        /// </summary>
        /// <param name="docName">Wiki page name - SpaceName.PageName</param>
        /// <param name="filePath">The path to the uploaded file.</param>
        /// <returns>The result of the operation(Success - true/Failure - false).</returns>
        public bool AddAttachment(string docName, string filePath)
        {
            try
            {
                FileInfo fi = new FileInfo(filePath);
                FileStream fs = new FileStream(filePath, FileMode.Open); 
                byte[] buffer = new byte[fi.Length];
                fs.Read(buffer, 0, (int)fi.Length);
                fs.Close();
                Attachment att = new Attachment(docName);
                att.fileName = Path.GetFileName(filePath);
                proxy.AddAttachment(token, 0, att, buffer);
                return true;
            }
            catch (IOException ex)
            {
                Log.Exception(ex);
                return false;
            }   
        }

        /// <summary>
        /// Adds and XWiki opbject to a XWiki document.
        /// </summary>
        /// <param name="docName">The name of the XWiki Document.</param>
        /// <param name="ClassName">The class name of the XWiki object.</param>
        /// <param name="fieldsValues">The values of the object's properties.</param>
        /// <returns>The index/id of the added object.</returns>
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

        /// <summary>Gets the download url of an attachment.
        /// </summary>
        /// <param name="docFullName">Wiki page name - SpaceName.PageName</param>
        /// <param name="attachmentName"></param>
        /// <returns>A string representing the URL of the attached file.</returns>
        public string GetAttachmentURL(string docFullName, string attachmentName)
        {
            Attachment[] attachments = proxy.GetAttachments(token, docFullName);
            foreach (Attachment att in attachments)
            {
                if (att.fileName == attachmentName)
                {
                    return att.url;
                }
            }
            throw new Exception("There is no attachment with the name '" + attachmentName + "'");
        }

        /// <summary>
        /// Gets the binary data of the attached file.
        /// </summary>
        /// <param name="pageName">Wiki page name - SpaceName.PageName</param>
        /// <param name="fileName">Attached file name</param>
        /// <returns>The binary data of the file.</returns>
        public byte[] GetAttachmentContent(string pageName, string fileName)
        {
            byte[] content = proxy.GetAttachmentData(token, pageName, fileName, "");
            return content;
        }

        /// <summary>
        /// Adds a file as an attachment to a wiki page.
        /// </summary>
        /// <param name="space">The name of the space.</param>
        /// <param name="page">the name of the page.</param>
        /// <param name="filePath">The path to the uploaded file.</param>
        /// <returns>The result of the operation(Success - true/Failure - false).</returns>
        public bool AddAttachment(string space, string page, string filePath)
        {
            string docFullName = space + separator + page;
            return AddAttachment(docFullName, filePath);
        }

        /// <summary>
        /// Adds a file to a wiki page as an attachment. The data transfer is asynchronuous.
        /// </summary>
        /// <param name="docName">Wiki page name - SpaceName.PageName</param>
        /// <param name="filePath">The path to the uploaded file.</param>
        public void AddAttachmentAsync(string docName, string filePath)
        {
            AttachmentInfo info = new AttachmentInfo();
            info.pageId = docName;
            info.filePath = filePath;
            Thread t = new Thread(AttachFile);
            t.Start(info);
        }

        /// <summary>
        /// Adds a file to a wiki page as an attachment. The data transfer is asynchronuous.
        /// </summary>
        /// <param name="space">The name of the space.</param>
        /// <param name="page">The name of the space.</param>
        /// <param name="filePath">The path to the uploaded file.</param>
        public void AddAttachmentAsync(string space, string page, string filePath)
        {
            String pageFullName = space + separator + page;
            AddAttachment(pageFullName, filePath);
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
        /// <param name="space">The name of the wiki space.</param>
        /// <param name="page">The short name of the page.</param>
        /// <returns>The rendered content of the page.</returns>
        public string GetRenderedPageContent(string space, string page)
        {
            String pageFullName = space + separator + page;
            return GetRenderedPageContent(pageFullName);
        }

        /// <summary>
        /// Gets the url of a wiki page for the view action.
        /// </summary>
        /// <param name="documentFullName">The full name of the wiki page.</param>
        /// <returns>The url of the wiki page.</returns>
        public string GetURL(string documentFullName)
        {
            Page page = proxy.GetPage(token, documentFullName);
            return page.url;
        }

        /// <summary>
        /// Gets the availbale syntaxes for the server's rendered.
        /// </summary>
        /// <returns>A string list containing the names of the wiki server syntaxes.</returns>
        public List<String> GetAvailableSyntaxes()
        {
            //TODO: Get this from the server.
            List<String> syntaxes = new List<string>();
            foreach (String s in Properties.Settings.Default.XmlRpcSyntaxes)
            {
                syntaxes.Add(s);
            }
            return syntaxes;
        }


        #endregion
    }

    /// <summary>
    /// Auxiliary struct used to send data to other threads.
    /// </summary>
    internal struct AttachmentInfo
    {
        internal string pageId;
        internal string filePath;
    }
}
