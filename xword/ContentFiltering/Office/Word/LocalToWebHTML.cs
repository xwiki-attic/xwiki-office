using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using XWiki.Xml;
using System.Collections;

namespace XWiki.Office.Word
{
    public class LocalToWebHTML : AbstractConverter
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
            content = content.Replace("<o:p></o:p>", "<br />");
            content = content.Replace("<p>&nbsp;</p>", "<br />");
            content = content.Replace(">&nbsp;<", "><");
            content = content.Replace("<o:p>", "");
            content = content.Replace("</o:p>", "");
            content = content.Replace("&nbsp;", " ");
            xmlDoc.LoadXml(content);
            ClearStyles(ref xmlDoc);
            FilterGrammarAndSpellingErrors(ref xmlDoc);
            AdaptImages(ref xmlDoc);            
            AdaptLists(ref xmlDoc);
            AdaptMacros(ref xmlDoc);
            ClearOfficeAttributes(ref xmlDoc);
            StringBuilder sb = new StringBuilder(xmlDoc.GetIndentedXml());
            sb.Replace(" xmlns=\"\"","");
            return sb.ToString();
        }

        /// <summary>
        /// Removes 'class' attribute from text marked as  containing grammar or spelling errors.
        /// (when values are 'gramE' or 'spellE'). Removes 'lang' attribute. Adds a space character
        /// (' ') to the affected text, to make sure words marked as errors are separated.
        /// </summary>
        /// <param name="xmlDoc">A refrence to the xml document.</param>
        private void FilterGrammarAndSpellingErrors(ref XmlDocument xmlDoc)
        {
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("span");
            bool insertASpace = false;
            XmlNode tempNode = null;
            foreach (XmlNode node in nodes)
            {
                insertASpace = false;
                XmlAttribute classAttribute = node.Attributes["class"];
                if (classAttribute != null)
                {
                    if (classAttribute.Value.ToLower().Trim().IndexOf("grame") >= 0
                        ||
                        classAttribute.Value.ToLower().Trim().IndexOf("spelle") >= 0)                   
                    {
                        node.Attributes.Remove(classAttribute);
                        insertASpace = true;
                    }
                }
                XmlAttribute langAttribute = node.Attributes["lang"];
                if (langAttribute != null)
                {
                    node.Attributes.Remove(langAttribute);
                    insertASpace = true;
                }

                if (insertASpace)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        tempNode = node.ChildNodes[0];
                    }
                    else
                    {
                        tempNode = node;
                    }
                    if (tempNode.Value != null)
                    {
                        tempNode.Value += " ";
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the style attributes from the Word generated content
        /// </summary>
        /// <param name="xmlDoc">A refrence to the xml document.</param>
        private void ClearStyles(ref XmlDocument xmlDoc)
        {
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            XPathExpression expression = navigator.Compile("//@style");
            XPathNodeIterator xIterator = navigator.Select(expression);
            foreach (XPathNavigator nav in xIterator)
            {
                nav.DeleteSelf();                
            }
            expression = navigator.Compile("//@class");
            xIterator = navigator.Select(expression);
            foreach (XPathNavigator nav in xIterator)
            {
                if (nav.Value == "MsoNormal" || nav.Value == "MsoNormalTable" || nav.Value == "MsoTableGrid")
                {
                    nav.DeleteSelf();
                }
            }
            expression = navigator.Compile("//td[@valign]");
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("td");
            foreach (XmlNode node in nodes)
            {
                //XmlAttribute valign = node.Attributes["valign"];
                //node.Attributes.Remove(valign);
                node.Attributes.RemoveAll();
            }
        }

        /// <summary>
        /// Deletes all office specific attributes
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        private void ClearOfficeAttributes(ref XmlDocument xmlDoc)
        {
            XPathNavigator navigator = xmlDoc.CreateNavigator();
            XmlNamespaceManager nsMr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsMr.AddNamespace(String.Empty, "http://www.w3.org/1999/xhtml");
            nsMr.AddNamespace("v", "urn:schemas-microsoft-com:vml");
            nsMr.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
            nsMr.AddNamespace("w", "urn:schemas-microsoft-com:office:word");
            nsMr.AddNamespace("m", "http://schemas.microsoft.com/office/2004/12/omml");

            XPathExpression expression = navigator.Compile("//@v:* | //@o:* | //@w:* | //@m:*");
            XPathNodeIterator xIterator = navigator.Select(expression.Expression, nsMr);
            foreach (XPathNavigator nav in xIterator)
            {
                nav.DeleteSelf();
            }
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
        /// Replaces the read-only Word content controls with XWiki macro markup.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document instance.</param>
        private void AdaptMacros(ref XmlDocument xmlDoc)
        {
            XmlNodeList macroNodes = xmlDoc.GetElementsByTagName("Sdt", "urn:schemas-microsoft-com:office:word");
            XmlDocumentFragment docFrag = xmlDoc.CreateDocumentFragment();
            Dictionary<String, String> macros = this.manager.States.Macros;
            //We use a new list because the XmlNodeList will break when operationg with its' elements.
            List<XmlNode> nodeList = new List<XmlNode>();
            foreach (XmlNode node in macroNodes)
            {
                nodeList.Add(node);
            }
            foreach (XmlNode node in nodeList)
            {
                try
                {
                    String id = node.Attributes["ID"].Value;
                    if (macros.ContainsKey(id))
                    {
                        String content = macros[id];
                        docFrag.InnerXml = content;
                        node.ParentNode.ReplaceChild(docFrag, node);
                    }
                }
                catch (NullReferenceException nre)
                {
                    Log.Exception(nre);
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
            }
        }

        /// <summary>
        /// Adapts to the lists to a less styled format.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document instance.</param>
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
                foundExtraLists = RemoveExtraLists(ref xmlDoc);
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
            RemoveDivFromLists(ref xmlDoc, "ul");
            RemoveDivFromLists(ref xmlDoc, "ol");
            MoveChildListToTheirParent(ref xmlDoc);
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
                //A 'li' with no innerText but with 'ul' or 'ol' children should be moved up
                if (node.NodeType == XmlNodeType.Element && (""+node.Value).Length<1)
                {
                    if (node.ChildNodes[0].Name == "ul" || node.ChildNodes[0].Name == "ol")
                    {
                        XmlNode prevListItem = node.PreviousSibling;
                        //XmlNode subList = node.RemoveChild(node.FirstChild);
                        XmlNodeList children = node.ChildNodes;
                        foreach (XmlNode child in children)
                        {
                            prevListItem.AppendChild(child);
                        }
                        //prevListItem.AppendChild(subList);
                        foundExtraLists = true;
                    }
                }
            }
            return foundExtraLists;
        }

        /// <summary>
        /// Remove 'div' elements used for alignment and move their children in the correct position.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        /// <param name="listName">List type/name (like "ol", "ul").</param>
        private void RemoveDivFromLists(ref XmlDocument xmlDoc,string listName)
        {
            XmlNodeList lists = xmlDoc.GetElementsByTagName(listName);
            Dictionary<XmlNode, List<XmlNode>> childrenToMoveUp = new Dictionary<XmlNode, List<XmlNode>>();
            foreach (XmlNode node in lists)
            {
                if (node.Name.ToLower().Trim() == "div")
                {
                    XmlNode prevListItem = node.PreviousSibling;
                    XmlNode parent = node.ParentNode;
                    List<XmlNode> value = new List<XmlNode>();
                    if (prevListItem != null)
                    {
                        if (childrenToMoveUp.ContainsKey(prevListItem))
                        {
                            value = childrenToMoveUp[prevListItem];
                        }
                        value.Add(node);
                        childrenToMoveUp.Add(prevListItem, value);
                    }
                }
            }

            foreach (XmlNode key in childrenToMoveUp.Keys)
            {
                XmlNode prevListItem = (XmlNode)key;
                XmlNode parent = prevListItem.ParentNode;
                List<XmlNode> nodes = childrenToMoveUp[key];
                foreach (XmlNode node in nodes)
                {
                    //take all the children from this div and put them in the right position
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        parent.InsertAfter(child, prevListItem);
                    }
                    //remove the node
                    parent.RemoveChild(node);
                }
            }

        }

        /// <summary>
        /// Move orphan 'ul' and 'ol' elements to their coresponding 'li' parent.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        private void MoveChildListToTheirParent(ref XmlDocument xmlDoc)
        {
            MoveElementsUp(ref xmlDoc, "ul");
            MoveElementsUp(ref xmlDoc, "ol");
        }

        /// <summary>
        /// Move an element from an inner list to it's correct position.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document.</param>
        /// <param name="element">Element type/name (like "ol", "ul").</param>
        private void MoveElementsUp(ref XmlDocument xmlDoc, string element)
        {
            Dictionary<XmlNode, List<XmlNode>> childrenToMoveUp = new Dictionary<XmlNode, List<XmlNode>>();

            XmlNodeList items = xmlDoc.GetElementsByTagName(element.ToLower().Trim());
            foreach (XmlNode node in items)
            {
                XmlNode prevListItem = node.PreviousSibling;
                XmlNode parent = node.ParentNode;
                List<XmlNode> value = new List<XmlNode>();
                if (prevListItem != null)
                {
                    if (prevListItem.Name.ToLower().Trim() == "li")
                    {
                        if (childrenToMoveUp.ContainsKey(prevListItem))
                        {
                            value = childrenToMoveUp[prevListItem];
                        }
                        value.Add(node);
                        childrenToMoveUp.Add(prevListItem, value);
                    }
                }
            }
            foreach (XmlNode key in childrenToMoveUp.Keys)
            {
                XmlNode liParent = (XmlNode)key;
                XmlNode parent = liParent.ParentNode;
                List<XmlNode> nodes = childrenToMoveUp[key];
                foreach (XmlNode node in nodes)
                {
                    parent.RemoveChild(node);
                    liParent.AppendChild(node);
                }
            }
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
