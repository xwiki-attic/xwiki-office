using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Office.Word;
using System.Xml;
using System.Threading;
using System.IO;
using System.Net;

namespace ContentFiltering.Office.Word.Filters
{
    public class WebImageAdaptorFilter:IDOMFilter
    {
        private ConversionManager manager;
        private string serverURL;
        private string localFolder;
        private string localFilename;

        public WebImageAdaptorFilter(ConversionManager manager)
        {
            this.manager = manager;
            serverURL = manager.States.ServerURL;
            localFolder = manager.States.LocalFolder;
            localFilename = manager.States.LocalFileName; 
        }

        #region IDOMFilter Members
        /// <summary>
        /// Adapts the html source returned by the XWiki server and makes it usable by Word using a local html file.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml dom.</param>
        public void Filter(ref XmlDocument xmlDoc)
        {
            XmlNodeList images = xmlDoc.GetElementsByTagName("img");
            foreach (XmlNode node in images)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    XmlAttribute vshapesAttr = node.Attributes["v:shapes"];
                    if (vshapesAttr != null)
                    {
                        node.Attributes.Remove(vshapesAttr);
                    }
                    //Creating an additional attribute to help identifing the image in the html.
                    String src = node.Attributes["src"].Value;
                    XmlAttribute attr = xmlDoc.CreateAttribute(ImageInfo.XWORD_IMG_ATTRIBUTE);
                    //Adding the attribute to the xhtml code.
                    Guid imgId = Guid.NewGuid();
                    attr.Value = imgId.ToString();
                    node.Attributes.Append(attr);
                    //Adding the image to the current image list.
                    ImageInfo imgInfo = new ImageInfo();
                    imgInfo.imgWebSrc = src;
                    if (node.Attributes["alt"] != null)
                    {
                        imgInfo.altText = node.Attributes["alt"].Value;
                    }
                    manager.States.Images.Add(imgId, imgInfo);
                    //Downloading image
                    String imgURL = node.Attributes["src"].Value;
                    if (imgURL == "") continue;
                    if (imgURL[0] == '/')
                    {
                        imgURL = serverURL + imgURL;
                    }
                    ParameterizedThreadStart pts = new ParameterizedThreadStart(DownloadImage);
                    String folder = localFolder + "\\" + localFilename + "_Files";
                    Object param = new ImageDownloadInfo(imgURL, folder, imgInfo);
                    pts.Invoke(param);
                    imgURL = folder + "\\" + Path.GetFileName(imgURL);
                    imgURL = "file:///" + imgURL.Replace("\\", "/");
                    node.Attributes["src"].Value = imgURL;
                }
            }
        }

        #endregion

        /// <summary>
        /// Downloads the image from the server and saves it to a local file.
        /// </summary>
        /// <param name="obj">The image data. Instance of ImageDownloadInfo used in cross thread data sharing.</param>
        private void DownloadImage(Object obj)
        {
            try
            {
                ImageDownloadInfo idi = (ImageDownloadInfo)obj;
                String targetFolder = idi.DownloadFolder;
                String URI = idi.URI;
                WebClient webClient = new WebClient();
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }
                String path = targetFolder + "\\" + Path.GetFileName(URI);
                FileInfo fileInfo = new FileInfo(path);
                byte[] binaryContent = webClient.DownloadData(URI);
                FileStream fileStream = fileInfo.Create();
                fileStream.Write(binaryContent, 0, binaryContent.Length);
                fileStream.Close();
                //Set the image element properties in the converters imageList.
                idi.ImageInfo.filePath = fileInfo.FullName;
                idi.ImageInfo.imgLocalSrc = "file:///" + fileInfo.FullName.Replace("\\", "/");
                idi.ImageInfo.fileURI = URI;
                idi.ImageInfo.fileSize = fileInfo.Length;
                idi.ImageInfo.fileCreationDate = fileInfo.CreationTime;
            }
            catch (InvalidCastException) { }
            catch (WebException) { };
        }
    }
}
