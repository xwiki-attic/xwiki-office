namespace XWord
{
    partial class XWikiRibbon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Office.Tools.Ribbon.RibbonDialogLauncher ribbonDialogLauncher1 = new Microsoft.Office.Tools.Ribbon.RibbonDialogLauncher();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItem1 = new Microsoft.Office.Tools.Ribbon.RibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItem2 = new Microsoft.Office.Tools.Ribbon.RibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItem3 = new Microsoft.Office.Tools.Ribbon.RibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItem4 = new Microsoft.Office.Tools.Ribbon.RibbonDropDownItem();
            this.tab1 = new Microsoft.Office.Tools.Ribbon.RibbonTab();
            this.XWikiTab = new Microsoft.Office.Tools.Ribbon.RibbonTab();
            this.XEGroup = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.btnNewPage = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnPublishDocument = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.toggleButton1 = new Microsoft.Office.Tools.Ribbon.RibbonToggleButton();
            this.AttachmentsGroup = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.uploadAttToPage = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.downloadAtt = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.selectionOptionsGroup = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.btnAddPage = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnEditPage = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnUpload = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnDownload = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnDownloadAndOpen = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnShowPages = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnShowAttachments = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.btnViewInBrowser = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.OptionsGroup = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.btnXWordOptions = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.syncSaving = new Microsoft.Office.Tools.Ribbon.RibbonCheckBox();
            this.dropDownSyntax = new Microsoft.Office.Tools.Ribbon.RibbonDropDown();
            this.dropDownSaveFormat = new Microsoft.Office.Tools.Ribbon.RibbonDropDown();
            this.separator1 = new Microsoft.Office.Tools.Ribbon.RibbonSeparator();
            this.btnAboutXWord = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.tab1.SuspendLayout();
            this.XWikiTab.SuspendLayout();
            this.XEGroup.SuspendLayout();
            this.AttachmentsGroup.SuspendLayout();
            this.selectionOptionsGroup.SuspendLayout();
            this.OptionsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            this.tab1.Visible = false;
            // 
            // XWikiTab
            // 
            this.XWikiTab.Groups.Add(this.XEGroup);
            this.XWikiTab.Groups.Add(this.AttachmentsGroup);
            this.XWikiTab.Groups.Add(this.selectionOptionsGroup);
            this.XWikiTab.Groups.Add(this.OptionsGroup);
            this.XWikiTab.Label = "XWord";
            this.XWikiTab.Name = "XWikiTab";
            // 
            // XEGroup
            // 
            this.XEGroup.Items.Add(this.btnNewPage);
            this.XEGroup.Items.Add(this.btnPublishDocument);
            this.XEGroup.Items.Add(this.toggleButton1);
            this.XEGroup.Label = "XWiki Pages";
            this.XEGroup.Name = "XEGroup";
            // 
            // btnNewPage
            // 
            this.btnNewPage.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnNewPage.Description = "Add a new page to the wiki.";
            this.btnNewPage.Image = global::XWord.Properties.Resources.Crystal_Clear_action_edit_add;
            this.btnNewPage.KeyTip = "A";
            this.btnNewPage.Label = "Add page";
            this.btnNewPage.Name = "btnNewPage";
            this.btnNewPage.ScreenTip = "Add a new page to the wiki.";
            this.btnNewPage.ShowImage = true;
            this.btnNewPage.SuperTip = "You will be prompted to fill in a dialog box with the wiki space, the page name a" +
                "nd title. After that Word will open a new document that will be saved as your ne" +
                "w page.";
            this.btnNewPage.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnNewPage_Click);
            // 
            // btnPublishDocument
            // 
            this.btnPublishDocument.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnPublishDocument.Image = global::XWord.Properties.Resources.Crystal_Clear_action_apply;
            this.btnPublishDocument.Label = "Publish document";
            this.btnPublishDocument.Name = "btnPublishDocument";
            this.btnPublishDocument.ScreenTip = "Publish Page to XWiki";
            this.btnPublishDocument.ShowImage = true;
            this.btnPublishDocument.SuperTip = "Saves the current document as a wiki page on the XWiki server.";
            this.btnPublishDocument.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnSavePage_Click);
            // 
            // toggleButton1
            // 
            this.toggleButton1.Checked = true;
            this.toggleButton1.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.toggleButton1.Image = global::XWord.Properties.Resources.Crystal_Clear_filesystem_folder_green;
            this.toggleButton1.Label = "Wiki explorer";
            this.toggleButton1.Name = "toggleButton1";
            this.toggleButton1.ScreenTip = "Toggle Wiki Explorer";
            this.toggleButton1.ShowImage = true;
            this.toggleButton1.SuperTip = "Use this button to show or to hide the wiki explorer.";
            this.toggleButton1.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.toggleButton1_Click);
            // 
            // AttachmentsGroup
            // 
            this.AttachmentsGroup.Items.Add(this.uploadAttToPage);
            this.AttachmentsGroup.Items.Add(this.downloadAtt);
            this.AttachmentsGroup.Label = "XWiki Attachments";
            this.AttachmentsGroup.Name = "AttachmentsGroup";
            // 
            // uploadAttToPage
            // 
            this.uploadAttToPage.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.uploadAttToPage.Description = "Uploads the current document to the wiki.";
            this.uploadAttToPage.Enabled = false;
            this.uploadAttToPage.Image = global::XWord.Properties.Resources.Nuvola_apps_download_manager2;
            this.uploadAttToPage.Label = "Attach current document";
            this.uploadAttToPage.Name = "uploadAttToPage";
            this.uploadAttToPage.ScreenTip = "Uploads the active document to the wiki.";
            this.uploadAttToPage.ShowImage = true;
            this.uploadAttToPage.SuperTip = "The file will be attached to the page that is selected in the wiki explorer.";
            this.uploadAttToPage.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.uploadAttToPage_Click);
            // 
            // downloadAtt
            // 
            this.downloadAtt.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.downloadAtt.Enabled = false;
            this.downloadAtt.Image = global::XWord.Properties.Resources.Nuvola_apps_download_manager;
            this.downloadAtt.Label = "Download selected file";
            this.downloadAtt.Name = "downloadAtt";
            this.downloadAtt.ScreenTip = "Download the selected attachment.";
            this.downloadAtt.ShowImage = true;
            this.downloadAtt.SuperTip = "Downloads the selected attachment to the local file system. To select an attchmen" +
                "t you must navigate the wiki using the Wiki explorer.";
            this.downloadAtt.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.downloadAtt_Click);
            // 
            // selectionOptionsGroup
            // 
            this.selectionOptionsGroup.Items.Add(this.btnAddPage);
            this.selectionOptionsGroup.Items.Add(this.btnEditPage);
            this.selectionOptionsGroup.Items.Add(this.btnUpload);
            this.selectionOptionsGroup.Items.Add(this.btnDownload);
            this.selectionOptionsGroup.Items.Add(this.btnDownloadAndOpen);
            this.selectionOptionsGroup.Items.Add(this.btnShowPages);
            this.selectionOptionsGroup.Items.Add(this.btnShowAttachments);
            this.selectionOptionsGroup.Items.Add(this.btnViewInBrowser);
            this.selectionOptionsGroup.Label = "Wiki Explorer Selection";
            this.selectionOptionsGroup.Name = "selectionOptionsGroup";
            // 
            // btnAddPage
            // 
            this.btnAddPage.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnAddPage.Image = global::XWord.Properties.Resources.Crystal_Clear_action_edit_add;
            this.btnAddPage.Label = "Add page";
            this.btnAddPage.Name = "btnAddPage";
            this.btnAddPage.ScreenTip = "Add a page to the selected space.";
            this.btnAddPage.ShowImage = true;
            this.btnAddPage.SuperTip = "You will be prompted to fill in a dialog box with the wiki space, the page name a" +
                "nd title. After that Word will open a new document that will be saved as your ne" +
                "w page.";
            this.btnAddPage.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnAddPage_Click);
            // 
            // btnEditPage
            // 
            this.btnEditPage.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnEditPage.Image = global::XWord.Properties.Resources.Crystal_Clear_action_edit;
            this.btnEditPage.Label = "Edit page";
            this.btnEditPage.Name = "btnEditPage";
            this.btnEditPage.ScreenTip = "Edit the selected page.";
            this.btnEditPage.ShowImage = true;
            this.btnEditPage.SuperTip = "You will open the selected page in edit mode. A local html file will be used in W" +
                "ord and then it will be adapted and saved to the XWiki server.";
            this.btnEditPage.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnEditPage_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnUpload.Image = global::XWord.Properties.Resources.Nuvola_apps_download_manager2;
            this.btnUpload.Label = "Upload";
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.ScreenTip = "Upload the current document to the selected wiki page.";
            this.btnUpload.ShowImage = true;
            this.btnUpload.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.uploadAttToPage_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnDownload.Image = global::XWord.Properties.Resources.Nuvola_apps_download_manager;
            this.btnDownload.Label = "Download";
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.ScreenTip = "Download the selected attachment.";
            this.btnDownload.ShowImage = true;
            this.btnDownload.SuperTip = "Downloads the selected attachment to the local file system. To select an attchmen" +
                "t you must navigate the wiki using the Wiki explorer.";
            this.btnDownload.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.downloadAtt_Click);
            // 
            // btnDownloadAndOpen
            // 
            this.btnDownloadAndOpen.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnDownloadAndOpen.Image = global::XWord.Properties.Resources.Crystal_Clear_download_and_open;
            this.btnDownloadAndOpen.Label = "Download and open";
            this.btnDownloadAndOpen.Name = "btnDownloadAndOpen";
            this.btnDownloadAndOpen.ScreenTip = "Downloads and opens the selected attachment.";
            this.btnDownloadAndOpen.ShowImage = true;
            this.btnDownloadAndOpen.SuperTip = "You will be prompted to select a save location for the attachment. Then the file " +
                "will be opened with the default application that is assigned for its type.";
            this.btnDownloadAndOpen.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnDownloadAndOpen_Click);
            // 
            // btnShowPages
            // 
            this.btnShowPages.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnShowPages.Image = global::XWord.Properties.Resources.Crystal_Clear_action_view_tree96;
            this.btnShowPages.Label = "Show pages";
            this.btnShowPages.Name = "btnShowPages";
            this.btnShowPages.ScreenTip = "Shows the pages for the selectes space.";
            this.btnShowPages.ShowImage = true;
            this.btnShowPages.SuperTip = "The selected space node will be expanded in order to see the pages.";
            this.btnShowPages.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnShowPages_Click);
            // 
            // btnShowAttachments
            // 
            this.btnShowAttachments.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnShowAttachments.Image = global::XWord.Properties.Resources.Crystal_Clear_action_view_tree96;
            this.btnShowAttachments.Label = "Show attachments";
            this.btnShowAttachments.Name = "btnShowAttachments";
            this.btnShowAttachments.ScreenTip = "Shows the attachments for the selected page.";
            this.btnShowAttachments.ShowImage = true;
            this.btnShowAttachments.SuperTip = "The selected space node will be expanded in order to see the attachments.";
            this.btnShowAttachments.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnShowAttachments_Click);
            // 
            // btnViewInBrowser
            // 
            this.btnViewInBrowser.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnViewInBrowser.Image = global::XWord.Properties.Resources.browser;
            this.btnViewInBrowser.Label = "View in browser";
            this.btnViewInBrowser.Name = "btnViewInBrowser";
            this.btnViewInBrowser.ScreenTip = "View the selected page in browser.";
            this.btnViewInBrowser.ShowImage = true;
            this.btnViewInBrowser.SuperTip = "THe selected page will be opened in the system\'s default browser.";
            this.btnViewInBrowser.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnViewInBrowser_Click);
            // 
            // OptionsGroup
            // 
            this.OptionsGroup.DialogLauncher = ribbonDialogLauncher1;
            this.OptionsGroup.Items.Add(this.btnXWordOptions);
            this.OptionsGroup.Items.Add(this.syncSaving);
            this.OptionsGroup.Items.Add(this.dropDownSyntax);
            this.OptionsGroup.Items.Add(this.dropDownSaveFormat);
            this.OptionsGroup.Items.Add(this.separator1);
            this.OptionsGroup.Items.Add(this.btnAboutXWord);
            this.OptionsGroup.Label = "XWord";
            this.OptionsGroup.Name = "OptionsGroup";
            // 
            // btnXWordOptions
            // 
            this.btnXWordOptions.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnXWordOptions.Image = global::XWord.Properties.Resources.Crystal_Clear_app_package_settings;
            this.btnXWordOptions.Label = "XWord Options";
            this.btnXWordOptions.Name = "btnXWordOptions";
            this.btnXWordOptions.ScreenTip = "Shows the XWord Options dialog.";
            this.btnXWordOptions.ShowImage = true;
            this.btnXWordOptions.SuperTip = "Here you can setup a connection to a XWiki server or select the locations for you" +
                "r local wiki pages.";
            this.btnXWordOptions.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.Show_ConnetionDialog);
            // 
            // syncSaving
            // 
            this.syncSaving.Enabled = false;
            this.syncSaving.Label = "Synchronize saving with Word";
            this.syncSaving.Name = "syncSaving";
            this.syncSaving.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.SyncSaving_Click);
            // 
            // dropDownSyntax
            // 
            ribbonDropDownItem1.Label = "XWiki 2.0";
            ribbonDropDownItem2.Label = "XHTML";
            this.dropDownSyntax.Items.Add(ribbonDropDownItem1);
            this.dropDownSyntax.Items.Add(ribbonDropDownItem2);
            this.dropDownSyntax.Label = "Server syntax:       ";
            this.dropDownSyntax.Name = "dropDownSyntax";
            this.dropDownSyntax.ScreenTip = "The syntax used to save the page on the XWiki server.";
            this.dropDownSyntax.SelectionChanged += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.dropDownSyntax_SelectionChanged);
            // 
            // dropDownSaveFormat
            // 
            ribbonDropDownItem3.Label = "HTML";
            ribbonDropDownItem4.Label = "Filtered HTML";
            this.dropDownSaveFormat.Items.Add(ribbonDropDownItem3);
            this.dropDownSaveFormat.Items.Add(ribbonDropDownItem4);
            this.dropDownSaveFormat.Label = "Local save format:";
            this.dropDownSaveFormat.Name = "dropDownSaveFormat";
            this.dropDownSaveFormat.ScreenTip = "The format used to save your local wiki pages.";
            this.dropDownSaveFormat.ItemsLoading += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.dropDownSaveFormat_ItemsLoading);
            this.dropDownSaveFormat.SelectionChanged += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.dropDownSaveFormat_SelectionChanged);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // btnAboutXWord
            // 
            this.btnAboutXWord.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnAboutXWord.Image = global::XWord.Properties.Resources.XWiki;
            this.btnAboutXWord.Label = "About XWord";
            this.btnAboutXWord.Name = "btnAboutXWord";
            this.btnAboutXWord.ScreenTip = "About XWord";
            this.btnAboutXWord.ShowImage = true;
            this.btnAboutXWord.SuperTip = "Displays a summay about this Word Extension.";
            this.btnAboutXWord.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.btnAboutXWord_Click);
            // 
            // XWikiRibbon
            // 
            this.Name = "XWikiRibbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab1);
            this.Tabs.Add(this.XWikiTab);
            this.Load += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonUIEventArgs>(this.XWikiRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.XWikiTab.ResumeLayout(false);
            this.XWikiTab.PerformLayout();
            this.XEGroup.ResumeLayout(false);
            this.XEGroup.PerformLayout();
            this.AttachmentsGroup.ResumeLayout(false);
            this.AttachmentsGroup.PerformLayout();
            this.selectionOptionsGroup.ResumeLayout(false);
            this.selectionOptionsGroup.PerformLayout();
            this.OptionsGroup.ResumeLayout(false);
            this.OptionsGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        private Microsoft.Office.Tools.Ribbon.RibbonTab XWikiTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup XEGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup OptionsGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox syncSaving;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnXWordOptions;
        /// <summary>
        /// Group containing the buttons for handling attachments.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonGroup AttachmentsGroup;
        /// <summary>
        /// Button used to upload the active document to the selected page.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonButton uploadAttToPage;
        /// <summary>
        /// Button used to upload the download the selected attachment.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonButton downloadAtt;
        /// <summary>
        /// Button used to create a new page.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonButton btnNewPage;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButton1;
        /// <summary>
        /// Button used to save/publish the active document to the wiki.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonButton btnPublishDocument;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup selectionOptionsGroup;
        /// <summary>
        /// Button used to add the a new page to the wiki.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonButton btnAddPage;
        /// <summary>
        /// Button used to upload the active document to the seleted page.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonButton btnUpload;
        /// <summary>
        /// Button used to donwload the selected attachment.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonButton btnDownload;
        /// <summary>
        /// Button used to show the pages of a space in Wiki Explorer.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonButton btnShowPages;
        /// <summary>
        /// Button used to show the attachments of a page in Wiki Exploerer.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonButton btnShowAttachments;
        /// <summary>
        /// Button used to view the selected page in the default browser.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonButton btnViewInBrowser;
        /// <summary>
        /// Downloads and opens the selected attachement with the assigned application.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonButton btnDownloadAndOpen;
        /// <summary>
        /// Edit the selected page with Word.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonButton btnEditPage;
        /// <summary>
        /// Contains the syntax options to save a page on the wiki.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDownSyntax;
        /// <summary>
        /// Contains a list with the html formats that Word can use to save the local wiki pages.
        /// </summary>
        public Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDownSaveFormat;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAboutXWord;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
    }

    partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonReadOnlyCollection
    {
        internal XWikiRibbon XWikiRibbon
        {
            get { return this.GetRibbon<XWikiRibbon>(); }
        }
    }
}
