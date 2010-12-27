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
    public partial class XWikiXMLRPCClient : IXWikiClient
    {
        private bool isLoggedIn;
        //Default value for encoding
        private Encoding encoding = Encoding.GetEncoding("UTF-8");

        private string serverUrl;
        private string username;
        private string password;
        private string token;

        private const string separator = ".";
        IXWikiProxy proxy;

        private const string XML_RPC_PATH = "/xmlrpc";
        private const string DEFAULT_APP_CONTEXT = "/xwiki";
        ServerInfo serverInfo;

        public ServerInfo ServerInfo
        {
            get { return serverInfo; }
            set { serverInfo = value; }
        }
        
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
            Login();
            if (!isLoggedIn)
            {
                proxy.Url = this.serverUrl + DEFAULT_APP_CONTEXT + XML_RPC_PATH;
                Login();
            }
        }


        private void Login()
        {
            try
            {
                token = proxy.Login(this.username, this.password);
                serverInfo = proxy.GetServerInfo(token);
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
                int serverMajorVersion = Int32.Parse(serverInfo.majorVersion);
                int serverMinorVersion = Int32.Parse(serverInfo.minorVersion);
                if (serverMajorVersion >= 2 && serverMinorVersion >= 0)
                {
                    page.content = proxy.Convert(token, page.content, "xhtml/1.0", syntax);
                }
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
        public int AddObject(string docName, string className, System.Collections.Specialized.NameValueCollection fieldsValues)
        {
            XmlRpcStruct objectDictionary = new XmlRpcStruct();
            foreach (string key in fieldsValues.Keys)
            {
                objectDictionary.Add(key, fieldsValues[key]);
            }
            XWikiObject obj = new XWikiObject();
            obj.className = className;
            obj.objectDictionary = objectDictionary;
            obj.pageId = docName;
            obj.guid = string.Empty;
            obj.prettyName = fieldsValues["name"];
            obj = proxy.StoreObject(token, obj);
            return obj.id;
        }

        /// <summary>
        /// Updates a xwiki object.
        /// </summary>
        /// <param name="pageFullName">The full name of the page containing the object.</param>
        /// <param name="className">The class name of the object.</param>
        /// <param name="fieldsValues">Name-value pairs containig corespongin to the field names and values ov the object.</param>
        public void UpdateObject(string pageFullName, string className, System.Collections.Specialized.NameValueCollection fieldsValues)
        {
            XmlRpcStruct objectDictionary = new XmlRpcStruct();
            foreach (string key in fieldsValues.Keys)
            {
                objectDictionary.Add(key, fieldsValues[key]);
            }
            XWikiObject obj = new XWikiObject();
            obj.className = className;
            obj.objectDictionary = objectDictionary;
            obj.pageId = pageFullName;
            obj.guid = string.Empty;
            obj.prettyName = fieldsValues["name"];
            obj = proxy.StoreObject(token, obj);
        }

        /// <summary>
        /// Gets an object from a page.
        /// </summary>
        /// <param name="pageId">Page name - SpaceName.PageName.</param>
        /// <param name="className">XWiki class name.</param>
        /// <param name="id">Index number of the object.</param>
        /// <returns>An XWikiObject of type 'className' from the specified page.</returns>
        public XWikiObject GetObject(String pageId, String className, int id)
        {
            return proxy.GetObject(token, pageId, className, id);
        }

        /// <summary>
        /// Gets the objects from a page.
        /// </summary>
        /// <param name="pageId">Full page name - SpanceName.PageName</param>
        /// <returns>An array of <code>XWikiObjectSummary</code> - summary data for object in the given page.</returns>
        public XWikiObjectSummary[] GetObjects(String pageId)
        {
            return proxy.GetObjects(token, pageId);
        }


        /// <summary>
        /// Removes an object from a page.
        /// </summary>
        /// <param name="pageId">Full page name - SpaceName.PageName.</param>
        /// <param name="className">XWiki class name.</param>
        /// <param name="id">Index number of the object.</param>
        public void RemoveObject(String pageId, String className, int id)
        {
            proxy.RemoveObject(token, pageId, className, id);
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
            //TODO: use getRenderedContent when fixed and use only one transport(remove GetPage)
            String renderedContent = null;
            bool supportsXWikiNewRendering = true;
            
            //Get the page data including the wiki content
            Page page = proxy.GetPage(token, pageFullName);

            int version = Int32.Parse(serverInfo.majorVersion);
            if (version >= 2)
            {
                try
                {                    
                    renderedContent = proxy.RenderPageContent(token, pageFullName, page.content,
                                                              page.syntaxId, "annotatedxhtml/1.0");
                }
                catch (Exception ex)
                {
                    supportsXWikiNewRendering = false;
                    Log.Exception(ex);               
                }
            }
            if((!supportsXWikiNewRendering) || version < 2)
            {
                renderedContent  = proxy.RenderContent(token, page.space, page.id, page.content);
            }
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
        /// Gets the availbale syntaxes for the server's renderer.
        /// </summary>
        /// <returns>A string list containing the names of the configured wiki server syntaxes.</returns>
        public List<String> GetConfiguredSyntaxes()
        {
            List<String> syntaxes = new List<string>();
            if (serverInfo.ConfiguredSyntaxes != null && serverInfo.ConfiguredSyntaxes != "")
            {
                string[] separators = { " ", "[", "]", "," };
                syntaxes.AddRange(serverInfo.ConfiguredSyntaxes.Split(separators, StringSplitOptions.RemoveEmptyEntries));
                return syntaxes;
            }
            else
            {
                foreach (String s in Properties.Settings.Default.XmlRpcSyntaxes)
                {
                    syntaxes.Add(s);
                }
            }
            return syntaxes;
        }

        /// <summary>
        /// Retrieves the history of a wiki page.
        /// </summary>
        /// <param name="pageId">The fullname of the page.</param>
        /// <returns>An PageHistorySummary array containing the history data.</returns>
        public PageHistorySummary[] GetPageHistory(String pageId)
        {
            return proxy.GetPageHistory(token, pageId);
        }

        /// <summary>
        /// Gets the default wiki syntax of the server.
        /// </summary>
        /// <returns>A string representing the id of the </returns>
        public String GetDefaultServerSyntax()
        {
            return serverInfo.DefaultSyntax;
        }

        /// <summary>
        /// Gets the app context from the server URL
        /// </summary>
        private String GetAppContext()
        {
            char separator = '/';
            if (serverUrl.Contains(separator))
            {
                return serverUrl.Split(separator)[1];
            }
            else
            {
                return null;
            }
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
