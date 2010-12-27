#region LGPL license
/*
 * See the NOTICE file distributed with this work for additional
 * information regarding copyright ownership.
 *
 * This is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this software; if not, write to the Free
 * Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
 * 02110-1301 USA, or see the FSF site: http://www.fsf.org.
 */
#endregion //license

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
using XWord.VstoExtensions;
using XWiki.Logging;
using XWiki.Model;
using ContentFiltering.Office.Word.Cleaners;
using ContentFiltering.StyleSheetExtensions;
using Rpc = XWiki.XmlRpc;

namespace XWord
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

        Dictionary<String, Rpc.PageHistorySummary[]> pagesHistory = new Dictionary<string, XWiki.XmlRpc.PageHistorySummary[]>();

        private const string DOWNLOAD_FOLDER = "XWord"; //"MyDocuments\XWord"
        private string TEMP_UPLOAD_FILES_FOLDER = Environment.SpecialFolder.ApplicationData.ToString() + @"\XWordTempData\UploadedFiles";
        private string TEMP_FILES_FOLDER = Environment.SpecialFolder.ApplicationData.ToString() + @"\XWordTempData\Pages";

        private Object missing = Type.Missing;

        String startDocument = "<html>\n<head>\n</head>\n<body>";
        String endDocument = "\n</body>\n</html>";

        String newPageText = "Hi! This is your new page. Please put your content here and then share it with others by saving it on the wiki.";


        Word.Options wordOptions;
        bool checkGrammarAsYouType = false;
        bool checkGrammarWithSpelling = false;
        bool checkSpellingAsYouType = false;
        bool contextualSpeller = false;

        //The instance of the newest Word document.
        private Word.Document newDoc;

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
            addin.Application.WindowDeactivate += new Word.ApplicationEvents4_WindowDeactivateEventHandler(KeepNewWindowActivated);
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
                path = addin.AddinSettings.DownloadedAttachmentsRepository;
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
                UserNotifier.Message("This command is not available because no document is open.");
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
                Directory.CreateDirectory(addin.AddinSettings.PagesRepository);
                try
                {
                    String targetFile = addin.AddinSettings.PagesRepository + "\\" + docName;
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
            if (IsOpened(pageFullName))
            {
                UserNotifier.Message("You are already editing this page.");
                return;
            }
            if (!this.Client.LoggedIn)
            {
                Client.Login(addin.username, addin.password);
            }
            if (IsProtectedPage(pageFullName, addin.ProtectedPages))
            {
                String message = "You cannot edit this page." + Environment.NewLine;
                message += "This page contains scrips that provide functionality to the wiki.";
                UserNotifier.StopHand(message);
                return;
            }
            LoadingDialog loadingDialog = new LoadingDialog("Opening page...");
            ThreadPool.QueueUserWorkItem(new WaitCallback(loadingDialog.ShowSyncDialog));
            
            GetPage(pageFullName);
            loadingDialog.CloseSyncDialog();
        }

        /// <summary>
        /// Retrieves the versioning information for a wiki page, and registers it ot the history dictionary.
        /// </summary>
        /// <param name="pageFullName">The full name of the page.</param>
        private void IndexPageHistory(String pageFullName)
        {
            Rpc.PageHistorySummary[] pageHistory = Client.GetPageHistory(pageFullName);
            if (pagesHistory.ContainsKey(pageFullName))
            {
                pagesHistory[pageFullName] = pageHistory;
            }
            else
            {
                pagesHistory.Add(pageFullName, pageHistory);
            }
        }

        /// <summary>
        /// Updates the history of a page, the last author and specifies if the page was modified.
        /// </summary>
        /// <param name="pageFullName">The full name of the wiki page.</param>
        /// <param name="wasModified">Reference specifing if the page was modified since was opened in Word or compared last time.</param>
        /// <param name="lastAuthor">Reference to the username of last author.</param>
        private Rpc.PageHistorySummary[] CheckPageHistory(String pageFullName, out bool wasModified, out String lastAuthor)
        {
            wasModified = false;
            lastAuthor = "";
            Rpc.PageHistorySummary[] updatedPageHistory = Client.GetPageHistory(pageFullName);
            try
            {
                Rpc.PageHistorySummary latestVersion = updatedPageHistory[updatedPageHistory.Length - 1];
                int pageVersionCount = pagesHistory[pageFullName].Length;
                Rpc.PageHistorySummary versionAtOpening = pagesHistory[pageFullName][pageVersionCount - 1];
                if (latestVersion.version > versionAtOpening.version)
                {
                    wasModified = true;
                }
                else if (latestVersion.version == versionAtOpening.version)
                {
                    if (latestVersion.minorVersion > versionAtOpening.minorVersion)
                    {
                        wasModified = true;
                    }
                }
                lastAuthor = latestVersion.modifier;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                wasModified = false;
            }
            return updatedPageHistory;
        }

        /// <summary>
        /// If the document was modified externally, prompts the user with the document merge screen.
        /// </summary>
        /// <param name="pageFullName">The full name of the wiki page.</param>
        /// <returns>Specifies if the page was modified since it was last merged or opened in Word.</returns>
        private bool MergeWithLatestVersion(String pageFullName)
        {
            bool wasModified = false;
            String lastAuthor = "";
            Rpc.PageHistorySummary[] history = CheckPageHistory(pageFullName, out wasModified, out lastAuthor);
            if (wasModified)
            {
                String message = "The wiki page was modified by " + lastAuthor + "." + Environment.NewLine;
                message += "You need to merge the versions then publish the page. Continue?";
                DialogResult result = UserNotifier.YesNoQuestion(message);
                if (result != DialogResult.Yes)
                {
                    return true;
                }
                String localFileName = "";
                Word.Document originalDoc = addin.ActiveDocumentInstance;
                OpenForMerge(pageFullName, out localFileName);
                Word.Document revizedDoc = OpenHTMLDocument(localFileName);
                originalDoc = addin.Application.MergeDocuments(originalDoc, revizedDoc,
                                                 Word.WdCompareDestination.wdCompareDestinationOriginal,
                                                 Word.WdGranularity.wdGranularityCharLevel,
                                                 true, true, true, true, true, true, true, true,
                                                 true, true, "Word Version", lastAuthor,
                                                 Word.WdMergeFormatFrom.wdMergeFormatFromPrompt);
                Object _false = false;
                revizedDoc.ActiveWindow.Visible = false;
                revizedDoc.Close(ref _false, ref _false, ref _false);
                //If document was merged then it's considered to be at the latest version.
                pagesHistory[pageFullName] = history;
            }
            return wasModified;
        }

        void Application_MailMergeAfterMerge(Microsoft.Office.Interop.Word.Document Doc, Microsoft.Office.Interop.Word.Document DocResult)
        {
            DocResult.Save();
        }


        /// <summary>
        /// Retrieves a page from the wiki and save it locally in order to be merged to the active document.
        /// </summary>
        /// <param name="pageFullName">The full name of the wiki page.</param>
        /// <param name="localFileName">String reference to the file path of the created document.</param>
        private void OpenForMerge(String pageFullName, out String localFileName)
        {
            String content = Client.GetRenderedPageContent(pageFullName);
            String suffix = "__LATEST";
            String folder = addin.AddinSettings.PagesRepository + "TempPages";
            
            localFileName = pageFullName.Replace(".", "-") + suffix;
            ConvertToNormalFolder(folder);
            ConversionManager pageConverter;
            //TODO: The converter info should be merged.
            pageConverter = new ConversionManager(addin.AddinSettings, addin.serverURL, folder, pageFullName, localFileName, addin.Client);
            //pageConverters.Add(pageFullName + suffix, pageConverter);
            
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
            StreamWriter writer = new StreamWriter(localFileName, false, Encoding.UTF8);
            writer.Write(pageContent.ToString());
            writer.Close();
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

                //TODO: Add versioniong info to the xmlrpc Page model
                IndexPageHistory(pageFullName);

                String localFileName = pageFullName.Replace(".", "-");
                String folder = addin.AddinSettings.PagesRepository + "TempPages";
                ConvertToNormalFolder(folder);
                //content = new WebToLocalHTML(addin.serverURL, folder, localFileName).AdaptSource(content);
                ConversionManager pageConverter;
                if (pageConverters.ContainsKey(pageFullName))
                {
                    pageConverter = pageConverters[pageFullName];
                }
                else
                {
                    pageConverter = new ConversionManager(addin.AddinSettings, addin.serverURL, folder, pageFullName, localFileName, addin.Client);
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
                
                StreamWriter writer = new StreamWriter(localFileName, false, Encoding.UTF8);
                writer.Write(pageContent.ToString());
                writer.Close();

                #region OpenLocalDocument
                //Register new local filename as a wiki page.
                addin.EditedPages.Add(localFileName, pageFullName);
                addin.currentPageFullName = pageFullName;
                //Open the file with Word
                Word.Document doc = OpenHTMLDocument(localFileName);
                #endregion//Open local document

                //Mark just-opened document as saved. This prevents a silly confirmation box that
                //warns about unsaved changes when closing an unchanged document.
                doc.Saved = true;

                //The opened doc becomes the newest doc used for window reactivation.
                newDoc = doc;
                if (addin.ActiveDocumentInstance != doc)
                {
                    doc.Activate();
                }
            }
            catch (IOException ex)
            {
                UserNotifier.Error(ex.Message);
            }
        }

        /// <summary>
        /// Save user settings for grammar and spelling checking.
        /// </summary>
        private void SaveGrammarAndSpellingSettings()
        {
            wordOptions = addin.Application.Options;
            checkGrammarAsYouType = wordOptions.CheckGrammarAsYouType;
            checkGrammarWithSpelling = wordOptions.CheckGrammarWithSpelling;
            checkSpellingAsYouType = wordOptions.CheckSpellingAsYouType;
            contextualSpeller = wordOptions.ContextualSpeller;
        }

        /// <summary>
        /// Disable grammar and spelling checking.
        /// </summary>
        private void DisableGrammarAndSpellingChecking()
        {
            wordOptions.CheckGrammarAsYouType = false;
            wordOptions.CheckGrammarWithSpelling = false;
            wordOptions.CheckSpellingAsYouType = false;
            wordOptions.ContextualSpeller = false;
        }

        /// <summary>
        /// Restore user settings for grammar and spelling checking.
        /// </summary>
        private void RestoreGrammarAndSpellingSettings()
        {
            wordOptions.CheckGrammarAsYouType = checkGrammarAsYouType;
            wordOptions.CheckGrammarWithSpelling = checkGrammarWithSpelling;
            wordOptions.CheckSpellingAsYouType = checkSpellingAsYouType;
            wordOptions.ContextualSpeller = contextualSpeller;
        }

        /// <summary>
        /// Saves the page to the wiki. Shows a message box with an error if the operation fails.
        /// </summary>
        /// <param name="pageName">The full name of the wiki page.</param>
        /// <param name="pageContent">The contant to be saved.</param>
        /// <param name="syntax">The wiki syntax of the saved page.</param>
        /// <returns>TRUE if the page was saved successfully.</returns>
        private bool SavePage(String pageName, ref String pageContent, String syntax)
        {
            bool saveSucceeded = false;
            SaveGrammarAndSpellingSettings();
            DisableGrammarAndSpellingChecking();

            if (!this.Client.LoggedIn)
            {
                Client.Login(addin.username, addin.password);
            }

            if (!Client.SavePageHTML(pageName, pageContent, syntax))
            {
                ReportSaveProblem();
                saveSucceeded = false;
            }
            else
            {
                saveSucceeded = true;
                //mark the page from wiki structure as published
                bool markedDone = false;
                foreach (Space sp in addin.wiki.spaces)
                {
                    foreach (XWikiDocument xwdoc in sp.documents)
                    {
                        if ((xwdoc.space + "." + xwdoc.name) == pageName)
                        {
                            sp.published = true;
                            xwdoc.published = true;
                            markedDone = true;
                            break;//inner foreach
                        }
                    }
                    if (markedDone)
                    {
                        break;//outer foreach
                    }
                }
            }

            RestoreGrammarAndSpellingSettings();

            return saveSucceeded;
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
                UserNotifier.Exclamation("You are not currently editing a wiki page");
                return;
            }

            if (addin.CurrentPagePublished)
            {
                //Checks if a newer version exists. If yes, then the publishing is canceled redardless if
                //the user  merged the documents or not.
                if (MergeWithLatestVersion(addin.currentPageFullName))
                {
                    return;
                }
            }
            LoadingDialog loadingDialog = new LoadingDialog("Saving to wiki...");
            ThreadPool.QueueUserWorkItem(new WaitCallback(loadingDialog.ShowSyncDialog));
            SaveToXwiki();

            //TODO: Create an SaveCompleted event. Listen for the event and do an async version update 
            //Update local history to the latest version
            pagesHistory[addin.currentPageFullName] = Client.GetPageHistory(addin.currentPageFullName);
            
            loadingDialog.CloseSyncDialog();
            //After a new page has been published to XWiki, refresh the tree view
            //so the user can see his/her page plus other pages that might have been
            //created while the user was working on the current one.
            if (!addin.CurrentPagePublished)
            {
                Globals.XWikiAddIn.XWikiTaskPane.RefreshWikiExplorer();
                addin.CurrentPagePublished = true;
            }
        }


        /// <summary>
        /// Saves the currently edited page or document to the server.
        /// </summary>
        private void SaveToXwiki()
        {
            try
            {
                addin.AnnotationMaintainer.ClearComments();
                String contentFilePath = "";
                addin.ReinforceApplicationOptions();
                String filePath = addin.ActiveDocumentFullName;
                String currentFileName = Path.GetDirectoryName(addin.ActiveDocumentFullName);
                currentFileName += "\\" + Path.GetFileNameWithoutExtension(addin.ActiveDocumentFullName);
                String tempExportFileName = currentFileName + "_TempExport.html";
                if (!ShadowCopyDocument(addin.ActiveDocumentInstance, tempExportFileName, addin.SaveFormat))
                {
                    ReportSaveProblem();
                    return;
                }
                contentFilePath = tempExportFileName;
                StreamReader sr = new StreamReader(contentFilePath, Client.ServerEncoding);
                String fileContent = sr.ReadToEnd();
                sr.Close();
                File.Delete(contentFilePath);
                String cleanHTML = "";

                cleanHTML = new CommentsRemover().Clean(fileContent);
                cleanHTML = new HeadSectionRemover().Clean(cleanHTML);

                ConversionManager pageConverter;
                if (pageConverters.ContainsKey(addin.currentPageFullName))
                {
                    pageConverter = pageConverters[addin.currentPageFullName];
                }
                else
                {
                    pageConverter = new ConversionManager(addin.AddinSettings, addin.serverURL, Path.GetDirectoryName(contentFilePath),
                                                          addin.currentPageFullName, Path.GetFileName(contentFilePath), addin.Client);
                }
                cleanHTML = pageConverter.ConvertFromWordToWeb(cleanHTML);

                SSXManager ssxManager = SSXManager.BuildFromLocalHTML(pageConverter, cleanHTML);

                cleanHTML = new BodyContentExtractor().Clean(cleanHTML);

                //openHTMLDocument(addin.currentLocalFilePath);
                if (addin.AddinStatus.Syntax == null)
                {
                    addin.AddinStatus.Syntax = addin.DefaultSyntax;
                }

                if (SavePage(addin.currentPageFullName, ref cleanHTML, addin.AddinStatus.Syntax))
                {
                    ssxManager.UploadStyleSheetExtensions();
                    pageConverter.UploadAttachments();
                    addin.AnnotationMaintainer.UpdateAnnotations();
                }
            }
            catch (COMException ex)
            {
                string message = "An internal Word error appeared when trying to save your file.";
                message += Environment.NewLine + ex.Message;
                Log.Exception(ex);
                UserNotifier.Error(message);
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
        /// <param name="spaceName">The name of the wiki space.</param>
        /// <param name="pageName">The name of page.</param>
        /// <param name="pageTitle">The title of the page.</param>
        /// <param name="sender">
        /// The instance of the fprm that started the action.
        /// This form need to be closed before swithing the Active Word Document.
        /// </param>
        public void AddNewPage(String spaceName, String pageName, String pageTitle, Form sender)
        {
            //Any modal dialog nust be closed before opening or closing active documents.
            if (sender != null)
            {
                sender.Hide();
                Application.DoEvents();
                sender.Close();
            }
            try
            {
                if (!this.Client.LoggedIn)
                {
                    Client.Login(addin.username, addin.password);
                }
                String pageFullName = spaceName + "." + pageName;
                String localFileName = pageFullName.Replace(".", "-");
                String folder = addin.AddinSettings.PagesRepository + "TempPages";
                ConvertToNormalFolder(folder);
                //content = new WebToLocalHTML(addin.serverURL, folder, localFileName).AdaptSource(content);
                ConversionManager pageConverter = new ConversionManager(addin.AddinSettings, addin.serverURL, folder, pageFullName, localFileName, addin.Client);
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
                Encoding encoding = Client.ServerEncoding;
                byte[] buffer = encoding.GetBytes(pageContent);
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();
                addin.currentPageFullName = pageFullName;
                addin.EditedPages.Add(localFileName, pageFullName);

                //Open the file with Word
                Word.Document doc = OpenHTMLDocument(localFileName);
                Application.DoEvents();
                doc.Activate();
                newDoc = doc;

                //If it's a new space, add it to the wiki structure and mark it as unpublished
                List<Space> spaces = Globals.XWikiAddIn.wiki.spaces;
                Space space = null;
                foreach (Space sp in spaces)
                {
                    if (sp.name == spaceName)
                    {
                        space = sp;

                        //Add the new page to the wiki structure and mark it as unpublished
                        XWikiDocument xwdoc = new XWikiDocument();
                        xwdoc.name = pageName;
                        xwdoc.published = false;
                        xwdoc.space = spaceName;
                        space.documents.Add(xwdoc);

                        break;
                    }
                }

                if (space == null)
                {
                    space = new Space();
                    space.name = spaceName;
                    space.published = false;
                    Globals.XWikiAddIn.wiki.spaces.Add(space);

                    //Add the new page to the wiki structure and mark it as unpublished
                    XWikiDocument xwdoc = new XWikiDocument();
                    xwdoc.name = pageName;
                    xwdoc.published = false;
                    xwdoc.space = spaceName;
                    space.documents.Add(xwdoc);
                }
            }
            catch (IOException ex)
            {
                UserNotifier.Error(ex.Message);
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
            dialog.CustomPlaces.Add(addin.AddinSettings.DownloadedAttachmentsRepository);
            dialog.CustomPlaces.Add(addin.AddinSettings.PagesRepository);
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
                UserNotifier.Error("There was an error on the server. The pages in MSOffice space don't have programming rights");
                hasErrors = true;
            }
            else if (content.Contains(HTTPResponses.WRONG_REQUEST))
            {
                Log.Error("Server " + addin.serverURL + " wrong request");
                UserNotifier.Error("Server error: Wrong request");
                hasErrors = true;
            }
            else if (content.Contains(HTTPResponses.NO_EDIT_RIGHTS))
            {
                Log.Information("User tried to edit a page on " + addin.serverURL + " whithout edit rights");
                UserNotifier.Error("You dont have the right to edit this page");
                hasErrors = true;
            }
            else if (content.Contains(HTTPResponses.NO_GROOVY_RIGHTS))
            {
                Log.Error("Server " + addin.serverURL + " error on parsing groovy - no groovy rights");
                String message = "There was an error on the server." + Environment.NewLine;
                message += "Please contact your server adminitrator. Error on executing groovy page in MSOffice space";
                UserNotifier.Error(message);
                hasErrors = true;
            }
            else if (content.Contains(HTTPResponses.INSUFFICIENT_MEMMORY))
            {
                Log.Error("Server " + addin.serverURL + " reported OutOfMemmoryException");
                String message = "There was an error on the server." + Environment.NewLine;
                message += "The server has insufficient memmory to execute the current tasks.";
                UserNotifier.Error(message);
                hasErrors = true;
            }
            else if (content.Contains(HTTPResponses.VELOCITY_PARSER_ERROR))
            {
                Log.Error("Server " + addin.serverURL + " error when parsing page. ");
                String message = "There was an error on the server" + Environment.NewLine;
                message += "'Error while parsing velocity page'";
                UserNotifier.Error(message);
                hasErrors = true;
            }
            return hasErrors;
        }

        /// <summary>
        /// Removes all protected pages from the Word wiki structure.
        /// </summary>
        /// <param name="wiki">The wiki instance.</param>
        /// <param name="wildcards">The list of protected pages wildcards.</param>
        public void HideProtectedPages(Wiki wiki, List<String> wildcards)
        {
            foreach (XWikiDocument doc in wiki.GetAllDocuments())
            {
                foreach (String wildcard in wildcards)
                {
                    String docFullName = doc.space + "." + doc.name;
                    if (UtilityClass.IsWildcardMatch(wildcard, docFullName, true))
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
                Object encoding = MsoEncoding.msoEncodingUTF8;
                Object missing = Type.Missing;
                Object originalFilePath = document.FullName;
                Object initialDocSaveFormat = document.SaveFormat;
                document.SaveAs(ref copyPath, ref format, ref missing, ref missing, ref missing, ref missing,
                                ref missing, ref missing, ref missing, ref missing, ref missing, ref encoding,
                                ref missing, ref missing, ref missing, ref missing);
                //Refresh the active instance(required after acquiring chart data from Excel)
                document = addin.Application.ActiveWindow.Document;
                document.SaveAs(ref originalFilePath, ref initialDocSaveFormat, ref missing, ref missing, ref missing, ref missing,
                                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                                ref missing, ref missing, ref missing, ref missing);
            }
            catch (IOException ioex) 
            {
                Log.Exception(ioex);
                UserNotifier.Error(ioex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Activates the newest Word window.
        /// </summary>
        /// <param name="Doc">The document instance that triggers the deactivate event.</param>
        /// <param name="Wn">The document's </param>
        void KeepNewWindowActivated(Word.Document doc, Word.Window Wn)
        {
            if (newDoc == doc)
            {
                doc.Activate();
            }
        }

        /// <summary>
        /// Reloads the active document, if it's a wiki page.
        /// </summary>
        public void RefreshActiveDocument()
        {
            String activeWikiPageName = GetActivePageName();
            if (activeWikiPageName != null)
            {                 
                //references for COM interop
                object no = false;
                object missing = Type.Missing;

                //close the active document
                addin.ActiveDocumentInstance.Close(ref no, ref missing, ref missing);

                //reload the document
                EditPage(activeWikiPageName);                
            }
        }

        /// <summary>
        /// Gets the fullname of the wiki page edited in the active document.
        /// </summary>
        /// <returns>
        /// A string representing the name of the currently edited wiki page.
        /// If the active documet is not a wiki page then null is returned.
        /// </returns>
        public String GetActivePageName()
        {
            foreach (String docPath in addin.EditedPages.Keys)
            {
                if (docPath == addin.ActiveDocumentFullName)
                {
                    return addin.EditedPages[docPath];
                }
            }
            return null;
        }

        private void ReportSaveProblem()
        {
            String error = Log.GetLastException().Message;
            String message = "There was an error on the server when trying to save the page";
            message += Environment.NewLine + "Details:" + Environment.NewLine + error;
            UserNotifier.Error(message);
        }
    }
}
