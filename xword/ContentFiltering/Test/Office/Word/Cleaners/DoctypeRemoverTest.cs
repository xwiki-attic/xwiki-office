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
    /// Test class for <code>DoctypeRemover</code>.
    /// </summary>
    [TestFixture]
    public class DoctypeRemoverTest
    {
        private string initialHTML;
        private string expectedHTML;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DoctypeRemoverTest()
        {
            initialHTML = "";
            expectedHTML = "";
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\"> "
                + "<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en-US\" lang=\"en-US\"> "
                + "<body>Content</body>"
                + "</html>";

            expectedHTML = "<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en-US\" lang=\"en-US\"> "
                + "<body>Content</body>"
                + "</html>";
        }

        [Test]
        public void TestCleaner()
        {
            bool canLoadXML = false;
            IHTMLCleaner doctypeRemover = new DoctypeRemover();
            initialHTML = doctypeRemover.Clean(initialHTML);
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