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
    /// Test class for <code>StyleRemoverFilter</code>.
    /// </summary>
    [TestFixture]
    public class StyleRemoverFilterTest
    {
        private ConversionManager manager;
        private string initialHTML;
        private string expectedHTML;
        private XmlDocument initialXmlDoc;
        private XmlDocument expectedXmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StyleRemoverFilterTest()
        {
            manager = ConversionManagerTestUtil.DummyConversionManager();
            initialHTML = "";
            expectedHTML = "";
            initialXmlDoc = new XmlDocument();
            expectedXmlDoc = new XmlDocument();
        }

        /// <summary>
        /// Initialize the test.
        /// </summary>
        [TestFixtureSetUp]
        public void GlobalSetUp()
        {
            initialHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\""
                + " xmlns:o=\"urn:schemas-microsoft-com:office:office\""
                + " xmlns:w=\"urn:schemas-microsoft-com:office:word\""
                + " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\""
                + " xmlns=\"http://www.w3.org/TR/REC-html40\">"
                + "<head><title>Title</title></head>"
                + "<body style=\"tab-interval:.5in\">"
                + "<div>"
                + "<div style=\"mso-element:para-border-div;border:none;border-bottom:solid #4F81BD 1.0pt;"
                + "mso-border-bottom-themecolor:accent1;padding:0in 0in 4.0pt 0in\"> "
                + "<h1>Heading 1</h1>"
                + "<h2>Heading 2</h2>"
                + "<p class=\"MsoNormal\">Normal text content goes here.</p> "
                + "<p class=\"MsoNormal\"><span style=\"so-no-proof:yes\">"
                + "<img width=\"106\" height=\"106\" src=\"EraseMe001_files/image002.jpg\" alt=\"bg001.bmp\"/>"
                + "</span></p>"
                + "<table class=\"MsoNormalTable\" border=\"1\" cellspacing=\"0\" cellpadding=\"0\""
                + " style=\"margin-left:12.2pt;border-collapse:collapse;border:none;mso-border-alt:"
                + " solid windowtext .5pt;mso-padding-alt:0in 5.4pt 0in 5.4pt;mso-border-insideh:"
                + " .5pt solid windowtext;mso-border-insidev:.5pt solid windowtext\"> "
                + " <tr style=\"mso-yfti-irow:0;mso-yfti-firstrow:yes;height:21.05pt\"> "
                + "  <td width=\"285\" colspan=\"3\" valign=\"top\" style=\"width:213.95pt;border:solid windowtext 1.0pt;"
                + "  mso-border-alt:solid windowtext .5pt;padding:0in 5.4pt 0in 5.4pt;height:21.05pt\"> "
                + "  <p class=\"MsoNormal\">1</p> "
                + "  </td> "
                + " </tr> "
                + " <tr style=\"mso-yfti-irow:1;height:23.75pt\"> "
                + "  <td width=\"78\" rowspan=\"2\" valign=\"top\" style=\"width:58.4pt;border:solid windowtext 1.0pt;"
                + "  border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;"
                + "  padding:0in 5.4pt 0in 5.4pt;height:23.75pt\"> "
                + "  <p class=\"MsoNormal\" >2</p> "
                + "  </td> "
                + "  <td width=\"207\" colspan=\"2\" valign=\"top\" style=\"width:155.55pt;border-top:none;"
                + "  border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;"
                + "  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;"
                + "  mso-border-alt:solid windowtext .5pt;padding:0in 5.4pt 0in 5.4pt;height:23.75pt\"> "
                + "  <p class=\"MsoNormal\" >3</p> "
                + "  </td> "
                + " </tr> "
                + " <tr style=\"mso-yfti-irow:2;mso-yfti-lastrow:yes;height:25.15pt\"> "
                + "  <td width=\"90\" valign=\"top\" style=\"width:67.25pt;border-top:none;border-left:"
                + "  none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;"
                + "  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;"
                + "  mso-border-alt:solid windowtext .5pt;padding:0in 5.4pt 0in 5.4pt;height:25.15pt\"> "
                + "  <p class=\"MsoNormal\" >4</p> "
                + "  </td> "
                + "  <td width=\"118\" valign=\"top\" style=\"width:88.3pt;border-top:none;border-left:"
                + "  none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;"
                + "  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;"
                + "  mso-border-alt:solid windowtext .5pt;padding:0in 5.4pt 0in 5.4pt;height:25.15pt\"> "
                + "  <p class=\"MsoNormal\" >5</p> "
                + "  </td> "
                + " </tr> "
                + "</table> "
                + "<p class=\"MsoNoSpacing\">Some no spacing text content</p> "
                + "</div></div>"
                + "</body></html>";


            expectedHTML = "<html  xmlns:v=\"urn:schemas-microsoft-com:vml\""
                + " xmlns:o=\"urn:schemas-microsoft-com:office:office\""
                + " xmlns:w=\"urn:schemas-microsoft-com:office:word\""
                + " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\""
                + " xmlns=\"http://www.w3.org/TR/REC-html40\">"
                + "<head><title>Title</title></head>"
                + "<body>"
                + "<div>"
                + "<div>"
                + "<h1>Heading 1</h1>"
                + "<h2>Heading 2</h2>"
                + "<p>Normal text content goes here.</p> "
                + "<p><span>"
                + "<img width=\"106\" height=\"106\" src=\"EraseMe001_files/image002.jpg\" alt=\"bg001.bmp\"/>"
                + "</span></p>"
                + "<table border=\"1\" cellspacing=\"0\" cellpadding=\"0\">"
                + " <tr> "
                + "  <td colspan=\"3\" > "
                + "  <p>1</p> "
                + "  </td> "
                + " </tr> "
                + " <tr> "
                + "  <td rowspan=\"2\"> "
                + "  <p>2</p> "
                + "  </td> "
                + "  <td colspan=\"2\"> "
                + "  <p>3</p> "
                + "  </td> "
                + " </tr> "
                + " <tr> "
                + "  <td > "
                + "  <p>4</p> "
                + "  </td> "
                + "  <td > "
                + "  <p>5</p> "
                + "  </td> "
                + " </tr> "
                + "</table> "
                + "<p>Some no spacing text content</p> "
                + "</div></div>"
                + "</body></html>";

            initialXmlDoc.LoadXml(initialHTML);
            expectedXmlDoc.LoadXml(expectedHTML);
        }

        /// <summary>
        /// Test method for style remover filter. Verifies if styles and attributes that sould be transformed in CSS (like width, height) 
        /// are removed, but other attributes (like colspan, rowspan) are preserved.
        /// </summary>
        [Test]
        public void TestFilter()
        {
            StyleRemoverFilter styleRemoverFilter = new StyleRemoverFilter(manager);
            styleRemoverFilter.Filter(ref initialXmlDoc);
            Assert.IsTrue(XmlDocComparator.AreIdentical(initialXmlDoc, expectedXmlDoc));
        }
    }
}
