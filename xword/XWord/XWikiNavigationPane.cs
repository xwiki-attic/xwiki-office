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
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml.Serialization;
using Tools = Microsoft.Office.Tools;
using Word = Microsoft.Office.Interop.Word;
using XWiki;
using XWiki.Clients;
using System.Diagnostics;
using XWiki.Logging;
using UICommons;

namespace XWord
{
    public partial class XWikiNavigationPane : UserControl
    {
        /// <summary>
        /// Specifies the level where the wiki spaces are placed in the tree view.
        /// </summary>
        public const int TREE_SPACE_LEVEL = 0;
        /// <summary>
        /// Specifies the level where the pages are placed in the tree view.
        /// </summary>
        public const int TREE_PAGE_LEVEL = 1;
        /// <summary>
        /// Specifies the level where the attachments are placed in the tree view.
        /// </summary>
        public const int TREE_ATTACHMENT_LEVEL = 2;
        /// <summary>
        /// The tag for the spaces.
        /// </summary>
        public const string TREE_SPACE_TAG = "space";
        /// <summary>
        /// The tag for the pages.
        /// </summary>
        public const string TREE_PAGE_TAG = "page";
        /// <summary>
        /// The tag for attachments.
        /// </summary>
        public const string TREE_ATTACHMENT_TAG = "attachment";
        /// <summary>
        /// Specifies the index of the image displayed by the selected node.
        /// </summary>
        public const int SELECTED_NODE_IMAGE_INDEX = 3;

        /// <summary>
        /// The tag for WikiExplorer
        /// </summary>
        public static String XWIKI_EXPLORER_TAG = "WIKI_EXPLORER";

        /// <summary>
        /// The title of the Wiki Explorer taskpanes.
        /// </summary>
        public static String TASK_PANE_TITLE = " Wiki Explorer";

        private const String EMPTY_NODE_TAG = "EMPTY";

        private bool loadingWikiData;
        
        //NetworkCredential nc = new NetworkCredential("Admin", "admin");
        XmlSerializer serializer = new XmlSerializer(typeof(WikiStructure));
        private object missing = Type.Missing;

        #region properties

        /// <summary>
        /// Gets or sets the rendered content of the Active Document.
        /// <remarks>
        /// Use this property to set and get the document's visible text.
        /// </remarks>
        /// </summary>
        public String DocumentContent
        {
            get
            {
                Word.Range range = Addin.Application.ActiveDocument.Range(ref missing, ref missing);
                range.TextRetrievalMode.IncludeHiddenText = true;
                return range.Text;
            }
            set
            { 
                Word.Range range = Addin.Application.ActiveDocument.Range(ref missing, ref missing);
                range.Text = value;
            }
        }

        /// <summary>
        /// The instance that the addin uses for conecting to the wiki server.
        /// </summary>
        public IXWikiClient Client
        {
            get { return Globals.XWikiAddIn.Client; }
            set { Globals.XWikiAddIn.Client = value; }
        }

        /// <summary>
        /// The cookies stored in the addin's web Client.
        /// </summary>
        public List<String> Cookies
        {
            get { return XWikiAddIn.cookies; }
            set { XWikiAddIn.cookies = value; }
        }

        /// <summary>
        /// Gets or sets the WikiStructure of wiki the add-in is curreltly connected to.
        /// </summary>
        public WikiStructure Wiki
        {
            get { return Globals.XWikiAddIn.wiki; }
            set { Globals.XWikiAddIn.wiki = value; }
        }

        /// <summary>
        /// Gets the instance of the add-in.
        /// </summary>
        public XWikiAddIn Addin
        {
            get
            {
                return Globals.XWikiAddIn;
            }
        }

        /// <summary>
        /// Instance containing the most common actions the add-in could perform.
        /// </summary>
        public AddinActions AddinActions
        {
            get { return Globals.XWikiAddIn.AddinActions; }
        }

        #endregion

        /// <summary>
        /// Instantiates the user control.
        /// <remarks>Constructor</remarks>
        /// </summary>
        /// <param name="addin">The global instance of the addin.</param>
        public XWikiNavigationPane(XWikiAddIn addin)
        {
            InitializeComponent();
            pictureBox.Visible = false;
            loadingWikiData = false;
        }

