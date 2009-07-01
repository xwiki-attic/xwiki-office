using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ContentFiltering.Office.Word.Cleaners;

namespace ContentFiltering.Test.Office.Word.Cleaners
{
    /// <summary>
    /// Test class for <code>CorrectTagsClosingCleaner</code> pre-DOM filter.
    /// </summary>
    [TestFixture]
    public class CorrectTagsClosingCleanerTest
    {
        private string initialHTML1;
        private string initialHTML2;
        private string expectedHTML1;
        private string expectedHTML2;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CorrectTagsClosingCleanerTest()
        {
            initialHTML1 = "";
            initialHTML2 = "";
            expectedHTML1 = "";
            expectedHTML2 = "";
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML1 = "<html><body><img src=\"img1.jpg\"></body></html>";
            initialHTML2 = "<html><body><p>Text<br>Text</p></body></html>";
            
            expectedHTML1 = "<html><body><img src=\"img1.jpg\" /></body></html>";
            expectedHTML2 = "<html><body><p>Text<br />Text</p></body></html>";
        }

        [Test]
        public void TestCleaner()
        {
            IHTMLCleaner tagClosingCleaner1 = new CorrectTagsClosingCleaner("img");
            initialHTML1 = tagClosingCleaner1.Clean(initialHTML1);

            IHTMLCleaner tagClosingCleaner2 = new CorrectTagsClosingCleaner("br");
            initialHTML2 = tagClosingCleaner2.Clean(initialHTML2);
            
            Assert.AreEqual(initialHTML1, expectedHTML1);
            Assert.AreEqual(initialHTML2, expectedHTML2);
        }
    }
}