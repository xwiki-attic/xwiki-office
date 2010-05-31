using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using XWiki.Clients;
using XWiki.XmlRpc;

namespace XWiki.Annotations
{
    public class AnnotationsIO
    {
        const string ANNOTATION_CLASS_NAME = "AnnotationCode.AnnotationClass";

        IXWikiClient client;

        public AnnotationsIO(IXWikiClient client)
        {
            this.client = client;
        }
        
        public List<Annotation> DownloadAnnotations(String pageFullName)
        {
            List<Annotation> annotations = new List<Annotation>();
            XWikiObjectSummary[] objects = client.GetObjects(pageFullName);
            foreach (XWikiObjectSummary objSum in objects)
            {
                if (objSum.className == ANNOTATION_CLASS_NAME)
                {
                    XWikiObject obj = client.GetObject(pageFullName, ANNOTATION_CLASS_NAME, objSum.id);
                    annotations.Add(Annotation.FromRpcObject(obj));
                }
            }
            return annotations;
        }

        public void UpdateAnnotations(List<Annotation> annotations)
        {
            foreach (Annotation annotation in annotations)
            {
                NameValueCollection nvc = annotation.ToNameValuePairs();
                client.UpdateObject(annotation.PageId, ANNOTATION_CLASS_NAME, annotation.Id, nvc);
            }
        }

        public void AddAnnotations(List<Annotation> annotations)
        {
            foreach (Annotation annotation in annotations)
            {
                NameValueCollection nvc = annotation.ToNameValuePairs();
                client.AddObject(annotation.PageId, ANNOTATION_CLASS_NAME, nvc);
            }
        }
    }
}
