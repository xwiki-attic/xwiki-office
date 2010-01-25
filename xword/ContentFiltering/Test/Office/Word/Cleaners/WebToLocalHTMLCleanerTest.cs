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
using System.Text;
using NUnit.Framework;
using System.Xml;
using ContentFiltering.Office.Word.Cleaners;
using System.IO;

namespace ContentFiltering.Test.Office.Word.Cleaners
{
    /// <summary>
    /// Test class for <code>WebToLocalHTMLCleaner</code>.
    /// </summary>
    [TestFixture]
    public class WebToLocalHTMLCleanerTest
    {
        private string htmlContent;
        private string htmlOpeningTag;
        private XmlDocument xmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WebToLocalHTMLCleanerTest()
        {
            htmlContent = "";
            htmlOpeningTag = "";
            xmlDoc = new XmlDocument();
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            htmlContent = LoadTextFromFile(@"Test\Office\Word\Cleaners\TestsResources\PreDomFiltersTestPage.html");
            htmlOpeningTag = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\"" +
                        " xmlns:o=\"urn:schemas-microsoft-com:office:office\"" +
                        " xmlns:w=\"urn:schemas-microsoft-com:office:word\"" +
                        " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\"" +
                        " xmlns=\"http://www.w3.org/1999/xhtml\">";
        }

        // <summary>
        /// Loads an HTML file from disk, cleans the content with WebToLocalHTMLCleaner and then it
        /// tries to load the cleaned HTML into an XmlDocument.
        /// </summary>
        [Test]
        public void TestCleaner()
        {
            bool noException = true;
            try
            {
                htmlContent = new WebToLocalHTMLCleaner(htmlOpeningTag).Clean(htmlContent);
                xmlDoc.LoadXml(htmlContent);
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e);
                noException = false;
            }

            Assert.IsTrue(noException);
        }


        private string LoadTextFromFile(string path)
        {
            string content = "";
            try
            {
                TextReader tr = new StreamReader(path);
                content = tr.ReadToEnd();
                tr.Close();
            }
            catch
            {
            }
            return content;
        }
    }
}
