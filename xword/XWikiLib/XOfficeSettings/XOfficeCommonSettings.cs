using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XWiki;

namespace XWord
{
    /// <summary>
    /// Structure used for writing and retrieving settings data from Isolated Storage.
    /// </summary>
    [Serializable]
    public class XOfficeCommonSettings
    {
        private string pagesRepository;        
        private string downloadedAttachmentsRepository;
        private XWikiClientType clientType;

        /// <summary>
        /// Creates a new instance of the class.
        /// <remarks>Used for serialization.</remarks>
        /// </summary>
        public XOfficeCommonSettings()
        {
            pagesRepository = Path.GetTempPath();
            downloadedAttachmentsRepository = Path.GetTempPath();
            clientType = XWikiClientType.XML_RPC;
        }

        /// <summary>
        /// Gets or sets the value for the pages repositoy.
        /// </summary>
        public string PagesRepository
        {
            get { return pagesRepository; }
            set { pagesRepository = value; }
        }

        /// <summary>
        /// Gets ot sets the value of the path where the downloaded attachments are stored.
        /// </summary>
        public string DownloadedAttachmentsRepository
        {
            get { return downloadedAttachmentsRepository; }
            set { downloadedAttachmentsRepository = value; }
        }

        /// <summary>
        /// Gets or sets the value for the connetivity type setting.
        /// </summary>
        public XWikiClientType ClientType
        {
            get { return clientType; }
            set { clientType = value; }
        }
    }
}
