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
using ContentFiltering.Office.Word.Cleaners;
using System.Xml;

namespace ContentFiltering.Test.Office.Word.Cleaners
{
    /// <summary>
    /// Test class for <code>CommentsRemover</code>.
    /// </summary>
    [TestFixture]
    public class CommentsRemoverTest
    {
        private string initialHTML;
        private string expectedHTML;

        public CommentsRemoverTest()
        {
            initialHTML = "";
            expectedHTML = "";
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\""
                + " xmlns:o=\"urn:schemas-microsoft-com:office:office\""
                + " xmlns:w=\"urn:schemas-microsoft-com:office:word\""
                + " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\""
                + " xmlns=\"http://www.w3.org/TR/REC-html40\"> "
                + "<head>"
                + "<!--[if gte mso 9]><xml>"
                + " <o:OfficeDocumentSettings>"
                + "   <o:AllowPNG/>"
                + "   <o:PixelsPerInch>120</o:PixelsPerInch>"
                + "   <o:TargetScreenSize>1024x768</o:TargetScreenSize>"
                + "  </o:OfficeDocumentSettings>"
                + "</xml><![endif]-->"
                + "</head>"
                + "<body>"
                + "<!-- the comment -->"
                + "<p>the paragraph</p>"
                + "</body>"
                + "</html>";

            expectedHTML = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\""
                + " xmlns:o=\"urn:schemas-microsoft-com:office:office\""
                + " xmlns:w=\"urn:schemas-microsoft-com:office:word\""
                + " xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\""
                + " xmlns=\"http://www.w3.org/TR/REC-html40\"> "
                + "<head>"
                + "</head>"
                + "<body>"
                + "<p>the paragraph</p>"
                + "</body>"
                + "</html>";
        }

        [Test]
        public void TestCleaner()
        {
            bool canLoadXML = false;
            initialHTML = new CommentsRemover().Clean(initialHTML);
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
