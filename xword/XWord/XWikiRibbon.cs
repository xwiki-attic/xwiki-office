using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Tools.Ribbon;
using Word = Microsoft.Office.Interop.Word;
using VSTO = Microsoft.Office.Tools;

using XWord.VstoExtensions;
using XWiki.Logging;
using XWiki;
using UICommons;

namespace XWord
{
    /// <summary>
    /// Provides XWiki specific UI elements using the Office 2007 Ribbon UI.
    /// </summary>
    public partial class XWikiRibbon : OfficeRibbon
    {

        Word.ApplicationEvents4_DocumentBeforeSaveEventHandler saveHandler;
        /// <summary>
        /// The addin instance.
        /// </summary>
        private XWikiAddIn Addin
        {
            get { return Globals.XWikiAddIn; }
        }

        private List<String> syntaxes;

        /// <summary>
        /// Default constructor. Initializes all components.
        /// </summary>
        public XWikiRibbon()
        {
            InitializeComponent();
            saveHandler = new Word.ApplicationEvents4_DocumentBeforeSaveEventHandler(Application_DocumentBeforeSave);
        }

        private void XWikiRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            Addin.ClientInstanceChanged += new XWikiAddIn.ClientInstanceChangedHandler(Addin_ClientInstanceChanged);
        }

        void Addin_ClientInstanceChanged(object sender, EventArgs e)
        {
            //Refresh the available syntaxes.
            syntaxes = Addin.Client.GetAvailableSyntaxes();
            dropDownSyntax.Items.Clear();
            foreach (String syntax in syntaxes)
            {
                RibbonDropDownItem rddi = new RibbonDropDownItem();
                rddi.Label = syntax;
                dropDownSyntax.Items.Add(rddi);
            }
            dropDownSyntax.SelectedItemIndex = 0;
        }
        /// <summary>
        /// Refreshes the buttons from the selectionOptionsGroup according to the connection state.
        /// </summary>
        public void Refresh(object sender, EventArgs e) 
        {
            if (Addin.Client.LoggedIn)
            {
                if (!selectionOptionsGroup.Visible)
                {
                    selectionOptionsGroup.Visible = true;
                }
            }
            else if (selectionOptionsGroup.Visible)
            {
                selectionOptionsGroup.Visible = false;
            }
        }

        /// <summary>
        /// Changes the selected item from dropDownSyntax control with the one
        /// that is labeled with toSyntax string.
        /// </summary>
        /// <param name="toSyntax">Existing item in dropDownSyntax to be selected.</param>
        public void SwitchSyntax(string toSyntax)
        {
            for (int i = 0; i < dropDownSyntax.Items.Count; i++)
                if (dropDownSyntax.Items[i].Label == toSyntax)
                    dropDownSyntax.SelectedItem = dropDownSyntax.Items[i];
        }

        private void SyncSaving_Click(object sender, RibbonControlEventArgs e)
        {
            if (syncSaving.Checked)
            {
                Addin.Application.DocumentBeforeSave += saveHandler;
            }
            else
            {
                Addin.Application.DocumentBeforeSave -= saveHandler;
            }
        }

        void Application_DocumentBeforeSave(Microsoft.Office.Interop.Word.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {
            throw new NotImplementedException();
        }

        private void toggleButton1_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.XWikiAddIn.ToggleTaskPanes();
        }

        private void Show_ConnetionDialog(object sender, RibbonControlEventArgs e)
        {
            AddinSettingsForm addinSettingsForm = new AddinSettingsForm();
            new AddinSettingsFormManager(ref addinSettingsForm).EnqueueAllHandlers();

            DialogResult result = addinSettingsForm.ShowDialog();
            if (result == DialogResult.OK)
            {                
                //AddTaskPanes(); TODO: Sync all taskpanes
            }
        }

        private void uploadAttToPage_Click(object sender, RibbonControlEventArgs e)
        {
            String page = Globals.XWikiAddIn.AddinStatus.TaskPaneSelectedPage.Get("page");
            if (page == null)
            {
                UserNotifier.Exclamation("You need to select a pege in the wiki explorer first.");
                return;
            }
            bool finished = Globals.XWikiAddIn.AddinActions.AttachCurrentFile(page);
            if (finished)
            {
                UserNotifier.Message("Upload finished.");
            }
            else
            {
                UserNotifier.Error("Upload failed. Make sure you have selected a page before uploading");
            }
        }

        private void btnNewPage_Click(object sender, RibbonControlEventArgs e)
        {
            AddPageForm addPageForm = new AddPageForm(ref Globals.XWikiAddIn.wiki);
            new AddPageFormManager(ref addPageForm).EnqueueAllHandlers();
            addPageForm.ShowDialog();
        }

