using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using XWiki;
using XWiki.Clients;
using XWord.Util;
using XWiki.Office.Word;
using XWiki.Html;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Core;
using XWriter.VstoExtensions;

namespace XWriter
{
    /// <summary>
    /// Provides implementations for the common addin actions.
    /// This class should contain most VSTO specific code.
    /// </summary>
    public class AddinActions
    {
        //The instance of the current addin
        XWikiAddIn addin;
        //Utility class used for html processing
        HtmlUtil htmlUtil = new HtmlUtil();
        //A dictionary, storing the converter instances for each opened page.
        Dictionary<String, ConversionManager> pageConverters = new Dictionary<string, ConversionManager>();
        
        private const string DOWNLOAD_FOLDER = "XWord"; //"MyDocuments\XWord"
        private string TEMP_UPLOAD_FILES_FOLDER = Environment.SpecialFolder.ApplicationData.ToString() + @"\XWordTempData\UploadedFiles";
        private string TEMP_FILES_FOLDER = Environment.SpecialFolder.ApplicationData.ToString() + @"\XWordTempData\Pages";

        private Object missing = Type.Missing;

        String startDocument = "<html>\n<head>\n</head>\n<body>";
        String endDocument = "\n</body>\n</html>";

        String newPageText = "Hi! This is your new page. Please put your content here and then share it with others by saving it on the wiki.";

        /// <summary>
        /// Generic webclient used for conneting to xwiki.
        /// </summary>        
        public IXWikiClient Client
        {
            get
            {
                return addin.Client;
            }
        }

        /// <summary>
        /// Creates an instance for this class.
        /// </summary>
        /// <param name="addin">The instance of the current add-in.</param>
        public AddinActions(XWikiAddIn addin)
        {
            this.addin = addin;
        }

        /// <summary>
        /// Downloads the file to a local folder. The folder is located in MyDocuments
        /// </summary>
        /// <param name="pageFullName">FullName of the wiki page - Space.Name</param>
        /// <param name="attachmentName">Name of the attached file</param>
        /// <param name="path">The location where the attachment will be downloaded. Use null for default location.</param>
        /// <returns></returns>
        public FileInfo DownloadAttachment(String pageFullName, String attachmentName, String path)
        {
            if (!this.Client.LoggedIn)
            {
                Client.Login(addin.username, addin.password);
            }
            if (path == null)
            {
                path = addin.DownloadedAttachmentsRepository;
                if (new FileInfo(path).Exists)
                {
                    File.Create(path);
                }
                path = path + "\\" + UtilityClass.GenerateUniqueFileName(attachmentName, path);
            }
            FileInfo fileInfo = new FileInfo(path);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            byte[] binaryContent = Client.GetAttachmentContent(pageFullName, attachmentName);
            FileStream fileStream = fileInfo.Create();
            fileStream.Write(binaryContent, 0, binaryContent.Length);
            fileStream.Close();
            return fileInfo;
        }

        /// <summary>
        /// Saves the document and attaches it to a wiki page
        /// </summary>
        /// <param name="pageFullName">The full name of the wiki page.</param>
        public bool AttachCurrentFile(String pageFullName)
        {
            if (pageFullName != null)
            {
                if (!this.Client.LoggedIn)
                {
                    Client.Login(addin.username, addin.password);
                }
                int index = pageFullName.IndexOf(".");
                String space = pageFullName.Substring(0, index);
                String page = pageFullName.Substring(index + 1);
                return AttachCurrentFile(space, page);
            }
            else
            {
                return false;
            }
           
        }

