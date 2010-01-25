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
    /// Test class for <code>CorrectTagsClosingCleaner</code> pre-DOM filter.
    /// </summary>
    [TestFixture]
    public class CorrectTagsClosingCleanerTest
    {
        private string initialHTML1;
        private string initialHTML2;
        private string expectedHTML1;
        private string expectedHTML2;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CorrectTagsClosingCleanerTest()
        {
            initialHTML1 = "";
            initialHTML2 = "";
            expectedHTML1 = "";
            expectedHTML2 = "";
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            initialHTML1 = "<html><body><img src=\"img1.jpg\"></body></html>";
            initialHTML2 = "<html><body><p>Text<br>Text</p></body></html>";
            
            expectedHTML1 = "<html><body><img src=\"img1.jpg\" /></body></html>";
            expectedHTML2 = "<html><body><p>Text<br />Text</p></body></html>";
        }

        [Test]
        public void TestCleaner()
        {
            bool canLoadXML = false;
            IHTMLCleaner tagClosingCleaner1 = new CorrectTagsClosingCleaner("img");
            initialHTML1 = tagClosingCleaner1.Clean(initialHTML1);

            IHTMLCleaner tagClosingCleaner2 = new CorrectTagsClosingCleaner("br");
            initialHTML2 = tagClosingCleaner2.Clean(initialHTML2);
            
            Assert.AreEqual(initialHTML1, expectedHTML1);
            Assert.AreEqual(initialHTML2, expectedHTML2);

            try
            {
                new XmlDocument().LoadXml(initialHTML1);
                new XmlDocument().LoadXml(initialHTML2);
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