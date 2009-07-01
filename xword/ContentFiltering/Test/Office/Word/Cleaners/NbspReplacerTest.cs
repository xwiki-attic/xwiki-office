using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ContentFiltering.Office.Word.Cleaners;

namespace ContentFiltering.Test.Office.Word.Cleaners
{
    /// <summary>
    /// Test class for <code>NbspReplacer</code>.
    /// </summary>
    [TestFixture]
    public class NbspReplacerTest
    {
        private string initialHTML;
        private string expectedHTML;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NbspReplacerTest()
        {
            initialHTML = "";
            expectedHTML = "";
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML = "<html><body><p>Some&nbsp;content</p><p>More content&nbsp;here</p></body></html>";
            expectedHTML = "<html><body><p>Some content</p><p>More content here</p></body></html>";
        }

        [Test]
        public void Test()
        {
            initialHTML = new NbspReplacer().Clean(initialHTML);
            Assert.AreEqual(initialHTML, expectedHTML);
        }
    }
}