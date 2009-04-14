using System;
using System.Collections.Generic;
using System.Linq;
using Text = System.Text;
using System.Net;
using System.Collections;
using System.IO;


namespace XWiki.Clients
{
    /// <summary>
    /// Adaptor over System.Net.WebClient
    /// Uses plain HTTP calls to send and retrieve data from the XWiki server
    /// Service pages on the server are in the MSOffice space
    /// </summary>
    public class XWikiHTTPClient : IXWikiClient
    {
        WebClient webClient;

        private string serverURL;
        private List<String> cookies;
        private string username;
        private string password;
        private String[] xWikiCookies = {"JSESSIONID",
                                          "username",
                                          "password",
                                          "rememeberme",
                                          "validation"};
        private bool isLoggedIn;
        private Text.Encoding encoding;
        /// <param name="serverURL">The url of the server.</param>
        /// <param name="username">The username used to authenticate.</param>
        /// <param name="password">The passowrd used to authenticate.</param>
        public XWikiHTTPClient(String serverURL, String username, String password)
        {
            this.serverURL = serverURL;
            this.username = username;
            this.password = password;
            cookies = new List<string>();
            webClient = new WebClient();
            isLoggedIn = Login();
        }

        /// <summary>
        /// Inserts the authetication cookies into the request headers.
        /// </summary>
        private void InsertCookies()
        {
            foreach (String s in Cookies)
            {
                webClient.Headers.Add("Cookie", s);
            }
        }

        /// <summary>
        /// String collection containing the client's cookies.
        /// </summary>
        public List<String> Cookies
        {
            get { return cookies; }
            set { cookies = value; }
        }

        /// <summary>
        /// The WebClient instance.
        /// </summary>
        public WebClient WebClient
        {
            get { return webClient; }
            set { webClient = value; }
        }

        /// <summary>
        /// The webclient's headers.
        /// </summary>
        public WebHeaderCollection Headers
        {
            get { return webClient.Headers; }
            set { webClient.Headers = value; }
        }

        /// <summary>
        /// The url of the server that the client connects to.
        /// </summary>
        public string ServerURL
        {
            get { return serverURL; }
            set { serverURL = value; }
        }        

        /// <summary>
        /// Authenticates the user to the server.
        /// </summary>
        /// <returns>True if the opration succeded. False if the operation failed.</returns>
        private bool Login()
        {
            try
            {
                String targetURL = ServerURL + XWikiURLs.LoginURL;
                targetURL += "?j_username=" + username + "&j_password=" + password + "&j_rememberme=true&xpage=plain";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetURL);
                request.UserAgent = "XWriter - .NET Framework Client";
                request.Method = "POST";
                request.AllowAutoRedirect = false;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                
                String cookiesString = response.Headers.Get("Set-Cookie");
                foreach (String s in xWikiCookies)
                {
                    Cookies.Add(GetCookie(s, cookiesString));
                }
            }
            catch (Exception)
            {
                isLoggedIn = false;
                return false;
            }
            isLoggedIn = true;
            try
            {
                encoding = Encoding();
            }
            catch (InvalidEncodingNameException ex)
            {
                encoding = null;
            }
            return true;
        }

        /// <summary>
        /// Parses the cookies for the server response and returns the requested value.
        /// <remarks>Returns "" if the searched value is not found.</remarks>
        /// </summary>
        /// <param name="searchedCookie"></param>
        /// <param name="cookiesString"></param>
        /// <returns></returns>
        private String GetCookie(String searchedCookie, String cookiesString)
        {
            int index = cookiesString.IndexOf(searchedCookie);
            if (index < 0)
                return "";
            int middleIndex = cookiesString.IndexOf("=", index);
            int lastIndex = cookiesString.IndexOf(";", index);
            String value = cookiesString.Substring(middleIndex + 1, lastIndex - middleIndex - 1);
            String headerString = cookiesString.Substring(index, lastIndex - index);
            return headerString;
        }

