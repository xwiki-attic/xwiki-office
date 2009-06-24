using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XWiki.Office.Word;
using XWiki.Clients;

namespace ContentFiltering.Test.Util
{
    public class ConversionManagerTestUtil
    {
        private static string serverURL = "http://127.0.0.1:8080";
        private static string localFolder = "localFolder";
        private static string docFullName = "docFullName";
        private static string localFileName = "localFileName";

        /// <summary>
        /// Creates a dummy <code>ConverionManeger</code> to be used by unit tests.
        /// </summary>
        /// <returns>An instance of a dummy <code>ConversionManager</code></returns>
        public static ConversionManager DummyConversionManager()
        {
            IXWikiClient client = null;
            client = XWikiClientTestUtil.CreateMockInstance();
            return new ConversionManager(serverURL, localFolder, docFullName, localFileName, client);
        }
    }
}
