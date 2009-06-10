using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using NUnit.Framework;

namespace XWord
{
    /// <summary>
    /// Test class for the ribbon.
    /// </summary>
    [TestFixture]
    public class XWikiRibbonTest
    {
        XWikiRibbon ribbon;

        /// <summary>
        /// Default constructor
        /// </summary>
        public XWikiRibbonTest()
        {
            
        }

        /// <summary>
        /// Initialize the test.
        /// </summary>
        [TestFixtureSetUp]
        public void GlobalSetUp()
        {
            ribbon = new XWikiRibbon();
        }

        /// <summary>
        /// Test method for DropDownSave format consistency
        /// </summary>
        [Test]
        public void TestDropDownSaveFormat()
        {
            string[] expectedValues = { "HTML", "Filtered HTML" };
            foreach (RibbonDropDownItem item in ribbon.dropDownSaveFormat.Items)
            {
                Assert.IsTrue(expectedValues.Contains(item.Label));
            }
        }
    }
}