        /// <summary>
        /// Gets the encoding of the wiki instance.
        /// </summary>
        /// <returns>The character encoding from the XWiki server.</returns>
        private Text.Encoding Encoding()
        {
            Text.Encoding enc;

            InsertCookies();
            String uri = ServerURL + XWikiURLs.GetEncoding;
            Stream data = webClient.OpenRead(uri);
            StreamReader reader = new StreamReader(data);
            String response = reader.ReadToEnd();
            data.Close();
            reader.Close();
            try
            {
                enc = Text.Encoding.GetEncoding(response);
            }
            catch (ArgumentException e)
            {
                // TODO: Log this error
                throw new InvalidEncodingNameException();
            }
            return enc;
        }

        /// <summary>
        /// Starts reading from an address.
        /// </summary>
        /// <param name="address">The adress.</param>
        /// <returns>A stream with the read data.</returns>
        public Stream OpenRead(String address)
        {
            InsertCookies();
            return webClient.OpenRead(address);
        }

        /// <summary>
        /// Starts reading from an address.
        /// </summary>
        /// <param name="address">The adress.</param>
        /// <returns>A stream with the read data.</returns>
        public Stream OpenRead(Uri address)
        {
            InsertCookies();
            return webClient.OpenRead(address);
        }

        /// <summary>
        /// Uploads(POSTs) the values to the server.
        /// </summary>
        /// <param name="address">The adress where the values are uploaded.</param>
        /// <param name="values">Name value collection with the upload data.</param>
        /// <returns>A stream with the server response.</returns>
        public byte[] UploadValues(string address, System.Collections.Specialized.NameValueCollection values)
        {
            InsertCookies();
            return webClient.UploadValues(address, values);
        }

        /// <summary>
        /// Uploads the values to the server.
        /// </summary>
        /// <param name="address">The adress where the values are uploaded.</param>
        /// <param name="method">The mothod used to send data to the server.</param>
        /// <param name="values">Name value collection with the upload data.</param>
        /// <returns>A stream with the server response.</returns>
        public byte[] UploadValues(string address,string method,System.Collections.Specialized.NameValueCollection values)
        {
            InsertCookies();
            return webClient.UploadValues(address, method, values);
        }

        /// <summary>
        /// Uploads data to the server.
        /// </summary>
        /// <param name="address">The adress where the data will be uploaded.</param>
        /// <param name="data">The data that will be uploaded.</param>
        /// <returns>The server response.</returns>
        public byte[] UploadData(string address, byte[] data)
        {
            InsertCookies();
            return webClient.UploadData(address, data);
        }

        /// <summary>
        /// Uploads data to the server.
        /// </summary>
        /// <param name="address">The adress where the data will be uploaded.</param>
        /// <param name="data">The data that will be uploaded.</param>
        /// <returns>The server response.</returns>
        public byte[] UploadData(Uri address, byte[] data)
        {
            InsertCookies();
            return webClient.UploadData(address, data);
        }

        /// <summary>
        /// Uploads data to the server.
        /// </summary>
        /// <param name="address">The adress where the data will be uploaded.</param>
        /// <param name="method">The web method used to upload the data.</param>
        /// <param name="data">The data that will be uploaded.</param>
        /// <see cref="System.Net.WebClient.UploadData(string, string, byte[])"/>
        /// <returns>The server response.</returns>
        public byte[] UploadData(string address, string method, byte[] data)
        {
            InsertCookies();
            return webClient.UploadData(address, method, data);
        }

        /// <summary>
        /// Uploads data to the server.
        /// </summary>
        /// <param name="address">The adress where the data will be uploaded.</param>
        /// <param name="method">The web method used to upload the data.</param>
        /// <param name="data">The data that will be uploaded.</param>
        /// <returns>The server response.</returns>
        public byte[] UploadData(Uri address, string method, byte[] data)
        {
            InsertCookies();
            return webClient.UploadData(address, method, data);
        }

