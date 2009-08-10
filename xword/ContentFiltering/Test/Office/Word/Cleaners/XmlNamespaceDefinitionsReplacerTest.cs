using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ContentFiltering.Office.Word.Cleaners;
using System.Xml;

namespace ContentFiltering.Test.Office.Word.Cleaners
{
    /// <summary>
    /// Test class for <code>XmlNamespaceDefinitionsReplacer</code>.
    /// </summary>
    [TestFixture]
    public class XmlNamespaceDefinitionsReplacerTest
    {
        private string initialHTML;
        private string expectedHTML;
        private string newTag;

        public XmlNamespaceDefinitionsReplacerTest()
        {
            initialHTML = "";
            expectedHTML = "";
            newTag = "";
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML = "<html><body><p>Content</p></body></html>";
            expectedHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\""
                        + " xmlns:o=\"urn:schemas-microsoft-com:office:office\""
                        + " xmlns:w=\"urn:schemas-microsoft-com:office:word\""
                        + " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\""
                        + " xmlns=\"http://www.w3.org/1999/xhtml\">"
                        + "<body><p>Content</p></body></html>";
            newTag = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\""
                        + " xmlns:o=\"urn:schemas-microsoft-com:office:office\""
                        + " xmlns:w=\"urn:schemas-microsoft-com:office:word\""
                        + " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\""
                        + " xmlns=\"http://www.w3.org/1999/xhtml\">";
        }


        [Test]
        public void TestCleaner()
        {
            bool canLoadXML = false;
            initialHTML = new XmlNamespaceDefinitionsReplacer(newTag).Clean(initialHTML);
            Assert.AreEqual(initialHTML, expectedHTML);
            try
            {
                new XmlDocument().LoadXml(initialHTML);
                canLoadXML = true;
            }
            catch
            {
                canLoadXML = false;
            }
            Assert.IsTrue(canLoadXML);

        }


    }
}