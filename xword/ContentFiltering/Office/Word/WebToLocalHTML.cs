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
using System.Linq;
using System.IO;
using System.Net;
using System.Threading;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using XWiki.Clients;
using XWiki.Html;
using XWiki.Xml;
using System.Collections;
using ContentFiltering.Office.Word.Filters;
using ContentFiltering.Office.Word.Cleaners;

namespace XWiki.Office.Word
{
    /// <summary>
    /// Class used to package the image information between threads.
    /// </summary>
    class ImageDownloadInfo
    {
        String _URI;
        /// <summary>
        /// The URI of the image.
        /// </summary>
        public String URI
        {
            get { return _URI; }
            set { _URI = value; }
        }
        /// <summary>
        /// The folder where the local image is located.
        /// </summary>
        String downloadFolder;

        private ImageInfo imageInfo;
        /// <summary>
        /// Gets or sets the states of the XWord image.
        /// </summary>
        public ImageInfo ImageInfo
        {
            get { return imageInfo; }
            set { imageInfo = value; }
        }

        /// <summary>
        /// The folder where the image is downloaded to.
        /// </summary>
        public String DownloadFolder
        {
            get { return downloadFolder; }
            set { downloadFolder = value; }
        }

        /// <summary>
        /// Creates a new instance for the ImageInfo class.
        /// </summary>
        /// <param name="URI">The URI of the image.</param>
        /// <param name="downloadFolder">The folder where the image will be downloaded.</param>
        /// <param name="imageInfo">The states of the image on the server and on the local file system.</param>
        public ImageDownloadInfo(String URI, String downloadFolder, ImageInfo imageInfo)
        {
            this._URI = URI;
            this.downloadFolder = downloadFolder;
            this.ImageInfo = imageInfo;
        }
    }

    /// <summary>
    /// Adapts the html source returned by the XWiki server and makes it usable by Word using a local html file.
    /// </summary>
    class WebToLocalHTML : AbstractConverter
    {
        private const string IMAGE_TAG = "<img";
        private const string IMAGE_SOURCE_ATTRIBUTE = "src=\"";

        /// <summary>
        /// Creates a new instance of the WebToLocalHTML class.
        /// </summary>
        /// <param name="manager">The instance of the bidirectional conversion manager.</param>
        public WebToLocalHTML(ConversionManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Adapts the Word generated content to XWiki friendly content.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public String AdaptSource(String content)
        {
            XmlDocument xmlDoc = new XmlDocument();
            content = new WebToLocalHTMLCleaner(HTML_OPENING_TAG).Clean(content);

            //content = content.Insert(0, DOCTYPE);
            try
            {
                xmlDoc.LoadXml(content);
            }
            catch (XmlException ex)
            {
                Log.Exception(ex);
                return "Sorry, a problem appeared when loading the page";
            }
            

            List<IDOMFilter> webToLocalFilters = new List<IDOMFilter>()
            {
                new WebMacrosAdaptorFilter(manager),
                new WebImageAdaptorFilter(manager),
                new WebListsAdaptorFilter(manager),
                new WebToLocalStyleFilter(manager)
            };

            foreach (IDOMFilter webToLocalFilter in webToLocalFilters)
            {
                webToLocalFilter.Filter(ref xmlDoc);
            }

            return xmlDoc.GetIndentedXml();
        }

    }
}