using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

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
            XmlNodeList styleNodes = xmlDoc.GetElementsByTagName("style");
            XmlNodeList allElements = xmlDoc.GetElementsByTagName("*");
            Hashtable identifiedCSSClassesAndIDs = ExtractCSSClassesAndIDs(styleNodes);

            foreach (XmlNode element in allElements)
            {
                XmlAttribute classAttribute = element.Attributes["class"];
                XmlAttribute idAttribute = element.Attributes["id"];

                if (classAttribute != null)
                {
                    string cssClassName = classAttribute.Value;

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
        /// Extracts the CSS classes and ids from the 'style' nodes.
        /// </summary>
        /// <param name="styleNodes">A list of style nodes from the document.</param>
        /// <returns>A hashtable with CSS classes names (and CSS ids names) and their properties.</returns>
        private static Hashtable ExtractCSSClassesAndIDs(XmlNodeList styleNodes)
        {
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
                    char[] comma = { ',' };
                    string[] cssNames = cssClass.Substring(0, firstBrace).Split(comma);
                    foreach (string className in cssNames)
                    {
                        //do not include the dot in the CSS class name or the pound in CSS id
                        classesNames.Add(className.Trim().Substring(1));
                    }

                    foreach (string identifiedClassName in classesNames)
                    {
                        identifiedCSSClassesAndIDs.Add(identifiedClassName, properties);
                    }
                }
            }
            return identifiedCSSClassesAndIDs;
        }



    }
}
