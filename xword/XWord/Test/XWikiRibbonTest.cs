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
using Microsoft.Office.Tools.Ribbon;
using NUnit.Framework;

namespace XWord
{
    /// <summary>
    /// Test class for the ribbon.
    /// </summary>
    [TestFixture]
    public class XWikiRibbonTest
    {
        XWikiRibbon ribbon;

        /// <summary>
        /// Default constructor
        /// </summary>
        public XWikiRibbonTest()
        {
            
        }

        /// <summary>
        /// Initialize the test.
        /// </summary>
        [TestFixtureSetUp]
        public void GlobalSetUp()
        {
            ribbon = new XWikiRibbon();
        }

        /// <summary>
        /// Test method for DropDownSave format consistency
        /// </summary>
        [Test]
        public void TestDropDownSaveFormat()
        {
            string[] expectedValues = { "HTML", "Filtered HTML" };
            foreach (RibbonDropDownItem item in ribbon.dropDownSaveFormat.Items)
            {
                Assert.IsTrue(expectedValues.Contains(item.Label));
            }
        }
    }
}