        /// <summary>
        /// Saves the document and attaches it to a wiki page
        /// </summary>
        /// <param name="space">The name of the wiki space.</param>
        /// <param name="page">The name of the page.</param>
        public bool AttachCurrentFile(String space, String page)
        {
            if (space == null || page == null)
            {
                Log.Error("Trying to attach a file to a page with an invalid name!");
                return false;
            }
            if (!this.Client.LoggedIn)
            {
                Client.Login(addin.username, addin.password);
            }
            bool operationCompleted = false; ;
            if (addin.Application.ActiveDocument == null)
            {
                MessageBox.Show("This command is not available because no document is open.", "XWord");
            }
            try
            {
                addin.Application.ActiveDocument.Save();
            }
            catch (System.Runtime.InteropServices.COMException) { }
            //Test if the user doesnt cancel the saving process.
            if (addin.Application.ActiveDocument.Saved && (!addin.Application.ActiveDocument.FullName.Equals("Document1")))
            {
                String docFullName = addin.Application.ActiveDocument.FullName;
                String docName = addin.Application.ActiveDocument.Name;
                Directory.CreateDirectory(addin.PagesRepository);
                try
                {
                    String targetFile = addin.PagesRepository + "\\" + docName;
                    File.Copy(docFullName, targetFile, true);
                    Client.AddAttachment(space, page, targetFile);
                    operationCompleted = true;
                }
                catch (IOException ioex)
                {
                    Log.ExceptionSummary(ioex);
                }
                catch (Exception ex)
                {
                    Log.ExceptionSummary(ex);
                }
            }
            return operationCompleted;
        }

        /// <summary>
        /// Starts a new process. 
        ///  - If the file is executable then it will execute that file.
        ///  - If the file file is resistered to be opened with another application
        ///  then it will be opened with that application
        ///  - If the file doesnt have an assigned application then Windows Explorer openes it's directory.
        /// </summary>
        /// <param name="fullFileName">The full path and name of the file.</param>
        public void StartProcess(String fullFileName)
        {
            Process p = new Process();
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(fullFileName);
                p.StartInfo = psi;
                p.Start();
            }
            catch (Win32Exception)
            {
                p.StartInfo = new ProcessStartInfo("explorer.exe", Path.GetDirectoryName(fullFileName));
                p.Start();
            }
            catch (Exception ex)
            {
                Log.ExceptionSummary(ex);
            }
        }

