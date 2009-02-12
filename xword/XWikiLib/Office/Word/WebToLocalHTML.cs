using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Threading;
using System.Text;
using System.Xml;
using XWiki.Clients;
using XWiki.Html;

namespace XWiki.Office.Word
{
    class ImageDownloadInfo
    {
        String _URI;

        public String URI
        {
            get { return _URI; }
            set { _URI = value; }
        }
        String downloadFolder;

        private ImageInfo imageInfo;

        public ImageInfo ImageInfo
        {
            get { return imageInfo; }
            set { imageInfo = value; }
        }

        public String DownloadFolder
        {
            get { return downloadFolder; }
            set { downloadFolder = value; }
        }
        public ImageDownloadInfo(String URI, String downloadFolder, ImageInfo imageInfo)
        {
            this._URI = URI;
            this.downloadFolder = downloadFolder;
            this.ImageInfo = imageInfo;
        }
    }

    class WebToLocalHTML : AbstractConverter
    {
        private const string IMAGE_TAG = "<img";
        private const string IMAGE_SOURCE_ATTRIBUTE = "src=\"";

        public WebToLocalHTML(ConversionManager manager)
        {
            this.manager = manager;
        }
        public String AdaptSource(String content)
        {
            XmlDocument xmlDoc = new XmlDocument();
            content = htmlUtil.RemoveOfficeNameSpacesTags(content);
            //String namespaces = htmlUtil.GetXmlNamespaceDefinitions(content);
            content = htmlUtil.CleanHTML(content, false);
            content = htmlUtil.ReplaceXmlNamespaceDefinitions(content, HTML_OPENING_TAG);
            //content = content.Insert(0, DOCTYPE);
            try
            {
                xmlDoc.LoadXml(content);
            }
            catch (XmlException)
            {
                System.Windows.Forms.MessageBox.Show("Sorry the page you requested seems to have an invalid html source", "XWord");
                return "Sorry, a problem appeared when loading the page";
            }
            AdaptImages(ref xmlDoc);
            return xmlDoc.InnerXml;
        }

        private void AdaptImages(ref XmlDocument xmlDoc)
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
                        imgURL = ServerURL + imgURL;
                    }
                    ParameterizedThreadStart pts = new ParameterizedThreadStart(DownloadImage);
                    String folder = LocalFolder + "\\" + LocalFilename + "_Files";
                    Object param = new ImageDownloadInfo(imgURL, folder, imgInfo);
                    pts.Invoke(param);
                    imgURL = folder + "\\" + Path.GetFileName(imgURL);
                    imgURL = "file:///" + imgURL.Replace("\\", "/");
                    node.Attributes["src"].Value = imgURL;
                }
            }
        }

        private void AdaptLists(ref XmlDocument xmlDoc)
        {

        }

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
                idi.ImageInfo.imgLocalSrc = "file:///" + fileInfo.FullName.Replace("\\","/");
                idi.ImageInfo.fileURI = URI;
                idi.ImageInfo.fileSize = fileInfo.Length;
                idi.ImageInfo.fileCreationDate = fileInfo.CreationTime;
            }
            catch (InvalidCastException) { }
            catch (WebException) { };
        }
    }
}