        /// <summary>
        /// Downloads data from the server.
        /// </summary>
        /// <param name="address">The adress of the resource.</param>
        /// <returns>A byte array with the downloaded data.</returns>
        private byte[] DownloadData(Uri address)
        {
            InsertCookies();
            return webClient.DownloadData(address);
        }

        /// <summary>
        /// Downloads data from the server.
        /// </summary>
        /// <param name="address">The adress of the resource.</param>
        /// <returns>A byte array with the downloaded data.</returns>
        private byte[] DownloadData(String address)
        {
            InsertCookies();
            return webClient.DownloadData(address);
        }

        /// <summary>
        /// Parses a velocity response into a list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private List<String> ParseList(String input)
        {
            input = input.Replace("\n", "");
            input = input.Replace("\t", "");
            input = input.Replace("<p/>","");
            String[] array = input.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<String> list = new List<string>();
            foreach (String s in array)
            {
                if (!(s.Equals("") || s.Equals(Environment.NewLine)))
                {
                    list.Add(s);
                }
            }
            return list;
        }

        /// <summary>
        /// Converts a string to a specified encoding.
        /// </summary>
        /// <param name="content">The string to be converted.</param>
        /// <returns>The convertes string.</returns>
        private String ConvertEncoding(string content)
        {
            Text.Encoding iso = Text.Encoding.GetEncoding("ISO-8859-1");
            Text.Encoding unicode = Text.Encoding.UTF8;
            byte[] byteContent = unicode.GetBytes(content);
            return iso.GetString(byteContent);
        }

        #region IXWikiClient Members

        /// <summary>
        /// Specifies if the current user is logged in.
        /// </summary>
        public bool LoggedIn
        {
            get
            {
                return isLoggedIn;
            }
        }

        /// <summary>
        /// Specifies if the current encoding from the XWiki server.
        /// </summary>
        public Text.Encoding ServerEncoding
        {
            get
            {
                if (encoding == null)
                {
                    try
                    {
                        encoding = Encoding();
                    }
                    catch (InvalidEncodingNameException ex)
                    {
                        encoding = Text.Encoding.GetEncoding("ISO-8859-1");
                    }
                }
                return encoding;
            }
        }

