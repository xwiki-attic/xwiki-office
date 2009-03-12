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
        /// Adapts the 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
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
            AdaptMacros(ref xmlDoc);
            AdaptImages(ref xmlDoc);
            return xmlDoc.InnerXml;
        }

        /// <summary>
        /// Adapts the html source to convert XWiki macros to Word Content Controls.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml dom containing the source.</param>
        private void AdaptMacros(ref XmlDocument xmlDoc)
        {
            XmlNode node = xmlDoc;
            ReplaceMacros(ref node, ref xmlDoc);
        }

        /// <summary>
        /// Replaces the macros in a xml node with a Word content control tag.
        /// </summary>
        /// <param name="node">The xml node to be adapted.</param>
        /// <param name="xmlDoc">A refrence to the xml document.</param>
        private void ReplaceMacros(ref XmlNode node, ref XmlDocument xmlDoc)
        {
            int context = 0; //0 - outside macros, 1- inside macro.
            List<XmlNode> macroNodes = new List<XmlNode>();
            List<XmlNode> regularNodes = new List<XmlNode>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.NodeType == XmlNodeType.Comment)
                {
                    if (childNode.InnerText.StartsWith("startmacro"))
                    {
                        context = 1;
                    }
                    else if (childNode.InnerText.StartsWith("stopmacro"))
                    {
                        context = 0;
                    }
                }
                else if(childNode.NodeType != XmlNodeType.Document && childNode.NodeType != XmlNodeType.DocumentType )
                {
                    if (context == 0)
                    {
                        regularNodes.Add(childNode);
                    }
                    else
                    {
                        macroNodes.Add(childNode);
                    }
                }
            }
            if (macroNodes.Count > 0)
            {                
                try
                {
                    XmlNode element = GenerateContentControlNode(ref xmlDoc);
                    XmlNode parent = macroNodes[0].ParentNode;
                    parent.InsertBefore(element, macroNodes[0]);
                    foreach (XmlNode n in macroNodes)
                    {
                        XmlNode nn = parent.RemoveChild(n);
                        element.AppendChild(n);
                    }
                }
                catch (XmlException ex) { };
            }
            foreach (XmlNode n in regularNodes)
            {
                XmlNode clone = n.Clone();
                n.ParentNode.ReplaceChild(clone, n);
                ReplaceMacros(ref clone, ref xmlDoc);
            }
        }

        /// <summary>
        /// Generates a new node instance for the Word Content Control.
        /// </summary>
        /// <param name="xmlDoc">A refence to the xml document.</param>
        /// <returns>The instance of the new node.</returns>
        private XmlNode GenerateContentControlNode(ref XmlDocument xmlDoc)
        {
            //Initialize the node of the content control.
            XmlElement element = xmlDoc.CreateElement("w:Sdt", "urn:schemas-microsoft-com:office:word");
            XmlAttribute sdtLocked = xmlDoc.CreateAttribute("SdtLocked");
            sdtLocked.Value = "t";
            XmlAttribute contentLocked = xmlDoc.CreateAttribute("ContentLocked");
            contentLocked.Value = "t";
            XmlAttribute docPart = xmlDoc.CreateAttribute("DocPart");
            docPart.Value = "DefaultPlaceholder_22675703";
            Random random = new Random();
            XmlAttribute id = xmlDoc.CreateAttribute("ID");
            id.Value = random.Next(9000000, 9999999).ToString();
            element.Attributes.Append(sdtLocked);
            element.Attributes.Append(contentLocked);
            element.Attributes.Append(docPart);
            element.Attributes.Append(id);
            return element;
        }

        /// <summary>
        /// Adapts the html source returned by the XWiki server and makes it usable by Word using a local html file.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml dom.</param>
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

        /// <summary>
        /// Adapts lthe lists from the XWiki server to a format used by Word.
        /// </summary>
        /// <param name="xmlDoc"></param>
        private void AdaptLists(ref XmlDocument xmlDoc)
        {
            throw new NotImplementedException();
        }

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