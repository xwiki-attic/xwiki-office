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
    /// Test class for <code>ListCharsCleaner</code>.
    /// </summary>
    [TestFixture]
    public class ListCharsCleanerTest
    {
        private string initialHTML;
        private string expectedHTML;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ListCharsCleanerTest()
        {
            initialHTML = "";
            expectedHTML = "";
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML = "<html><body><p>·Item1<br/>·Item2</p><p>§Another item</p></body></html>";
            expectedHTML = "<html><body><p>oItem1<br/>oItem2</p><p>oAnother item</p></body></html>";
        }

        [Test]
        public void TestCleaner()
        {
            bool canLoadXML = false;
            initialHTML = new ListCharsCleaner().Clean(initialHTML);
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
