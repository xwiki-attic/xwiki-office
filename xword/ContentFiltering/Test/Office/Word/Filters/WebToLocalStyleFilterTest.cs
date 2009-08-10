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


            expectedHTML = "<html><head><title>TITLE</title>"

                //the 'style' node should be inserted in the head section
                + "<style>"

                //the CSS from SSX objects should be inserted in the style node
                + XWikiClientTestUtil.CSS_CONTENT_XOFFICE0
                + XWikiClientTestUtil.CSS_CONTENT_ID1
                + "</style></head>"
                + "<body>"

                //the CSS should be inlined
                + "<p class=\"xoffice0\" style=\"" + XWikiClientTestUtil.CSS_PROPERTIES_XOFFICE0 + "\">Text0</p>"
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
