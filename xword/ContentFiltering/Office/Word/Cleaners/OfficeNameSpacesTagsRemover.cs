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
    /// Removes the tags that are in the office namespaces.
    /// </summary>
    public class OfficeNameSpacesTagsRemover : IHTMLCleaner
    {
        #region IHTMLCleaner Members

        /// <summary>
        /// Removes the tags that are in the office namespaces.
        /// </summary>
        /// <param name="content">The original content.</param>
        /// <returns>The cleaned content.</returns>
        public string Clean(string htmlSource)
        {
            bool foundTags = false;
            int startIndex = 0;
            int endIndex = 0;
            do
            {
                foundTags = false;
                startIndex = htmlSource.IndexOf("<o:", startIndex);
                if (startIndex >= 0)
                {
                    endIndex = htmlSource.IndexOf("</o:", startIndex);
                    if (endIndex >= 0)
                    {
                        endIndex = htmlSource.IndexOf(">", endIndex + 1);
                        htmlSource = htmlSource.Remove(startIndex, endIndex - startIndex + 1);
                    }
                    foundTags = true;
                    startIndex = endIndex - (endIndex - startIndex + 1);
                }
            } while (foundTags);
            return htmlSource;
        }

        #endregion IHTMLCleaner Members
    }
}