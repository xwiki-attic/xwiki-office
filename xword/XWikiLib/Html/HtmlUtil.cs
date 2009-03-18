using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using TidyNet;
using TidyNet.Dom;

namespace XWiki.Html
{
    /// <summary>
    /// Provides functionalities for cleaning and repairing html code.
    /// </summary>
    public class HtmlUtil
    {
        /// <summary>
        /// Cleans a given html code. 
        /// </summary>
        /// <param name="htmlSource">The html code.</param>
        /// <returns>The cleaned html code.</returns>
        public String HtmlToXhtml(String htmlSource)
        {
            return CleanHTML(htmlSource, false);            
        }

        /// <summary>
        /// Cleans a Word html source.
        /// </summary>
        /// <param name="htmlSource">The initila html source.</param>
        /// <returns>The cleaned html.</returns>
        public String WordHtmlToXhtml(String htmlSource)
        {
            return CleanHTML(htmlSource, true);            
        }

        /// <summary>
        /// Uses Tidy.Net to clean a html source.
        /// </summary>
        /// <param name="htmlSource">The original html source.</param>
        /// <param name="isWordHtml">Specifies if the source is an output from Microsoft Word</param>
        /// <returns>The cleaned Html.</returns>
        public String CleanHTML(String htmlSource,bool isWordHtml)
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
                return htmlSource;
            }
            string cleanContent = Encoding.UTF8.GetString(output.ToArray());
            return cleanContent;
        }

        /// <summary>
        /// Gets a list with all the tags that contain attributes.
        /// </summary>
        /// <param name="htmlSource">The html source.</param>
        /// <returns>A of strings with the tags containing attributes.</returns>
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

        /// <summary>
        /// Corrects the attributes that miss ' or ".
        /// </summary>
        /// <param name="htmlSource">The original html source code.</param>
        /// <returns>The source with corrected attributes.</returns>
        public String CorrectAttributes(String htmlSource)
        {
            StringBuilder sb = new StringBuilder(htmlSource);
            List<String> tags = GetTagsWithAttributes(htmlSource);
            foreach(String initialValue in tags)
            {
                String value = initialValue;
                char[] separators = {' ','>','/','\r'};
                bool hasChanged = false;
                foreach (String s in initialValue.Split(separators))
                {
                    String[] attribute = s.Split('=');
                    if(attribute.Length == 2)
                    {
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

        /// <summary>
        /// Removes the tags that are in the office namespaces.
        /// </summary>
        /// <param name="content">The original content.</param>
        /// <returns>The cleaned content.</returns>
        public String RemoveOfficeNameSpacesTags(String content)
        {
            bool foundTags = false;
            int startIndex = 0;
            int endIndex = 0;
            do
            {
                foundTags = false;
                startIndex = content.IndexOf("<o:", startIndex);
                if (startIndex >= 0)
                {
                    endIndex = content.IndexOf("</o:", startIndex);
                    if(endIndex >= 0)
                    {
                        endIndex = content.IndexOf(">",endIndex + 1);
                        content = content.Remove(startIndex, endIndex - startIndex + 1);                       
                    }
                    foundTags = true;
                    startIndex = endIndex - (endIndex - startIndex + 1);
                }                
            } while (foundTags);
            return content;
        }

        /// <summary>
        /// Removes a char sequence that starts and ends with the given valaues.
        /// </summary>
        /// <param name="content">The initial content.</param>
        /// <param name="tagBegining">The begining of the char sequence.</param>
        /// <param name="tagEnding">The end of the char sequence.</param>
        /// <returns></returns>
        public String RemoveSpecificTagContent(String content, String tagBegining, String tagEnding)
        {
            bool foundTags = false;
            int startIndex = 0;
            int endIndex = 0;
            do
            {
                foundTags = false;
                startIndex = content.IndexOf(tagBegining, startIndex);
                if (startIndex >= 0)
                {
                    endIndex = content.IndexOf(tagEnding, startIndex + tagBegining.Length);
                    if (endIndex >= 0)
                    {
                        content = content.Remove(startIndex, endIndex - startIndex + tagEnding.Length);
                    }
                    foundTags = true;
                    startIndex = endIndex - (endIndex - startIndex + 1);
                }
            } while (foundTags);
            return content;
        }

        /// <summary>
        /// Gets the content between the opening and closing html tags.
        /// </summary>
        /// <param name="htmlCode">The html source to be </param>
        /// <returns>the inner html of the body.</returns>
        public String GetBodyContent(String htmlCode)
        {
            //Delete header & footer
            int startIndex, endIndex;
            startIndex = htmlCode.IndexOf("<body");
            endIndex = htmlCode.IndexOf(">", startIndex);
            htmlCode = htmlCode.Remove(0, endIndex + 1);
            startIndex = htmlCode.IndexOf("</body");
            if (startIndex >= 0)
                htmlCode = htmlCode.Remove(startIndex);
            return htmlCode;
        }

        /// <summary>
        /// Indents the given html source.
        /// </summary>
        /// <param name="htmlSource">The html source.</param>
        /// <returns>A string with the new source.</returns>
        public String IndentContent(String htmlSource)
        {
            Tidy tidy = new Tidy();
            tidy.Options.IndentContent = true;
            TidyMessageCollection tmc = new TidyMessageCollection();
            MemoryStream input = new MemoryStream();
            MemoryStream output = new MemoryStream();

            byte[] byteArray = Encoding.UTF8.GetBytes(htmlSource);
            input.Write(byteArray, 0, byteArray.Length);
            input.Position = 0;
            tidy.Parse(input, output, tmc);

            htmlSource = Encoding.UTF8.GetString(output.ToArray());
            return htmlSource;
        }

        /// <summary>
        /// Removes the doctype declaration from an given html code.
        /// </summary>
        /// <param name="htmlCode">The original html code.</param>
        /// <returns>The modified html code.</returns>
        public String RemoveDoctype(String htmlCode)
        {
            int startIndex, endIndex;
            startIndex = htmlCode.IndexOf("<!DOCTYPE");
            endIndex = htmlCode.IndexOf(">", startIndex);
            return htmlCode.Remove(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// Gets a string representing the opening html tag with the XML namespace definitions, if any.
        /// </summary>
        /// <param name="htmlCode">The html source to be processed</param>
        /// <returns>a string representing the opening html tag.</returns>
        public String GetXmlNamespaceDefinitions(String htmlCode)
        {
            int startIndex, endIndex;
            startIndex = htmlCode.IndexOf("<html");

            if (startIndex < 0)
            {
                return null;
            }
            else
            {
                endIndex = htmlCode.IndexOf(">", startIndex);
                return htmlCode.Substring(startIndex, endIndex - startIndex + 1);
            }
        }

        /// <summary>
        /// Replaces the opening html tag with a given one.
        /// </summary>
        /// <param name="htmlCode">The html source.</param>
        /// <param name="newHtmlTag">The new html tag.</param>
        /// <returns></returns>
        public String ReplaceXmlNamespaceDefinitions(String htmlCode, String newHtmlTag)
        {
            String oldHtmlTag = GetXmlNamespaceDefinitions(htmlCode);
            if (oldHtmlTag == null)
            {
                if (!htmlCode.Contains("<body"))
                {
                    htmlCode = htmlCode.Insert(0, "<body>");
                    htmlCode = htmlCode.Insert(htmlCode.Length, "</body>");
                }
                htmlCode = htmlCode.Insert(0, newHtmlTag);
                htmlCode = htmlCode.Insert(htmlCode.Length, "</html>");
            }
            else
            {
                htmlCode = htmlCode.Replace(oldHtmlTag, newHtmlTag);            
            }
            return htmlCode;
        }

        /// <summary>
        /// Replaces the body tag with a new given one.
        /// </summary>
        /// <param name="initialContent">The initial html code.</param>
        /// <param name="newBodyTag">The new body tag.</param>
        /// <returns>The new html code.</returns>
        public String ReplaceBody(String initialContent, String newBodyTag)
        {
            int startIndex, endIndex;
            startIndex = initialContent.IndexOf("<body");
            endIndex = initialContent.IndexOf(">", startIndex);
            String body = initialContent.Substring(startIndex, endIndex - startIndex + 1);
            return initialContent.Replace(body, newBodyTag);
        }
    }    
}
