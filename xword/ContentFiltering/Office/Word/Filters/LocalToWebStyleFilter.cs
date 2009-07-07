using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Office.Word;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;

namespace ContentFiltering.Office.Word.Filters
{
    /// <summary>
    /// Extracts the CSS inline styles, optimizes CSS and adds CSS classes in the head section for current page.
    /// </summary>
    public class LocalToWebStyleFilter : IDOMFilter
    {
        private int counter = 0;
        private Hashtable cssClasses;
        private ConversionManager manager;
        
        public LocalToWebStyleFilter(ConversionManager manager)
        {
            this.manager = manager;
            this.cssClasses = new Hashtable();
            this.counter = 0;
        }

        #region IDOMFilter Members

        /// <summary>
        /// Extracts the CSS inline styles, optimizes CSS and adds CSS classes in the head section for current page
        /// </summary>
        /// <param name="xmlDoc">A reference to a XmlDocument instance.</param>
        public void Filter(ref System.Xml.XmlDocument xmlDoc)
        {
            XmlNode body = xmlDoc.GetElementsByTagName("body")[0];
            XmlNode head = xmlDoc.GetElementsByTagName("head")[0];
            if (head == null)
            {
                head = xmlDoc.CreateNode(XmlNodeType.Element, "head", xmlDoc.NamespaceURI);
                body.ParentNode.InsertBefore(head, body);
            }
            ConvertCSSClassesToInlineStyles(ref head, ref body, ref xmlDoc);
            body = ConvertInlineStylesToCssClasses(body, ref xmlDoc);
            OptimizeCssClasses();
            InsertCssClassesInHeader(ref head, ref xmlDoc);
        }

        #endregion IDOMFilter Members


        private void ConvertCSSClassesToInlineStyles(ref XmlNode head, ref XmlNode body, ref XmlDocument xmlDoc)
        {
            //TODO: XOFFICE-125
        }

        /// <summary>
        /// Extracts inline styles and replaces them with CSS classes.
        /// </summary>
        /// <param name="xnode">Node to filter</param>
        /// <param name="xmlDoc">A reference to the XmlDocument.</param>
        /// <returns>Filtered node: 'class' attribute instead of 'style'.</returns>
        private XmlNode ConvertInlineStylesToCssClasses(XmlNode xnode, ref XmlDocument xmlDoc)
        {
            XmlNode node = xnode;
            if (node.ChildNodes.Count > 0)
            {
                RemoveXOfficeCSSClasses(ref node);
                ExtractStyle(ref node, ref xmlDoc);
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    XmlNode childNode = node.ChildNodes[i];
                    childNode = ConvertInlineStylesToCssClasses(childNode, ref xmlDoc);
                }
            }
            return node;
        }

        /// <summary>
        /// Removes previous XOffice CSS classes.
        /// </summary>
        /// <param name="node">A reference to an XmlNode to filter.</param>
        private void RemoveXOfficeCSSClasses(ref XmlNode node)
        {
            XmlAttribute classAttribute = node.Attributes["class"];
            if (classAttribute != null)
            {
                if (classAttribute.Value.IndexOf("xoffice") >= 0)
                {
                    classAttribute.Value = Regex.Replace(classAttribute.Value, "xoffice[0-9]+", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                }
                node.Attributes.Remove(node.Attributes["class"]);
                if (("" + classAttribute.Value).Length > 0)
                {
                    node.Attributes.Append(classAttribute);
                }
            }
        }

        private void ExtractStyle(ref XmlNode node, ref XmlDocument xmlDoc)
        {
            if (node.Attributes.Count > 0)
            {
                if (node.Attributes["style"] != null)
                {
                    if (node.ChildNodes.Count > 0 && ("" + node.Attributes["style"].Value).Length > 0)
                    {
                        string className = "xoffice" + counter;
                        string classValue = CleanCSSProperties(node.Attributes["style"].Value);
                        if (classValue.Length > 0)
                        {

                            cssClasses.Add("." + className, classValue);
                            node.Attributes.Remove(node.Attributes["style"]);
                            XmlAttribute classAttribute = node.Attributes["class"];
                            if (classAttribute == null)
                            {
                                classAttribute = xmlDoc.CreateAttribute("class");
                            }
                            else
                            {
                                classAttribute.Value += " ";
                            }
                            classAttribute.Value += className;
                            node.Attributes.Remove(node.Attributes["class"]);
                            node.Attributes.Append(classAttribute);
                            counter++;

                        }
                    }
                    else
                    {
                        //An empty node, so delete it's attributes 
                        //This way the node could be safely removed by other DOM filters
                        node.Attributes.RemoveAll();
                    }
                }
            }
        }

        /// <summary>
        /// Cleans CSS properties by removing the ones specific to MS Office.
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private string CleanCSSProperties(string style)
        {
            StringBuilder acceptedProperties = new StringBuilder();

            string[] separator = new string[1];
            separator[0] = ";";
            string[] props = style.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string property in props)
            {
                string propName = property.Substring(0, property.IndexOf(':'));
                if (ValidCSSProperties.GetList().Contains(propName.ToLower().Trim()))
                {
                    acceptedProperties.Append(property);
                    acceptedProperties.Append(";");
                }
            }
            return acceptedProperties.ToString();
        }

        
        private void OptimizeCssClasses()
        {
            //TODO: XOFFICE-126
        }

