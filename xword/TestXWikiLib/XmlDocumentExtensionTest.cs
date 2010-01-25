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
using System.Xml;
using XWiki.Xml;
using NUnit.Framework;

namespace TestContentFiltering
{
    /// <summary>
    /// Test class for XmlDocument extensions.
    /// </summary>
    [TestFixture]
    public class XmlDocumentExtensionTest
    {
        private XmlDocument xmlDoc;
        private string html;
        private string expectedHtml;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public XmlDocumentExtensionTest()
        {
        }

        /// <summary>
        /// Initialize the test.
        /// </summary>
        [TestFixtureSetUp]
        public void GlobalSetUp()
        {
            xmlDoc = new XmlDocument();
            html = "<html><head><title>Page Title</title></head>"
                + "<body>\n<h1 align=\"center\">Document Title</h1>"
                + "<p>Some content goes here</p>"
                + "<p><b>Related <u>links</u>:</b><br/>"
                + "<a href=\"http://www.example.com\">Example.com</a></p>"
                + "</body></html>";
            expectedHtml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + Environment.NewLine +
                "<html>" + Environment.NewLine +
                "  <head>" + Environment.NewLine +
                "    <title>Page Title</title>" + Environment.NewLine +
                "  </head>" + Environment.NewLine +
                "  <body>" + Environment.NewLine +
                "    <h1 align=\"center\">Document Title</h1>" + Environment.NewLine +
                "    <p>Some content goes here</p>" + Environment.NewLine +
                "    <p>" + Environment.NewLine +
                "      <b>Related <u>links</u>:</b>" + Environment.NewLine +
                "      <br />" + Environment.NewLine +
                "      <a href=\"http://www.example.com\">Example.com</a>" + Environment.NewLine +
                "    </p>" + Environment.NewLine +
                "  </body>" + Environment.NewLine +
                "</html>";
        }

        /// <summary>
        /// Test method for GetIndendedXml() extension method.
        /// </summary>
        [Test]
        public void TestGetIndentedXml()
        {
            string indentedHtml;
            xmlDoc.LoadXml(html);
            indentedHtml = xmlDoc.GetIndentedXml();
            Assert.AreEqual(indentedHtml, expectedHtml);
        }
    }
}