        private void btnSavePage_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                if (Addin.currentPageFullName == "" || Addin.currentPageFullName == null)
                {
                    AddPageForm addPageForm = new AddPageForm(ref Addin.wiki, false, true);
                    new AddPageFormManager(ref addPageForm).EnqueueAllHandlers();
                    addPageForm.ShowDialog();
                }
                else
                {
                    Addin.AddinActions.SaveToServer();
                }
            }
            catch(NullReferenceException ex)
            {
                Log.Exception(ex);
                UserNotifier.Exclamation("You are not currently editing a wiki page");
            }
        }

        private void btnAddPage_Click(object sender, RibbonControlEventArgs e)
        {
            TreeView treeView = Globals.XWikiAddIn.XWikiTaskPane.treeView;
            if (treeView.SelectedNode != null)
            {
                String spaceName = treeView.SelectedNode.Text;
                AddPageForm addPageForm = new AddPageForm(ref Globals.XWikiAddIn.wiki, spaceName);
                new AddPageFormManager(ref addPageForm).EnqueueAllHandlers();
                addPageForm.ShowDialog();
            }
            else
            {
                //see XOFFICE-20
                //MessageBox.Show("You need to select a space in the wiki explorer.","XWord");
                AddPageForm addPageForm = new AddPageForm(ref Globals.XWikiAddIn.wiki, true, false);
                new AddPageFormManager(ref addPageForm).EnqueueAllHandlers();
                addPageForm.ShowDialog();
            }
        }

        private void downloadAtt_Click(object sender, RibbonControlEventArgs e)
        {
            if (Addin.XWikiTaskPane.treeView.SelectedNode != null)
            {
                String page = Globals.XWikiAddIn.AddinStatus.TaskPaneSelectedPage.Get("page");
                String attachmentName = Addin.XWikiTaskPane.treeView.SelectedNode.Name;
                String path = Globals.XWikiAddIn.AddinActions.SaveFileDialog(attachmentName);
                if (path != null)
                {
                    FileInfo attachmentInfo = Addin.AddinActions.DownloadAttachment(page, attachmentName, path);
                    UserNotifier.Message("Download finised.");
                }
            }
            else
            {
                UserNotifier.Exclamation("You need to select an attachment in the wiki explorer.");
            }
        }

        private void btnDownloadAndOpen_Click(object sender, RibbonControlEventArgs e)
        {
            if (Addin.XWikiTaskPane.treeView.SelectedNode != null)
            {
                String page = Globals.XWikiAddIn.AddinStatus.TaskPaneSelectedPage.Get("page");
                String attachmentName = Addin.XWikiTaskPane.treeView.SelectedNode.Name;
                String path = Globals.XWikiAddIn.AddinActions.SaveFileDialog(attachmentName);
                if (path != null)
                {
                    FileInfo attachmentInfo = Addin.AddinActions.DownloadAttachment(page, attachmentName, path);
                    Addin.AddinActions.StartProcess(attachmentInfo.FullName);
                }
            }
            else
            {
                UserNotifier.Exclamation("You need to select an attachment in the wiki explorer.");
            }
        }      

        private void btnShowPages_Click(object sender, RibbonControlEventArgs e)
        {
            TreeNode node = Addin.XWikiTaskPane.treeView.SelectedNode;
            if (node != null)
            {
                Addin.XWikiTaskPane.ShowPages(node);
            }
        }

        private void btnShowAttachments_Click(object sender, RibbonControlEventArgs e)
        {
            XWikiNavigationPane pane = Addin.XWikiTaskPane;
            TreeNode node = pane.treeView.SelectedNode;
            if (node != null)
            {
                pane.ShowAttachments(node);
            }
        }

        private void btnEditPage_Click(object sender, RibbonControlEventArgs e)
        {
            TreeView treeView = Addin.XWikiTaskPane.treeView;
            if (treeView.SelectedNode != null)
            {
                String pageFullName = treeView.SelectedNode.Name;
                Addin.AddinActions.EditPage(pageFullName);
            }
        }

        private void btnViewInBrowser_Click(object sender, RibbonControlEventArgs e)
        {
            Addin.XWikiTaskPane.ViewInBrowser();
        }

        private void dropDownSyntax_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            String selectedValue = dropDownSyntax.SelectedItem.Label;
            if(syntaxes.Contains(selectedValue))
            {
                Addin.AddinStatus.Syntax = selectedValue;
            }
            else
            {
                UserNotifier.StopHand("Invalid syntax selected");
            }
        }

        private void dropDownSaveFormat_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            if (dropDownSaveFormat.SelectedItem.Label == "Filtered HTML")
            {
                Addin.SaveFormat = Word.WdSaveFormat.wdFormatFilteredHTML;
            }
            else if (dropDownSaveFormat.SelectedItem.Label == "HTML")
            {
                Addin.SaveFormat = Word.WdSaveFormat.wdFormatHTML;
            }			
        }

        private void dropDownSaveFormat_ItemsLoading(object sender, RibbonControlEventArgs e)
        {
            dropDownSaveFormat.SelectedItemIndex = 0;
        }

        private void btnAboutXWord_Click(object sender, RibbonControlEventArgs e)
        {
            new AboutXWord().ShowDialog();
        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            Word.ContentControl cc;
            //Addin.ActiveDocumentInstance.eProtectDoc(true);
            //Addin.ActiveDocumentInstance.eProtectBookMark("Item24", false);
            Object range = Addin.ActiveDocumentContentRange;
            cc = Addin.ActiveDocumentContentRange.ContentControls.Add(Word.WdContentControlType.wdContentControlRichText, ref range);
            cc.LockContentControl = true;
            cc.LockContents = true;    
            
        }
    }
}
