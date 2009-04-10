using System;

namespace XWiki.XmlRpc
{
    /// <summary>
    /// Contains the attributes of a page.
    /// </summary>
    public struct Attachment
    {
        /// <summary>
        /// Comment on file attachment.
        /// </summary>
        public String comment;

        /// <summary>
        /// Filesize in bytes.
        /// </summary>
        public String fileSize;

        /// <summary>
        /// The download url for the attachment.
        /// </summary>
        public String url;

        /// <summary>
        /// The title of the attachment.
        /// </summary>
        public String title;

        /// <summary>
        /// The name of the file.
        /// </summary>
        public String fileName;

        /// <summary>
        /// The date when the file was created.
        /// </summary>
        public DateTime created;

        /// <summary>
        /// The mime-type of the attachment.
        /// </summary>
        public String contentType;

        /// <summary>
        /// The id of the page: [wiki:]Space.page[?param1=value1&param2=value2..]
        /// </summary>
        public String pageId;

        /// <summary>
        /// The user that attached the file.
        /// </summary>
        public String creator;

        /// <summary>
        /// Constructor, initializes the fields with the default values.
        /// Creates a new Attacment instance.
        /// </summary>
        /// <param name="discard" Parameter used because structs cannot have explicit parameterless constructors.
        public Attachment(Object discard)
        {
            comment = "";
            fileSize = "0";
            url = "";
            title = "";
            fileName = "";
            created = DateTime.Now;
            contentType = "";
            pageId = "";
            creator = "";
        }
    }
}