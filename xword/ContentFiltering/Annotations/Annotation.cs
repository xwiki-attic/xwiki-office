using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        private String selectionRightContext;
        private String originalSelection;
        private String author;
        private String target;
        private DateTime date;
        private String state;
        private int id;
        private String pageId;
        private AnnotationClientStatus clientStatus;


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
                return selectionRightContext;
            }
            set
            {
                selectionRightContext = value;
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

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public String PageId
        {
            get
            {
                return pageId;
            }
            set
            {
                pageId = value;
            }
        }

        public AnnotationClientStatus ClientStatus
        {
            get
            {
                return clientStatus;
            }
            set
            {
                clientStatus = value;
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
            annotation.Id = obj.id;
            annotation.PageId = obj.pageId;
            return annotation;
        }

        public XWikiObject ToRcpObject()
        {
            XWikiObject obj = new XWikiObject();
            obj.id = this.id;
            obj.pageId = this.PageId;
            obj.objectDictionary = new CookComputing.XmlRpc.XmlRpcStruct();
            obj.objectDictionary.Add("annotation", this.AnnotationText);
            obj.objectDictionary.Add("date", this.Date);
            obj.objectDictionary.Add("author", this.Author);
            obj.objectDictionary.Add("originalSelection", this.OriginalSelection);
            obj.objectDictionary.Add("selection", this.Selection);
            obj.objectDictionary.Add("selectionRightContext", this.SelectionRightContext);
            obj.objectDictionary.Add("selectionLeftContext", this.SelectionLeftContext);
            obj.objectDictionary.Add("state", this.State);
            obj.objectDictionary.Add("target", this.Target);
            return obj;
        }

        public NameValueCollection ToNameValuePairs()
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("annotation", this.AnnotationText);
            nvc.Add("date", this.Date.ToString());
            nvc.Add("author", this.Author);
            nvc.Add("originalSelection", this.OriginalSelection);
            nvc.Add("selection", this.Selection);
            nvc.Add("selectionRightContext", this.SelectionRightContext);
            nvc.Add("selectionLeftContext", this.SelectionLeftContext);
            nvc.Add("state", this.State);
            nvc.Add("target", this.Target);
            return nvc;
        }
    }
}