        /// <summary>
        /// Event triggered on the cutom form load.
        /// </summary>
        /// <param name="sender">Teh control that triggered the event. Usualy the current instance.</param>
        /// <param name="e">The parameters of the event.</param>
        private void XWikiNavigationPane_Load(object sender, EventArgs e)
        {
            treeView.ImageList = iconList;
            treeView.SelectedImageIndex = SELECTED_NODE_IMAGE_INDEX;
            this.BuildTree();
        }

        /// <summary>
        /// Parses the wiki structure from the given string.
        /// And attaches the new instance to the addin instance.
        /// </summary>
        /// <param name="s"></param>
        private void ParseWikiStructure(String s)
        {
            s = s.Replace("<p/>", "");
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] buffer = encoding.GetBytes(s);
            MemoryStream memoryStream = new MemoryStream(buffer, false);
            XmlSerializer serializer = new XmlSerializer(typeof(WikiStructure));
            WikiStructure wiki = (WikiStructure)serializer.Deserialize(memoryStream);
            memoryStream.Close();

            //keep unpublished spaces and pages
            WikiStructure oldWikiStruct = null;
            if (Addin.wiki != null)
            {
                oldWikiStruct = Addin.wiki.GetUnpublishedWikiStructure();
            }            
            Addin.wiki = wiki;                      
        }

        /// <summary>
        /// Adds the nodes to the tree.
        /// </summary>
        public void BuildTree()
        {
            treeView.Nodes.Clear();
            if (Wiki != null)
            {
                foreach (Space space in Wiki.spaces)
                {
                    TreeNode node = treeView.Nodes.Add(space.name);
                    node.Name = space.name;
                    node.ImageIndex = TREE_SPACE_LEVEL;
                    //mark unpublished spaces   
                   
                    if (!space.published)
                    {
                        node.ForeColor = Color.Blue;
                    }                    

                    foreach (XWikiDocument doc in space.documents)
                    {
                        TreeNode childNode = node.Nodes.Add(doc.space + "." + doc.name, doc.name);
                        childNode.ImageIndex = TREE_PAGE_LEVEL;
                        if (!doc.published)
                        {
                            //mark unpublished pages
                            childNode.ForeColor = Color.Blue;
                            if (space.published)
                            {
                                //mark published spaces with unpublished pages
                                node.ForeColor = Color.BlueViolet;
                            }
                        }
                    }
                    if (space.documents.Count == 0)
                    {
                        //Add a empty child node to force the tree to display a + on the parent node
                        TreeNode discardNode = node.Nodes.Add("Empty");
                        discardNode.Tag = EMPTY_NODE_TAG;
                    }
                }
            }
        }

