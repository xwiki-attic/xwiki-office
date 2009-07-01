using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ContentFiltering.Office.Word.Cleaners;

namespace ContentFiltering.Test.Office.Word.Cleaners
{
    /// <summary>
    /// Test for <code>CorrectAttributesCleaner</code> pre-DOM filter.
    /// </summary>
    [TestFixture]
    public class CorrectAttributesCleanerTest
    {
        private string initialHTML;
        private string expectedHTML;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CorrectAttributesCleanerTest()
        {
            initialHTML = "";
            expectedHTML = "";
        }

        [TestFixtureSetUp]
        public void GlobalSetup()
        {
            initialHTML = "<html><head><title>Title</title></head><body>"
                + "<p id=p1>text</p>"
                + "<p style='font-color:red;'>text</p>"
                + "<p id=p2 class=copyright>copyright notes</p>"
                + "<font color=\"red\">red text</font>"
                + "</body></html>";
            expectedHTML = "<html><head><title>Title</title></head><body>"
                + "<p id=\"p1\">text</p>"
                + "<p style='font-color:red;'>text</p>"
                + "<p id=\"p2\" class=\"copyright\">copyright notes</p>"
                + "<font color=\"red\">red text</font>"
                + "</body></html>";
        }
        [Test]
        public void TestCleaner()
        {
            initialHTML = new CorrectAttributesCleaner().Clean(initialHTML);
            Assert.AreEqual(initialHTML, expectedHTML);
        }
    }
}