        /// <summary>
        /// Specifies if a wiki page is opened for editing.
        /// </summary>
        /// <param name="pageFullName">The full name of the page.</param>
        /// <returns>True if the page is already opened. False otherwise.</returns>
        public bool IsOpened(String pageFullName)
        {
            if (addin.EditedPages.ContainsValue(pageFullName))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Edits a wiki page.
        /// The LoadingDialog is shown during the operation.
        /// <see cref="EditPage"/>
        /// </summary>
        /// <param name="pageFullName">The full name of the wiki page that is being opened for editing.</param>
        public void EditPage(String pageFullName)
        {
            if(IsOpened(pageFullName))
            {
                MessageBox.Show("You are already editing this page.", "XWord");
                return;
            }
            if (!this.Client.LoggedIn)
            {
                Client.Login(addin.username, addin.password);
            }
            if(IsProtectedPage(pageFullName, addin.ProtectedPages))
            {
                String message = "You cannot edit this page." + Environment.NewLine;
                message += "This page contains scrips that provide functionality to the wiki.";
                MessageBox.Show(message, "XWord", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            LoadingDialog loadingDialog = new LoadingDialog("Opening page...");
            ThreadPool.QueueUserWorkItem(new WaitCallback(loadingDialog.ShowSyncDialog));
            Thread.Sleep(500);
            GetPage(pageFullName);
            loadingDialog.CloseSyncDialog();
        }

        /// <summary>
        /// Edits a wiki page.
        /// </summary>
        /// <param name="_pageFullName">The full name of the wiki page that is being opened for editing.</param>
        private void GetPage(Object _pageFullName)
        {
            try
            {
                if (!this.Client.LoggedIn)
                {
                    Client.Login(addin.username, addin.password);
                }
                String pageFullName = (String)_pageFullName;
                //Read from server
                String content = Client.GetRenderedPageContent(pageFullName);
                
                String localFileName = pageFullName.Replace(".", "-");
                String folder = addin.PagesRepository + "TempPages";
                ConvertToNormalFolder(folder);               
                //content = new WebToLocalHTML(addin.serverURL, folder, localFileName).AdaptSource(content);
                ConversionManager pageConverter;
                if (pageConverters.ContainsKey(pageFullName))
                {
                    pageConverter = pageConverters[pageFullName];
                }
                else
                {
                    pageConverter = new ConversionManager(addin.serverURL, folder, pageFullName, localFileName, addin.Client);
                    pageConverters.Add(pageFullName, pageConverter);
                }
                content = pageConverter.ConvertFromWebToWord(content);
                localFileName = folder + "\\" + localFileName + ".html";
                addin.currentLocalFilePath = localFileName;
                StringBuilder pageContent = new StringBuilder(content);
                //Process the content
                pageContent.Insert(0, startDocument);
                pageContent.Append(endDocument);
                //Save the file
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                FileStream stream = new FileStream(localFileName, FileMode.Create);
                byte[] buffer = UTF8Encoding.UTF8.GetBytes(pageContent.ToString());
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();
                
                addin.currentPageFullName = pageFullName;

                //Open the file with Word
                Word.Document doc =  OpenHTMLDocument(localFileName);
                addin.EditedPages.Add(localFileName, pageFullName);

                //Mark just-opened document as saved. This prevents a silly confirmation box that
                //warns about unsaved changes when closing an unchanged document.
                doc.Saved = true;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        /// <summary>
        /// Saves the page to the wiki. Shows a message box with an error if the operation fails.
        /// </summary>
        /// <param name="pageName">The full name of the wiki page.</param>
        /// <param name="pageContent">The contant to be saved.</param>
        /// <param name="syntax">The wiki syntax of the saved page.</param>
        private void SavePage(String pageName, ref String pageContent, String syntax)
        {
            if (!this.Client.LoggedIn)
            {
                Client.Login(addin.username, addin.password);
            }
            if (!Client.SavePageHTML(pageName, pageContent, syntax))
            {
                Log.Error("Failed to save page " + pageName + "on server " + addin.serverURL);
                MessageBox.Show("There was an error on the server when trying to save the page");
            }
        }

        /// <summary>
        /// Opens a local(document) file with Word.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>Returns a Document instance.</returns>
        private Word.Document OpenHTMLDocument(String path)
        {
            object format = Word.WdOpenFormat.wdOpenFormatWebPages;
            object filePath = path;
            Word.Document newDoc = addin.Application.Documents.Open(ref filePath,
                                                     ref missing, ref missing, ref missing,
                                                     ref missing, ref missing, ref missing,
                                                     ref missing, ref missing, ref format,
                                                     ref missing, ref missing, ref missing,
                                                     ref missing, ref missing, ref missing);
            addin.ActiveDocumentInstance.WebOptions.AllowPNG = true;
            addin.ActiveDocumentInstance.WebOptions.Encoding = MsoEncoding.msoEncodingUnicodeLittleEndian;
            return newDoc;
        }

        /// <summary>
        /// Saves the currently edited page or document to the server.
        /// Displays the operation in progress dialog.
        /// </summary>
        public void SaveToServer()
        {
            if (addin.currentPageFullName == "" || addin.currentPageFullName == null)
            {
                MessageBox.Show("You are not currently editing a wiki page", "XWord", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            bool continueWithSaving = ShowSwitchSyntaxDialog();
            if (!continueWithSaving)
            {
                return;
            }
            LoadingDialog loadingDialog = new LoadingDialog("Saving to wiki...");
            ThreadPool.QueueUserWorkItem(new WaitCallback(loadingDialog.ShowSyncDialog));
            SaveToXwiki();
            loadingDialog.CloseSyncDialog();
        }

        /// <summary>
        /// If current syntax is XWiki 2.0 and the page contains table(s), promt the user
        /// to switch to XHTML syntax with an Yes/No/Cancel message box.
        /// </summary>
        /// <param name="cleanHTML">Cleaned HTML source code.</param>
        /// <returns>FALSE if the user presses 'Cancel', meaning the saving should not continue. TRUE in other cases.</returns>
        private bool ShowSwitchSyntaxDialog()
        {
            if (addin.AddinStatus.Syntax == null)
            {
                addin.AddinStatus.Syntax = addin.DefaultSyntax;
            }
            if ((addin.ActiveDocumentInstance.Tables.Count>0) && 
                (addin.AddinStatus.Syntax.ToLower().IndexOf("xhtml") < 0))
            {
                DialogResult dr;
                string infoMessage, caption;
                infoMessage = "This page contains elements that can not properly be saved inXWiki 2.0 syntax.";
                infoMessage += Environment.NewLine;
                infoMessage += "Would you like to switch to XHTML syntax?";
                caption = "Feature Not Yet Implemented";
            
                dr = System.Windows.Forms.MessageBox.Show(infoMessage, caption, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                
                if (dr == DialogResult.Yes)
                {
                    addin.AddinStatus.Syntax = "XHTML";
                    Globals.Ribbons.XWikiRibbon.SwitchSyntax("XHTML");
                }
                if (dr == DialogResult.Cancel)
                    return false;

            }
            return true;
        }

        /// <summary>
        /// Saves the currently edited page or document to the server.
        /// </summary>
        private void SaveToXwiki()
        {
            try
            {
                String contentFilePath = "";
                addin.ReinforceApplicationOptions();
                String filePath = addin.ActiveDocumentFullName;
                String currentFileName = Path.GetDirectoryName(addin.ActiveDocumentFullName);
                currentFileName += "\\" + Path.GetFileNameWithoutExtension(addin.ActiveDocumentFullName);
                String tempExportFileName = currentFileName + "_TempExport.html";
                if (!ShadowCopyDocument(addin.ActiveDocumentInstance, tempExportFileName, addin.SaveFormat))
                {
                    MessageBox.Show("There was an error when trying to save the page.", "XWord");
                    return;
                }
                contentFilePath = tempExportFileName;
                StreamReader sr = new StreamReader(contentFilePath);
                String fileContent = sr.ReadToEnd();
                sr.Close();
                File.Delete(contentFilePath);
                String cleanHTML = "";
                cleanHTML = htmlUtil.RemoveSpecificTagContent(fileContent, "<!--", "-->");
                cleanHTML = htmlUtil.RemoveSpecificTagContent(cleanHTML, "<![", "]>");
                //cleanHTML = htmlUtil.RemoveSpecificTagContent(cleanHTML, "<meta", ">");
                cleanHTML = htmlUtil.RemoveSpecificTagContent(cleanHTML, "<head>", "</head>");
                //cleanHTML = htmlUtil.CleanHTML(cleanHTML, true);
                //cleanHTML = htmlUtil.GetBodyContent(cleanHTML);
                ConversionManager pageConverter;
                if (pageConverters.ContainsKey(addin.currentPageFullName))
                {
                    pageConverter = pageConverters[addin.currentPageFullName];
                }
                else
                {
                    pageConverter = new ConversionManager(addin.serverURL, Path.GetDirectoryName(contentFilePath),
                                                          addin.currentPageFullName, Path.GetFileName(contentFilePath), addin.Client);
                }
                cleanHTML = pageConverter.ConvertFromWordToWeb(cleanHTML);
                cleanHTML = htmlUtil.GetBodyContent(cleanHTML);
                //openHTMLDocument(addin.currentLocalFilePath);
                if (addin.AddinStatus.Syntax == null)
                {
                    addin.AddinStatus.Syntax = addin.DefaultSyntax;
                }


                //Convert the source to the propper encoding.
                Encoding iso = Client.ServerEncoding;
                byte[] content = Encoding.Unicode.GetBytes(cleanHTML);
                byte[] wikiContent = null;
                wikiContent = Encoding.Convert(Encoding.Unicode, iso, content);
                cleanHTML = iso.GetString(wikiContent);
                SavePage(addin.currentPageFullName, ref cleanHTML, addin.AddinStatus.Syntax);
            }
            catch (COMException ex)
            {
                string message = "An internal Word error appeared when trying to save your file.";
                message += Environment.NewLine + ex.Message;
                Log.Exception(ex);
                MessageBox.Show(message, "XWord");
            }
        }

        /// <summary>
        /// Starts editing a new wiki page. The page will not be created in the wiki until the fisrt save.
        /// </summary>
        /// <param name="space">The name of the wiki space</param>
        /// <param name="pageName">The name of page</param>
        public void AddNewPage(String space, String pageName)
        {
            AddNewPage(space, pageName, null, null);
        }

        /// <summary>
        /// Converts the folder to a "Normal" type folder.
        /// </summary>
        /// <param name="folder"></param>
        public void ConvertToNormalFolder(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            if (di.Attributes == FileAttributes.ReadOnly)
            {
                di.Attributes = FileAttributes.Normal;
            }
        }

        /// <summary>
        /// Starts editing a new wiki page. The page will not be created in the wiki until the fisrt save.
        /// </summary>
        /// <param name="space">The name of the wiki space.</param>
        /// <param name="pageName">The name of page.</param>
        /// <param name="pageTitle">The title of the page.</param>
        /// <param name="sender">
        /// The instance of the fprm that started the action.
        /// This form need to be closed before swithing the Active Word Document.
        /// </param>
        public void AddNewPage(String space, String pageName, String pageTitle, Form sender)
        {
            //Any modal dialog nust be closed before opening or closing active documents.
            if (sender != null)
            {
                sender.Close();
            }
            try
            {
                if (!this.Client.LoggedIn)
                {
                    Client.Login(addin.username, addin.password);
                }
                String pageFullName = space + "." + pageName;
                String localFileName = pageFullName.Replace(".", "-");
                String folder = addin.PagesRepository + "TempPages";
                ConvertToNormalFolder(folder);
                //content = new WebToLocalHTML(addin.serverURL, folder, localFileName).AdaptSource(content);
                ConversionManager pageConverter = new ConversionManager(addin.serverURL, folder, pageFullName, localFileName, addin.Client);
                localFileName = folder + "\\" + localFileName + ".html";
                addin.currentLocalFilePath = localFileName;
                //Save the file
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                String pageContent = "<h1>" + pageTitle + "</h1>" + Environment.NewLine;
                pageContent = pageContent + newPageText;
                FileStream stream = new FileStream(localFileName, FileMode.Create);
                //byte[] buffer = UTF8Encoding.UTF8.GetBytes(pageContent.ToString());
                Encoding iso = Client.ServerEncoding;
                byte[] buffer = iso.GetBytes(pageContent);
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();
                addin.currentPageFullName = pageFullName;
                
                //Open the file with Word
                Word.Document doc = OpenHTMLDocument(localFileName);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Displays a SaveFileDialog
        /// </summary>
        /// <param name="fileName">Optional name of file to be saved.</param>
        /// <returns>The path to the file to be saved. Null if the user cancels the saving.</returns>
        public String SaveFileDialog(String fileName)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (fileName != null)
            {
                dialog.DefaultExt = Path.GetExtension(fileName);
                dialog.FileName = Path.GetFileNameWithoutExtension(fileName);
                dialog.Filter = "(*." + dialog.DefaultExt + ")|*." + dialog.DefaultExt;
            }
            dialog.CheckPathExists = true;
            dialog.CustomPlaces.Add(addin.DownloadedAttachmentsRepository);
            dialog.CustomPlaces.Add(addin.PagesRepository);
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Searches the server response for error strings.
        /// </summary>
        /// <param name="content">The server response.</param>
        /// <returns>True if the response contains error reports. False if the response does not ocntain error reports.</returns>
        public bool CheckForErrors(string content)
        {
            bool hasErrors = false;
            if (content.Contains(HTTPResponses.NO_PROGRAMMING_RIGHTS))
            {
                Log.Error("Server " + addin.serverURL + " has no programming rights on getPageservice");
                MessageBox.Show("There was an error on the server. The pages in MSOffice space don't have programming rights");
                hasErrors = true;
            }
            else if(content.Contains(HTTPResponses.WRONG_REQUEST))
            {
                Log.Error("Server " + addin.serverURL + " wrong request");
                MessageBox.Show("Error: Wrong request");
                hasErrors = true;
            }
            else if(content.Contains(HTTPResponses.NO_EDIT_RIGHTS))
            {
                Log.Information("User tried to edit a page on " + addin.serverURL + " whithout edit rights");
                MessageBox.Show("You dont have the right to edit this page");
                hasErrors = true;
            }
            else if(content.Contains(HTTPResponses.NO_GROOVY_RIGHTS))
            {
                Log.Error("Server " + addin.serverURL + " error on parsing groovy - no groovy rights");
                String message = "There was an error on the server." + Environment.NewLine;
                message +=  "Please contact your server adminitrator. Error on executing groovy page in MSOffice space";
                MessageBox.Show(message);
                hasErrors = true;
            }
            else if(content.Contains(HTTPResponses.INSUFFICIENT_MEMMORY))
            {
                Log.Error("Server " + addin.serverURL + " reported OutOfMemmoryException");
                String message = "There was an error on the server." + Environment.NewLine;
                message += "The server has insufficient memmory to execute the current tasks.";
                MessageBox.Show(message);
                hasErrors = true;
            }
            else if(content.Contains(HTTPResponses.VELOCITY_PARSER_ERROR))
            {
                Log.Error("Server " + addin.serverURL + " error when parsing page. ");
                String message = "There was an error on the server" + Environment.NewLine;
                message += "'Error while parsing velocity page'";
                MessageBox.Show(message);
                hasErrors = true;
            }
            return hasErrors;
        }

        /// <summary>
        /// Removes all protected pages from the Word wiki structure.
        /// </summary>
        /// <param name="wiki">The wiki instance.</param>
        /// <param name="wildcards">The list of protected pages wildcards.</param>
        public void HideProtectedPages(WikiStructure wiki, List<String> wildcards)
        {
            foreach (XWikiDocument doc in wiki.GetAllDocuments())
            {
                foreach (String wildcard in wildcards)
                {
                    String docFullName = doc.space + "." + doc.name;
                    if(UtilityClass.IsWildcardMatch(wildcard, docFullName, true))
                    {
                        wiki.RemoveXWikiDocument(doc);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies if a page is protected or not.
        /// </summary>
        /// <param name="pageFullName">The full name of the page.</param>
        /// <param name="wildCards">The wildcard list of protected pages.</param>
        /// <returns></returns>
        public bool IsProtectedPage(String pageFullName, List<String> wildCards)
        {
            bool isProtectedPage = false;
            foreach (String wildcard in wildCards)
            {
                if (UtilityClass.IsWildcardMatch(wildcard, pageFullName, true))
                {
                    isProtectedPage = true;
                }
            }
            return isProtectedPage;
        }

        /// <summary>
        /// Copies a document to a specified path.
        /// </summary>
        /// <param name="document">The Document instance.</param>
        /// <param name="path">The path where the new file will be saved.</param>
        /// <param name="saveFormat">The save format.</param>
        /// <exception cref="IOException">When the file cannot be saved.</exception>
        /// <remarks>The document is saved in Unicode little endian encoding.</remarks>
        /// <returns>True if the operation succedes. False otherwise.</returns>
        public bool ShadowCopyDocument(Word.Document document, string path, Word.WdSaveFormat saveFormat)
        {
            try
            {
                Object format = saveFormat;
                Object copyPath = path;
                Object encoding = MsoEncoding.msoEncodingUnicodeLittleEndian;
                Object missing = Type.Missing;
                Object originalFilePath = document.FullName;
                Object initialDocSaveFormat = document.SaveFormat;
                document.SaveAs(ref copyPath, ref format, ref missing, ref missing, ref missing, ref missing,
                                ref missing, ref missing, ref missing, ref missing, ref missing, ref encoding,
                                ref missing, ref missing, ref missing, ref missing);
                document.SaveAs(ref originalFilePath, ref initialDocSaveFormat, ref missing, ref missing, ref missing, ref missing,
                                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                                ref missing, ref missing, ref missing, ref missing);                
            }
            catch (IOException ioex)
            {
                Log.Exception(ioex);
                MessageBox.Show(ioex.Message);
                return false;
            }
            
            return true;
        }
    }
}
