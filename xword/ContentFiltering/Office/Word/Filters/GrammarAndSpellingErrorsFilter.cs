using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XWiki.Office.Word;
namespace ContentFiltering.Office.Word.Filters
{
    public class GrammarAndSpellingErrorsFilter:IDOMFilter
    {
        private ConversionManager manager;

        public GrammarAndSpellingErrorsFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        #region IDOMFilter Members
        
        /// <summary>
        /// Removes 'class' attribute from text marked as  containing grammar or spelling errors.
        /// (when values are 'gramE' or 'spellE'). Removes 'lang' attribute. Adds a space character
        /// (' ') to the affected text, to make sure words marked as errors are separated.
        /// </summary>
        /// <param name="xmlDoc">A reference to an xml document.</param>
        public void Filter(ref XmlDocument xmlDoc)
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

        #endregion
    }
}
