using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWiki.Clients
{
    public class XWikiXMLRPCClient : IXWikiClient
    {
        private bool isLoggedIn;
        /// <summary>
        /// XML-RPC implementation of IXWikiCLient.
        /// Provides access to the main features of XWiki using the XML-RPC API.
        /// </summary>
        public XWikiXMLRPCClient()
        {
            throw new Exception("Class not implemented yet.");
        }

        #region IXWikiClient Members

        /// <summary>
        /// Specifies if the current user is logged in.
        /// </summary>
        public bool IsLoggedIn
        {
            get
            {
                return isLoggedIn;
            }
        }
              
        /// <summary>
        /// Authenticates the user to the server.
        /// </summary>
        /// <returns>True if the operation succedes. False if the operation fails.</returns>
        public bool Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream GetWikiStructure()
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream GetWikiPageAsPlainHTML(string docName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetSpaces()
        {
            throw new NotImplementedException();
        }

        public List<string> GetPages(string spaceName)
        {
            throw new NotImplementedException();
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

        public List<string> GetDocumentAttachmentList(string docFullName)
        {
            throw new NotImplementedException();
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

        public string GetRenderedPageContent(string pageFullName)
        {
            throw new NotImplementedException();
        }

        public string GetRenderedPageContent(string space, string page)
        {
            throw new NotImplementedException();
        }

        public string GetURL(string documentFullName, string xwikiAction)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