        /// <summary>
        /// Event triggered when the user double clicks in the Wiki Explorer.
        /// <remarks>Each node level has its' specific action.</remarks>
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event paramaters.</param>
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                TreeNode node = treeView.SelectedNode;
                //If double click on a space name -> show pages
                if (node.Level == TREE_SPACE_LEVEL)
                {
                    ShowPages(node);
                }
                //If double click on a page name -> show attachments
                if (node.Level == TREE_PAGE_LEVEL)
                {
                    //getPage(node.Name);
                    Globals.Ribbons.XWikiRibbon.uploadAttToPage.Enabled = true;
                    ShowAttachments(node);
                }
                //If double click on a attachment name -> Download and open the file
                else if (node.Level == TREE_ATTACHMENT_LEVEL && node.Parent != null)
                {
                    String pageName = node.Parent.Name;
                    String attachmentname = node.Name;
                    String path = AddinActions.SaveFileDialog(attachmentname);
                    if (path != null)
                    {
                        FileInfo attachmentInfo = AddinActions.DownloadAttachment(pageName, attachmentname, path);
                        AddinActions.StartProcess(attachmentInfo.FullName);
                        Globals.Ribbons.XWikiRibbon.uploadAttToPage.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Connects to a XWiki server and retrieves the attachment list to for a page.
        /// </summary>
        /// <remarks>An async method is recomended so that it does not block the UI</remarks>
        /// <param name="node">The node of a tree control</param>
        public void ShowAttachments(TreeNode node)
        {
            if (node.Level == TREE_PAGE_LEVEL)
            {
                List<String> attachments = Client.GetDocumentAttachmentList(node.Name);
                node.Nodes.Clear();
                node.ImageIndex = TREE_PAGE_LEVEL;
                foreach (String s in attachments)
                {
                    TreeNode childNode = new TreeNode(s);
                    childNode.Name = s;
                    childNode.Tag = TREE_ATTACHMENT_TAG;
                    childNode.ImageIndex = TREE_ATTACHMENT_LEVEL;
                    node.Nodes.Add(childNode);
                }
                if (!node.IsExpanded)
                {
                    node.ExpandAll();
                }
            }
        }

        /// <summary>
        /// Connects to a XWiki server and retrieves the pages list to for a space.
        /// </summary>
        /// <param name="node">The node of a tree control</param>
        public void ShowPages(TreeNode node)
        {
            if (node.Level == TREE_SPACE_LEVEL)
            {
                List<String> pages = Client.GetPagesNames(node.Name);
                node.Nodes.Clear();
                node.ImageIndex = TREE_SPACE_LEVEL;
                pages.Sort();
                foreach (String pageName in pages)
                {
                    String pageFullName = node.Name + "." + pageName;
                    TreeNode childNode = node.Nodes.Add(pageName);
                    childNode.Name = pageFullName;
                    childNode.Tag = TREE_PAGE_TAG;
                    childNode.ImageIndex = TREE_PAGE_LEVEL;
                }
                if (!node.IsExpanded)
                {                    
                    node.ExpandAll();
                }
            }
        }

        /// <summary>
        /// Event triggered when the Wiki Explorer loses the focus.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event paramaters.</param>
        private void treeView1_Leave(object sender, EventArgs e)
        {
            //Disables all buttons in the context menu.
            Globals.Ribbons.XWikiRibbon.DisableTreeNavigationActions();
        }

        /// <summary>
        /// Fires right after a new node is selected in the navigation tree.
        /// </summary>
        /// <param name="sender">The control that fired the event.</param>
        /// <param name="e">The arguments of the event</param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Globals.Ribbons.XWikiRibbon.DisableTreeNavigationActions();
            ResetNavigationContextMenus();
            if (treeView.SelectedNode != null)
            {
                switch (treeView.SelectedNode.Level)
                {
                    case TREE_SPACE_LEVEL:
                        //Enabling contextMenus
                        cmnuNewPage.Enabled = true;
                        Globals.Ribbons.XWikiRibbon.btnAddPage.Enabled = true;
                        Globals.Ribbons.XWikiRibbon.btnShowPages.Enabled = true;
                        break;
                    case TREE_PAGE_LEVEL:
                        //synchronizing the Ribbon UI
                        Globals.Ribbons.XWikiRibbon.uploadAttToPage.Enabled = true;
                        Globals.Ribbons.XWikiRibbon.btnShowAttachments.Enabled = true;
                        Globals.Ribbons.XWikiRibbon.btnViewInBrowser.Enabled = true;
                        Globals.Ribbons.XWikiRibbon.btnEditPage.Enabled = true;
                        Globals.XWikiAddIn.AddinStatus.TaskPaneSelectedPage.Set("space", treeView.SelectedNode.Parent.Name);
                        Globals.XWikiAddIn.AddinStatus.TaskPaneSelectedPage.Set("page", treeView.SelectedNode.Name);
                        //Enabling contextMenus
                        cmnuAttachFile.Enabled = true;
                        cmnuEditPage.Enabled = true;
                        cmnuViewInBrowser.Enabled = true;
                        break;
                    case TREE_ATTACHMENT_LEVEL:
                        //synchronizing the Ribbon UI
                        Globals.Ribbons.XWikiRibbon.downloadAtt.Enabled = true;
                        Globals.Ribbons.XWikiRibbon.btnDownload.Enabled = true;
                        Globals.Ribbons.XWikiRibbon.btnDownloadAndOpen.Enabled = true;
                        //Enabling contextMenus
                        cmnuDownloadOpenFile.Enabled = true;
                        cmnuDownloadFile.Enabled = true; 
                        break;
                    default:
                        break;
                }
            }            
        }




        /// <summary>
        /// Downloads the selected file to the local disk and openes it(optional).
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void cmnuDownloadOpenFile_Click(object sender, EventArgs e)
        {
            String page = Globals.XWikiAddIn.AddinStatus.TaskPaneSelectedPage.Get("page");
            String attachmentName = treeView.SelectedNode.Name;
            String path = AddinActions.SaveFileDialog(attachmentName);
            if (path != null)
            {
                FileInfo attachmentInfo = AddinActions.DownloadAttachment(page, attachmentName, null);
                if (sender == cmnuDownloadOpenFile)
                {
                    AddinActions.StartProcess(attachmentInfo.FullName);
                }
                else
                {
                    UserNotifier.Message("Download finised.");
                }
            }
        }

        /// <summary>
        /// Attaches the current Word document to the selected wiki page.
        /// <remarks>Event triggered when the "Attach file" button in the context menu is pressed.</remarks>
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void cmnuAttachFile_Click(object sender, EventArgs e)
        {
            String page = Globals.XWikiAddIn.AddinStatus.TaskPaneSelectedPage.Get("page");
            bool finished = Globals.XWikiAddIn.AddinActions.AttachCurrentFile(page);
            if (finished)
            {
                UserNotifier.Message("Upload finished.");
            }
        }

        /// <summary>
        /// //Enables the buttons that start actions that that are valid for the current selected node.
        /// </summary>
        private void ResetNavigationContextMenus()
        {
            cmnuAttachFile.Enabled = false;
            cmnuDownloadFile.Enabled = false;
            cmnuDownloadOpenFile.Enabled = false;
            cmnuEditPage.Enabled = false;
            cmnuNewPage.Enabled = false;
            cmnuViewInBrowser.Enabled = false;
        }

        /// <summary>
        /// Event triggered when the "Edit Page" button in the context menu is pressed.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void cmnuEditPage_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Level == TREE_PAGE_LEVEL)
            {
                String pageFullName = treeView.SelectedNode.Name;
                AddinActions.EditPage(pageFullName);                
            }
        }

