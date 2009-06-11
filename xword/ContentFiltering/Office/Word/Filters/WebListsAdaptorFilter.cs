using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Office.Word;
using System.Xml;

namespace ContentFiltering.Office.Word.Filters
{
    public class WebListsAdaptorFilter:IDOMFilter
    {
        private ConversionManager manager;

        public WebListsAdaptorFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        #region IDOMFilter Members

        /// <summary>
        /// Adapts the HTML lists to lists known by MS Word, because Word doesn't like
        /// 'ul' inside 'li' elements with innerText.
        /// </summary>
        /// <param name="xmlDoc">A reference to an xml document.</param>
        public void Filter(ref XmlDocument xmlDoc)
        {
            Dictionary<XmlNode, List<XmlNode>> itemsToMoveUp = new Dictionary<XmlNode, List<XmlNode>>();
            XmlNodeList listItems = xmlDoc.GetElementsByTagName("li");

            //itentify <li> elements with <ul> children
            foreach (XmlNode node in listItems)
            {
                XmlNodeList children = node.ChildNodes;
                //only nodes with both text and other xml elements
                if (("" + node.Value).Length < 1)
                {
                    continue;
                }

                foreach (XmlNode child in children)
                {
                    if (child.Name.ToLower().Trim() == "ul" || child.Name.ToLower().Trim() == "ol")
                    {
                        List<XmlNode> value = new List<XmlNode>();

                        if (itemsToMoveUp.ContainsKey(node))
                        {
                            value = itemsToMoveUp[node];
                        }
                        value.Add(child);
                        itemsToMoveUp.Add(node, value);
                    }
                }
            }

            //move <ul> elements one level up if they are inside <li> elements with no innerText
            foreach (XmlNode node in itemsToMoveUp.Keys)
            {
                foreach (XmlNode child in itemsToMoveUp[node])
                {
                    XmlNode n = node.RemoveChild(child);
                    node.ParentNode.InsertAfter(n, node);
                }
            }
        }

        #endregion
    }
}
