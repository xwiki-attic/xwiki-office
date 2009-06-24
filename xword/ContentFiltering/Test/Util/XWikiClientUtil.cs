using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Clients;
using NUnit.Mocks;

namespace ContentFiltering.Test.Util
{
    public class XWikiClientTestUtil
    {
        /// <summary>
        /// Returns an IXWikiClient mock implementation to be used by unit tests.
        /// </summary>
        /// <returns>An IXWikiClient mock instance.</returns>
        public static IXWikiClient CreateMockInstance()
        {
            DynamicMock myXWikiClientMock=new DynamicMock(typeof(IXWikiClient));
            myXWikiClientMock.ExpectAndReturn("Login", true,new object[2]);
            myXWikiClientMock.ExpectAndReturn("GetAvailableSyntaxes", new List<string>() { "XWiki 2.0", "XHTML" });
            myXWikiClientMock.ExpectAndReturn("GetPagesNames", new List<string>() { "Page1" }, new object[1]);
            myXWikiClientMock.ExpectAndReturn("SavePageHTML", true,new object[3]);
            myXWikiClientMock.ExpectAndReturn("AddAttachment", true, new object[2] { "docFullName", "localFolder\\Document1_TempExport_files/image002.jpg" });
            myXWikiClientMock.ExpectAndReturn("GetAttachmentURL", "http://127.0.0.1:8080/xwiki/bin/download/Main/Page1/image002.jpg", new object[] { "docFullName","image002.jpg" });

            return (IXWikiClient)myXWikiClientMock.MockInstance;
        }
    }

 
}
