using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;

namespace ContentFiltering.Html
{
    /// <summary>
    /// Provides static methods for working with CSS.
    /// </summary>
    public class CSSUtil
    {
        /// <summary>
        /// Inlines CSS for nodes with an id and/or class attribute.
        /// </summary>
        /// <param name="xmlDoc">A reference to an <code>XmlDocument</code>.</param>
        public static void InlineCSS(ref XmlDocument xmlDoc)
        {

            XmlNodeList allElements = xmlDoc.GetElementsByTagName("*");
            Hashtable identifiedCSSClassesAndIDs = ExtractCSSClassesAndIDs(ref xmlDoc);

            foreach (XmlNode element in allElements)
            {
                XmlAttribute classAttribute = element.Attributes["class"];
                XmlAttribute idAttribute = element.Attributes["id"];

                if (classAttribute != null)
                {
                    char[] whiteSpace = { ' ' };
                    string[] cssClassNames = ("" + classAttribute.Value).Split(whiteSpace, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string cssClassName in cssClassNames)
                    {
                        if (identifiedCSSClassesAndIDs.ContainsKey(cssClassName))
                        {
                            XmlAttribute styleAttribute = null;
                            styleAttribute = element.Attributes["style"];
                            if (styleAttribute == null)
                            {
                                styleAttribute = element.Attributes.Append(xmlDoc.CreateAttribute("style"));
                                styleAttribute.Value = "";
                            }
                            styleAttribute.Value += identifiedCSSClassesAndIDs[cssClassName];

                            //remove inlined CSS class
                            classAttribute.Value = classAttribute.Value.Replace(cssClassName, "").Trim();

                            if (classAttribute.Value.Length < 1)
                            {
                                element.Attributes.Remove(classAttribute);
                            }
                        }
                    }

                }

                if (idAttribute != null)
                {
                    string cssIdName = idAttribute.Value;
                    if (identifiedCSSClassesAndIDs.ContainsKey(cssIdName))
                    {
                        XmlAttribute styleAttribute = null;
                        styleAttribute = element.Attributes["style"];
                        if (styleAttribute == null)
                        {
                            styleAttribute = element.Attributes.Append(xmlDoc.CreateAttribute("style"));
                            styleAttribute.Value = "";
                        }
                        styleAttribute.Value += identifiedCSSClassesAndIDs[cssIdName];
                    }
                }
            }
        }

        /// <summary>
        /// Extracts and removes the CSS classes and ids from the 'style' nodes.
        /// Deletes all the style nodes and creates a new one with unparsed CSS.
        /// </summary>
        /// <param name="xmlDoc">A reference to an <code>XmlDocument</code>.</param>
        /// <returns>A hashtable with CSS classes names (and CSS ids names) and their properties.</returns>
        private static Hashtable ExtractCSSClassesAndIDs(ref XmlDocument xmlDoc)
        {
            XmlNodeList styleNodes = xmlDoc.GetElementsByTagName("style");
            StringBuilder preservedCSS = new StringBuilder();

            //extract CSS classes and ids
            Hashtable identifiedCSSClassesAndIDs = new Hashtable();
            foreach (XmlNode styleNode in styleNodes)
            {
                char[] separator = { '}' };
                string[] css = styleNode.InnerText.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string cssClass in css)
                {
                    //several CSS classes can have same properties
                    string properties = "";
                    List<string> classesNames = new List<string>();

                    int firstBrace = cssClass.IndexOf('{');
                    if (firstBrace < 0)
                    {
                        continue;
                    }
                    properties = cssClass.Substring(firstBrace + 1).Replace('"', '\'');

                    //clean whitespaces (spaces, tabs, new lines) from CSS properites
                    Regex whiteSpaceRegex = new Regex("\\s+", RegexOptions.Singleline | RegexOptions.Multiline);
                    properties = whiteSpaceRegex.Replace(properties, " ");

                    char[] comma = { ',' };
                    string[] cssNames = cssClass.Substring(0, firstBrace).Split(comma);
                    foreach (string className in cssNames)
                    {
                        string cname = className.Trim();
                        //only if it's a CSS class name or CSS id
                        //and it's not cascaded CSS
                        //and does not have pseudoselectors
                        if ((cname.IndexOf('.') == 0 || cname.IndexOf('#') == 0) && cname.IndexOf(':') < 0 && cname.IndexOf(' ') < 0)
                        {
                            //do not include the dot in the CSS class name or the pound in CSS id
                            classesNames.Add(cname.Substring(1));
                        }
                        else //since we can not handle that CSS, preserve it
                        {
                            preservedCSS.Append(cssClass).Append("}").Append(Environment.NewLine);
                        }
                    }

                    foreach (string identifiedClassName in classesNames)
                    {
                        string currentProperties = "";
                        if (identifiedCSSClassesAndIDs.Contains(identifiedClassName))
                        {
                            currentProperties = identifiedCSSClassesAndIDs[identifiedClassName].ToString();
                        }

                        currentProperties += properties;
                        identifiedCSSClassesAndIDs.Remove(identifiedClassName);
                        identifiedCSSClassesAndIDs.Add(identifiedClassName, currentProperties);
                    }
                }
            }
            //remove style nodes
            List<XmlNode> styleNodesToRemove = new List<XmlNode>();
            foreach (XmlNode styleNode in styleNodes)
            {
                styleNodesToRemove.Add(styleNode);
            }
            foreach (XmlNode styleNodeToRemove in styleNodesToRemove)
            {
                styleNodeToRemove.ParentNode.RemoveChild(styleNodeToRemove);
            }

