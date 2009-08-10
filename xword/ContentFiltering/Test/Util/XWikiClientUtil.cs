using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Clients;
using NUnit.Mocks;
using XWiki.XmlRpc;
using CookComputing.XmlRpc;

namespace ContentFiltering.Test.Util
{
    public class XWikiClientTestUtil
    {
        const string SSX_CLASS_NAME = "XWiki.StyleSheetExtension";
        const string XOFFICE_SSX = "XOfficeStyle";
        const string PAGE_FULL_NAME = "docFullName";

        public const string CSS_PROPERTIES_XOFFICE0 = "font-family:sans-serif;";
        public const string CSS_CONTENT_XOFFICE0 = ".xoffice0 {" + CSS_PROPERTIES_XOFFICE0 + "}";

        public const string CSS_PROPERTIES_ID1 = "color:red;";
        public const string CSS_CONTENT_ID1 = "#id1 {" + CSS_PROPERTIES_ID1 + "}";


        /// <summary>
        /// Returns an IXWikiClient mock implementation to be used by unit tests.
        /// </summary>
        /// <returns>An IXWikiClient mock instance.</returns>
        public static IXWikiClient CreateMockInstance()
        {
            DynamicMock xWikiClientMock = new DynamicMock(typeof(IXWikiClient));
            xWikiClientMock.ExpectAndReturn("Login", true, new object[2]);
            xWikiClientMock.ExpectAndReturn("GetAvailableSyntaxes", new List<string>() { "XWiki 2.0", "XHTML" });
            xWikiClientMock.ExpectAndReturn("GetPagesNames", new List<string>() { "Page1" }, new object[1]);
            xWikiClientMock.ExpectAndReturn("SavePageHTML", true, new object[3]);
            xWikiClientMock.ExpectAndReturn("AddAttachment", true, new object[2] { PAGE_FULL_NAME, "localFolder\\Document1_TempExport_files/image002.jpg" });
            xWikiClientMock.ExpectAndReturn("GetAttachmentURL", "http://127.0.0.1:8080/xwiki/bin/download/Main/Page1/image002.jpg", new object[] { PAGE_FULL_NAME, "image002.jpg" });


            SetObjSummariesForGetObjects(ref xWikiClientMock);

            SetObjForGetObject(ref xWikiClientMock);

            return (IXWikiClient)xWikiClientMock.MockInstance;
        }


        /// <summary>
        /// Sets the "ExpectAndReturn" for "GetObjects" method: one 'SomeClass' and one SSX object summaries.
        /// </summary>
        /// <param name="xWikiClientMock">A reference to a <code>DynamicMock</code>.</param>
        private static void SetObjSummariesForGetObjects(ref DynamicMock xWikiClientMock)
        {
            XWikiObjectSummary xWikiObjectSummary1 = new XWikiObjectSummary();
            xWikiObjectSummary1.className = "SomeClass";
            xWikiObjectSummary1.id = 1;
            xWikiObjectSummary1.pageId = PAGE_FULL_NAME;

            XWikiObjectSummary xWikiObjectSummary2 = new XWikiObjectSummary();
            xWikiObjectSummary2.className = SSX_CLASS_NAME;
            xWikiObjectSummary2.id = 1;
            xWikiObjectSummary2.pageId = PAGE_FULL_NAME;

            XWikiObjectSummary[] objSummaries = new XWikiObjectSummary[]
            {
                xWikiObjectSummary1,
                xWikiObjectSummary2
            };

            xWikiClientMock.ExpectAndReturn("GetObjects", objSummaries, new object[1] { PAGE_FULL_NAME });
        }

        /// <summary>
        /// Sets the "ExpectAndReturn" for "GetObject" method.
        /// The returned SSX object has: name='XOfficeStyle', use='onDemand', parse='0' and the 'code' is
        /// some CSS for '.xoffice0' class and '#id1" id.
        /// </summary>
        /// <param name="xWikiClientMock">A reference to a <code>DynamicMock</code>.</param>
        private static void SetObjForGetObject(ref DynamicMock xWikiClientMock)
        {
            XmlRpcStruct dictionary = new XmlRpcStruct();
            dictionary.Add("code", CSS_CONTENT_XOFFICE0 + CSS_CONTENT_ID1);
            dictionary.Add("name", "XOfficeStyle");
            dictionary.Add("use", "onDemand");
            dictionary.Add("parse", "0");

            XWikiObject xWikiObject = new XWikiObject();
            xWikiObject.className = SSX_CLASS_NAME;
            xWikiObject.id = 1;
            xWikiObject.pageId = PAGE_FULL_NAME;
            xWikiObject.prettyName = "XOfficeStyle";
            xWikiObject.objectDictionary = dictionary;

            xWikiClientMock.ExpectAndReturn("GetObject", xWikiObject, new object[3] { PAGE_FULL_NAME, SSX_CLASS_NAME, 1 });
        }
    }


}
