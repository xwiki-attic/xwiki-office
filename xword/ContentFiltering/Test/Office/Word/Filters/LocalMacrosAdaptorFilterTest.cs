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
    /// Test class for <code>LocalMacrosAdaptorFilter</code>.
    /// </summary>
    [TestFixture]
    public class LocalMacrosAdaptorFilterTest
    {
        private ConversionManager manager;
        private string initialHTML;
        private string expectedHTML;
        private XmlDocument initialXmlDoc;
        private XmlDocument expectedXmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocalMacrosAdaptorFilterTest()
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
            //'known' macros
            manager.States.Macros.Add("9225601", "generated html content");

            initialHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\" xmlns=\"http://www.w3.org/1999/xhtml\">"
                    + "<body>"
                    + "<h1>Heading 1</h1>"
                    + "<p>Normal text</p>"
                    + "<w:Sdt SdtLocked=\"t\" ContentLocked=\"t\" ID=\"9225601\">"
                    + "<p>&lt;img border=\"0\" src=\"<span class=\"nobr\"><a href=\"http://opi.yahoo.com/online?u=yahoohelper&amp;amp;m=g&amp;amp;t=1\">"
                    + "          http://opi.yahoo.com/online?u=yahoohelper&amp;amp;m=g&amp;amp;t=1</a></span>\" alt=\"yahoo yahoohelper\" /&gt;"
                    + "<span><w:sdtPr></w:sdtPr></span>"
                    + "</p>"
                    + "</w:Sdt>"
                    + "<w:Sdt DocPart=\"DefaultPlaceholder_22675703\" Text=\"t\" ID=\"15075750\"><p>Some text goes here"
                    + "<w:sdtPr></w:sdtPr></p></w:Sdt><p>More text goes here.</p>"
                    + "</body>"
                    + "</html>";

            expectedHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\" xmlns=\"http://www.w3.org/1999/xhtml\">"
                    + "<body>"
                    + "<h1>Heading 1</h1>"
                    + "<p>Normal text</p>"
                    + "generated html content"
                    + "<w:Sdt DocPart=\"DefaultPlaceholder_22675703\" Text=\"t\" ID=\"15075750\"><p>Some text goes here"
                    + "<w:sdtPr></w:sdtPr></p></w:Sdt><p>More text goes here.</p>"
                    + "</body>"
                    + "</html>";

            initialXmlDoc = new XmlDocument();
            expectedXmlDoc = new XmlDocument();

            initialXmlDoc.LoadXml(initialHTML);
            expectedXmlDoc.LoadXml(expectedHTML);
        }

        /// <summary>
        /// Test method for local macros adaptor filter.
        /// Content controls which represent XWiki macros should be tranformed (in macros)
        /// and content controls which belong to Word should not be altered.
        /// </summary>
        [Test]
        public void TestFilter()
        {
            new LocalMacrosAdaptorFilter(manager).Filter(ref initialXmlDoc);
            XmlNodeList stds = initialXmlDoc.GetElementsByTagName("Sdt", "urn:schemas-microsoft-com:office:word");
            
            bool foundElement9225601 = false;
            bool foundElement15075750 = false;
            
            foreach (XmlNode std in stds)
            {
                if (std.Attributes["ID"] != null)
                {
                    if (std.Attributes["ID"].Value == "9225601")
                    {
                        foundElement9225601 = true;
                    }
                    if (std.Attributes["ID"].Value == "15075750")
                    {
                        foundElement15075750 = true;
                    }
                }
            }

            Assert.IsTrue(foundElement15075750);
            Assert.IsFalse(foundElement9225601);

            Assert.IsTrue(initialXmlDoc.InnerText.ToLower().IndexOf("generated html content") >= 0);
        }
    }
}
