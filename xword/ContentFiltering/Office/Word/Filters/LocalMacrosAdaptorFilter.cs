using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XWiki.Office.Word;
using XWiki;

namespace ContentFiltering.Office.Word.Filters
{
    public class LocalMacrosAdaptorFilter:IDOMFilter
    {
        private ConversionManager manager;

        public LocalMacrosAdaptorFilter(ConversionManager manager)
        {
            this.manager = manager;
        }

        #region IDOMFilter Members

        /// <summary>
        /// Replaces the read-only Word content controls with XWiki macro markup.
        /// </summary>
        /// <param name="xmlDoc">A reference to the xml document instance.</param>
        public void Filter(ref XmlDocument xmlDoc)
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

        #endregion
    }
}
