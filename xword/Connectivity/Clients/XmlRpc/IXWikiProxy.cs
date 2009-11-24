using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Proxy for connecting to XWiki via XML-RPC
    /// </summary>
    public interface IXWikiProxy : IXmlRpcProxy
    {
        #region Authentication

        /// <summary>
        /// Authenticates a user to the XWiki Server.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>
        /// The authentication token. 
        /// The token will be reused in all methods that require authentication
        /// </returns>
        [XmlRpcMethod("confluence1.login")]
        String Login(String username, String password);

        /// <summary>
        /// Logs out a user from the XWiki server.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <returns>True if the operation succeded. False otherwise.</returns>
        [XmlRpcMethod("confluence1.logout")]
        bool Logout(String token);

        #endregion//Authentication

        /// <summary>
        /// Gets a summary data about the server.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <returns>Summary data about the server.</returns>
        [XmlRpcMethod("confluence1.getServerInfo")]
        ServerInfo GetServerInfo(String token);

        #region Spaces

        /// <summary>
        /// Gets a list containg all spaces names in the wiki.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <returns>A list with all the spaces in the wiki.</returns>
        [XmlRpcMethod("confluence1.getSpaces")]
        SpaceSummary[] GetSpaces(String token);

        /// <summary>
        /// Gets the summary data for a space.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="spaceId">The id of the space.</param>
        /// <returns>The data regarding a space.</returns>
        [XmlRpcMethod("confluence1.getSpace")]
        Space GetSpace(String token, String spaceId);

        /// <summary>
        /// Adds a space to the wiki.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="spaceInfo">A Space instance containing data about the new space.</param>
        /// <returns>A new Space instance with the updated space data after the saving.</returns>
        [XmlRpcMethod("confluence1.addSpace")]
        Space AddSpace(String token, Space spaceInfo);

        /// <summary>
        /// Removes a space from the wiki.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="spaceKey">The name of the space.</param>
        /// <returns>True is the space is successfully removed. False otherwise.</returns>
        [XmlRpcMethod("confluence1.removeSpace")]
        bool RemoveSpace(String token, String spaceKey);

        #endregion//Spaces

        #region Pages

        /// <summary>
        /// Gets the data of a page.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="pageId">The id of the page.</param>
        /// <returns>The data of the page.</returns>
        [XmlRpcMethod("confluence1.getPage")]
        Page GetPage(String token, String pageId);

        /// <summary>
        /// Gets a summary data for the pages in a space.
        /// </summary>
        /// <param name="token">The authentication page.</param>
        /// <param name="spaceId">The id of the page.</param>
        /// <returns>Summary data for the pages in the specified space.</returns>
        [XmlRpcMethod("confluence1.getPages")]
        PageSummary[] GetPages(String token, String spaceId);

        /// <summary>
        /// Stores the content of a wiki page to the server.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="page">Page object conatining the information about the page, including the content.</param>
        /// <returns>A new instance of the page containing the updated info.</returns>
        [XmlRpcMethod("confluence1.storePage")]
        Page StorePage(String token, Page page);

        /// <summary>
        /// Stores the content of a wiki page to the server.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="page">Page object conatining the information about the page, including the content.</param>
        /// <param name="checkVersion">
        /// When true checks if the page already exists 
        /// and saves the content only if the page doesn't exist yet.
        /// When false the content is saved no matter if the page already exists or not.
        /// </param>
        /// <returns>A new instance of the page containing the updated info.</returns>
        [XmlRpcMethod("confluence1.storePage")]
        Page StorePage(String token, Page page, bool checkVersion);

        /// <summary>
        /// Gets the rendered content of a page.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="space">The space the contains the rendered page.</param>
        /// <param name="pageId">The id of the page.</param>
        /// <param name="content">The unrendered content of the page.</param>
        /// <returns>The rendered content of the page.</returns>
        [XmlRpcMethod("confluence1.renderContent")]
        String RenderContent(String token, String space, String pageId, String content);

        /// <summary>
        /// Removes a page from the wiki.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="pageId">The id of the page.</param>
        /// <returns>True if the page was removed successfully. False otherwise.</returns>
        [XmlRpcMethod("confluence1.removePage")]
        bool RemovePage(String token, String pageId);

        /// <summary>
        /// Gets the history of a page.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="pageId">The id of the page.</param>
        /// <returns>The history of the page.</returns>
        [XmlRpcMethod("confluence1.getPageHistory")]
        PageHistorySummary[] GetPageHistory(String token, String pageId);        

        /// <summary>
        /// Gets the history of the last modified pages.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="date">The date since when the history should be retrieved.</param>
        /// <param name="numberOfResults">The number of results.</param>
        /// <param name="start">The start offset.</param>
        /// <param name="fromLatest">Specifies if the retrived data should be start with the lates modification.</param>
        /// <returns>A history summary of the last modifications.</returns>
        [XmlRpcMethod("confluence1.getModifiedPagesHistory")]
        PageHistorySummary[] GetModifiedPagesHistory(String token, DateTime date, int numberOfResults, int start,
            bool fromLatest);

        #endregion//Pages

        #region Classes

        /// <summary>
        /// Gets the information about a XWiki class.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="className">The name of the XWiki class.</param>
        /// <returns>A XWikiClass instance containing the description of the class.</returns>
        [XmlRpcMethod("confluence1.getClass")]
        XWikiClass GetClass(String token, String className);

        /// <summary>
        /// Gets a summary about the existing XWiki classes on the current server.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <returns>A summary about the existing XWiki classes on the current server.</returns>
        [XmlRpcMethod("confluence1.getClasses")]
        XWikiClassSummary[] GetClasses(String token);

        #endregion//Classes

        #region Attachments

        /// <summary>
        /// Gets the list of attachments from a wiki page.
        /// </summary>
        /// <param name="token">The authentication page.</param>
        /// <param name="pageId">The id of the page containing the attachment</param>
        /// <returns></returns>
        [XmlRpcMethod("confluence1.getAttachments")]
        Attachment[] GetAttachments(String token, String pageId);

        /// <summary>
        /// Attaches a file to a wiki page.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="contentId"></param>
        /// <param name="attachment">Attachment instance specifiing the filename and other attributes.</param>
        /// <param name="attachmentData">The binary data for the attachment.</param>
        /// <returns>A new instance of an Attachment containing all attributes for the attached file.</returns>
        [XmlRpcMethod("confluence1.addAttachment")]
        Attachment AddAttachment(String token, int contentId, Attachment attachment, byte[] attachmentData);

        /// <summary>
        /// Gets the binary data of an attachment.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="pageId">The id of the page.</param>
        /// <param name="fileName">The name of the attached file.</param>
        /// <param name="versionNumber">The version of the attachment.</param>
        /// <returns>The binary data of the attachment.</returns>
        [XmlRpcMethod("confluence1.getAttachmentData")]
        byte[] GetAttachmentData(String token, String pageId, String fileName, String versionNumber);

        /// <summary>
        /// Removes an attachment from a page
        /// </summary>
        /// <param name="token">The autehntication token.</param>
        /// <param name="pageId">The id of the page.</param>
        /// <param name="fileName">The name of the attached file.</param>
        /// <returns>True if the attachement was successfully removed. False otherwise.</returns>
        [XmlRpcMethod("confluence1.removeAttachment")]
        Boolean RemoveAttachment(String token, String pageId, String fileName);

        #endregion//Attachments

        #region Objects

        /// <summary>
        /// Gets the XWiki objects in a XWiki document.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="pageId">The id of the page.</param>
        /// <returns>An array with summary data about the XWiki Objects in the given page.</returns>
        [XmlRpcMethod("confluence1.getObjects")]
        XWikiObjectSummary[] GetObjects(String token, String pageId);

        /// <summary>
        /// Retrives information about an object in a wiki document.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="pageId">The id of the page.</param>
        /// <param name="className">The class name of the object.</param>
        /// <param name="id">The id/index of the object.</param>
        /// <returns>An XWikiObject instance containing detailed data about the xwiki object.</returns>
        [XmlRpcMethod("confluence1.getObject")]
        XWikiObject GetObject(String token, String pageId, String className, int id);

        /// <summary>
        /// Retrives information about an object in a wiki document.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="pageId">The id of the page.</param>
        /// <param name="guid">The guid of the object.</param>
        /// <returns>An XWikiObject instance containing detailed data about the xwiki object.</returns>
        [XmlRpcMethod("confluence1.getObject")]
        XWikiObject GetObject(String token, String pageId, String guid);

        /// <summary>
        /// Stores a object to a wiki document.
        /// </summary>
        /// <param name="token">The autthentication token.</param>
        /// <param name="objectMap">An XWikiObject instance with the data to be stored.</param>
        /// <returns>An XWikiObject instance containing all updated attributes after the object has been stored.</returns>
        [XmlRpcMethod("confluence1.storeObject")]
        XWikiObject StoreObject(String token, XWikiObject objectMap);

        /// <summary>
        /// Stores a object to a wiki document.
        /// </summary>
        /// <param name="token">The autthentication token.</param>
        /// <param name="objectMap">An XWikiObject instance with the data to be stored.</param>
        /// <returns>An XWikiObject instance containing all updated attributes after the object has been stored.</returns>
        /// <param name="checkVersion">Specifies if the existence of an previous version of the object
        /// should be checked before storing the object.</param>
        [XmlRpcMethod("confluence1.storeObject")]
        XWikiObject StoreObject(String token, XWikiObject objectMap, bool checkVersion);

        /// <summary>
        /// Removes an XWiki object from a wiki document.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="pageId">The id of the page.</param>
        /// <param name="className">The XWiki class name of the object instance to be deleted.</param>
        /// <param name="id">The id(index) of the object ot be deleted.</param>
        /// <returns>True if the object is removed sucessfully. False otherwise.</returns>
        [XmlRpcMethod("confluence1.removeObject")]
        bool RemoveObject(String token, String pageId, String className, int id);

        #endregion//Objects

        #region Comments

        /// <summary>
        /// Gets the comments from a page.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="pageId">The id of the page containing the comments to be retrieved.</param>
        /// <returns>An array of Comment instances.</returns>
        [XmlRpcMethod("confluence1.getComments")]
        Comment[] GetComments(String token, String pageId);

        /// <summary>
        /// Retrieves information about a given comment.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="commentId">The id of the comment. Eg: "Main.WebHome?commentId=0"</param>
        /// <returns>A Comment instance with detailed information about the requested comment.</returns>
        [XmlRpcMethod("confluence1.getComment")]
        Comment GetComment(String token, String commentId);

        /// <summary>
        /// Adds a comment to a wiki page.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="comment">A comment instance containing the comment's content.</param>
        /// <returns>A updated instance of the comment with the new attributes values generated while saving.</returns>
        [XmlRpcMethod("confluence1.addComment")]
        Comment AddComment(String token, Comment comment);

        /// <summary>
        /// Removes a comment from a wiki page.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="commentId">The id of the comment. Eg: "Main.WebHome?commentId=0"</param>
        /// <returns>True if the comment is removed successfully. False otherwise.</returns>
        [XmlRpcMethod("confluence1.removeComment")]
        bool RemoveComment(String token, String commentId);

        #endregion//Comments

        /// <summary>
        /// Searches the server using hql queries
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="query">The hql query on the XWiki data model.</param>
        /// <param name="maxResults">Maximum number of results to be returned by the query.</param>
        /// <returns>A result set for the speified query.</returns>
        [XmlRpcMethod("confluence1.search")]
        XmlRpcStruct[] Search(String token, String query, int maxResults);

        #region rendering

        /// <summary>
        /// Converts a wiki source from a syntax to another syntax.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="source">The content to be converted.</param>
        /// <param name="initialSyntaxId">The initial syntax of the source.</param>
        /// <param name="targetSyntaxId">The final syntax of the returned content.</param>
        /// <returns>The converted source.</returns>
        [XmlRpcMethod("confluence1.convert")]
        String Convert(String token, String source, String initialSyntaxId, String targetSyntaxId);

        /// <summary>
        /// Gets all syntaxes supported by the rendering parsers as an input for a syntax conversion.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <returns>A list containing all syntaxes supported by rendering parsers.</returns>
        [XmlRpcMethod("confluence1.getInputSyntaxes")]
        String[] GetInputSyntaxes(String token);

        /// <summary>
        /// Gets all syntaxes supported by the rendering as an output for a syntax conversion.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <returns>A list containing all syntaxes supported by renderers.</returns>
        [XmlRpcMethod("confluence1.getOutputSyntaxes")]
        String[] GetOutputSyntaxes(String token);

        #endregion
    }
}
