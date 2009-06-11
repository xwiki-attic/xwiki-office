using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XWiki.Office.Word;
using System.IO;

namespace ContentFiltering.Office.Word.Filters
{
    public class LocalImageAdaptorFilter:IDOMFilter
    {
        private ConversionManager manager;

        public LocalImageAdaptorFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        #region IDOMFilter Members

        /// <summary>
        /// Adapts the images from the local(file:///) to the xwiki format.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        public void Filter(ref XmlDocument xmlDoc)
        {
            XmlNodeList images = xmlDoc.GetElementsByTagName("img");
            List<String> adaptedSrcs = new List<String>();
            foreach (XmlNode node in images)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    String imagePath = node.Attributes["src"].Value;
                    if (!adaptedSrcs.Contains(imagePath))
                    {
                        String newPath = "";
                        List<Guid> imgIds = GetMatchingImages(node);
                        if (imgIds.Count != 0)
                        {
                            ImageInfo imageInfo = manager.States.Images[imgIds[0]];
                            newPath = imageInfo.imgWebSrc;
                        }
                        else
                        {
                            //set src and upload
                            String attachmentName = Path.GetFileName(imagePath);
                            manager.States.LocalFolder = manager.States.LocalFolder.Replace("\\\\", "\\");
                            if (!Path.IsPathRooted(imagePath))
                            {
                                imagePath = Path.Combine(manager.States.LocalFolder, imagePath);
                            }
                            bool sucess = manager.XWikiClient.AddAttachment(manager.States.PageFullName, imagePath);
                            //TODO report if the attachment cannot be loaded.
                            newPath = manager.XWikiClient.GetAttachmentURL(manager.States.PageFullName, attachmentName);
                        }
                        node.Attributes["src"].Value = newPath;
                        adaptedSrcs.Add(newPath);
                    }
                }
            }
            BorderImages(ref xmlDoc);
        }

        #endregion

        /// <summary>
        /// Adds comments before and after image tags.
        /// </summary>
        /// <param name="xmlDoc">A reference to the filtered XmlDocument instance.</param>
        private void BorderImages(ref XmlDocument xmlDoc)
        {
            foreach (XmlNode node in xmlDoc.GetElementsByTagName("img"))
            {
                String imageName = node.Attributes["src"].Value;
                imageName = Path.GetFileName(imageName);
                XmlNode startComment = xmlDoc.CreateComment("startimage:" + imageName);
                XmlNode endComment = xmlDoc.CreateComment("stopimage");
                XmlNode parent = node.ParentNode;
                parent.InsertBefore(startComment, node);
                parent.InsertAfter(endComment, node);
            }
        }

        /// <summary>
        /// Gets a list with the GUIDs assigned assigned to image across the html source.
        /// </summary>
        /// <param name="node">Teh </param>
        /// <returns></returns>
        private List<Guid> GetMatchingImages(XmlNode node)
        {
            List<Guid> imgIds = new List<Guid>();
            XmlAttribute srcAttr = node.Attributes["src"];
            if (srcAttr == null)
            {
                return imgIds;
            }
            foreach (KeyValuePair<Guid, ImageInfo> pair in manager.States.Images)
            {
                String firstLocalPath = pair.Value.imgLocalSrc.Replace("\\", "/");
                String currentLocalPath = srcAttr.Value.Replace("\\", "/");
                if (firstLocalPath.Contains(currentLocalPath)) //
                {
                    imgIds.Add(pair.Key);
                }
            }
            return imgIds;
        }

        /// <summary>
        /// Specifies if an image is new or has been modified.
        /// </summary>
        /// <param name="node">The XML node(element) that contains the image tag</param>
        /// <returns>Returns true is the image is new or has been modified. Otherwise returns false.</returns>
        private bool IsImageDirty(XmlNode node)
        {
            //Tests if the image was added in Word
            if (HasXWordId(node))
            {
                return true;
            }
            else
            {
                //Gets the unique identifier for the image
                String imgId = node.Attributes[ImageInfo.XWORD_IMG_ATTRIBUTE].Value;
                Guid imgGuid;
                try
                {
                    imgGuid = new Guid(imgId);
                }
                catch (Exception)
                {
                    return true;
                }
                ImageInfo imageInfo = manager.States.Images[imgGuid];
                //Verifies if the image was modified.
                String src = node.Attributes["src"].Value;
                if (!Path.IsPathRooted(src))
                {
                    src = Path.Combine(manager.States.LocalFolder, src);
                }
                FileInfo fileInfo = new FileInfo(src);
                if ((fileInfo.CreationTime == imageInfo.fileCreationDate) && (fileInfo.FullName == imageInfo.filePath))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Specifies if an image has an XWordId 
        /// </summary>
        /// <param name="node">The currently processed</param>
        /// <returns>True if the note has a xword attribute, false if not.</returns>
        private bool HasXWordId(XmlNode node)
        {
            XmlAttribute idAttr = node.Attributes[ImageInfo.XWORD_IMG_ATTRIBUTE];
            if (idAttr == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
