using System;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains the attributes of a comment.
    /// </summary>
    public struct Comment
    {
        /// <summary>
        /// The content of the comment.
        /// </summary>
        public string content;

        /// <summary>
        /// The id of the page contining the comment.
        /// </summary>
        public string pageId;

        /// <summary>
        /// The url of the comment.
        /// </summary>
        public string url;

        /// <summary>
        /// The user that posted the comment.
        /// </summary>
        public string creator;

        /// <summary>
        /// The date when the comment was created.
        /// </summary>
        public DateTime created;

        /// <summary>
        /// The id of the comment. This id identifies the comment in the entire wiki.
        /// Eg: "Main.WebHome?commentId=0"
        /// </summary>
        public string id;

        /// <summary>
        /// The title of the comment.
        /// </summary>
        public string title;

    }
}