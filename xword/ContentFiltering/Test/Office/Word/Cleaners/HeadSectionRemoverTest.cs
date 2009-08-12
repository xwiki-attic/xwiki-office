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
    /// Test class for <code>HeadSectionRemover</code>.
    /// </summary>
    [TestFixture]
    public class HeadSectionRemoverTest
    {
        private string initialHTML;
        private string expectedHTML;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public HeadSectionRemoverTest()
        {
            initialHTML = "";
            expectedHTML="";
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML = "<html><head xmlns=\"\"><title>Title</title><link rel=\"stylesheet\" href=\"style.css\"/></head>"
                + "<body><p>Content</p></body></html>";

            expectedHTML = "<html><body><p>Content</p></body></html>";
        }

        [Test]
        public void TestCleaner()
        {
            bool canLoadXML = false;
            initialHTML = new HeadSectionRemover().Clean(initialHTML);
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