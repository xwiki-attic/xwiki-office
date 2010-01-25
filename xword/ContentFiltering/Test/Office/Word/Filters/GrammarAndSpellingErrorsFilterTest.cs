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
using ContentFiltering.Test.Util;
using ContentFiltering.Office.Word.Filters;
using XWiki.Office.Word;

namespace ContentFiltering.Test.Office.Word.Filters
{

    /// <summary>
    /// Test class for <code>GrammarAndSpellingErrorsFilter</code>.
    /// </summary>
    [TestFixture]
    public class GrammarAndSpellingErrorsFilterTest
    {
        private ConversionManager manager;
        private string initialHTML;
        private string cleanedHTML;
        private XmlDocument initialXmlDoc;
        private XmlDocument cleanedXmlDoc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GrammarAndSpellingErrorsFilterTest()
        {
            manager = ConversionManagerTestUtil.DummyConversionManager();
            initialHTML = "";
            cleanedHTML = "";
            initialXmlDoc = new XmlDocument();
            cleanedXmlDoc=new XmlDocument();
        }

        /// <summary>
        /// Initialize the test.
        /// </summary>
        [TestFixtureSetUp]
        public void GlobalSetUp()
        {
            initialHTML = "<html>"
                    + "<body>"
                    + "<div>"
                    + "<h1>Page title <span class=\"SpellE\">gos</span> here</h1>"
                    + "<p>Page content goes here.</p>"
                    + "<p><span class=\"GramE\">And</span> some <span class=\"SpellE\">errrors</span> goes here.</p>"
                    + "<p>We <span class=\"SpellE\">shold</span> have both grammar and <span class=\"SpellE\">spelings</span><span class=\"SpellE\">erors</span>.</p>"
                    + "<p>He <span class=\"GramE\">have</span> apples.</p>"
                    + "<p><span lang=\"EN-US\">Un cuvant</span></p>"
                    + "</div>"
                    + "</body>"
                    + "</html>";

            cleanedHTML ="<html>"
                    + "<body>"
                    + "<div>"
                    + "<h1>Page title <span>gos </span> here</h1>"
                    + "<p>Page content goes here.</p>"
                    + "<p><span>And </span> some <span>errrors </span> goes here.</p>"
                    + "<p>We <span>shold </span> have both grammar and <span>spelings </span><span>erors </span>.</p>"
                    + "<p>He <span>have </span> apples.</p>"
                    + "<p><span>Un cuvant</span></p>"
                    + "</div>"
                    + "</body>"
                    + "</html>";

            initialXmlDoc.LoadXml(initialHTML);
            cleanedXmlDoc.LoadXml(cleanedHTML);
        }

        /// <summary>
        /// Test method for grammar and spelling errors filter.
        /// </summary>
        [Test]
        public void TestFilter()
        {
            new GrammarAndSpellingErrorsFilter(manager).Filter(ref initialXmlDoc);
            Assert.IsTrue(XmlDocComparator.AreIdentical(initialXmlDoc, cleanedXmlDoc));
        }
    }
}
