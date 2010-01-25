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
using System.IO;

namespace XWord.Util
{
    /// <summary>
    /// Provides misc functionality for the add-in.
    /// </summary>
    public class UtilityClass
    {
        /// <summary>
        /// Generates an unique filename based on the existing files in a folder.
        /// </summary>
        /// <param name="initialFileName">The initial file name.</param>
        /// <param name="folder">The folder where the file will be saved.</param>
        /// <returns>The assigned filename.</returns>
        public static String GenerateUniqueFileName(String initialFileName, String folder)
        {
            int sufix = 1;
            String fileName = initialFileName;
            if (!folder.EndsWith(@"\"))
            {
                folder += @"\";
            }
            while (File.Exists(folder + fileName))
            {
                fileName = Path.GetFileNameWithoutExtension(initialFileName) + sufix.ToString();
                fileName += Path.GetExtension(initialFileName);
                sufix++;
            }
            return fileName;
        }

        /// <summary>
        /// Compares a string withs another string that contains whildcards.
        /// </summary>
        /// <param name="wildcard">The wildcard string.</param>
        /// <param name="text">The compared strings.</param>
        /// <param name="casesensitive">Specifies if the comparation shouls be case sensitive or not.</param>
        /// <returns>True if the string mathches the wildcard. False otherwize.</returns>
        public static bool IsWildcardMatch(String wildcard, String text, bool casesensitive)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(wildcard.Length + 10);
            sb.Append("^");
            for (int i = 0; i < wildcard.Length; i++)
            {
                char c = wildcard[i];
                switch (c)
                {
                    case '*':
                        sb.Append(".*");
                        break;
                    case '?':
                        sb.Append(".?");
                        break;
                    default:
                        sb.Append(System.Text.RegularExpressions.Regex.Escape(wildcard[i].ToString()));
                        break;
                }
            }
            sb.Append("$");

            System.Text.RegularExpressions.Regex regex;
            if (casesensitive)
                regex = new System.Text.RegularExpressions.Regex(sb.ToString(), System.Text.RegularExpressions.RegexOptions.None);
            else
                regex = new System.Text.RegularExpressions.Regex(sb.ToString(), System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return regex.IsMatch(text);
        }
    }
}
