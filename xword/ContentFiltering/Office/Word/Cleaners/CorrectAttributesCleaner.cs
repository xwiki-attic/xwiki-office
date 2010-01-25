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
    /// Corrects the attributes that miss ' or ".
    /// </summary>
    public class CorrectAttributesCleaner : IHTMLCleaner
    {
        #region IHTMLCleaner Members

        /// <summary>
        /// Corrects the attributes that miss ' or ".
        /// </summary>
        /// <param name="htmlSource">The original html source code.</param>
        /// <returns>The source with corrected attributes.</returns>
        public string Clean(string htmlSource)
        {
            StringBuilder sb = new StringBuilder(htmlSource);
            List<String> tags = GetTagsWithAttributes(htmlSource);
            foreach (String initialValue in tags)
            {
                String value = initialValue;
                string[] separators = { " ", ">", "/>", "/ >", "\r" };
                bool hasChanged = false;
                
                foreach (String s in initialValue.Split(separators,StringSplitOptions.RemoveEmptyEntries))
                {
                    int equalSignPosition = s.IndexOf('=');
                    if (equalSignPosition > 0)
                    {
                        String[] attribute = { s.Substring(0, equalSignPosition), s.Substring(equalSignPosition+1) };
                        try
                        {
                            String newValue = attribute[1];
                            if (attribute[1][0] != '\'' && attribute[1][0] != '\"')
                            {
                                newValue = attribute[0] + "=\"" + attribute[1] + "\"";
                                value = value.Replace(s, newValue);
                                hasChanged = true;
                            }
                        }
                        catch (IndexOutOfRangeException) { };
                    }
                }
                if (hasChanged)
                {
                    sb = sb.Replace(initialValue, value);
                }
            }
            return sb.ToString();
        }

        #endregion IHTMLCleaner Members

        /// <summary>
        /// Gets a list with all the tags that contain attributes.
        /// </summary>
        /// <param name="htmlSource">The html source.</param>
        /// <returns>A list of strings with the tags containing attributes.</returns>
        public List<String> GetTagsWithAttributes(String htmlSource)
        {
            List<String> tags = new List<String>();
            int startIndex = 0;
            int endIndex = 0;
            do
            {
                startIndex = htmlSource.IndexOf('<', endIndex);
                if (startIndex >= 0)
                {
                    endIndex = htmlSource.IndexOf('>', startIndex);
                    if (endIndex >= 0)
                    {
                        String tag = htmlSource.Substring(startIndex, endIndex - startIndex + 1);
                        if (tag.Contains('='))
                        {
                            tags.Add(tag);
                        }
                    }
                }

            } while (startIndex < (htmlSource.Length - 1) && endIndex < (htmlSource.Length - 1) && (startIndex >= 0) && (endIndex >= 0));
            return tags;
        }
    }
}