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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using XWiki;
using XWiki.XmlRpc;

namespace XWiki.Clients
{
    /// <summary>
    /// Primary interface used in communication between XOffice and XWiki servers.
    /// </summary>
    public interface IXWikiClient
    {
        /// <summary>
        /// Specifies if the current user is logged in to the server.
        /// </summary>
        bool LoggedIn 
        { 
            get;
        }

        /// <summary>
        /// Gets or sets the base URL of the server.
        /// </summary>
        String ServerURL
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the encoding from the XWiki server.
        /// </summary>
        Encoding ServerEncoding
        {
            get;
        }

        /// <summary>
        /// Specifies the protocol used by the client to communicate with the server.
        /// </summary>
        XWikiClientType ClientType
        {
            get;
        }

        /// <summary>
        /// Basic authentification method.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Bool - login succesfull/failed</returns>
        bool Login(String username, String password);

        /// <summary>
        /// Gets the availbale syntaxes for the server's rendered.
        /// </summary>
        /// <returns>A string list containing the names of the wiki server syntaxes.</returns>
        List<String> GetConfiguredSyntaxes();

        /// <summary>
        /// Gets the default wiki syntax of the server.
        /// </summary>
        /// <returns>A string representing the id of the </returns>
        String GetDefaultServerSyntax();
        
        /// <summary>
        /// Gets the spaces form the wiki instance.
        /// </summary>
        /// <returns>A list containg spaces names.</returns>
        List<String> GetSpacesNames();

        /// <summary>
        /// Gets the pages of a wiki space. 
        /// </summary>
        /// <param name="spaceName"></param>
        /// <returns>A list containing the pages in the specified space.</returns>
        List<String> GetPagesNames(String spaceName);

        /// <summary>
        /// Sets the html content of a page and then saves the page.
        /// </summary>
        /// <param name="docName">Wiki page name - SpaceName.PageName</param>
        /// <param name="content">The html source to be saved.</param>
        /// <param name="syntax">The syntax to which the source will be converted.</param>
        /// <permission>Requires programming rights for wiki page services.</permission>
        /// <returns>The result of the operation(Success - true/Failure - false).</returns>
        bool SavePageHTML(String docName, String content, String syntax);

        /// <summary>
        /// Adds a file as an attachment to a wiki page.
        /// </summary>
        /// <param name="docName">Wiki page name - SpaceName.PageName</param>
        /// <param name="filePath">The path to the uploaded file.</param>
        /// <returns>The result of the operation(Success - true/Failure - false).</returns>
        bool AddAttachment(String docName, String filePath);

        /// <summary>
        /// Adds a file as an attachment to a wiki page.
        /// </summary>
        /// <param name="space">The name of the space.</param>
        /// <param name="page">the name of the page.</param>
        /// <param name="filePath">The path to the uploaded file.</param>
        /// <returns>The result of the operation(Success - true/Failure - false).</returns>
        bool AddAttachment(String space, String page, String filePath);

        /// <summary>
        /// Adds a file to a wiki page as an attachment. The data transfer is asynchronuous.
        /// </summary>
        /// <param name="docName">Wiki page name - SpaceName.PageName</param>
        /// <param name="filePath">The path to the uploaded file.</param>
        void AddAttachmentAsync(String docName, String filePath);

        /// <summary>
        /// Adds a file to a wiki page as an attachment. The data transfer is asynchronuous.
        /// </summary>
        /// <param name="space">The name of the space.</param>
        /// <param name="page">The name of the space.</param>
        /// <param name="filePath">The path to the uploaded file.</param>
        void AddAttachmentAsync(String space, String page, String filePath);

        /// <summary>
        /// Gets a list with the attached files of a page
        /// </summary>
        /// <param name="docFullName">Wiki page name - SpaceName.PageName</param>
        /// <returns></returns>
        List<String> GetDocumentAttachmentList(String docFullName);

