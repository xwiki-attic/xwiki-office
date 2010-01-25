#region LGPL license
/*
 * See the NOTICE file distributed with this work for additional
 * information regarding copyright ownership.
 *
 * This is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this software; if not, write to the Free
 * Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
 * 02110-1301 USA, or see the FSF site: http://www.fsf.org.
 */
#endregion //license

using System;
using System.Collections.Generic;
using System.Linq;
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
        private XmlDocument expectedXmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocalToWebStyleFilterTest()
        {
            manager = ConversionManagerTestUtil.DummyConversionManager();
            initialHTML = "";
            expectedHTML = "";
            initialXmlDoc = new XmlDocument();
            expectedXmlDoc = new XmlDocument();
        }

        /// <summary>
        /// Test setup.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML = "<html><head>"
                + "<style>"
                + "p {color:black;}"
                + "table.normalTable {border:1px silver solid;}"
                + "input[type=text] {background-color:cyan;}"
                + ".xoffice0 {font-family:monospace;}"
                + "</style>"
                + "<style>"
                + ".redcode {color:red;}"
                + "</style></head>"
                + "<body>"
                + "<p style=\"font-family:Verdana,sans-serif;font-size:100%;\">Verdana content</p>"
                + "<p class=\"xoffice0\">some code</p>"
                + "<p style=\"font-family:Verdana,sans-serif;font-size:100%;\">More verdana content</p>"
                + "<p class=\"xoffice0\">more code</p>"
                + " <p class=\"xoffice0 redcode\">errors text</p>"
                + "</body></html>";
            initialXmlDoc.LoadXml(initialHTML);

            expectedHTML = "<html><head>"
                + "<style>"
                + "p {color:black;}"
                + "table.normalTable {border:1px silver solid;}"
                + "input[type=text] {background-color:cyan;}"
                + ".xoffice0, .xoffice2{font-family:Verdana,sans-serif;font-size:100%;}"
                + ".xoffice1, .xoffice3{font-family:monospace;}"
                + ".xoffice4{color:red;font-family:monospace;}"
                + "</style></head>"
                + "<body>"
                + "<p class=\"xoffice0\">Verdana content</p>"
                + "<p class=\"xoffice1\">some code</p>"
                + "<p class=\"xoffice2\">More verdana content</p>"
                + "<p class=\"xoffice3\">more code</p>"
                + "<p class=\"xoffice4\">errors text</p>"
                + "</body></html>";

            expectedXmlDoc.LoadXml(expectedHTML);
        }

        /// <summary>
        /// Tests the LocalToWebStyle filter:
        ///  Only one 'style' node;
        ///  No inline styles;
        ///  Only 'xoffice[0-9]+' CSS classes.
        /// </summary>
        [Test]
        public void TestLocalToWebStyleFilter()
        {
            bool foundInlineStyles = false;
            bool foundNonXOfficeClasses = false;

            new LocalToWebStyleFilter(manager).Filter(ref initialXmlDoc);

            initialXmlDoc.Normalize();
            expectedXmlDoc.Normalize();

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

            Assert.IsFalse(foundInlineStyles);
            Assert.IsFalse(foundNonXOfficeClasses);

            XmlNodeList totalStyleNodes = initialXmlDoc.GetElementsByTagName("style");
            Assert.IsNotNull(totalStyleNodes);
            Assert.IsTrue(totalStyleNodes.Count == 1);

            XmlNode styleNode = initialXmlDoc.GetElementsByTagName("style")[0];
            Assert.IsNotNull(styleNode);

            string cssContent = ExtractStyleContent(styleNode);

            int cssClassesCount = CountCSSClasses(cssContent);

            Assert.IsTrue(cssClassesCount == 5);
            //Assert.IsTrue(OptimizedCSSClasses(cssContent));
            Assert.IsTrue(XmlDocComparator.AreIdentical(initialXmlDoc, expectedXmlDoc));
        }

        /// <summary>
        /// Extracts the CSS content from a style node.
        /// </summary>
        /// <param name="styleNode">A style <code>XmlNode</code>.</param>
        /// <returns>The string representing the CSS content.</returns>
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
        /// Counts the 'xoffice' CSS classes from a CSS content.
        /// </summary>
        /// <param name="cssContent">The CSS content.</param>
        /// <returns>Number of 'xoffice' CSS classes found.</returns>
        private int CountCSSClasses(string cssContent)
        {
            int count = 0;
            int startIndex = 0;
            do
            {
                startIndex = cssContent.IndexOf(".xoffice", startIndex);
                //if found, search from the next position
                if (startIndex >= 0)
                {
                    startIndex++;
                    count++;
                }

            } while (startIndex >= 0);
            return count;
        }

        /// <summary>
        /// Verifies the CSS content has five classes grouped in 3 parts.
        /// </summary>
        /// <param name="cssContent">The CSS content.</param>
        /// <returns>TRUE if CSS seems to be optimized.</returns>
        private bool OptimizedCSSClasses(string cssContent)
        {
            bool foundOptimizedCSS = false;
            char[] separator = new char[] { '}' };
            string[] groups = cssContent.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (groups.Length == 3)
            {
                foundOptimizedCSS = (CountCSSClasses(groups[0]) + CountCSSClasses(groups[1]) + CountCSSClasses(groups[2])) == 5;
            }
            return foundOptimizedCSS;
        }
    }
}
