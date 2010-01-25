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
using ContentFiltering.Office.Word.Filters;
using System.Xml;
using XWiki.Office.Word;
using ContentFiltering.Test.Util;

namespace ContentFiltering.Test.Office.Word.Filters
{
    /// <summary>
    /// Test class for <code>LocalImageAdaptorFilter</code>.
    /// </summary>
    [TestFixture]
    public class LocalImageAdaptorFilterTest
    {
        private ConversionManager manager;
        private string initialHTML;
        private XmlDocument initialXmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocalImageAdaptorFilterTest()
        {
            manager = ConversionManagerTestUtil.DummyConversionManager();
            initialHTML = "";
            initialXmlDoc = new XmlDocument();
        }

        /// <summary>
        /// Initialize the test.
        /// </summary>
        [TestFixtureSetUp]
        public void GlobalSetUp()
        {
            initialHTML = "<html>"
                    + "<body>"
                    + "<h1>Heading 1</h1>"
                    + "<p><span>"
                    + "<img width=\"645\" height=\"459\" src=\"Document1_TempExport_files/image002.jpg\" alt=\"DevManager-server.png\" />"
                    + "</span></p>"
                    + "</body>"
                    + "</html>";

            initialXmlDoc.LoadXml(initialHTML);
        }

        /// <summary>
        /// Test method for local image adaptor filter.
        /// Verifies if images are 'bordered' with comments and image sources start with 'http'.
        /// </summary>
        [Test]
        public void TestFilter()
        {
            new LocalImageAdaptorFilter(manager).Filter(ref initialXmlDoc);
            XmlNodeList images=initialXmlDoc.GetElementsByTagName("img");
            foreach (XmlNode image in images)
            {
                Assert.AreEqual(image.PreviousSibling.NodeType, XmlNodeType.Comment);
                Assert.IsTrue(image.PreviousSibling.Value.IndexOf("startimage") >= 0);
                Assert.AreEqual(image.NextSibling.NodeType, XmlNodeType.Comment);
                Assert.IsTrue(image.NextSibling.Value.IndexOf("stopimage") >= 0);
                Assert.IsTrue(image.Attributes["src"].Value.IndexOf("http") >= 0);
            }
        }
    }
}