        /// <summary>Gets the download url of an attachment.
        /// </summary>
        /// <param name="docFullName">Wiki page name - SpaceName.PageName</param>
        /// <param name="attachmentName"></param>
        /// <returns>A string representing the URL of the attached file.</returns>
        String GetAttachmentURL(String docFullName, String attachmentName);

        /// <summary>
        /// Adds an object to a page
        /// </summary>
        /// <param name="docName">Wiki page name - SpaceName.PageName</param>
        /// <param name="className">XWiki class name of the new instance</param>
        /// <param name="fieldsValues">Pair that contains the field name and it's value</param>
        /// <returns>Index number of the object</returns>
        int AddObject(String docName, String className, NameValueCollection fieldsValues);

        /// <summary>
        /// Updates a xwiki object.
        /// </summary>
        /// <param name="pageFullName">The full name of the page containing the object.</param>
        /// <param name="className">The class name of the object.</param>
        /// <param name="fieldsValues">Name-value pairs containig corespongin to the field names and values ov the object.</param>
        void UpdateObject(string pageFullName, string className, System.Collections.Specialized.NameValueCollection fieldsValues);

        /// <summary>
        /// Updates a xwiki object.
        /// </summary>
        /// <param name="pageFullName">The full name of the page containing the object.</param>
        /// <param name="className">The class name of the object.</param>
        /// <param name="objectIndex">The index of the object in the document.</param>
        /// <param name="fieldsValues">Name-value pairs containig corespongin to the field names and values ov the object.</param>
        String UpdateObject(string pageFullName, string className, int objectIndex, System.Collections.Specialized.NameValueCollection fieldsValues);

        /// <summary>
        /// Gets an object from a page.
        /// </summary>
        /// <param name="pageId">Page name - SpaceName.PageName.</param>
        /// <param name="className">XWiki class name.</param>
        /// <param name="id">Index number of the object.</param>
        /// <returns>An XWikiObject of type 'className' from the specified page.</returns>
        XWikiObject GetObject(String pageId, String className, int id);

        /// <summary>
        /// Gets the objects from a page.
        /// </summary>
        /// <param name="pageId">Full page name - SpanceName.PageName</param>
        /// <returns>An array of <code>XWikiObjectSummary</code> - summary data for object in the given page.</returns>
        XWikiObjectSummary[] GetObjects(String pageId);

        /// <summary>
        /// Removes an object from a page.
        /// </summary>
        /// <param name="pageId">Full page name - SpaceName.PageName.</param>
        /// <param name="className">XWiki class name.</param>
        /// <param name="id">Index number of the object.</param>
        void RemoveObject(String pageId, String className, int id);


        /// <summary>
        /// Gets the binary data of the attached file.
        /// </summary>
        /// <param name="pageName">Wiki page name - SpaceName.PageName</param>
        /// <param name="fileName">Attached file name</param>
        /// <returns>The binary data of the file.</returns>
        byte[] GetAttachmentContent(String pageName, String fileName);

        /// <summary>
        /// Gets the rendered content of the specified page.
        /// </summary>
        /// <param name="pageFullName">Wiki page name - SpaceName.PageName</param>
        /// <returns>The rendered content of the page.</returns>
        String GetRenderedPageContent(String pageFullName);

        /// <summary>
        /// Gets the rendered content of the specified page.
        /// </summary>
        /// <param name="space">The name of the space.</param>
        /// <param name="page">The name of the page</param>
        /// <returns>The rendered content of the page.</returns>
        String GetRenderedPageContent(String space, String page);

        /// <summary>
        /// Gets the url of a wiki page for the view action.
        /// </summary>
        /// <param name="documentFullName">The full name of the wiki page.</param>
        /// <returns>The url of the wiki page.</returns>
        String GetURL(String documentFullName);

        /// <summary>
        /// Retrieves the history of a wiki page.
        /// </summary>
        /// <param name="pageId">The fullname of the page.</param>
        /// <returns>An PageHistorySummary array containing the history data.</returns>
        PageHistorySummary[] GetPageHistory(String pageId);
    }
}
