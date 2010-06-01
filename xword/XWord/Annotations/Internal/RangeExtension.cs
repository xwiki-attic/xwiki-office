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

#endregion //


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = Microsoft.Office.Interop.Word;

namespace XWord.Annotations
{
    internal static class RangeExtension
    {
        private static String[] whiteSpaces = new String[] { "\n", "\r", " ", "\t"};

        public static String GetCleanedText(this Word.Range range)
        {
            return ClearContent(range.Text);
        }

        public static String GetCleanedText(this String text)
        {
            return ClearContent(text);
        }

        private static String ClearContent(String content)
        {          
            
            if (content != null)
            {
                foreach (String s in whiteSpaces)
                {
                    content = content.Replace(s, "");
                }
            }
            return content;
        }

        public static bool IsWhiteSpace(this String s)
        {
            return whiteSpaces.Contains(s);
        }

        /// <summary>
        /// Computes the offset for the Word content and the cleared content.
        /// </summary>
        /// <param name="content">The text of a Word range.</param>
        /// <param name="newLineChars">The charachters to be cleared.</param>
        /// <returns>
        /// A dictionary instance. 
        /// KEY: The index of the next non-cleared char in the output string. 
        /// Value: the number of deleted chars in the initial string preceding the char in the output string.</returns>
        public static Dictionary<int, int> MapDeletedCharsOffsets(this Word.Range range)
        {
            String content = range.Text;
            Dictionary<int, int> deletedCharsMap = new Dictionary<int, int>();
            int deletedCharsNo = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (whiteSpaces.Contains(content[i].ToString()))
                {
                    deletedCharsNo++;
                    int newIndex = i - deletedCharsNo + 1;
                    if (deletedCharsMap.ContainsKey(newIndex))
                    {
                        deletedCharsMap[newIndex] = deletedCharsNo;
                    }
                    else
                    {
                        deletedCharsMap.Add(newIndex, deletedCharsNo);
                    }
                }
            }
            return deletedCharsMap;
        }
    }
}
