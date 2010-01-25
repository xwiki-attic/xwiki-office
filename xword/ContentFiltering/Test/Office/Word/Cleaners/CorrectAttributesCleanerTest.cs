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
    /// Test for <code>CorrectAttributesCleaner</code> pre-DOM filter.
    /// </summary>
    [TestFixture]
    public class CorrectAttributesCleanerTest
    {
        private string initialHTML;
        private string expectedHTML;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CorrectAttributesCleanerTest()
        {
            initialHTML = "";
            expectedHTML = "";
        }

        [TestFixtureSetUp]
        public void GlobalSetup()
        {
            initialHTML = "<html><head><title>Title</title>"
                +"<meta http-equiv=Content-Type content=text/html;charset=utf-8/>"
                +"<style type=text/css>body {color:black;}</style>"
                +"<link rel=stylesheet type=text/css href=style.css />"
                +"<script type=text/javascript>var x=1;</script>"
                +"</head><body>"
                + "<p id=p1>text</p>"
                + "<p style='font-color:red;'>text</p>"
                + "<p id=p2 class=copyright>copyright notes</p>"
                + "<font color=\"red\">red text</font>"
                + "</body></html>";
            expectedHTML = "<html><head><title>Title</title>"
                +"<meta http-equiv=\"Content-Type\" content=\"text/html;charset=utf-8\"/>"
                + "<style type=\"text/css\">body {color:black;}</style>"
                + "<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\" />"
                + "<script type=\"text/javascript\">var x=1;</script>"
                +"</head><body>"
                + "<p id=\"p1\">text</p>"
                + "<p style='font-color:red;'>text</p>"
                + "<p id=\"p2\" class=\"copyright\">copyright notes</p>"
                + "<font color=\"red\">red text</font>"
                + "</body></html>";
        }
        [Test]
        public void TestCleaner()
        {
            bool canLoadXML = false;
            initialHTML = new CorrectAttributesCleaner().Clean(initialHTML);
            Assert.AreEqual(expectedHTML, initialHTML);

            try
            {
                new XmlDocument().LoadXml(initialHTML);
                canLoadXML = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(initialHTML.Replace(">",">" + Environment.NewLine));
                Console.WriteLine(e);
                canLoadXML = false;
            }

            Assert.IsTrue(canLoadXML);
        }
    }
}