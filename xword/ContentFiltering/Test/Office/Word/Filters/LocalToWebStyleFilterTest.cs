using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml;
using ContentFiltering.Test.Util;
using ContentFiltering.Office.Word.Filters;
using XWiki.Office.Word;

namespace ContentFiltering.Test.Office.Word.Filters
{
    /// <summary>
    /// Test class for <code>LocalToWebStyleFilter</code>.
    /// </summary>
    [TestFixture]
    public class LocalToWebStyleFilterTest
    {
        private ConversionManager manager;
        private string initialHTML;
        private string expectedHTML;
        private XmlDocument initialXmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocalToWebStyleFilterTest()
        {
            manager = ConversionManagerTestUtil.DummyConversionManager();
            initialHTML = "";
            expectedHTML = "";
            initialXmlDoc = new XmlDocument();
        }

        /// <summary>
        /// Test setup.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML = "<html><head><style>.xoffice0 {font-family:monospace;}</style></head>"
                + "<body><p style=\"font-family:Verdana,sans-serif;font-size:10pt;\">Some content</p>"
                + "<p class=\"xoffice0\">some code</p>"
                + "<p style=\"font-family:Verdana,sans-serif;font-size:10pt;\">More content</p>"
                + "<p class=\"xoffice0\">more code</p>"
                + "</body></html>";

            expectedHTML = "<html><head><style>"
                + " .xoffice0, .xoffice2 {font-family:Verdana,sans-serif;font-size:10pt;} "
                + " .xoffice1 .xoffice3 {font-family:monospace;}"
                + " </style></head>"
                + "<body>"
                + "<p class=\"xoffice0\">Some content</p>"
                + "<p class=\"xoffice1\">some code</p>"
                + "<p class=\"xoffice2\">More content</p>"
                + "<p class=\"xoffice3\">more code</p>"
                + "</body></html>";

            initialXmlDoc.LoadXml(initialHTML);
        }

        /// <summary>
        /// Tests the LocalToWebStyle filter:
        ///  - No inline styles.
        ///  - Only 'xoffice[0-9]+' CSS classes.
        ///  - Exactly 4 'xoffice[0-9]+' CSS classes, grouped in 2 parts (optimized).
        /// </summary>
        [Test]
        public void TestLocalToWebStyleFilter()
        {
            bool foundInlineStyles = false;
            bool foundNonXOfficeClasses = false;

            new LocalToWebStyleFilter(manager).Filter(ref initialXmlDoc);

            XmlNodeList allNodes = initialXmlDoc.GetElementsByTagName("*");

            foreach (XmlNode node in allNodes)
            {
                //searching for inline styles
                if (node.Attributes["style"] != null)
                {
                    if (("" + node.Attributes["style"].Value).Length > 0)
                    {
                        foundInlineStyles = true;
                        break; //no need to continue searching other problems
                    }
                }

                //searching for non-XOffice CSS classes in nodes
                if (node.Attributes["class"] != null)
                {
                    if (("" + node.Attributes["class"].Value).Length > 0)
                    {
                        if (node.Attributes["class"].Value.ToLower().IndexOf("xoffice") < 0)
                        {
                            foundNonXOfficeClasses = true;
                            break;
                        }
                    }
                }
            }

            XmlNode styleNode = initialXmlDoc.GetElementsByTagName("style")[0];
            string cssContent = ExtractStyleContent(styleNode);

            Assert.IsFalse(foundInlineStyles);
            Assert.IsFalse(foundNonXOfficeClasses);
            Assert.IsNotNull(styleNode);
            Assert.IsTrue(CountCSSClasses(cssContent) == 4);
            Assert.IsTrue(OptimizedCSSClasses(cssContent));
        }

        private string ExtractStyleContent(XmlNode styleNode)
        {
            string cssContent = "";
            if (styleNode != null)
            {
                if (("" + styleNode.InnerText).Length > 0)
                {
                    cssContent = styleNode.InnerText;
                }
            }
            return cssContent;
        }

        /// <summary>
        /// Counts the CSS classes from a CSS content.
        /// </summary>
        /// <param name="cssContent">The CSS content.</param>
        /// <returns>Number of CSS classes found.</returns>
        private int CountCSSClasses(string cssContent)
        {
            int count = 0;
            int startIndex = 0;
            while (startIndex != -1)
            {
                while (startIndex >= 0)
                {
                    startIndex = cssContent.IndexOf(".xoffice", startIndex);
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Verifies the CSS content four classes grouped in 2 parts.
        /// </summary>
        /// <param name="cssContent">The CSS content.</param>
        /// <returns>TRUE if CSS seems to be optimized.</returns>
        private bool OptimizedCSSClasses(string cssContent)
        {
            bool foundOptimizedCSS = false;
            char[] separator = new char[] { '}' };
            string[] groups = cssContent.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (groups.Length == 2)
            {
                Console.WriteLine("Counting..");
                foundOptimizedCSS = (CountCSSClasses(groups[0]) == 2) && (CountCSSClasses(groups[1]) == 2);
            }
            return foundOptimizedCSS;
        }
    }
}
