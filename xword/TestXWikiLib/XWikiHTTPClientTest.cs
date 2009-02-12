using XWiki.Clients;
using Testing = Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Net;

namespace TestXWikiLib
{
    
    
    /// <summary>
    ///This is a test class for XWikiHTTPClientTest and is intended
    ///to contain all XWikiHTTPClientTest Unit Tests
    ///</summary>
    [TestClass()]
    public class XWikiHTTPClientTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {

        }
        
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {

        }
        
        #endregion


        /// <summary>
        ///A test for WebClient
        ///</summary>
        [TestMethod()]
        public void WebClientTest()
        {
            string serverURL = "http://localhost:8080";
            string username = "Admin";
            string password = "admin"; 
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password);
            Assert.IsNotNull(target.WebClient);
        }

        /// <summary>
        ///A test for ServerURL
        ///</summary>
        [TestMethod()]
        public void ServerURLTest()
        {
            string serverURL = "http://localhost:8080";
            string username = "Admin";
            string password = "admin";
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password);
            string actual = target.ServerURL;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual, serverURL);
        }

        /// <summary>
        ///A test for Headers
        ///</summary>
        [TestMethod()]
        public void HeadersTest()
        {
            string serverURL = "http://localhost:8080";
            string username = "Admin";
            string password = "admin";
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password);
            WebHeaderCollection actual = target.Headers;
            Assert.IsNotNull(target);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for Cookies
        ///</summary>
        [TestMethod()]
        public void CookiesTest()
        {
            string serverURL = "http://localhost:8080";
            string username = "Admin";
            string password = "admin";
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password);
            List<String> actual = target.Cookies;
            Assert.IsNotNull(target);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for UploadValues
        ///</summary>
        [TestMethod(),ExpectedException(typeof(ArgumentException))]
        public void UploadValuesTest1()
        {
            string serverURL = "http://localhost:8080";
            string username = "Admin";
            string password = "admin";
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password);
            target.WebClient = new WebClientMock();
            string address = string.Empty;
            string method = string.Empty;
            NameValueCollection values = null;
            byte[] expected = WebClientMock.responseOK;
            byte[] actual;
            actual = target.UploadValues(address, method, values);
        }

        /// <summary>
        ///A test for UploadValues
        ///</summary>
        [TestMethod()]
        public void UploadValuesTest2()
        {
            string serverURL = "http://localhost:8080";
            string username = "Admin";
            string password = "admin";
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password);
            target.WebClient = new WebClientMock();
            string address = "http://localhost:8080";
            string method = "POST";
            NameValueCollection values = new NameValueCollection();
            byte[] expected = WebClientMock.responseOK;
            byte[] actual;
            actual = target.UploadValues(address, method, values);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UploadValues
        ///</summary>
        [TestMethod()]
        public void UploadValuesTest()
        {
            string serverURL = "http://localhost:8080";
            string username = "Admin";
            string password = "admin";
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password);
            target.WebClient = new WebClientMock();
            string address = "http://localhost:8080";
            NameValueCollection values = new NameValueCollection();
            byte[] expected = WebClientMock.responseOK;
            byte[] actual;
            actual = target.UploadValues(address, values);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UploadData
        ///</summary>
        [TestMethod()]
        public void UploadDataTest3()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string address = string.Empty; // TODO: Initialize to an appropriate value
            byte[] data = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.UploadData(address, data);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UploadData
        ///</summary>
        [TestMethod()]
        public void UploadDataTest2()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string address = string.Empty; // TODO: Initialize to an appropriate value
            string method = string.Empty; // TODO: Initialize to an appropriate value
            byte[] data = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.UploadData(address, method, data);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UploadData
        ///</summary>
        [TestMethod()]
        public void UploadDataTest1()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            Uri address = null; // TODO: Initialize to an appropriate value
            string method = string.Empty; // TODO: Initialize to an appropriate value
            byte[] data = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.UploadData(address, method, data);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UploadData
        ///</summary>
        [TestMethod()]
        public void UploadDataTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            Uri address = null; // TODO: Initialize to an appropriate value
            byte[] data = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.UploadData(address, data);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SavePageHTML
        ///</summary>
        [TestMethod()]
        public void SavePageHTMLTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string docName = string.Empty; // TODO: Initialize to an appropriate value
            string content = string.Empty; // TODO: Initialize to an appropriate value
            string syntax = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.SavePageHTML(docName, content, syntax);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ParseList
        ///</summary>
        [TestMethod()]
        [DeploymentItem("XWikiLib.dll")]
        public void ParseListTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            XWikiHTTPClient_Accessor target = new XWikiHTTPClient_Accessor(param0); // TODO: Initialize to an appropriate value
            string input = string.Empty; // TODO: Initialize to an appropriate value
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            actual = target.ParseList(input);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for OpenRead
        ///</summary>
        [TestMethod()]
        public void OpenReadTest1()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string address = string.Empty; // TODO: Initialize to an appropriate value
            Stream expected = null; // TODO: Initialize to an appropriate value
            Stream actual;
            actual = target.OpenRead(address);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for OpenRead
        ///</summary>
        [TestMethod()]
        public void OpenReadTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            Uri address = null; // TODO: Initialize to an appropriate value
            Stream expected = null; // TODO: Initialize to an appropriate value
            Stream actual;
            actual = target.OpenRead(address);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Login
        ///</summary>
        [TestMethod()]
        [DeploymentItem("XWikiLib.dll")]
        public void LoginTest1()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            XWikiHTTPClient_Accessor target = new XWikiHTTPClient_Accessor(param0); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Login();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Login
        ///</summary>
        [TestMethod()]
        public void LoginTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string username1 = string.Empty; // TODO: Initialize to an appropriate value
            string password1 = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Login(username1, password1);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for InsertCookies
        ///</summary>
        [TestMethod()]
        [DeploymentItem("XWikiLib.dll")]
        public void InsertCookiesTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            XWikiHTTPClient_Accessor target = new XWikiHTTPClient_Accessor(param0); // TODO: Initialize to an appropriate value
            target.InsertCookies();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetWikiStructure
        ///</summary>
        [TestMethod()]
        public void GetWikiStructureTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            Stream expected = null; // TODO: Initialize to an appropriate value
            Stream actual;
            actual = target.GetWikiStructure();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetWikiPageAsPlainHTML
        ///</summary>
        [TestMethod()]
        public void GetWikiPageAsPlainHTMLTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string docName = string.Empty; // TODO: Initialize to an appropriate value
            Stream expected = null; // TODO: Initialize to an appropriate value
            Stream actual;
            actual = target.GetWikiPageAsPlainHTML(docName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetURL
        ///</summary>
        [TestMethod()]
        public void GetURLTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string documentFullName = string.Empty; // TODO: Initialize to an appropriate value
            string xwikiAction = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetURL(documentFullName, xwikiAction);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSpaces
        ///</summary>
        [TestMethod()]
        public void GetSpacesTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            actual = target.GetSpaces();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetRenderedPageContent
        ///</summary>
        [TestMethod()]
        public void GetRenderedPageContentTest1()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string pageFullName = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetRenderedPageContent(pageFullName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetPages
        ///</summary>
        [TestMethod()]
        public void GetPagesTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string spaceName = string.Empty; // TODO: Initialize to an appropriate value
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            actual = target.GetPages(spaceName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetDocumentAttachmentList
        ///</summary>
        [TestMethod()]
        public void GetDocumentAttachmentListTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string docFullName = string.Empty; // TODO: Initialize to an appropriate value
            List<string> expected = null; // TODO: Initialize to an appropriate value
            List<string> actual;
            actual = target.GetDocumentAttachmentList(docFullName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCookie
        ///</summary>
        [TestMethod()]
        [DeploymentItem("XWikiLib.dll")]
        public void GetCookieTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            XWikiHTTPClient_Accessor target = new XWikiHTTPClient_Accessor(param0); // TODO: Initialize to an appropriate value
            string searchedCookie = string.Empty; // TODO: Initialize to an appropriate value
            string cookiesString = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetCookie(searchedCookie, cookiesString);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAttachmentURL
        ///</summary>
        [TestMethod()]
        public void GetAttachmentURLTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string docFullName = string.Empty; // TODO: Initialize to an appropriate value
            string attachmentName = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetAttachmentURL(docFullName, attachmentName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAttachmentContent
        ///</summary>
        [TestMethod()]
        public void GetAttachmentContentTest1()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string URL = string.Empty; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.GetAttachmentContent(URL);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAttachmentContent
        ///</summary>
        [TestMethod()]
        public void GetAttachmentContentTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string pageName = string.Empty; // TODO: Initialize to an appropriate value
            string fileName = string.Empty; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.GetAttachmentContent(pageName, fileName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DownloadData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("XWikiLib.dll")]
        public void DownloadDataTest1()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            XWikiHTTPClient_Accessor target = new XWikiHTTPClient_Accessor(param0); // TODO: Initialize to an appropriate value
            string address = string.Empty; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.DownloadData(address);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DownloadData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("XWikiLib.dll")]
        public void DownloadDataTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            XWikiHTTPClient_Accessor target = new XWikiHTTPClient_Accessor(param0); // TODO: Initialize to an appropriate value
            Uri address = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.DownloadData(address);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ConvertEncoding
        ///</summary>
        [TestMethod()]
        [DeploymentItem("XWikiLib.dll")]
        public void ConvertEncodingTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            XWikiHTTPClient_Accessor target = new XWikiHTTPClient_Accessor(param0); // TODO: Initialize to an appropriate value
            string content = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ConvertEncoding(content);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AddObject
        ///</summary>
        [TestMethod()]
        public void AddObjectTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string docName = string.Empty; // TODO: Initialize to an appropriate value
            string ClassName = string.Empty; // TODO: Initialize to an appropriate value
            NameValueCollection fieldsValues = null; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.AddObject(docName, ClassName, fieldsValues);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AddAttachmentAsync
        ///</summary>
        [TestMethod()]
        public void AddAttachmentAsyncTest1()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string space = string.Empty; // TODO: Initialize to an appropriate value
            string page = string.Empty; // TODO: Initialize to an appropriate value
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.AddAttachmentAsync(space, page, filePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AddAttachmentAsync
        ///</summary>
        [TestMethod()]
        public void AddAttachmentAsyncTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string docName = string.Empty; // TODO: Initialize to an appropriate value
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.AddAttachmentAsync(docName, filePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AddAttachment
        ///</summary>
        [TestMethod()]
        public void AddAttachmentTest2()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string docName = string.Empty; // TODO: Initialize to an appropriate value
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.AddAttachment(docName, filePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AddAttachment
        ///</summary>
        [TestMethod()]
        public void AddAttachmentTest1()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string docName = string.Empty; // TODO: Initialize to an appropriate value
            string fileName = string.Empty; // TODO: Initialize to an appropriate value
            Stream content = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.AddAttachment(docName, fileName, content);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AddAttachment
        ///</summary>
        [TestMethod()]
        public void AddAttachmentTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string space = string.Empty; // TODO: Initialize to an appropriate value
            string page = string.Empty; // TODO: Initialize to an appropriate value
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.AddAttachment(space, page, filePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for XWikiHTTPClient Constructor
        ///</summary>
        [TestMethod()]
        public void XWikiHTTPClientConstructorTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for GetRenderedPageContent
        ///</summary>
        [TestMethod()]
        public void GetRenderedPageContentTest()
        {
            string serverURL = string.Empty; // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            XWikiHTTPClient target = new XWikiHTTPClient(serverURL, username, password); // TODO: Initialize to an appropriate value
            string space = string.Empty; // TODO: Initialize to an appropriate value
            string page = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetRenderedPageContent(space, page);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
