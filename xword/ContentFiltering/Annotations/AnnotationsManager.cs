using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Clients;
using XWiki.XmlRpc;

namespace XWiki.Annotations
{
    public class AnnotationsManager
    {
        const string ANNOTATION_CLASS_NAME = "AnnotationCode.AnnotationClass";

        public List<Annotation> DownloadAnnotations(IXWikiClient client, String pageFullName)
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
    }
}
