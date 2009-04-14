using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWiki
{
    /// <summary>
    /// Contains a list with the XWiki Word services url's.
    /// </summary>
    public class XWikiURLs
    {
        static String loginURL = "/xwiki/bin/loginsubmit/XWiki/XWikiLogin";
        static String savePageURL = "/xwiki/bin/view/MSOffice/SavePageService";
        static String getPageURL = "/xwiki/bin/view/MSOffice/GetPageService";
        static String wikiStructureURL = "/xwiki/bin/view/MSOffice/StructureService?xpage=plain";
        static String attachmentServiceURL = "/xwiki/bin/view/MSOffice/AttachmentService?xpage=plain";
        static String protectedPagesURL = "/xwiki/bin/view/MSOffice/ProtectedPages?xpage=plain";
        static String getEncoding = "/xwiki/bin/view/MSOffice/GetEncodingService?xpage=plain";

        /// <summary>
        /// Gets or sets the URL of the service that handles attachments.
        /// </summary>
        public static String AttachmentServiceURL
        {
            get { return XWikiURLs.attachmentServiceURL; }
            set { XWikiURLs.attachmentServiceURL = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the service/page that handles authentication.
        /// </summary>
        public static String LoginURL
        {
            get { return loginURL; }
            set { loginURL = value; }
        }

        /// <summary>
        /// Gets the URL of the service that handles page editing and saving.
        /// </summary>
        public static String SavePageURL
        {
            get { return savePageURL; }
            set { savePageURL = value; }
        }

        /// <summary>
        /// Gets the URL of the service that provides page content and rendering.
        /// </summary>
        public static String GetPageURL
        {
            get { return getPageURL; }
            set { getPageURL = value; }
        }

        /// <summary>
        /// Gets the URL of the service that provides access to the structure of the wiki.
        /// </summary>
        public static String WikiStructureURL
        {
            get { return wikiStructureURL; }
            set { wikiStructureURL = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the page that contains the list of protected wiki pages.
        /// This wiki pages contain scripts that provide functionality to the wiki and cannot be edited
        /// with Word without having macro support.
        /// </summary>
        public static String ProtectedPagesURL
        {
            get { return protectedPagesURL; }
            set { protectedPagesURL = value; }
        }

        /// <summary>
        /// Gets the encoding of the wiki.
        /// </summary>
        public static String GetEncoding
        {
            get { return getEncoding; }
            set { getEncoding = value; }
        }
    }
}
