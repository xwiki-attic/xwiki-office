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
