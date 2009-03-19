using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace XWiki.Office.Word
{
    class LocalToWebHTML : AbstractConverter
    {

        public LocalToWebHTML(ConversionManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Adapts the HTML source from it's local MS Word form in order to be used by the wiki.
        /// </summary>
        /// <param name="content">The initial HTML source.</param>
        /// <returns>The adapted HTML code.</returns>
        public String AdaptSource(String content)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.XmlResolver = null;
            String uncleanedContent = htmlUtil.CorrectAttributes(content);
            uncleanedContent = htmlUtil.CorrectTagsClosing(uncleanedContent, "img");
            uncleanedContent = htmlUtil.CorrectTagsClosing(uncleanedContent, "br");
            content = htmlUtil.CleanHTML(uncleanedContent, true);
            if (content.Length == 0)
            {
                content = uncleanedContent;
            }
            //content = htmlUtil.RemoveOfficeNameSpacesTags(content);
            //content = htmlUtil.ReplaceBody(content, "<body>");
            content = htmlUtil.ReplaceXmlNamespaceDefinitions(content, HTML_OPENING_TAG);
            content = content.Replace('·','o');
            content = content.Replace('§', 'o');//"·"; "o"; "§";
            //Removing &nbsp; from Word and Tidy output
            content = content.Replace("<p>&nbsp;</p>", "<br />");
            content = content.Replace(">&nbsp;<", "><");
            xmlDoc.LoadXml(content);
            AdaptImages(ref xmlDoc);            
            AdaptLists(ref xmlDoc);            
            return xmlDoc.InnerXml;
        }

        /// <summary>
        /// Adapts the images from the local(file:///) to the xwiki format.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        private void AdaptImages(ref XmlDocument xmlDoc)
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

        /// <summary>
        /// Adapts to the lists to a less styled format.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml filtered document instance.</param>
        private void AdaptLists(ref XmlDocument xmlDoc)
        {
            XmlNodeList listItems = xmlDoc.GetElementsByTagName("li");
            //Remove the extra paragraph from list items.
            foreach (XmlNode node in listItems)
            {
                if (node.NodeType == XmlNodeType.Element && node.FirstChild.NodeType == XmlNodeType.Element)
                {
                    if (node.FirstChild.Name == "p")
                    {
                        node.InnerXml = node.FirstChild.InnerXml;
                    }                    
                }                
            }
            bool foundExtraLists = false;
            do
            {
                //foundExtraLists = RemoveExtraLists(ref xmlDoc);
            } while (foundExtraLists);
            //Remove attributes from list declarations.
            XmlNodeList lists = xmlDoc.GetElementsByTagName("ul");
            foreach (XmlNode node in lists)
            {
                node.Attributes.RemoveAll();
            }
            lists = xmlDoc.GetElementsByTagName("ol");
            foreach (XmlNode node in lists)
            {
                node.Attributes.RemoveAll();
            }
        }

        /// <summary>
        /// Removes the extra lists Word creates for sublists.
        /// The child 'ul' is moved to the previous sibling.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        private bool RemoveExtraLists(ref XmlDocument xmlDoc)
        {
            bool foundExtraLists = false;
            XmlNodeList listItems = xmlDoc.GetElementsByTagName("li");
            foreach (XmlNode node in listItems)
            {
                if (node.NodeType == XmlNodeType.Element && node.ChildNodes.Count == 1)
                {
                    if (node.ChildNodes[0].Name == "ul" || node.ChildNodes[0].Name == "ol")
                    {
                        XmlNode prevListItem = node.PreviousSibling;
                        XmlNode subList = node.RemoveChild(node.FirstChild);
                        prevListItem.AppendChild(subList);
                        foundExtraLists = true;
                    }
                }
            }
            return foundExtraLists;
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
        /// Adds comments before and after image tags.
        /// </summary>
        /// <param name="xmlDoc">A reference to the filtered XmlDocument instance.</param>
        private void BorderImages(ref XmlDocument xmlDoc)
        {
            foreach(XmlNode node in xmlDoc.GetElementsByTagName("img"))
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
    }
}
