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
using NUnit.Framework;
using XWiki.Office.Word;
using System.Xml;
using ContentFiltering.Test.Util;
using ContentFiltering.Office.Word.Filters;

namespace ContentFiltering.Test.Office.Word.Filters
{
    /// <summary>
    /// Test class for <code>WebMacrosAdaptorFilter</code>.
    /// </summary>
    [TestFixture]
    public class WebMacrosAdaptorFilterTest
    {
        private ConversionManager manager;
        private string initialHTML;
        private string expectedHTML;
        private XmlDocument initialXmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WebMacrosAdaptorFilterTest()
        {
            manager = ConversionManagerTestUtil.DummyConversionManager();
            initialHTML = "";
            expectedHTML = "";
            initialXmlDoc = new XmlDocument();
        }

        /// <summary>
        /// Global test setup.
        /// </summary>
        [TestFixtureSetUp]
        public void GlobalSetup()
        {
            initialHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\" xmlns=\"http://www.w3.org/1999/xhtml\">"
                + "<body>"
                + "<p>Normal text.</p>"
                + "<!--startmacro:velocity|-||-|#yahoo(\"yahoohelper\")-->"
                + "<p>generated html</p>"
                + "<!--stopmacro-->"
                + "</body></html>";

            expectedHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\" xmlns=\"http://www.w3.org/1999/xhtml\">"
                + "<body>"
                + "<p>Normal text.</p>"
                + "<w:Sdt SdtLocked=\"t\" ContentLocked=\"t\" DocPart=\"DefaultPlaceholder_9128480\" ID=\"9320493\">"
                + "<!--startmacro:velocity|-||-|#yahoo(\"yahoohelper\")-->"
                + "<p>generated html</p>"
                + "<!--stopmacro-->"
                + "</w:Sdt>"
                + "</body></html>";

            initialXmlDoc.LoadXml(initialHTML);
        }

        /// <summary>
        /// Test method for web macros adaptor filter. After filtering the XmlDocument should have one content control
        /// (w:Std) with 'ContentLocked', 'DocPart', 'ID' attributes and 'startmacro','stopmacro' and macro generated html
        /// as child nodes.
        /// </summary>
        [Test]
        public void TestFilter()
        {
            WebMacrosAdaptorFilter webmacrosAdaptorFilter = new WebMacrosAdaptorFilter(manager);
            webmacrosAdaptorFilter.Filter(ref initialXmlDoc);

            XmlNodeList wStds = initialXmlDoc.GetElementsByTagName("Sdt", "urn:schemas-microsoft-com:office:word");
            Assert.AreEqual(wStds.Count, 1);
            XmlNode wStd = wStds[0];
            Assert.AreEqual(wStd.Attributes["ContentLocked"].Value, "t");
            Assert.IsTrue(wStd.Attributes["DocPart"].Value.IndexOf("DefaultPlaceholder_") >= 0);
            Assert.IsNotNull(wStd.Attributes["ID"]);
            XmlNodeList wSdChildNodes = wStd.ChildNodes;

            bool foundStartMacro = false;
            bool foundGeneratedHtml = false;
            bool foundStopMacro = false;

            foreach (XmlNode child in wSdChildNodes)
            {
                if (child.NodeType == XmlNodeType.Comment)
                {
                    if (child.InnerText.IndexOf("startmacro") >= 0)
                    {
                        foundStartMacro = true;
                    }
                    if (child.InnerText.IndexOf("stopmacro") >= 0)
                    {
                        foundStopMacro = true;
                    }
                }
                else
                {
                    if (child.Name == "p")
                    {
                        if (child.InnerText == "generated html")
                        {
                            foundGeneratedHtml = true;
                        }
                    }
                }
            }

            Assert.IsTrue(foundStartMacro);
            Assert.IsTrue(foundGeneratedHtml);
            Assert.IsTrue(foundStopMacro);
            
        }
    }
}
