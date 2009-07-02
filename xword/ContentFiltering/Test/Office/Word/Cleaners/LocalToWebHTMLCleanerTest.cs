using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml;
using ContentFiltering.Office.Word.Cleaners;
using System.IO;

namespace ContentFiltering.Test.Office.Word.Cleaners
{
    /// <summary>
    /// Test class for <code>LocalToWebHTMLCleaner</code>.
    /// </summary>
    [TestFixture]
    public class LocalToWebHTMLCleanerTest
    {
        private string htmlContent;
        private string htmlOpeningTag;
        private XmlDocument xmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocalToWebHTMLCleanerTest()
        {
            htmlContent = "";
            htmlOpeningTag = "";
            xmlDoc = new XmlDocument();
        }


        [TestFixtureSetUp]
        public void TestSetup()
        {
            string path = @"Test\Office\Word\Cleaners\TestsResources\PreDomFiltersTestPage.html";
            htmlContent = "";
            try
            {
                TextReader tr = new StreamReader(path);
                htmlContent = tr.ReadToEnd();
                tr.Close();
            }
            catch
            {
            }

            htmlOpeningTag = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\"" +
                        " xmlns:o=\"urn:schemas-microsoft-com:office:office\"" +
                        " xmlns:w=\"urn:schemas-microsoft-com:office:word\"" +
                        " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\"" +
                        " xmlns=\"http://www.w3.org/1999/xhtml\">";
        }

        /// <summary>
        /// Loads an HTML file from disk, cleans the content with LocalToWebHTMLCleaner and then it
        /// tries to load the cleaned HTML into an XmlDocument.
        /// </summary>
        [Test]
        public void TestCleaner()
        {
            bool noException = true;

            try
            {
                htmlContent = new LocalToWebHTMLCleaner(htmlOpeningTag).Clean(htmlContent);
                xmlDoc.LoadXml(htmlContent);
            }
            catch (Exception e)
            {
                noException = false;
                Console.Error.WriteLine(e);
            }

            Assert.IsTrue(noException);
        }
    }
}
