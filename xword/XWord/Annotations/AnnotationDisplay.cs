using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = Microsoft.Office.Interop.Word;
using XWiki.Annotations;

namespace XWord.Annotations
{
    class AnnotationDisplay
    {
        private Word.Document document;
        private String clearContent;
        private Dictionary<int,int> deletedCharsMap;

        public AnnotationDisplay(Word.Document doc)
        {
            String[] newLineChars = { "\n", "\r", "\t"};
            this.document = doc;
            String wordContent = document.Content.Text;
            deletedCharsMap = MapDeletedCharsOffsets(wordContent, newLineChars);
            clearContent = ClearContent(wordContent, newLineChars);
        }

        private String ClearContent(String content, String[] newLineChars)
        {            
            foreach (String s in newLineChars)
            {
                content = content.Replace(s, "");
            }
            return content;
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
        private Dictionary<int, int> MapDeletedCharsOffsets(String content, String[] newLineChars)
        {
            Dictionary<int, int> deletedCharsMap = new Dictionary<int, int>();
            int deletedCharsNo = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (newLineChars.Contains(content[i].ToString()))
                {
                    deletedCharsNo++;
                    deletedCharsMap.Add(i - deletedCharsNo + 1, deletedCharsNo);
                }
            }
            return deletedCharsMap;
        }

        private int GetOffset(int clearedContentIndex)
        {
            int offset = 0;
            int minDelta = Int32.MaxValue;
            foreach(int k in deletedCharsMap.Keys)
            {
                int delta = clearedContentIndex - k;
                if ((minDelta > delta) && (k <= clearedContentIndex))
                {
                    minDelta = delta;
                    offset = deletedCharsMap[k];
                }
            }
            return offset;
        }


        /// <summary>
        /// Displays an annotation in a Word document.
        /// </summary>
        /// <param name="annotation"></param>
        public void DisplayAnnotation(Annotation annotation)
        {
            object annotationText = annotation.AnnotationText;
            Word.Range range;
            range = StringSearch(annotation);
            if (range != null)
            {
                Word.Comment comment = document.Comments.Add(range, ref annotationText);
                comment.Author = annotation.Author;
                comment.Initial = annotation.OriginalSelection;
            }
        }

        /// <summary>
        /// Returns the range of an anootation.
        /// </summary>
        /// <param name="annotation">The searched annotation.</param>
        /// <returns>An instance of an Word range.</returns>
        private Word.Range WordSearch(Annotation annotation)
        {
            object _true = true;
            Object missing = Type.Missing;
            object annotationContext = annotation.SelectionLeftContext + annotation.Selection +
                                       annotation.SelectionRightContext;
            object annotatedText = annotation.Selection;
            
            Word.Range range = document.Content;
            range.Find.ClearFormatting();

            //search for the annotation
            bool found = range.Find.Execute(ref annotationContext, ref _true, ref missing,
                           ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                           ref missing, ref missing, ref missing, ref missing, ref missing);
            //refine the range to the annotated text
            range.Find.Execute(ref annotatedText, ref  _true, ref missing,
                           ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                           ref missing, ref missing, ref missing, ref missing, ref missing);
            if (found)
            {
                return range;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the range of an anootation.
        /// </summary>
        /// <param name="annotation">The searched annotation.</param>
        /// <returns>An instance of an Word range.</returns>
        private Word.Range StringSearch(Annotation annotation)
        {
            String annotationContext = annotation.SelectionLeftContext + annotation.Selection +
                                       annotation.SelectionRightContext;
            
            int contextIndex = clearContent.IndexOf(annotationContext);
            object annotationStart = contextIndex + annotationContext.IndexOf(annotation.Selection);
            object annotationEnd = (int)annotationStart + annotation.Selection.Length;

            annotationStart = (int)annotationStart + GetOffset((int)annotationStart);
            annotationEnd = (int)annotationEnd + GetOffset((int)annotationEnd);
            if (contextIndex >= 0)
            {
                return document.Range(ref annotationStart, ref annotationEnd);
            }
            else
            {
                return null;
            }
        }
    }
}
