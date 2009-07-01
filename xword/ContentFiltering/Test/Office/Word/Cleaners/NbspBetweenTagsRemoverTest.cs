using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ContentFiltering.Office.Word.Cleaners;

namespace ContentFiltering.Test.Office.Word.Cleaners
{
    /// <summary>
    /// Test class for <code>NbspBetweenTagsRemover</code>.
    /// </summary>
    [TestFixture]
    public class NbspBetweenTagsRemoverTest
    {
        private string initialHTML;
        private string expectedHTML;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NbspBetweenTagsRemoverTest()
        {
            initialHTML = "";
            expectedHTML = "";
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML = "<html><body><p>&nbsp;<span>text</span></p><p>&nbsp;</p></body></html>";
            expectedHTML = "<html><body><p><span>text</span></p><p></p></body></html>";
        }

        [Test]
        public void Test()
        {
            initialHTML = new NbspBetweenTagsRemover().Clean(initialHTML);
            Assert.AreEqual(initialHTML, expectedHTML);
        }
    }
}