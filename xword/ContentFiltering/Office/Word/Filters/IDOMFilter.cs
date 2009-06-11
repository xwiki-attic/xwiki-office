using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XWiki.Office.Word;

namespace ContentFiltering.Office.Word.Filters
{
    /// <summary>
    /// Interface for XML cleaning.
    /// </summary>
    public interface IDOMFilter
    {
        /// <summary>
        /// The <code>Filter()</code> method implemented by each filter.
        /// </summary>
        /// <param name="xmlDoc">A reference to the <code>XmlDocument</code></param>
        void Filter(ref XmlDocument xmlDoc);
    }
}
