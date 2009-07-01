using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ContentFiltering.Office.Word.Cleaners;

namespace ContentFiltering.Test.Office.Word.Cleaners
{
    /// <summary>
    /// Test class for <code>BodyContentExtractor</code>.
    /// </summary>
    [TestFixture]
    public class BodyContentExtractorTest
    {
        private string initialHTML;
        private string expectedHTML;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BodyContentExtractorTest()
        {
            initialHTML = "";
            expectedHTML = "";
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML = "<html><head><body style=\"color:blue\"><h1>Header 1</h1><p>Body Content goes here</p></body></html>";
            expectedHTML = "<h1>Header 1</h1><p>Body Content goes here</p>";
        }

        [Test]
        public void TestCleaner()
        {
            IHTMLCleaner bodyContentExctractor = new BodyContentExtractor();
            initialHTML = bodyContentExctractor.Clean(initialHTML);
            Assert.AreEqual(initialHTML, expectedHTML);
        }
    }
}