        private void InsertCssClassesInHeader(ref XmlNode headNode, ref XmlDocument xmlDoc)
        {
            XmlNode styleNode = xmlDoc.CreateNode(XmlNodeType.Element, "style", xmlDoc.NamespaceURI);

            string value = "";

            foreach (Object key in cssClasses.Keys)
            {
                value += Environment.NewLine;
                value += (string)key;
                value += "{";
                value += cssClasses[key];
                value += "}";
            }

            XmlNode styleNodeContent = xmlDoc.CreateNode(XmlNodeType.Text, "#text", xmlDoc.NamespaceURI);
            styleNodeContent.Value = value;

            styleNode.AppendChild(styleNodeContent);
            headNode.AppendChild(styleNode);
        }
    }

    /// <summary>
    /// Most common valid CSS properties.
    /// </summary>
    /// TODO: move to application settings mechanism?
    class ValidCSSProperties
    {
        private static List<string> validCSSProperties = new List<string>()
        {
            "accelerator", "azimuth", "background", "background-attachment", "background-color", "background-image", 
            "background-position", "background-position-x", "background-position-y", "background-repeat", "behavior", "border", 
            "border-bottom", "border-bottom-color", "border-bottom-style", "border-bottom-width", "border-collapse", "border-color", 
            "border-left", "border-left-color", "border-left-style", "border-left-width", "border-right", "border-right-color", 
            "border-right-style", "border-right-width", "border-spacing", "border-style", "border-top", "border-top-color", 
            "border-top-style", "border-top-width", "border-width", "bottom", "caption-side", "clear", 
            "clip", "color", "content", "counter-increment", "counter-reset", "cue", 
            "cue-after", "cue-before", "cursor", "direction", "display", "elevation", 
            "empty-cells", "filter", "float", "font", "font-family", "font-size", 
            "font-size-adjust", "font-stretch", "font-style", "font-variant", "font-weight", "height", 
            "ime-mode", "include-source", "layer-background-color", "layer-background-image", "layout-flow", "layout-grid", 
            "layout-grid-char", "layout-grid-char-spacing", "layout-grid-line", "layout-grid-mode", "layout-grid-type", "left", 
            "letter-spacing", "line-break", "line-height", "list-style", "list-style-image", "list-style-position", 
            "list-style-type", "margin", "margin-bottom", "margin-left", "margin-right", "margin-top", 
            "marker-offset", "marks", "max-height", "max-width", "min-height", "min-width", 
            "orphans", "outline", "outline-color", "outline-style", "outline-width", "overflow", 
            "overflow-X", "overflow-Y", "padding", "padding-bottom", "padding-left", "padding-right", 
            "padding-top", "page", "page-break-after", "page-break-before", "page-break-inside", "pause", 
            "pause-after", "pause-before", "pitch", "pitch-range", "play-during", "position", 
            "size", "table-layout", "text-align", "text-decoration", "text-indent", "text-transform", 
            "text-shadow", "top", "vertical-align", "visibility", "white-space", "width", 
            "word-break", "word-spacing", "z-index"
        };

        /// <summary>
        /// Gets the list of valid CSS properties.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetList()
        {
            return validCSSProperties;
        }
    }
}
