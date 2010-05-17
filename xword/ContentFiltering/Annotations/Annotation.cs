using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.XmlRpc;

namespace XWiki.Annotations
{
    public class Annotation
    {

        private String annotationText;
        private String selection;
        private String selectionLeftContext;
        private String selectionRightCOntext;
        private String originalSelection;
        private String author;
        private String target;
        private DateTime date;
        private String state;


        public String AnnotationText
        {
            get
            {
                return annotationText;
            }
            set
            {
                annotationText = value;
            }
        }

        public String Author
        {
            get
            {
                return author;
            }
            set
            {
                author = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }

        public String OriginalSelection
        {
            get
            {
                return originalSelection;
            }
            set
            {
                originalSelection = value;
            }
        }

        public String Selection
        {
            get
            {
                return selection;
            }
            set
            {
                selection = value;
            }
        }

        public String SelectionLeftContext
        {
            get
            {
                return selectionLeftContext;
            }
            set
            {
                selectionLeftContext = value;
            }
        }

        public String SelectionRightContext
        {
            get
            {
                return selectionRightCOntext;
            }
            set
            {
                selectionRightCOntext = value;
            }
        }

        public String State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public String Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        public static Annotation FromRpcObject(XWikiObject obj)
        {
            Annotation annotation = new Annotation();
            annotation.AnnotationText = (string) obj.objectDictionary["annotation"];
            annotation.Author = (string) obj.objectDictionary["author"];
            annotation.Date = (DateTime)obj.objectDictionary["date"];
            annotation.OriginalSelection = (string) obj.objectDictionary["originalSelection"];
            annotation.Selection = (string) obj.objectDictionary["selection"];
            annotation.SelectionRightContext = (string) obj.objectDictionary["selectionRightContext"];
            annotation.SelectionLeftContext = (string) obj.objectDictionary["selectionLeftContext"];
            annotation.State = (string) obj.objectDictionary["state"];
            annotation.Target = (string) obj.objectDictionary["target"];
            return annotation;
        }
    }
}
