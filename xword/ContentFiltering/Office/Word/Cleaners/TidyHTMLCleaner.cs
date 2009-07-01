using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TidyNet;
using System.IO;
using XWiki;

namespace ContentFiltering.Office.Word.Cleaners
{
    /// <summary>
    /// Uses Tidy.Net to clean a html source.
    /// </summary>
    public class TidyHTMLCleaner : IHTMLCleaner
    {
        private bool isWordHtml;

        public TidyHTMLCleaner(bool isWordHtml)
        {
            this.isWordHtml = isWordHtml;
        }
        #region IHTMLCleaner Members
        /// <summary>
        /// Uses Tidy.Net to clean a html source.
        /// </summary>
        /// <param name="htmlSource">The original html source.</param>
        /// <param name="isWordHtml">Specifies if the source is an output from Microsoft Word</param>
        /// <returns>The cleaned Html.</returns>
        public string Clean(string htmlSource)
        {
            Tidy tidy = new Tidy();
            //Options required dor xhtml conversion.
            tidy.Options.DocType = DocType.Strict;
            tidy.Options.DropFontTags = true;
            tidy.Options.LogicalEmphasis = true;
            tidy.Options.Xhtml = true;
            tidy.Options.XmlOut = true;
            tidy.Options.MakeClean = true;
            tidy.Options.TidyMark = false;
            tidy.Options.DropEmptyParas = true;
            tidy.Options.IndentContent = true;
            tidy.Options.SmartIndent = true;
            tidy.Options.Word2000 = isWordHtml;
            tidy.Options.EncloseBlockText = true;

            tidy.Options.XmlTags = true;
            tidy.Options.FixComments = true;
            TidyMessageCollection tmc = new TidyMessageCollection();
            MemoryStream input = new MemoryStream();
            MemoryStream output = new MemoryStream();

            byte[] byteArray = Encoding.UTF8.GetBytes(htmlSource);
            input.Write(byteArray, 0, byteArray.Length);
            input.Position = 0;
            try
            {
                tidy.Parse(input, output, tmc);
            }
            catch (FormatException ex)
            {
                Log.Exception(ex);
                return htmlSource;
            }
            string cleanContent = Encoding.UTF8.GetString(output.ToArray());
            return cleanContent;
        }

        #endregion IHTMLCleaner Members
    }
}