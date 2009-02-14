using System;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Tools.Ribbon;
using Word = Microsoft.Office.Interop.Word;

namespace XWriter
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
            DialogResult result = new AddinSettingsForm().ShowDialog();
            if (result == DialogResult.OK)
            {                
                //AddTaskPanes(); TODO: Sync all taskpanes
            }
        }

        private void uploadAttToPage_Click(object sender, RibbonControlEventArgs e)
        {
            String page = Globals.XWikiAddIn.AddinStatus.TaskPaneSelectedPage.Get("page");
            bool finished = Globals.XWikiAddIn.AddinActions.AttachCurrentFile(page, null);
            if (finished)
            {
                MessageBox.Show("Upload finished.", "XWord");
            }
            else
            {
                MessageBox.Show("Upload failed. Make sure you have selected a page before uploading");
            }
        }

        private void btnNewPage_Click(object sender, RibbonControlEventArgs e)
        {
            new AddPageForm(ref Globals.XWikiAddIn.wiki).Show();
        }

        private void btnSavePage_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                if (Addin.currentPageFullName == "" || Addin.currentPageFullName == null)
                {
                    new AddPageForm(ref Addin.wiki, false, true).Show();
                }
                else
                {
                    Addin.AddinActions.SaveToServer();
                }
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("You are not currently editing a wiki page", "XWord", MessageBoxButtons.OK ,MessageBoxIcon.Exclamation);
            }
        }

        private void btnAddPage_Click(object sender, RibbonControlEventArgs e)
        {
            TreeView treeView = Globals.XWikiAddIn.XWikiTaskPane.treeView;
            if (treeView.SelectedNode != null)
            {
                String spaceName = treeView.SelectedNode.Text;
                new AddPageForm(ref Globals.XWikiAddIn.wiki, spaceName).Show();
            }
            else
            {
                //see XOFFICE-20
                //MessageBox.Show("You need to select a space in the wiki explorer.","XWord");
                new AddPageForm(ref Globals.XWikiAddIn.wiki, true, false).Show();
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
                    MessageBox.Show("Download finised.", "XWord");
                }
            }
            else
            {
                MessageBox.Show("You need to select an attachment in the wiki explorer.", "XWord");
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
                MessageBox.Show("You need to select an attachment in the wiki explorer.", "XWord");
            }
        }      

        private void btnShowPages_Click(object sender, RibbonControlEventArgs e)
        {
            TreeNode node = Addin.XWikiTaskPane.treeView.SelectedNode;
            if (node != null)
            {
                node.ExpandAll();
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
            Addin.AddinStatus.Syntax = dropDownSyntax.SelectedItem.Label;
        }

        private void dropDownSyntax_ItemsLoading(object sender, RibbonControlEventArgs e)
        {
            Addin.AddinStatus.Syntax = dropDownSyntax.Items[0].Label;
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

        private void helpGallery_ItemsLoading(object sender, RibbonControlEventArgs e)
        {
            
        }

        private void btnAboutXWord_Click(object sender, RibbonControlEventArgs e)
        {
            new AboutXWord().ShowDialog();
        }

        private void button20_Click(object sender, RibbonControlEventArgs e)
        {
            
        }
    }
}
