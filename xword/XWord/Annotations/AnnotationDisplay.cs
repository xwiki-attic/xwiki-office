using System;
using System.Collections.Generic;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;
using XWiki;
using XWiki.Annotations;
using XWord.Annotations;

namespace XWord.Annotations
{
    class AnnotationDisplay
    {
        private Word.Document document;
        private String clearContent;
        private Dictionary<int,int> deletedCharsMap;
        private const int MAX_LENGHT = 255;
        List<Word.Comment> displayedAnnotations;
        
        public AnnotationDisplay(Word.Document doc)
        {
            this.document = doc;
            displayedAnnotations = new List<Microsoft.Office.Interop.Word.Comment>();
            document.TextLineEnding = Microsoft.Office.Interop.Word.WdLineEndingType.wdLFOnly;
            document.Content.TextRetrievalMode.IncludeFieldCodes = true;
            document.Content.TextRetrievalMode.IncludeHiddenText = true;
            deletedCharsMap = document.Content.MapDeletedCharsOffsets();
            clearContent = document.Content.GetCleanedText();
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
                displayedAnnotations.Add(comment);
                Globals.XWikiAddIn.AnnotationMaintainer.RegisterAnnotation(annotation, comment);
                annotation.ClientStatus = AnnotationClientStatus.Unchanged;
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
            //required by COM interop
            object startOffset = GetOffset((int)annotationStart);
            object endOffset = GetOffset((int)annotationEnd);
            annotationStart = (int)annotationStart + (int)startOffset;
            annotationEnd = (int)annotationEnd + (int)endOffset;
            if (contextIndex >= 0)
            {
                Word.Range range = document.Range(ref annotationStart, ref annotationEnd);
                return AdjustRange(range, annotation);
            }
            else
            {
                return null;
            }   
        }

        private Word.Range AdjustRange(Word.Range range, Annotation annotation)
        {
            String initialText = range.GetCleanedText();
            bool match = IsMatch(range, annotation);
            String rangeText = "";
            int offset = Int32.MinValue;
            object rangeStart = range.Start;
            object rangeEnd = range.End;
            int rangeLength = (int)rangeEnd - (int)rangeStart;
            object selectionStart = annotation.Selection[0];
            try
            {
                while (!match)
                {
                    offset = range.Text.IndexOf((char)selectionStart);
                    object start = range.Start + (int)offset;
                    //object end = range.End + (int)offset;
                    object end = range.Start + rangeLength + (int)offset;
                    do
                    {
                        //extend the range to the selection length
                        range = document.Range(ref start, ref end);
                        range.TextRetrievalMode.IncludeHiddenText = false;
                        rangeText = range.GetCleanedText();
                        end = (int)end + 1;
                    } while (rangeText.Length != annotation.Selection.Length);
                    if (IsMatch(range, annotation))
                    {
                        match = true;
                    }
                    else if (offset < 0)
                    {
                        //if not found return initial range
                        return document.Range(ref rangeStart, ref rangeEnd);
                    }
                    else if (offset == 0)
                    {
                        start = (int)start + 1;
                        end = (int)start + 1;
                        range = document.Range(ref start, ref end);
                    }
                }
            }
            catch (NullReferenceException nre)
            {
                Log.Exception(nre);
                return null;
            }
            return range;
        }

        /// <summary>
        /// Determines if a Word range is matching the selected text for an annotation
        /// </summary>
        /// <param name="range">The Word range to be verified.</param>
        /// <param name="annotation">The displayed annotation.</param>
        /// <returns>True if the range contains the annotated text and does not start with white spaces.</returns>
        private bool IsMatch(Word.Range range, Annotation annotation)
        {
            String rangeText = range.GetCleanedText();
            return (rangeText == annotation.Selection) && (range.Text[0] == annotation.Selection[0]);
        }
    }
}
