using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using XWiki;

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
        /// Specifies the encoding from the XWiki server.
        /// </summary>
        Encoding ServerEncoding
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
        /// Gets the Wiki's structure, containing: spaces and pages.
        /// </summary>
        /// <returns>A stream with a serialized XWikiStructure instance</returns>
        Stream GetWikiStructure();

        /// <summary>
        /// Gets the rendered html of a page.
        /// </summary>
        /// <param name="docName">Wiki page name - SpaceName.PageName</param>
        /// <returns>A stream with the encoded html.</returns>
        Stream GetWikiPageAsPlainHTML(String docName);

        /// <summary>
        /// Gets the spaces form the wiki instance.
        /// </summary>
        /// <returns>A list containg spaces names.</returns>
        List<String> GetSpaces();

        /// <summary>
        /// Gets the pages of a wiki space. 
        /// </summary>
        /// <param name="spaceName"></param>
        /// <returns>A list containing the pages in the specified space.</returns>
        List<String> GetPages(String spaceName);

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
        /// <returns>The result of the operation(Success - true/Failure - false).</returns>
        bool AddAttachmentAsync(String docName, String filePath);

        /// <summary>
        /// Adds a file to a wiki page as an attachment. The data transfer is asynchronuous.
        /// </summary>
        /// <param name="space">The name of the space.</param>
        /// <param name="page">The name of the space.</param>
        /// <param name="filePath">The path to the uploaded file.</param>
        /// <returns>The result of the operation(Success - true/Failure - false).</returns>
        bool AddAttachmentAsync(String space, String page, String filePath);

        /// <summary>
        /// Gets a list with the attached files of a page
        /// </summary>
        /// <param name="docFullName">Wiki page name - SpaceName.PageName</param>
        /// <returns></returns>
        List<String> GetDocumentAttachmentList(String docFullName);

        /// <summary>
        /// </summary>
        /// <param name="docFullName">Wiki page name - SpaceName.PageName</param>
        /// <param name="attachmentName"></param>
        /// <returns>A string representing the URL of the attached file.</returns>
        String GetAttachmentURL(String docFullName, String attachmentName);

        /// <summary>
        /// Adds anf file as an attachment to a wiki page.
        /// </summary>
        /// <param name="docName">Wiki page name - SpaceName.PageName</param>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        /// <returns>The result of the operation(Success - true/Failure - false).</returns>
        bool AddAttachment(String docName, String fileName, Stream content);

        /// <summary>
        /// Adds an object to a page
        /// </summary>
        /// <param name="docName">Wiki page name - SpaceName.PageName</param>
        /// <param name="ClassName">XWiki class name of the new instance</param>
        /// <param name="fieldsValues">Pair that contains the field name and it's value</param>
        /// <returns>Index number of the object</returns>
        int AddObject(String docName, String ClassName, NameValueCollection fieldsValues);

        /// <summary>
        /// Gets the binary data of the attached file.
        /// </summary>
        /// <param name="pageName">Wiki page name - SpaceName.PageName</param>
        /// <param name="FileName">Attached file name</param>
        /// <returns>The binary data of the file.</returns>
        byte[] GetAttachmentContent(String pageName, String FileName);

        /// <summary>
        /// Gets the binary data of the attached file.
        /// </summary>
        /// <param name="URL">The URL of the file</param>
        /// <returns>The binary data of the file.</returns>
        byte[] GetAttachmentContent(String URL);

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
        /// Gets the document's relative URL for the specified action.
        /// </summary>
        /// <example>GetURL("Main.WebHome","edit");</example>
        /// <remarks>Most used actions are: view, edit, inline, delete,
        /// viewrev, pdf, preview, saveandcontinue, rollback, deleteversions,
        /// cancel, undelete, reset, register, objectadd, commentadd,
        /// objectremove, attach, upload, download, downloadrev,
        /// dot, delattachment, skin, jsx, ssx, login, loginsubmit,
        /// loginerror, logout, status, lifeblog, charting,
        /// gettables, createchart, previewchart, chartwizard,
        /// lock, redirect, admin, export, import, jcaptcha, unknown.
        /// </remarks>
        /// <param name="documentFullName">The full name of the xwiki page</param>
        /// <param name="xwikiAction">The struts action.</param>
        /// <returns>The URL for the specified page and action.</returns>
        String GetURL(String documentFullName, String xwikiAction);        
    }
}
