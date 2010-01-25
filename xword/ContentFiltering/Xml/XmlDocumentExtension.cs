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
using System.Xml;
using System.IO;

namespace XWiki.Xml
{
    /// <summary>
    /// Extension for XmlDocument class.
    /// </summary>
    public static class XmlDocumentExtension
    {
        /// <summary>
        /// Extension method providing indended output.
        /// </summary>
        /// <param name="xmlDoc">XmlDocument instance.</param>
        /// <returns>Indended XML</returns>
        public static string GetIndentedXml(this XmlDocument xmlDoc)
        {
            StreamReader sr;
            string indendedHTML;
            MemoryStream stream = new MemoryStream(xmlDoc.InnerText.Length);
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.Unicode);
            writer.Formatting = Formatting.Indented;

            xmlDoc.Save(writer);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            sr = new StreamReader(stream);
            indendedHTML = sr.ReadToEnd();

            writer.Close();
            sr.Close();
            stream.Close();

            return indendedHTML;
        }
    }
}
