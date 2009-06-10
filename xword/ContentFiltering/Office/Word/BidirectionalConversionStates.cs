using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace XWiki.Office.Word
{
    /// <summary>
    /// Contains the states of the bidirectional conversion(Wiki->Word, Word->Wiki)
    /// </summary>
    public class BidirectionalConversionStates
    {
        /// <summary>
        /// Creates an instance of the conversion states class.
        /// </summary>
        /// <param name="serverURL">The URL of the server.</param>
        public BidirectionalConversionStates(String serverURL)
        {
            this.serverURL = serverURL;
            images = new Dictionary<Guid, ImageInfo>();
            macros = new Dictionary<string, string>();
            links = new List<object>();
            styles = new List<object>();
        }

        /// <summary>
        /// Dictionary containig the macros in the converted page.
        /// Key - the id of the WordML(RichTextBox content control) element replacing the macro.
        /// Value - the macro in web html form.
        /// </summary>
        private Dictionary<String, String> macros;

        /// <summary>
        /// Gets  the page macros collection.
        /// Dictionary containig the macros in the converted page.
        /// Key - the id of the WordML(RichTextBox content control) element replacing the macro.
        /// Value - the macro in web html form.
        /// </summary>
        public Dictionary<String, String> Macros
        {
            get { return macros; }
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

        /// <summary>
        /// Gets or sets the full name of the edited page.
        /// </summary>
        public String PageFullName
        {
            get { return pageFullName; }
            set { pageFullName = value; }
        }

        /// <summary>
        /// The name of the local folder where the page is saved.
        /// </summary>
        private String localFolder;

        /// <summary>
        /// Gets or sets the name of the folder where the local file is stored.
        /// </summary>
        public String LocalFolder
        {
            get { return localFolder; }
            set { localFolder = value; }
        }

        /// <summary>
        /// Gets or sets the name of the local html file.
        /// </summary>
        private String localFileName;

        /// <summary>
        /// Gets or sets the name of the local file.
        /// </summary>
        public String LocalFileName
        {
            get { return localFileName; }
            set { localFileName = value; }
        }

        /// <summary>
        /// A list of the images that are in the html document.
        /// </summary>
        private Dictionary<Guid,ImageInfo> images;

        /// <summary>
        /// Dictionary containing the data of the images from both web and local versions of the document.
        /// </summary>
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
