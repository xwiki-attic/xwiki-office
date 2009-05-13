using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XWord
{
    /// <summary>
    /// Structure used for writing and retrieving data from Isolated Storage.
    /// </summary>
    [Serializable]
    public class RepositorySettings
    {
        private string pagesRepository;        
        private string downloadedAttachmentsRepository;        

        /// <summary>
        /// Creates a new instance of the class.
        /// <remarks>Used for serialization.</remarks>
        /// </summary>
        public RepositorySettings()
        {
            pagesRepository = Path.GetTempPath();
            downloadedAttachmentsRepository = Path.GetTempPath();
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
    }
}
