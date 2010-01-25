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

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Gets the inner html of the body.
    /// </summary>
    public class BodyContentExtractor : IHTMLCleaner
    {

        #region IHTMLCleaner Members

        /// <summary>
        /// Gets the content between the opening and closing html tags.
        /// </summary>
        /// <param name="htmlCode">The html source to be </param>
        /// <returns>The inner html of the body.</returns>
        public string Clean(string htmlCode)
        {
            //Delete header & footer
            int startIndex, endIndex;
            startIndex = htmlCode.IndexOf("<body");
            endIndex = htmlCode.IndexOf(">", startIndex);
            htmlCode = htmlCode.Remove(0, endIndex + 1);
            startIndex = htmlCode.IndexOf("</body");
            if (startIndex >= 0)
                htmlCode = htmlCode.Remove(startIndex);
            return htmlCode;
        }

        #endregion IHTMLCleaner Members
    }
}