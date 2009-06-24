using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml;
using ContentFiltering.Test.Util;
using ContentFiltering.Office.Word.Filters;
using XWiki.Office.Word;

namespace ContentFiltering.Test.Office.Word.Filters
{
    /// <summary>
    /// Test class for <code>OfficeAttributesRemoverFilter</code>.
    /// </summary>
    [TestFixture]    
    public class OfficeAttributesRemoverFilterTest
    {
        private ConversionManager manager;
        private string initialHTML;
        private string expectedHTML;
        private XmlDocument initialXmlDoc;
        private XmlDocument expectedXmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public OfficeAttributesRemoverFilterTest()
        {
            manager = ConversionManagerTestUtil.DummyConversionManager();
            initialHTML = "";
            expectedHTML = "";
            initialXmlDoc = new XmlDocument();
            expectedXmlDoc = new XmlDocument();
        }

        /// <summary>
        /// Initialize the test.
        /// </summary>
        [TestFixtureSetUp]
        public void GlobalSetUp()
        {
            initialHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\""
                + " xmlns:o=\"urn:schemas-microsoft-com:office:office\""
                + " xmlns:w=\"urn:schemas-microsoft-com:office:word\""
                + " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\""
                + " xmlns=\"http://www.w3.org/TR/REC-html40\" >"
                + "<body>"
                + "<p o:p=\"content\">o content</p>"
                + "<p w:Std=\"content\">w content</p>"
                + "<img src=\"img1.png\" v:shapes=\"_x0000_s1026\" />"
                + "<p m:f=\"content\">m content</p>"
                + "</body></html>";

            expectedHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\" xmlns=\"http://www.w3.org/TR/REC-html40\">"
                + "<body>"
                + "<p>o content</p>"
                + "<p>w content</p>"
                + "<img src=\"img1.png\" />"
                + "<p>m content</p>"
                + "</body></html>";


            initialXmlDoc.LoadXml(initialHTML);
            expectedXmlDoc.LoadXml(expectedHTML);
        }

        /// <summary>
        /// Test method for office attributes remover filter.
        /// </summary>
        [Test]
        public void TestFilter()
        {
            new OfficeAttributesRemoverFilter(manager).Filter(ref initialXmlDoc);
            Assert.IsTrue(XmlDocComparator.AreIdentical(initialXmlDoc, expectedXmlDoc));
        }
    }
}
