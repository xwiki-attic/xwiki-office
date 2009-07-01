using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ContentFiltering.Office.Word.Cleaners;

namespace ContentFiltering.Test.Office.Word.Cleaners
{
    /// <summary>
    /// Test class for <code>CommentsRemover</code>.
    /// </summary>
    [TestFixture]
    public class CommentsRemoverTest
    {
        private string initialHTML;
        private string expectedHTML;

        public CommentsRemoverTest()
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
                + "<head>"
                + "<!--[if gte mso 9]><xml>"
                + " <o:OfficeDocumentSettings>"
                + "   <o:AllowPNG/>"
                + "   <o:PixelsPerInch>120</o:PixelsPerInch>"
                + "   <o:TargetScreenSize>1024x768</o:TargetScreenSize>"
                + "  </o:OfficeDocumentSettings>"
                + "</xml><![endif]-->"
                + "</head>"
                + "<body>"
                + "<!-- the comment -->"
                + "<p>the paragraph</p>"
                + "</body>"
                + "</html>";

            expectedHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\""
                + " xmlns:o=\"urn:schemas-microsoft-com:office:office\""
                + " xmlns:w=\"urn:schemas-microsoft-com:office:word\""
                + " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\""
                + " xmlns=\"http://www.w3.org/TR/REC-html40\"> "
                + "<head>"
                + "</head>"
                + "<body>"
                + "<p>the paragraph</p>"
                + "</body>"
                + "</html>";
        }

        [Test]
        public void TestCleaner()
        {
            initialHTML = new CommentsRemover().Clean(initialHTML);
            Assert.AreEqual(initialHTML, expectedHTML);
        }
    }
}
