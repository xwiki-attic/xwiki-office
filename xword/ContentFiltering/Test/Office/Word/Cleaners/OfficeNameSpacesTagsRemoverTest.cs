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
    /// Test class for <code>OfficeNameSpacesTagsRemover</code>.
    /// </summary>
    [TestFixture]
    public class OfficeNameSpacesTagsRemoverTest
    {
        private string initialHTML;
        private string expectedHTML;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public OfficeNameSpacesTagsRemoverTest()
        {
            initialHTML = "";
            expectedHTML = "";
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\""
            + " xmlns:o=\"urn:schemas-microsoft-com:office:office\""
            + " xmlns:w=\"urn:schemas-microsoft-com:office:word\""
            + " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\""
            + " xmlns=\"http://www.w3.org/TR/REC-html40\">" 
            + "<body><p>Text</p><o:p>&nbsp;</o:p></body></html>";
            
            expectedHTML="<html xmlns:v=\"urn:schemas-microsoft-com:vml\""
            + " xmlns:o=\"urn:schemas-microsoft-com:office:office\""
            + " xmlns:w=\"urn:schemas-microsoft-com:office:word\""
            + " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\""
            + " xmlns=\"http://www.w3.org/TR/REC-html40\">"
            + "<body><p>Text</p></body></html>";
        }

        [Test]
        public void TestCleaner()
        {
            bool canLoadXML = false;
            IHTMLCleaner officeNameSpaceCleaner = new OfficeNameSpacesTagsRemover();
            initialHTML = officeNameSpaceCleaner.Clean(initialHTML);
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