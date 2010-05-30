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
using Word = Microsoft.Office.Interop.Word;
using XWord;
using XWiki.Annotations;


public class AnnotationMaintainer : IAnnotationMaintainer
{

    Dictionary<Annotation, Word.Comment> annotations;

    public AnnotationMaintainer()
    {
        annotations = new Dictionary<Annotation,Word.Comment>();
    }

    #region IAnnotationMaintener Members

    public void RegisterAnnotation(Annotation annotation, Word.Comment comment)
    {
        annotations.Add(annotation, comment);
    }

    public List<Annotation> GetAnnotationsForDocument(string pageId)
    {
        List<Annotation> docAnnotations = new List<Annotation>();
        foreach (Annotation ann in annotations.Keys)
        {
            if (ann.PageId == pageId)
            {
                docAnnotations.Add(ann);
            }
        }
        return docAnnotations;
    }

    /// <summary>
    /// Updates the annotations for the active page
    /// </summary>
    public List<Annotation> UpdateAnnotations()
    {
        String currentPageId = Globals.XWikiAddIn.currentPageFullName;
        List<Annotation> updatedAnnotations = GetAnnotationsForDocument(currentPageId);
        List<Word.Comment> activeDocComments = GetWordComments();
        List<Word.Comment> oldComments;
        List<Word.Comment> newComments;
        FilterWordComments(activeDocComments, out oldComments, out newComments);
        return updatedAnnotations;
    }
    #endregion

    private Annotation GetAnnotationForWordComment(Word.Comment comment)
    {
        foreach (Annotation a in annotations.Keys)
        {
            if (annotations[a] == comment)
            {
                return a; 
            }
        }
        return null;
    }

    private List<Word.Comment> GetWordComments()
    {
        List<Word.Comment> comments = new List<Word.Comment>();
        Word.Comments wComments = Globals.XWikiAddIn.ActiveDocumentInstance.Comments;
        foreach (Word.Comment comment in wComments)
        {
            comments.Add(comment);
        }
        return comments;
    }

    private void FilterWordComments(List<Word.Comment> activeComments, out List<Word.Comment> oldComments, out List<Word.Comment> newComments)
    {
        oldComments = new List<Word.Comment>();
        newComments = new List<Word.Comment>();
        List<Word.Comment> annotationComments = new List<Word.Comment>(annotations.Values);
        foreach (Word.Comment activeComment in activeComments)
        {
            bool isOldComment = false;
            foreach(Word.Comment annotationComment in annotationComments)
            {
                if(activeComment.Index == annotationComment.Index)
                {
                    isOldComment = true;
                }
            }
            if(!isOldComment)
            {
                newComments.Add(activeComment);
            }
        }
    }
}