        /// <summary>
        /// Event triggered when the "Save" button is pressed.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void btnSaveToXWiki_click(object sender, EventArgs e)
        {
            //Saves the currently edited page or document to the server.
            AddinActions.SaveToServer();
        }

        /// <summary>
        /// Event triggered when the context menu is opening.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void treeMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (treeView.SelectedNode == null)
            {
                //Enables the buttons that start actions that that are valid for the current selected node.
                ResetNavigationContextMenus();
            }
        }

        /// <summary>
        /// Event triggered when the "View in browser" button is pressed.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void cmnuViewInBrowser_Click(object sender, EventArgs e)
        {
            ViewInBrowser();        
        }

        /// <summary>
        /// Event triggered when the "New Page" context button is pressed.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event paramaters.</param>
        private void cmnuNewPage_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Level == TREE_SPACE_LEVEL)
            {
                String spaceName = treeView.SelectedNode.Text;
                WikiStructure wiki = Wiki;
                AddPageForm addPageForm = new AddPageForm(ref wiki, spaceName);
                new AddPageFormManager(ref addPageForm).EnqueueAllHandlers();
                addPageForm.Show();
            }
        }

        /// <summary>
        /// Event triggered when the context menu loses the focus.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void XWikiNavigationPane_Leave(object sender, EventArgs e)
        {
            //Disables all buttons.
            Globals.Ribbons.XWikiRibbon.DisableTreeNavigationActions();
        }

        /// <summary>
        /// Reloads the wiki structure displayed in the wiki explorer.
        /// </summary>
        public void RefreshWikiExplorer()
        {
            pictureBox.Visible = true;
            treeView.Nodes.Clear();
            pictureBox.BringToFront();
            loadingWikiData = true;
            timerUI.Start();
            try
            {
                backgroundWorker.RunWorkerAsync(this);
            }
            catch (InvalidOperationException)
            {
                UserNotifier.Error("This operation is already in progress");
            }
        }

        /// <summary>
        /// Opens the URL if the selected page in the default browser.
        /// </summary>
        public void ViewInBrowser()
        {
            if (treeView.SelectedNode != null)
            {
                if (treeView.SelectedNode.Level == TREE_PAGE_LEVEL)
                {
                    try
                    {
                        String pageFullName = treeView.SelectedNode.Name;
                        String url = Client.GetURL(pageFullName);
                        //url = Addin.serverURL + url;
                        Process p = new Process();
                        p.StartInfo = new ProcessStartInfo(url);
                        p.Start();
                    }
                    catch (Win32Exception ex)
                    {
                        UserNotifier.Error(ex.Message);
                    }
                    catch (WebException webex)
                    {
                        UserNotifier.Error(webex.Message);
                    }
                }
                else
                {
                    UserNotifier.Exclamation("No page is selected");
                }
            }
            else
            {
                UserNotifier.Exclamation("Please select a page in the Wiki Explorer first.");
            }
        }
        
        /// <summary>
        /// Reloads the wiki structure displayed in the wiki explorer.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void refreshWikiExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RefreshWikiExplorer();
        }

        /// <summary>
        /// Event triggered when the "Refresh" button is pressed.
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Cursor c = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            this.RefreshWikiExplorer();
            this.Cursor = c;
        }

        /// <summary>
        /// Event triggered when the "Add Space" button is pressed.
        /// </summary>
        private void btnAddSpace_Click(object sender, EventArgs e)
        {
            AddPageForm addPageForm = new AddPageForm(ref Globals.XWikiAddIn.wiki, true, false);
            new AddPageFormManager(ref addPageForm).EnqueueAllHandlers();
            addPageForm.ShowDialog();
        }

        /// <summary>
        /// Event triggered when the control resizes.
        /// </summary>
        private void XWikiNavigationPane_Resize(object sender, EventArgs e)
        {
            if (loadingWikiData)
            {
                pictureBox.Parent = this;
                pictureBox.Image = Properties.Resources.loadingWikiData;
                pictureBox.Visible = true;
                pictureBox.Height = 50;
                pictureBox.Width = 50;
                pictureBox.Top = this.Height / 2 - pictureBox.Height / 2;
                pictureBox.Left = this.Width / 2 - pictureBox.Width / 2;
                pictureBox.Visible = true;
                if (timerUI.Enabled == false)
                {
                    timerUI.Start();
                }
            }
        }

        /// <summary>
        /// Loads the wiki structure(Spaces and pages).
        /// </summary>
        private void LoadWikiStructure()
        {
            try
            {
                WikiStructure wiki = RequestWikiStructure();
                //keep unpublished spaces and pages
                WikiStructure oldWikiStruct = null;
                if (Addin.wiki != null)
                {
                    oldWikiStruct = Addin.wiki.GetUnpublishedWikiStructure();
                    AddUnpublishedData(ref wiki, ref oldWikiStruct);
                }
                
                //Attach the newest data to the addin
                Addin.wiki = wiki;
            }
            catch (Exception ex)
            {
                UserNotifier.Error(ex.Message);
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Requests the spaces and pages of the wiki from the server.
        /// </summary>
        /// <returns>A WikiStructure instance.</returns>
        private WikiStructure RequestWikiStructure()
        {
            WikiStructure wikiStructure = new WikiStructure();
            List<String> spacesNames = Client.GetSpacesNames();
            spacesNames.Sort();
            wikiStructure.AddSpaces(spacesNames);
            //TODO: Implement user option
            if (false)
            {
                foreach (String spaceName in spacesNames)
                {
                    List<String> pagesNames = Client.GetPagesNames(spaceName);
                    wikiStructure[spaceName].AddDocuments(pagesNames);
                }
            }
            //TODO: Add opt-in prefetch
            return wikiStructure;
        }

        private void AddUnpublishedData(ref WikiStructure actualWiki,ref WikiStructure unpublishedWiki)
        {
            //add local spaces and pages
            if (unpublishedWiki != null)
            {
                //add unexistent spaces from old structure
                //and update existing spaces with unpublished pages
                foreach (Space sp in unpublishedWiki.spaces)
                {
                    if (actualWiki.ContainsSpace(sp.name))
                    {
                        //The old local space containing local unpublished documents.
                        Space existingSpace = actualWiki[sp.name];
                        foreach (XWikiDocument xwd in sp.documents)
                        {
                            existingSpace.documents.Add(xwd);
                        }
                        existingSpace.published = true;
                    }
                    else
                    {
                        sp.published = false;
                        actualWiki.spaces.Add(sp);
                    }
                }
            }  
        }

        /// <summary>
        /// Gets a list with the protected pages and spaces in the wiki
        /// The names support whildcards.
        /// </summary>
        /// <returns>A list with the protected pages and spaces in the wiki.</returns>
        public List<String> GetProtectedPages()
        {
            String[] separators = { " ", "\n", "\t", "\r", ";", "\\" };
            String url = Addin.serverURL + XWikiURLs.ProtectedPagesURL;
            if (Client.ClientType == XWikiClientType.HTTP_Client)
            {
                XWikiHTTPClient httpClient = (XWikiHTTPClient)Client;
                Stream data = httpClient.OpenRead(url);
                StreamReader reader = new StreamReader(data);
                String pages = reader.ReadToEnd();
                String[] pagesArray = pages.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                return new List<string>(pagesArray);
            }
            else
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Loads the wiki data using a background thread.
        /// </summary>
        /// <param name="e">Event parameters. 'e.Argument' contains the </param>
        /// <param name="sender">The control that triggered the event.</param>
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (Client.LoggedIn)
                {
                    LoadWikiStructure();
                    Addin.ProtectedPages = GetProtectedPages();
                    //The pages will be displayed, and the user will be prompted
                    //when trying to edit a protected page.
                    //AddinActions.HideProtectedPages(wiki, addin.ProtectedPages);
                }
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                    Log.Exception(ex);
                }
                else
                {
                    treeView.Nodes.Clear();
                    loadingWikiData = false;
                    Log.Exception(ex);
                    UserNotifier.Error(ex.Message);
                }
            }
            backgroundWorker.ReportProgress(100, e.Argument);
        }

        /// <summary>
        /// Event triggered when the background worker reports it's progress.
        /// </summary>
        /// <param name="e">The event parameters</param>
        /// <param name="sender">The control that triggered the event.</param>
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //If work is completed
            if (e.UserState != null)
            {
                XWikiNavigationPane navPane = (XWikiNavigationPane)e.UserState;
                if (e.ProgressPercentage == 100)
                {
                    //navPane.pictureBox.Visible = false;
                    loadingWikiData = false;
                }
            }
        }

        /// <summary>
        /// Refreshes the wiki explorer after the data has finished loadind.
        /// </summary>
        private void timerUI_Tick(object sender, EventArgs e)
        {
            if (!loadingWikiData && pictureBox.Visible)
            {
                pictureBox.Visible = false;
                //Display the same content in all WikiExplorers
                SynchTaskPanes();
                //Stops the timer until the next data load.
                timerUI.Stop();
            } 
            else if (loadingWikiData)
            {
                pictureBox.Visible = true;
                pictureBox.BringToFront();
            }
        }

        /// <summary>
        /// Syncronizes all WikiExplorer task panes.
        /// </summary>
        public static void SynchTaskPanes()
        {
            foreach (Tools.CustomTaskPane ctp in Globals.XWikiAddIn.XWikiCustomTaskPanes)
            {
                String tag = (String)ctp.Control.Tag;
                if (tag.Contains(XWIKI_EXPLORER_TAG))
                {
                    XWikiNavigationPane pane = (XWikiNavigationPane)ctp.Control;
                    //Do sync work;
                    pane.BuildTree();
                }
            }
        }

        /// <summary>
        /// Deletes all nodes from the treview.
        /// </summary>
        public void ClearNodes()
        {
            treeView.Nodes.Clear();
        }

        /// <summary>
        /// Reloads data from the server and syncs all taskpanes
        /// </summary>
        public static void ReloadDataAndSyncAll()
        {
            foreach (Tools.CustomTaskPane ctp in Globals.XWikiAddIn.XWikiCustomTaskPanes)
            {
                String tag = (String)ctp.Control.Tag;
                if (tag.Contains(XWIKI_EXPLORER_TAG))
                {
                    XWikiNavigationPane pane = (XWikiNavigationPane)ctp.Control;
                    pane.ClearNodes();
                    pane.RefreshWikiExplorer();
                    break;
                }
            }
            SynchTaskPanes();
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //remove empty child nodes.
            foreach (TreeNode node in e.Node.Nodes)
            {
                String tag = (String)node.Tag;
                if (tag == EMPTY_NODE_TAG)
                {
                    node.Remove();
                }
            }
            //If has no child noded connect to the server
            if (e.Node.Level == TREE_SPACE_LEVEL && (e.Node.Nodes.Count == 0))
            {
                ShowPages(e.Node);
            }
        }
    }
}