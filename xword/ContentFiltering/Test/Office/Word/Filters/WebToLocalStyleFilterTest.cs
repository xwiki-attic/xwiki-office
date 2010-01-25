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
using XWiki.Office.Word;
using ContentFiltering.Test.Util;
using System.Xml;

namespace ContentFiltering.Test.Office.Word.Filters
{
    /// <summary>
    /// Test class for <code>WebToLocalStyleFilter</code>.
    /// </summary>
    [TestFixture]
    public class WebToLocalStyleFilterTest
    {
        private ConversionManager manager;
        private string initialHTML;
        private string expectedHTML;
        private XmlDocument initialXmlDoc;
        private XmlDocument expectedXmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WebToLocalStyleFilterTest()
        {
            manager = ConversionManagerTestUtil.DummyConversionManager();
            initialHTML = "";
            expectedHTML = "";
            initialXmlDoc = new XmlDocument();
            expectedXmlDoc = new XmlDocument();
        }

        /// <summary>
        /// Tests the Filter method from <code>WebToLocalStyleFilter</code>.
        /// </summary>
        [Test]
        public void TestFilter()
        {
            initialXmlDoc = new XmlDocument();
            expectedXmlDoc = new XmlDocument();

            initialHTML = "<html><head><title>TITLE</title></head>"
                + "<body>"
                + "<p class=\"xoffice0\">Text0</p>"
                + "<p><span id=\"id1\">Text1</span></p>"
                + "</body>"
                + "</html>";


            expectedHTML = "<html><head><title>TITLE</title></head>"
                + "<body>"
                
                //the CSS should be inlined, the classes for inlined CSS should be removed
                + "<p style=\"" + XWikiClientTestUtil.CSS_PROPERTIES_XOFFICE0 + "\">Text0</p>"
                + "<p><span id=\"id1\" style=\"" + XWikiClientTestUtil.CSS_PROPERTIES_ID1 + "\">Text1</span></p>"

                + "</body>"
                + "</html>";

            initialXmlDoc.LoadXml(initialHTML);
            expectedXmlDoc.LoadXml(expectedHTML);

            WebToLocalStyleFilter filter = new WebToLocalStyleFilter(manager);
            filter.Filter(ref initialXmlDoc);

            Assert.IsTrue(XmlDocComparator.AreIdentical(initialXmlDoc, expectedXmlDoc));
        }
    }
}
