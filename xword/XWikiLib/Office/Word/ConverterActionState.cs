using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWiki.Office.Word
{
    /// <summary>
    /// The 
    /// </summary>
    public enum ConverterActionState
    {
        Downloading,
        Uploading,
        EditingPage,
        StoringForOffileUsage,
        Exporting,
        Sleeping
    }
}