        /// <summary>
        /// Authenticates the user to the server.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if the operation succeded. False if the operation failed.</returns>
        public bool Login(string username, string password)
        {
            try
            {
                this.username = username;
                this.password = password;
                Login();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the spaces and pages of the wiki.
        /// </summary>
        /// <returns>A stream with a serialized WikiStructure object.</returns>
        public Stream GetWikiStructure()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the rendered content of the page.
        /// </summary>
        /// <param name="docName">The full name of the page.</param>
        /// <returns>A stream with content.</returns>
        public Stream GetWikiPageAsPlainHTML(string docName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the spaces of a wiki.
        /// </summary>
        /// <returns>A list containing the spaces names.</returns>
        public List<string> GetSpaces()
        {
            String url = serverURL + XWikiURLs.WikiStructureURL;
            url += "&output=getSpaces";
            Stream stream = this.OpenRead(url);
            StreamReader reader = new StreamReader(stream);
            String response = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return this.ParseList(response);            
        }

        /// <summary>
        /// Gets the pages names for the specified space.
        /// </summary>
        /// <param name="spaceName">The name of the space.</param>
        /// <returns>A list with the pages names.</returns>
        public List<string> GetPages(string spaceName)
        {
            String url = serverURL + XWikiURLs.WikiStructureURL;
            url += "&output=getPages&space=" + spaceName;
            Stream stream = this.OpenRead(url);
            StreamReader reader = new StreamReader(stream);
            String response = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return this.ParseList(response); 
        }

        /// <summary>
        /// Saves the content of the page in the specified syntax.
        /// </summary>
        /// <param name="docName">The full name of the page.</param>
        /// <param name="content">The html content to be converted.</param>
        /// <param name="syntax">The syntax in wich the html code will be converted.</param>
        /// <returns>True if the operation was completed successfully. Failse if saving failed.</returns>
        public bool SavePageHTML(string docName, string content, string syntax)
        {
            try
            {
                String targetURL = ServerURL + XWikiURLs.SavePageURL;
                System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection();
                parameters.Add("pagename", docName);
                parameters.Add("syntax", syntax);
                parameters.Add("pagecontent", content);
                parameters.Add("xpage", "plain");
                byte[] response = webClient.UploadValues(targetURL, "POST", parameters);
                String responseText = Text.Encoding.UTF8.GetString(response);
                return responseText.Contains(HTTPResponses.SAVE_OK);
            }
            catch (WebException)
            {
                return false;
            }
        }        
        
        /// <summary>
        /// Adds an attachment to a specified page.
        /// </summary>
        /// <param name="docName">The full name of the page.</param>
        /// <param name="fileName">The name of the file. This is the name that will appear in the wiki.</param>
        /// <param name="content">A stream to read the file content.</param>
        /// <returns></returns>
        public bool AddAttachment(string docName, string fileName, Stream content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds an object to a wiki page.
        /// </summary>
        /// <param name="docName">The fullname of the wiki page.</param>
        /// <param name="ClassName">The xwiki class name of the object.</param>
        /// <param name="fieldsValues">The values of the object's fields.</param>
        /// <returns>The index of the inserted object.</returns>
        public int AddObject(string docName, string ClassName, System.Collections.Specialized.NameValueCollection fieldsValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a list with the attachmets' names of the specified page.
        /// </summary>
        /// <param name="docFullName">THe full name of the wiki page.</param>
        /// <returns>A list with the names of the attachments.</returns>
        public List<string> GetDocumentAttachmentList(string docFullName)
        {
            String url = serverURL + XWikiURLs.WikiStructureURL;
            url += "&output=getAttachments&page=" + docFullName;
            Stream stream = this.OpenRead(url);
            StreamReader reader = new StreamReader(stream);
            String response = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return this.ParseList(response);
        }

        /// <summary>
        /// Gets the url of an attachment.
        /// </summary>
        /// <param name="docFullName">The full name of the wiki page.</param>
        /// <param name="attachmentName">The name of the attachment.</param>
        /// <returns>The url of the attached file.HttpResponses errors in caee if an error occures.</returns>
        public string GetAttachmentURL(string docFullName, string attachmentName)
        {
            String url = serverURL + XWikiURLs.AttachmentServiceURL;
            url += "&action=getAttachmentURL&pageFullName=" + docFullName;
            url += "&fileName=" + attachmentName;
            Stream stream = this.OpenRead(url);
            StreamReader reader = new StreamReader(stream);
            String response = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            if(response.Contains(HTTPResponses.NO_PROGRAMMING_RIGHTS))
            {
                return HTTPResponses.NO_PROGRAMMING_RIGHTS;
            }
            response = this.serverURL + this.ParseList(response)[0];
            return response;
        }

        /// <summary>
        /// Gets the content of an attachment.
        /// </summary>
        /// <param name="pageName">The full name of the wiki page.</param>
        /// <param name="fileName">The name of the attachment.</param>
        /// <returns>The content of the attachment.</returns>
        public byte[] GetAttachmentContent(string pageName, string fileName)
        {
            String URL = GetAttachmentURL(pageName, fileName);
            return GetAttachmentContent(URL);
        }

        /// <summary>
        /// Get the content of an attachment.
        /// </summary>
        /// <param name="URL">The url of the attachment.</param>
        /// <returns>The content of the attachment.</returns>
        public byte[] GetAttachmentContent(string URL)
        {
            return this.DownloadData(URL);
        }

        /// <summary>
        /// Adds an attachment to the wiki.
        /// </summary>
        /// <param name="docName">The full name of the wiki page.</param>
        /// <param name="filePath">The path to the file to be attached.</param>
        public bool AddAttachment(string docName, string filePath)
        {
            //Upload failure not treated yet!
            String uploadAddress = serverURL + XWikiURLs.AttachmentServiceURL;
            uploadAddress += "&page=" + docName + "&action=attachFile";
            byte[] buffer = webClient.UploadFile(uploadAddress, filePath);
            String response = Text.UTF8Encoding.UTF8.GetString(buffer);
            if(response.Contains(HTTPResponses.NO_PROGRAMMING_RIGHTS) || response.Contains(HTTPResponses.NO_GROOVY_RIGHTS))
            {
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Attaches a file to the wiki.
        /// </summary>
        /// <param name="space">The space name.</param>
        /// <param name="page">The short page name.</param>
        /// <param name="filePath">The path to the file to be attached.</param>
        public bool AddAttachment(string space, string page, string filePath)
        {
            //Upload failure not treated yet!
            String docFullName = space + "." + page;
            return AddAttachment(docFullName, filePath);
        }

        /// <summary>
        /// Attaches a file to the wiki by using an async call. 
        /// </summary>
        /// <param name="docName">The full name of the wiki page.</param>
        /// <param name="filePath">The path to the file to be attached.</param>
        public bool AddAttachmentAsync(string docName, string filePath)
        {
            String uploadAddress = serverURL + XWikiURLs.AttachmentServiceURL;
            uploadAddress += "&page=" + docName + "&action=attachFile";
            webClient.UploadFileAsync(new Uri(uploadAddress), filePath);
            return true;
        }

        /// <summary>
        /// Attaches a file to the wiki by using an async call. 
        /// </summary>
        /// <param name="space">The space name.</param>
        /// <param name="page">The short page name.</param>
        /// <param name="filePath">The path to the file to be attached.</param>
        public bool AddAttachmentAsync(string space, string page, string filePath)
        {
            String docFullName = space + "." + page;
            return AddAttachmentAsync(docFullName, filePath);
        }

        /// <summary>
        /// Gets the rendered content of a page.
        /// </summary>
        /// <param name="pageFullName">The full nae of the wiki page.</param>
        /// <returns>The rendred content of the page.</returns>
        public string GetRenderedPageContent(string pageFullName)
        {
            String response = null;
            Stream data = null;
            StreamReader reader = null;
            //Read from server
            try
            {
                String uri = ServerURL + XWikiURLs.GetPageURL;
                uri += "?pagename=" + pageFullName + "&action=getRenderedContent&xpage=plain";
                data = webClient.OpenRead(uri);
                reader = new StreamReader(data);
                response = reader.ReadToEnd();
                if (response.Contains(HTTPResponses.NO_EDIT_RIGHTS))
                {
                    return HTTPResponses.NO_EDIT_RIGHTS;
                }
                else if (response.Contains(HTTPResponses.NO_PROGRAMMING_RIGHTS))
                {
                    return HTTPResponses.NO_PROGRAMMING_RIGHTS;
                }                     
                data.Close();
                reader.Close();
            }
            finally
            {
                data.Close();
                reader.Close();
            }
            return response;            
        }

        /// <summary>
        /// Gets the rendered page content of a page.
        /// </summary>
        /// <param name="space">The space name.</param>
        /// <param name="page">The page name.</param>
        /// <returns>The rendered content of the page.</returns>
        public string GetRenderedPageContent(string space, string page)
        {
            String pageFullName = space + "." + page;
            return GetRenderedPageContent(pageFullName);
        }

        /// <summary>
        /// Gets the url of a wiki page with the specified action.
        /// </summary>
        /// <param name="documentFullName">The full name of the wiki page.</param>
        /// <param name="xwikiAction">The name of the action. Eg: edit, view, delete</param>
        /// <returns>The url of the wiki page.</returns>
        public string GetURL(string documentFullName, string xwikiAction)
        {
            String uri = ServerURL + XWikiURLs.GetPageURL;
            uri += "?pagename=" + documentFullName + "&xwikiAction=" + xwikiAction;
            uri += "&action=getDocURL&xpage=plain";
            Stream data = webClient.OpenRead(uri);
            StreamReader reader = new StreamReader(data);
            String response = reader.ReadToEnd();
            data.Close();
            reader.Close();
            return response;
        }

        #endregion
    }
}