            //only one style node, with the preserved CSS
            if (preservedCSS.ToString().Length > 0)
            {
                XmlNode remainingStyleNode = xmlDoc.CreateElement("style");
                remainingStyleNode.InnerText = preservedCSS.ToString().Trim();
                xmlDoc.GetElementsByTagName("head")[0].AppendChild(remainingStyleNode);
            }

            return identifiedCSSClassesAndIDs;
        }


        /// <summary>
        /// Groups CSS selectors (CSS classes and ids) with the same properties to minify the generated CSS content.
        /// </summary>
        public static Hashtable GroupCSSSelectors(Hashtable existingCSSSelectors)
        {
            Hashtable optimizedCSSSelectors = existingCSSSelectors;

            string cssPropsStr;
            string[] cssProperties;
            string[] separator = new string[1] { ";" };
            List<string> cssPropsList = new List<string>();

            //sort css properties from each selector in alphabetic order
            ICollection cssClassesKeys = optimizedCSSSelectors.Keys;

            //need an extra list in order to alter cssClassesKeys
            //(can not alter elements while iterate the collection)
            List<string> xofficeCssClasses = new List<string>();
            foreach (string key in cssClassesKeys)
            {
                xofficeCssClasses.Add(key);
            }

            foreach (string key in xofficeCssClasses)
            {
                cssPropsStr = ((string)optimizedCSSSelectors[key]).Replace('{', ' ').Replace('}', ' ').Trim();
                cssProperties = cssPropsStr.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                cssPropsList.Clear();
                foreach (string property in cssProperties)
                {
                    cssPropsList.Add(property.Trim() + ";");
                }
                cssPropsList.Sort();
                cssPropsStr = "{";
                foreach (string property in cssPropsList)
                {
                    cssPropsStr += property;
                }
                cssPropsStr += "}";
                optimizedCSSSelectors[key] = cssPropsStr;
            }

            //compare CSS selectors and group redundant ones
            //using an inverted hash of the optimizedCSSSelectors
            Hashtable invertedHash = new Hashtable(optimizedCSSSelectors.Count);
            foreach (string key in optimizedCSSSelectors.Keys)
            {
                string val = (string)optimizedCSSSelectors[key];
                string cssClass = "";
                if (invertedHash.ContainsKey(val))
                {
                    cssClass = (string)invertedHash[val];
                    cssClass += ", ";
                }
                cssClass += key;
                invertedHash[val] = cssClass;
            }
            optimizedCSSSelectors.Clear();
            foreach (string properties in invertedHash.Keys)
            {
                string groupedClasses = (string)invertedHash[properties];
                string props = properties.Replace('{', ' ').Replace('}', ' ').Trim();
                optimizedCSSSelectors.Add(groupedClasses, props);
            }

            return optimizedCSSSelectors;
        }


        /// <summary>
        /// Extracts inline styles and replaces them with CSS classes.
        /// </summary>
        /// <param name="xnode">Node to filter</param>
        /// <param name="xmlDoc">A reference to the XmlDocument.</param>
        /// <returns>Filtered node: 'class' attribute instead of 'style'.</returns>
        public static XmlNode ConvertInlineStylesToCssClasses(XmlNode xnode, ref XmlDocument xmlDoc, ref int counter, ref Hashtable cssClasses)
        {
            XmlNode node = xnode;
            if (node.ChildNodes.Count > 0)
            {
                RemoveXOfficeCSSClasses(ref node);
                ExtractStyle(ref node, ref xmlDoc, ref counter, ref cssClasses);
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    XmlNode childNode = node.ChildNodes[i];
                    childNode = ConvertInlineStylesToCssClasses(childNode, ref xmlDoc, ref counter, ref cssClasses);
                }
            }
            return node;
        }


        /// <summary>
        /// Extracts inline style from an <code>XmlNode</code> to a CSS class.
        /// Adds new CSS class to 'class' property of the node.
        /// </summary>
        /// <param name="node">A reference to the node to be filtered.</param>
        /// <param name="xmlDoc">A reference to the document containing the node.</param>
        private static void ExtractStyle(ref XmlNode node, ref XmlDocument xmlDoc, ref int counter, ref Hashtable cssClasses)
        {
            if (node.Attributes.Count > 0)
            {
                if (node.Attributes["style"] != null)
                {
                    if (node.ChildNodes.Count > 0 && ("" + node.Attributes["style"].Value).Length > 0)
                    {
                        string className = "xoffice" + counter;
                        string classValue = CSSUtil.CleanCSSProperties(node.Attributes["style"].Value);
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
        /// Removes previous XOffice CSS classes from an XmlNode.
        /// </summary>
        /// <param name="node">A reference to the XmlNode.</param>
        private static void RemoveXOfficeCSSClasses(ref XmlNode node)
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


        /// <summary>
        /// Cleans CSS properties by allowing only valid properties.
        /// </summary>
        /// <param name="style">Initial properties.</param>
        /// <returns>Cleaned properties.</returns>
        private static string CleanCSSProperties(string style)
        {
            StringBuilder acceptedProperties = new StringBuilder();

            string[] separator = new string[1];
            separator[0] = ";";
            string[] props = style.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string property in props)
            {
                string propName = property.Substring(0, property.IndexOf(':'));
                if (validCSSProperties.Contains(propName.ToLower().Trim()))
                {
                    acceptedProperties.Append(property);
                    acceptedProperties.Append(";");
                }
            }
            return acceptedProperties.ToString();
        }


        /// <summary>
        /// Common valid CSS properties.
        /// </summary>
        private static readonly List<string> validCSSProperties = new List<string>()
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

    }
}
