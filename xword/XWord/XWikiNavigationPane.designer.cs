namespace XWord
{
    /// <summary>
    /// A task pane used to browse the wiki structure.
    /// </summary>
    partial class XWikiNavigationPane
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XWikiNavigationPane));
            this.treeMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshWikiExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuNewPage = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuEditPage = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuViewInBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuAttachFile = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuDownloadFile = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuDownloadOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.timerUI = new System.Windows.Forms.Timer(this.components);
            this.iconList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAddSpace = new System.Windows.Forms.Button();
            this.treeView = new System.Windows.Forms.TreeView();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.treeMenuStrip.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // treeMenuStrip
            // 
            this.treeMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshWikiExplorerToolStripMenuItem,
            this.cmnuNewPage,
            this.cmnuEditPage,
            this.cmnuViewInBrowser,
            this.cmnuAttachFile,
            this.cmnuDownloadFile,
            this.cmnuDownloadOpenFile});
            this.treeMenuStrip.Name = "contextMenuStrip1";
            this.treeMenuStrip.Size = new System.Drawing.Size(274, 158);
            this.treeMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.treeMenuStrip_Opening);
            // 
            // refreshWikiExplorerToolStripMenuItem
            // 
            this.refreshWikiExplorerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("refreshWikiExplorerToolStripMenuItem.Image")));
            this.refreshWikiExplorerToolStripMenuItem.Name = "refreshWikiExplorerToolStripMenuItem";
            this.refreshWikiExplorerToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.refreshWikiExplorerToolStripMenuItem.Text = "Refresh Wiki Explorer";
            this.refreshWikiExplorerToolStripMenuItem.Click += new System.EventHandler(this.refreshWikiExplorerToolStripMenuItem_Click);
            // 
            // cmnuNewPage
            // 
            this.cmnuNewPage.Image = global::XWord.Properties.Resources.Crystal_Clear_action_edit_add;
            this.cmnuNewPage.Name = "cmnuNewPage";
            this.cmnuNewPage.Size = new System.Drawing.Size(273, 22);
            this.cmnuNewPage.Text = "Create new page";
            this.cmnuNewPage.Click += new System.EventHandler(this.cmnuNewPage_Click);
            // 
            // cmnuEditPage
            // 
            this.cmnuEditPage.Image = global::XWord.Properties.Resources.Crystal_Clear_action_edit;
            this.cmnuEditPage.Name = "cmnuEditPage";
            this.cmnuEditPage.Size = new System.Drawing.Size(273, 22);
            this.cmnuEditPage.Text = "Edit this page";
            this.cmnuEditPage.Click += new System.EventHandler(this.cmnuEditPage_Click);
            // 
            // cmnuViewInBrowser
            // 
            this.cmnuViewInBrowser.Image = global::XWord.Properties.Resources.browser;
            this.cmnuViewInBrowser.Name = "cmnuViewInBrowser";
            this.cmnuViewInBrowser.Size = new System.Drawing.Size(273, 22);
            this.cmnuViewInBrowser.Text = "View in browser";
            this.cmnuViewInBrowser.Click += new System.EventHandler(this.cmnuViewInBrowser_Click);
            // 
            // cmnuAttachFile
            // 
            this.cmnuAttachFile.Image = global::XWord.Properties.Resources.Nuvola_apps_download_manager2;
            this.cmnuAttachFile.Name = "cmnuAttachFile";
            this.cmnuAttachFile.Size = new System.Drawing.Size(273, 22);
            this.cmnuAttachFile.Text = "Attach current document to this page";
            this.cmnuAttachFile.Click += new System.EventHandler(this.cmnuAttachFile_Click);
            // 
            // cmnuDownloadFile
            // 
            this.cmnuDownloadFile.Image = global::XWord.Properties.Resources.Nuvola_apps_download_manager1;
            this.cmnuDownloadFile.Name = "cmnuDownloadFile";
            this.cmnuDownloadFile.Size = new System.Drawing.Size(273, 22);
            this.cmnuDownloadFile.Text = "Download file";
            this.cmnuDownloadFile.Click += new System.EventHandler(this.cmnuDownloadOpenFile_Click);
            // 
            // cmnuDownloadOpenFile
            // 
            this.cmnuDownloadOpenFile.Image = global::XWord.Properties.Resources.Crystal_Clear_download_and_open;
            this.cmnuDownloadOpenFile.Name = "cmnuDownloadOpenFile";
            this.cmnuDownloadOpenFile.Size = new System.Drawing.Size(273, 22);
            this.cmnuDownloadOpenFile.Text = "Download and open file";
            this.cmnuDownloadOpenFile.Click += new System.EventHandler(this.cmnuDownloadOpenFile_Click);
            // 
            // timerUI
            // 
            this.timerUI.Tick += new System.EventHandler(this.timerUI_Tick);
            // 
            // iconList
            // 
            this.iconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconList.ImageStream")));
            this.iconList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconList.Images.SetKeyName(0, "Crystal_Clear_filesystem_folder_blue.png");
            this.iconList.Images.SetKeyName(1, "Crystal_Clear_mimetype_document.png");
            this.iconList.Images.SetKeyName(2, "attach.gif");
            this.iconList.Images.SetKeyName(3, "Crystal_Clear_action_loopnone.png");
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.btnRefresh, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.btnAddSpace, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.treeView, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.pictureBox, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(214, 561);
            this.tableLayoutPanel.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(110, 6);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(101, 29);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnAddSpace
            // 
            this.btnAddSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddSpace.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSpace.Location = new System.Drawing.Point(3, 6);
            this.btnAddSpace.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.btnAddSpace.Name = "btnAddSpace";
            this.btnAddSpace.Size = new System.Drawing.Size(101, 29);
            this.btnAddSpace.TabIndex = 2;
            this.btnAddSpace.Text = "Add Space";
            this.btnAddSpace.UseVisualStyleBackColor = true;
            this.btnAddSpace.Click += new System.EventHandler(this.btnAddSpace_Click);
            // 
            // treeView
            // 
            this.tableLayoutPanel.SetColumnSpan(this.treeView, 2);
            this.treeView.ContextMenuStrip = this.treeMenuStrip;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView.HideSelection = false;
            this.treeView.HotTracking = true;
            this.treeView.Location = new System.Drawing.Point(5, 40);
            this.treeView.Margin = new System.Windows.Forms.Padding(5);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(204, 496);
            this.treeView.TabIndex = 3;
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            this.treeView.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView.Leave += new System.EventHandler(this.treeView1_Leave);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(3, 544);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(100, 14);
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            this.pictureBox.Visible = false;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            // 
            // XWikiNavigationPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.tableLayoutPanel);
            this.DoubleBuffered = true;
            this.Name = "XWikiNavigationPane";
            this.Size = new System.Drawing.Size(214, 561);
            this.Tag = "WIKI_EXPLORER";
            this.Load += new System.EventHandler(this.XWikiNavigationPane_Load);
            this.Leave += new System.EventHandler(this.XWikiNavigationPane_Leave);
            this.Resize += new System.EventHandler(this.XWikiNavigationPane_Resize);
            this.treeMenuStrip.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip treeMenuStrip;
        private System.Windows.Forms.Timer timerUI;
        private System.Windows.Forms.ToolStripMenuItem cmnuAttachFile;
        private System.Windows.Forms.ToolStripMenuItem cmnuDownloadFile;
        private System.Windows.Forms.ToolStripMenuItem cmnuDownloadOpenFile;
        private System.Windows.Forms.ImageList iconList;
        private System.Windows.Forms.ToolStripMenuItem cmnuNewPage;
        private System.Windows.Forms.ToolStripMenuItem cmnuEditPage;
        private System.Windows.Forms.ToolStripMenuItem cmnuViewInBrowser;
        private System.Windows.Forms.ToolStripMenuItem refreshWikiExplorerToolStripMenuItem;
        private System.Windows.Forms.Button btnAddSpace;
        /// <summary>
        /// The tree view that displays the wiki structure.
        /// </summary>
        public System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button btnRefresh;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.PictureBox pictureBox;
    }
}