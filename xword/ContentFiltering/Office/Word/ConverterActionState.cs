using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWiki.Office.Word
{
    /// <summary>
    /// The states of the conversion.
    /// </summary>
    public enum ConverterActionState
    {
        /// <summary>
        /// The page is retrieved from server.
        /// </summary>
        Downloading,
        /// <summary>
        /// The page is uploaded/saved on the server.
        /// </summary>
        Uploading,
        /// <summary>
        /// The user is editing the opened page.
        /// </summary>
        EditingPage,
        /// <summary>
        /// The page is stored for offline use.
        /// <remarks>
        /// Not yet implemented.
        /// </remarks>
        /// </summary>
        StoringForOffileUsage,
        /// <summary>
        /// Exporting a new document to wiki.
        /// </summary>
        Exporting
    }
}
