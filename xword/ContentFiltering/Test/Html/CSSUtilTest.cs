using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml;
using ContentFiltering.Html;
using ContentFiltering.Test.Util;
using System.Collections;

namespace ContentFiltering.Test.Html
{
    /// <summary>
    /// Test class for <code>CSSUtil</code>.
    /// </summary>
    [TestFixture]
    public class CSSUtilTest
    {
        private string initialHTML;
        private string expectedHTML;
        private XmlDocument initialXmlDoc;
        private XmlDocument expectedXmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CSSUtilTest()
        {
            initialHTML = "";
            expectedHTML = "";
            initialXmlDoc = new XmlDocument();
            expectedXmlDoc = new XmlDocument();
        }

        /// <summary>
        /// Global test setup.
        /// </summary>
        [TestFixtureSetUp]
        public void GlobalSetup()
        {
            initialHTML = "";
            expectedHTML = "";
            initialXmlDoc = new XmlDocument();
            expectedXmlDoc = new XmlDocument();
        }

        /// <summary>
        /// Tests the InlineCSS method.
        /// </summary>
        [Test]
        public void TestInlineCSS()
        {
            initialHTML = "<html><head><title>TITLE</title>"
                + "<style type=\"text/css\">"
                + Environment.NewLine
                + ".oneClass {font-family:sans-serif;font-size:90%;}"
                + "#oneId {color:red;}"
                + Environment.NewLine
                + "</style></head><body><h1>HEADER 1</h1>"
                + "<p class=\"oneClass\">Text 1</p>"
                + "<p><span id=\"oneId\">Text 2</span></p>"
                + "<p>Text 3 <span class=\"oneClass\" id=\"oneId\">Text 4</span> Text 5</p>"
                + "</body></html>";

            expectedHTML = "<html><head><title>TITLE</title>"
                + "<style type=\"text/css\">"
                + Environment.NewLine
                + ".oneClass {font-family:sans-serif;font-size:90%;}"
                + "#oneId {color:red;}"
                + Environment.NewLine
                + "</style></head><body><h1>HEADER 1</h1>"
                + "<p class=\"oneClass\" style=\"font-family:sans-serif;font-size:90%;\">Text 1</p>"
                + "<p><span id=\"oneId\" style=\"color:red;\">Text 2</span></p>"
                + "<p>Text 3 <span class=\"oneClass\" id=\"oneId\" style=\"font-family:sans-serif;font-size:90%;color:red;\">Text 4</span> Text 5</p>"
                + "</body></html>";

            initialXmlDoc.LoadXml(initialHTML);
            expectedXmlDoc.LoadXml(expectedHTML);

            CSSUtil.InlineCSS(ref initialXmlDoc);
            Assert.IsTrue(XmlDocComparator.AreIdentical(initialXmlDoc, expectedXmlDoc));
        }

        /// <summary>
        /// Performs a stress test for InlineCSS method: 15000 CSS classes and 15000 CSS ids for 30000 nodes.
        /// </summary>
        [Test]
        public void StressTestInlineCSS()
        {
            initialHTML = "<html><head><title>TITLE</title><style>{0}</style></head>"
                + Environment.NewLine
                + "<body>"
                + Environment.NewLine
                + "<div>{1}</div>"
                + "</body></html>";
            initialHTML = String.Format(initialHTML, GenerateStressTestStyle(), GenerateInitialElementsForStressTest());

            initialXmlDoc = new XmlDocument();
            initialXmlDoc.LoadXml(initialHTML);

            long startTicks = DateTime.Now.Ticks;
            CSSUtil.InlineCSS(ref initialXmlDoc);
            long stopTicks = DateTime.Now.Ticks;

            //inline operation duration, in miliseconds
            double inlineTimeMS = (1.0 * stopTicks - startTicks) / 10000;


            //inline operation should take less than 500 miliseconds
            Assert.IsTrue(inlineTimeMS < 500.00);
        }

        /// <summary>
        /// Generates 15000 CSS classes and 15000 CSS ids for the stress test.
        /// </summary>
        /// <returns>The CSS to be inserted in the 'style' section.</returns>
        private string GenerateStressTestStyle()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 15000; i++)
            {
                int r = i % 50;
                if (r == 0)
                {
                    sb.Append("{font-size:12px;color:#303030;}");
                    sb.Append(Environment.NewLine);
                }
                else
                {
                    sb.Append(".cssClass").Append(i);
                    if (r != 49)
                    {
                        sb.Append(", ");
                    }
                }
            }
            sb.Append(Environment.NewLine);

            for (int i = 1; i <= 15000; i++)
            {
                int r = i % 50;

                if (r == 0)
                {
                    sb.Append("{padding:4px;background-color:#FEFEFE;}");
                    sb.Append(Environment.NewLine);
                }
                else
                {
                    sb.Append("#cssID").Append(i);
                    if (r != 49)
                    {
                        sb.Append(", ");
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Generates a string contaning 30000 HTML paragraphs for the stress test.
        /// </summary>
        /// <returns>The HTML to be inserted in the 'body' section.</returns>
        private string GenerateInitialElementsForStressTest()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 15000; i++)
            {
                sb.Append("<p class=\"cssClass").Append(i).Append("\">Text with CSS CLASS ").Append(i).Append("</p>");
                sb.Append("<p id=\"cssID").Append(i).Append("\">Text with CSS ID ").Append(i).Append("</p>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Tests the GroupCSSSelectors method.
        /// </summary>
        [Test]
        public void TestGroupCSSSelectors()
        {
            Hashtable initialCSSClasses = new Hashtable();
            Hashtable optimizedCSSClasses = new Hashtable();

            //same initial CSS properties
            string props1 = "font-family:sans-serif;font-size:100%;color:red;";
            string props2 = "color:red;font-size:100%;font-family:sans-serif;";

            //the expected properties (ordered alphabetically)
            string expectedProp = "color:red;font-family:sans-serif;font-size:100%";

            //initial CSS selectors
            string[] classes1 = { ".xoffice0", ".xoffice1", ".xoffice2", "textarea" };
            string[] classes2 = { ".xoffice3", ".xoffice4", ".myCssClass", "#one", "p#two" };

            for (int i = 0; i < classes1.Length; i++)
            {
                initialCSSClasses.Add(classes1[i], props1);
            }

            for (int i = 0; i < classes2.Length; i++)
            {
                initialCSSClasses.Add(classes2[i], props2);
            }


            optimizedCSSClasses = CSSUtil.GroupCSSSelectors(initialCSSClasses);

            ICollection optimizedKeys = optimizedCSSClasses.Keys;

            //all the selectors should be grouped
            Assert.IsTrue(optimizedKeys.Count == 1);

            foreach (string key in optimizedKeys)
            {
                string val = optimizedCSSClasses[key].ToString();

                //in the new key there must be all initial CSS selectors
                foreach (string cssClass in classes1)
                {
                    Assert.IsTrue(key.IndexOf(cssClass) >= 0);
                }
                foreach (string cssClass in classes2)
                {
                    Assert.IsTrue(key.IndexOf(cssClass) >= 0);
                }

                //expected properties are the same, but ordered alphabetically
                Assert.IsTrue(val.IndexOf(expectedProp) >= 0);
            }

        }

    }
}
