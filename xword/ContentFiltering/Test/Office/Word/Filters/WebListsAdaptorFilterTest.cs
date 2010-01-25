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
    /// Test class for <code>WebListsAdaptorFilter</code>.
    /// </summary>
    [TestFixture]
    public class WebListsAdaptorFilterTest
    {
        private ConversionManager manager;
        private string initialHTML;
        private string expectedHTML;
        private XmlDocument initialXmlDoc;
        private XmlDocument expectedXmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WebListsAdaptorFilterTest()
        {
            manager=ConversionManagerTestUtil.DummyConversionManager();
            initialHTML="";
            expectedHTML="";
            initialXmlDoc=new XmlDocument();
            expectedXmlDoc=new XmlDocument();
        }

        /// <summary>
        /// Test setup.
        /// </summary>
        [TestFixtureSetUp]
        public void GlobalSetup()
        {
            initialHTML = "<html xmlns=\"http://www.w3.org/1999/xhtml\" lang=\"en\" xml:lang=\"en\">"
                    + "<head><title>Some Lists</title></head>"
                    + "<body id=\"body\" class=\"xwiki viewbody hideleft\">"
                    + "<p>Unordered list:</p>"
                    + "<ul>"
                    + "<li>Item 1</li>"
                    + "<li>Item2<ul>"
                    + "    <li>Item 2.1</li>"
                    + "    <li>Item 2.2<ul>"
                    + "        <li>Item 2.2.1</li>"
                    + "        <li>Item 2.2.2</li>"
                    + "        </ul>"
                    + "    </li>"
                    + "    <li>Item 2.3</li>"
                    + "    </ul>"
                    + "</li>"
                    + "<li>Item 3</li>"
                    + "</ul>"
                    + "<p>Ordered list:</p>"
                    + "<ol>"
                    + "<li>Item 1<ol>"
                    + "    <li>Item 1.1</li>"
                    + "    <li>Item 1.2</li>"
                    + "    </ol>"
                    + "</li>"
                    + "<li>Item 2</li>"
                    + "</ol>"
                    + "<p>End of file :-)</p>"
                    + "</body>"
                    + "</html>";

            expectedHTML = "<html xmlns=\"http://www.w3.org/1999/xhtml\" lang=\"en\" xml:lang=\"en\">"
                    + "<head><title>Some Lists</title></head>"
                    + "<body id=\"body\" class=\"xwiki viewbody hideleft\">"
                    + "<p>Unordered list:</p>"
                    + "<ul>"
                    + "<li>Item 1</li>"
                    + "<li>Item2</li>"
                    + "<ul>"
                    + "    <li>Item 2.1</li>"
                    + "    <li>Item 2.2</li>"
                    + "    <ul>"
                    + "        <li>Item 2.2.1</li>"
                    + "        <li>Item 2.2.2</li>"
                    + "    </ul>"
                    + "    <li>Item 2.3</li>"
                    + "</ul>"
                    + "<li>Item 3</li>"
                    + "</ul>"
                    + "<p>Ordered list:</p>"
                    + "<ol>"
                    + "<li>Item 1</li>"
                    + "<ol>"
                    + "   <li>Item 1.1</li>"
                    + "   <li>Item 1.2</li>"
                    + "</ol>"
                    + "<li>Item 2</li>"
                    + "</ol>"
                    + "<p>End of file :-)</p>"
                    + "</body>"
                    + "</html>";

            initialXmlDoc.LoadXml(initialHTML);
            
            expectedXmlDoc.LoadXml(expectedHTML);
        }

        /// <summary>
        /// Test method for web lists adaptor filter.
        /// </summary>
        [Test]
        public void TestFilter()
        {
            WebListsAdaptorFilter webListsAdaptorFilter = new WebListsAdaptorFilter(manager);
            webListsAdaptorFilter.Filter(ref initialXmlDoc);
            Assert.IsTrue(XmlDocComparator.AreIdentical(initialXmlDoc, expectedXmlDoc));
        }
    }
}
