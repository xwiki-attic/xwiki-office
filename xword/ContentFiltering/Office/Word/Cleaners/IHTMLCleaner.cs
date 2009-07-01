using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Interface for HTML cleaners (pre-DOM filters).
    /// </summary>
    public interface IHTMLCleaner
    {
        string Clean(string htmlSource);
    }
}