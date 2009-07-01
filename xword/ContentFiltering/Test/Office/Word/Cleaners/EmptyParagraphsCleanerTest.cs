using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ContentFiltering.Office.Word.Cleaners;

namespace ContentFiltering.Test.Office.Word.Cleaners
{
    /// <summary>
    /// Test class for <code>EmptyParagraphsCleaner</code>.
    /// </summary>
    [TestFixture]
    public class EmptyParagraphsCleanerTest
    {
        private string initialHTML;
        private string expectedHTML;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EmptyParagraphsCleanerTest()
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
                + " xmlns=\"http://www.w3.org/TR/REC-html40\"> "
                + "<body><p>Text</p><p>&nbsp;</p><o:p></o:p></body></html>";

            expectedHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\""
                + " xmlns:o=\"urn:schemas-microsoft-com:office:office\""
                + " xmlns:w=\"urn:schemas-microsoft-com:office:word\""
                + " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\""
                + " xmlns=\"http://www.w3.org/TR/REC-html40\"> "
                + "<body><p>Text</p><br /><br /></body></html>";
        }

        [Test]
        public void TestCleaner()
        {
            initialHTML = new EmptyParagraphsCleaner().Clean(initialHTML);
            Assert.AreEqual(initialHTML, expectedHTML);
        }
    }
}