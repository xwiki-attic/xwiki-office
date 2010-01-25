#region LGPL license
/*
 * See the NOTICE file distributed with this work for additional
 * information regarding copyright ownership.
 *
 * This is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this software; if not, write to the Free
 * Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
 * 02110-1301 USA, or see the FSF site: http://www.fsf.org.
 */
#endregion //license

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.XmlRpc;
using System.Xml;
using CookComputing.XmlRpc;
using XWiki.Office.Word;
using System.Collections.Specialized;
using XWiki.Clients;

namespace ContentFiltering.StyleSheetExtensions
{
    /// <summary>
    /// StyleSheet Extensions management class. 
    /// A SSXManager builded from cleaned HTML of a local editing page can be used to 
    /// add StyleSheetExtensions objects coresponding to the CSS of that page.
    /// A SSXManager builded from a remote wiki page can be used to get the CSS coresponding
    /// to the StyleSheetExtensions objects of that page.
    /// </summary>
    public class SSXManager
    {
        const string SSX_CLASS_NAME = "XWiki.StyleSheetExtension";
        const string XOFFICE_SSX = "XOfficeStyle";

        /// <summary>
        /// Creates a <code>SSXManager</code> from the cleaned HTML of a local editing page.
        /// </summary>
        /// <param name="pageConverter">An instance of the <code>ConversionManager</code> for this page.</param>
        /// <param name="cleanHTML">Cleaned HTML of the local page.</param>
        /// <returns>An instance of a <code>SSXManager</code>.</returns>
        public static SSXManager BuildFromLocalHTML(ConversionManager pageConverter, string cleanHTML)
        {
            SSXManager ssxManager = new SSXManager();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(cleanHTML);
            XmlNodeList styleNodes = xmlDoc.GetElementsByTagName("style");
            string pageCSSContent = "";

            foreach (XmlNode styleNode in styleNodes)
            {
                pageCSSContent += styleNode.InnerText;
            }

            XmlRpcStruct dictionary = new XmlRpcStruct();
            dictionary.Add("code", pageCSSContent);
            dictionary.Add("name", "XOfficeStyle");
            dictionary.Add("use", "currentPage");
            dictionary.Add("parse", "0");
            dictionary.Add("cache", "long");

            XWikiObject ssxObject = new XWikiObject();
            ssxObject.className = SSX_CLASS_NAME;
            ssxObject.pageId = pageConverter.States.PageFullName;
            ssxObject.objectDictionary = dictionary;

            ssxManager.pageFullName = pageConverter.States.PageFullName;
            ssxManager.pageCSSContent = pageCSSContent;
            ssxManager.pageStyleSheetExtensions = new List<XWikiObject>();
            ssxManager.pageStyleSheetExtensions.Add(ssxObject);
            ssxManager.pageConverter = pageConverter;
            return ssxManager;
        }

        /// <summary>
        /// Builds a <code>SSXManager</code> for a remote wiki page.
        /// </summary>
        /// <param name="pageConverter">An instance of the <code>ConversionManager</code> for this page.</param>
        /// <returns>An instance of a <code>SSXManager</code>.</returns>
        public static SSXManager BuildFromServerPage(ConversionManager pageConverter)
        {
            SSXManager ssxManager = new SSXManager();
            ssxManager.pageFullName = pageConverter.States.PageFullName;
            ssxManager.pageConverter = pageConverter;

            List<XWikiObject> ssxObjects = ssxManager.RetrieveStyleSheetExtensions();

            ssxManager.pageStyleSheetExtensions = ssxObjects;
            StringBuilder sb = new StringBuilder();
            foreach (XWikiObject ssxObject in ssxObjects)
            {
                sb.Append(ssxObject.objectDictionary["code"]);
                sb.Append(Environment.NewLine);
            }
            ssxManager.pageCSSContent = sb.ToString();

            return ssxManager;
        }


        private ConversionManager pageConverter;
        private string pageFullName;
        private List<XWikiObject> pageStyleSheetExtensions;
        private string pageCSSContent;


        /// <summary>
        /// An instance of SSXManager can be obtained only with its BuildFrom... methods.
        /// </summary>
        protected SSXManager()
        {
        }


        /// <summary>
        /// Gets the full name of the page.
        /// </summary>
        public string PageFullName
        {
            get { return pageFullName; }
        }

        /// <summary>
        /// Gets the CSS content for the page.
        /// </summary>
        public string PageCSSContent
        {
            get { return pageCSSContent; }
        }

        /// <summary>
        /// Gets a copy of the StyleSheetExtension XWikiObject list.
        /// </summary>
        public List<XWikiObject> PageStyleSheetExtensions
        {
            get { return pageStyleSheetExtensions.ToList(); }
        }


        /// <summary>
        /// Adds to server SSX objects for the current page.
        /// </summary>
        public void UploadStyleSheetExtensions()
        {
            IXWikiClient client = pageConverter.XWikiClient;
            int i = 0;
            foreach (XWikiObject ssxObject in pageStyleSheetExtensions)
            {
                NameValueCollection fieldsValues = new NameValueCollection();

                fieldsValues.Add("code", ssxObject.objectDictionary["code"].ToString());
                fieldsValues.Add("name", ssxObject.objectDictionary["name"].ToString());
                fieldsValues.Add("use", ssxObject.objectDictionary["use"].ToString());
                fieldsValues.Add("parse", ssxObject.objectDictionary["parse"].ToString());
                fieldsValues.Add("cache", ssxObject.objectDictionary["cache"].ToString());

                //remove existing XOffice style sheet extensions for current page
                List<XWikiObject> existingSSXObjects = RetrieveStyleSheetExtensions();
                foreach (XWikiObject existingSSX in existingSSXObjects)
                {
                    if (existingSSX.objectDictionary["name"] + "" == XOFFICE_SSX || existingSSX.prettyName == XOFFICE_SSX)
                    {
                        client.RemoveObject(pageFullName, SSX_CLASS_NAME, existingSSX.id);
                    }
                }

                client.AddObject(pageFullName, ssxObject.className, fieldsValues);
            }

        }

        /// <summary>
        /// Retrieve SSX object of the current page from the server.
        /// </summary>
        /// <returns>A list of <code>XWikiObject</code> containing SSX objects for the current page.</returns>
        protected List<XWikiObject> RetrieveStyleSheetExtensions()
        {
            XWikiObjectSummary[] objects = pageConverter.XWikiClient.GetObjects(pageFullName);
            List<XWikiObject> ssxObjects = new List<XWikiObject>();
            foreach (XWikiObjectSummary objSum in objects)
            {
                if (objSum.className == SSX_CLASS_NAME)
                {
                    XWikiObject obj = pageConverter.XWikiClient.GetObject(pageFullName, SSX_CLASS_NAME, objSum.id);
                    ssxObjects.Add(obj);
                }
            }

            return ssxObjects;
        }
    }
}
