using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace XWiki.Office.Word
{
    public class BidirectionalConversionStates
    {
        /// <summary>
        /// Creates an instance of the conversion states class.
        /// </summary>
        /// <param name="serverURL">The URL of the server.</param>
        /// <param name="pageFullName">The full name of the wiki page that is being edited.</param>
        public BidirectionalConversionStates(String serverURL)
        {
            this.serverURL = serverURL;
            images = new Dictionary<Guid, ImageInfo>();
            links = new List<object>();
            styles = new List<object>();
        }

        /// <summary>
        /// The action that is currently performed by the converter.
        /// </summary>
        private ConverterActionState currentActionState;

        /// <summary>
        /// Gets the curent action.
        /// </summary>
        public ConverterActionState CurrentActionState
        {
            get { return currentActionState; }
        }

        /// <summary>
        /// Sets the current action.
        /// </summary>
        /// <param name="value">The new action.</param>
        internal void SetActionState(ConverterActionState value)
        {
            currentActionState = value;
        }

        /// <summary>
        /// The URL of the server.
        /// </summary>
        private String serverURL;

        /// <summary>
        /// Gets or sets the URL of the Server.
        /// </summary>
        public String ServerURL
        {
            get { return serverURL; }
            set { serverURL = value; } 
        }

        /// <summary>
        /// The full name of the page that is currently edited.
        /// </summary>
        private String pageFullName;

        public String PageFullName
        {
            get { return pageFullName; }
            set { pageFullName = value; }
        }

        /// <summary>
        /// The name of the local folder where the page is saved.
        /// </summary>
        private String localFolder;

        public String LocalFolder
        {
            get { return localFolder; }
            set { localFolder = value; }
        }

        /// <summary>
        /// Gets or sets the name of the local html file.
        /// </summary>
        private String localFileName;

        public String LocalFileName
        {
            get { return localFileName; }
            set { localFileName = value; }
        }

        /// <summary>
        /// A list of the images that are in the html document.
        /// </summary>
        private Dictionary<Guid,ImageInfo> images;

        internal Dictionary<Guid, ImageInfo> Images
        {
            get { return images; }
        }

        /// <summary>
        /// True only if web page to local file conversion is in progress.
        /// </summary>
        public bool IsDownloading
        {
            get { return (currentActionState == ConverterActionState.Downloading); }
        }

        /// <summary>
        /// True only if local file to web page conversion is in progress.
        /// </summary>
        public bool IsUploading
        {
            get { return (currentActionState == ConverterActionState.Uploading); }
        }

        /// <summary>
        /// Not yet implemented.
        /// </summary>
        #region notImplemeted
        private List<Object> links;
        private List<Object> styles;
        private List<Object> metaData;
        #endregion

    }
}
