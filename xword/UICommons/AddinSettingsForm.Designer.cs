namespace UICommons
{
    partial class AddinSettingsForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddinSettingsForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConnection = new System.Windows.Forms.TabPage();
            this.ckAutoLogin = new System.Windows.Forms.CheckBox();
            this.txtServerURL = new System.Windows.Forms.TextBox();
            this.ckRememberMe = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblServerUrl = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.linkConnectDoc = new System.Windows.Forms.LinkLabel();
            this.comboProtocol = new System.Windows.Forms.ComboBox();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.tabFileRepository = new System.Windows.Forms.TabPage();
            this.grp = new System.Windows.Forms.GroupBox();
            this.btnAttachmentsRepo = new System.Windows.Forms.Button();
            this.txtAttachmentsRepo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnPagesRepo = new System.Windows.Forms.Button();
            this.txtPagesRepo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPrefetch = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkEnablePrefetch = new System.Windows.Forms.CheckBox();
            this.txtPrefetchPagesSetSize = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPrefetchInterval = new System.Windows.Forms.TextBox();
            this.lblPollTimeout = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabConnection.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabFileRepository.SuspendLayout();
            this.grp.SuspendLayout();
            this.tabPrefetch.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabConnection);
            this.tabControl.Controls.Add(this.tabFileRepository);
            this.tabControl.Controls.Add(this.tabPrefetch);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(416, 260);
            this.tabControl.TabIndex = 0;
            // 
            // tabConnection
            // 
            this.tabConnection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabConnection.Controls.Add(this.ckAutoLogin);
            this.tabConnection.Controls.Add(this.txtServerURL);
            this.tabConnection.Controls.Add(this.ckRememberMe);
            this.tabConnection.Controls.Add(this.txtPassword);
            this.tabConnection.Controls.Add(this.lblPassword);
            this.tabConnection.Controls.Add(this.txtUserName);
            this.tabConnection.Controls.Add(this.lblUserName);
            this.tabConnection.Controls.Add(this.lblServerUrl);
            this.tabConnection.Controls.Add(this.groupBox1);
            this.tabConnection.Location = new System.Drawing.Point(4, 22);
            this.tabConnection.Name = "tabConnection";
            this.tabConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tabConnection.Size = new System.Drawing.Size(408, 234);
            this.tabConnection.TabIndex = 0;
            this.tabConnection.Text = "Connection Settings";
            this.tabConnection.UseVisualStyleBackColor = true;
            // 
            // ckAutoLogin
            // 
            this.ckAutoLogin.AutoSize = true;
            this.ckAutoLogin.Checked = true;
            this.ckAutoLogin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckAutoLogin.Location = new System.Drawing.Point(161, 192);
            this.ckAutoLogin.Name = "ckAutoLogin";
            this.ckAutoLogin.Size = new System.Drawing.Size(113, 17);
            this.ckAutoLogin.TabIndex = 21;
            this.ckAutoLogin.Text = "Connect at startup";
            this.ckAutoLogin.UseVisualStyleBackColor = true;
            this.ckAutoLogin.CheckedChanged += new System.EventHandler(this.txtAnyConnectionSetting_TextChanged);
            // 
            // txtServerURL
            // 
            this.txtServerURL.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtServerURL.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.txtServerURL.Location = new System.Drawing.Point(102, 41);
            this.txtServerURL.Name = "txtServerURL";
            this.txtServerURL.Size = new System.Drawing.Size(271, 20);
            this.txtServerURL.TabIndex = 12;
            this.txtServerURL.Text = "http://localhost:8080";
            this.txtServerURL.TextChanged += new System.EventHandler(this.txtAnyConnectionSetting_TextChanged);
            // 
            // ckRememberMe
            // 
            this.ckRememberMe.AutoSize = true;
            this.ckRememberMe.Checked = true;
            this.ckRememberMe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckRememberMe.Location = new System.Drawing.Point(297, 192);
            this.ckRememberMe.Name = "ckRememberMe";
            this.ckRememberMe.Size = new System.Drawing.Size(94, 17);
            this.ckRememberMe.TabIndex = 18;
            this.ckRememberMe.Text = "Remember me";
            this.ckRememberMe.UseVisualStyleBackColor = true;
            this.ckRememberMe.CheckedChanged += new System.EventHandler(this.txtAnyConnectionSetting_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(102, 126);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(271, 20);
            this.txtPassword.TabIndex = 15;
            this.txtPassword.Text = "admin";
            this.txtPassword.TextChanged += new System.EventHandler(this.txtAnyConnectionSetting_TextChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(21, 129);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 17;
            this.lblPassword.Text = "Password:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(102, 84);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(271, 20);
            this.txtUserName.TabIndex = 14;
            this.txtUserName.Text = "Admin";
            this.txtUserName.TextChanged += new System.EventHandler(this.txtAnyConnectionSetting_TextChanged);
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(21, 87);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(63, 13);
            this.lblUserName.TabIndex = 13;
            this.lblUserName.Text = "User Name:";
            // 
            // lblServerUrl
            // 
            this.lblServerUrl.AutoSize = true;
            this.lblServerUrl.Location = new System.Drawing.Point(21, 44);
            this.lblServerUrl.Name = "lblServerUrl";
            this.lblServerUrl.Size = new System.Drawing.Size(56, 13);
            this.lblServerUrl.TabIndex = 11;
            this.lblServerUrl.Text = "Wiki URL:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.linkConnectDoc);
            this.groupBox1.Controls.Add(this.comboProtocol);
            this.groupBox1.Controls.Add(this.lblProtocol);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(383, 169);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // linkConnectDoc
            // 
            this.linkConnectDoc.AutoSize = true;
            this.linkConnectDoc.Location = new System.Drawing.Point(330, 148);
            this.linkConnectDoc.Name = "linkConnectDoc";
            this.linkConnectDoc.Size = new System.Drawing.Size(35, 13);
            this.linkConnectDoc.TabIndex = 25;
            this.linkConnectDoc.TabStop = true;
            this.linkConnectDoc.Text = "Why?";
            this.linkConnectDoc.Visible = false;
            this.linkConnectDoc.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkConnectDoc_LinkClicked);
            // 
            // comboProtocol
            // 
            this.comboProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProtocol.FormattingEnabled = true;
            this.comboProtocol.Location = new System.Drawing.Point(94, 145);
            this.comboProtocol.Name = "comboProtocol";
            this.comboProtocol.Size = new System.Drawing.Size(230, 21);
            this.comboProtocol.TabIndex = 24;
            this.comboProtocol.Visible = false;
            this.comboProtocol.SelectedIndexChanged += new System.EventHandler(this.comboProtocol_SelectedIndexChanged);
            // 
            // lblProtocol
            // 
            this.lblProtocol.AutoSize = true;
            this.lblProtocol.Location = new System.Drawing.Point(13, 148);
            this.lblProtocol.Name = "lblProtocol";
            this.lblProtocol.Size = new System.Drawing.Size(67, 13);
            this.lblProtocol.TabIndex = 23;
            this.lblProtocol.Text = "Connect via:";
            this.lblProtocol.Visible = false;
            // 
            // tabFileRepository
            // 
            this.tabFileRepository.Controls.Add(this.grp);
            this.tabFileRepository.Location = new System.Drawing.Point(4, 22);
            this.tabFileRepository.Name = "tabFileRepository";
            this.tabFileRepository.Padding = new System.Windows.Forms.Padding(3);
            this.tabFileRepository.Size = new System.Drawing.Size(408, 234);
            this.tabFileRepository.TabIndex = 1;
            this.tabFileRepository.Text = "File Repository";
            this.tabFileRepository.UseVisualStyleBackColor = true;
            // 
            // grp
            // 
            this.grp.Controls.Add(this.btnAttachmentsRepo);
            this.grp.Controls.Add(this.txtAttachmentsRepo);
            this.grp.Controls.Add(this.label5);
            this.grp.Controls.Add(this.btnPagesRepo);
            this.grp.Controls.Add(this.txtPagesRepo);
            this.grp.Controls.Add(this.label4);
            this.grp.Location = new System.Drawing.Point(8, 6);
            this.grp.Name = "grp";
            this.grp.Size = new System.Drawing.Size(383, 206);
            this.grp.TabIndex = 21;
            this.grp.TabStop = false;
            this.grp.Text = "Settings";
            // 
            // btnAttachmentsRepo
            // 
            this.btnAttachmentsRepo.Location = new System.Drawing.Point(308, 57);
            this.btnAttachmentsRepo.Name = "btnAttachmentsRepo";
            this.btnAttachmentsRepo.Size = new System.Drawing.Size(54, 23);
            this.btnAttachmentsRepo.TabIndex = 22;
            this.btnAttachmentsRepo.Text = "...";
            this.btnAttachmentsRepo.UseVisualStyleBackColor = true;
            this.btnAttachmentsRepo.Click += new System.EventHandler(this.btnAttachmentsRepo_Click);
            // 
            // txtAttachmentsRepo
            // 
            this.txtAttachmentsRepo.Location = new System.Drawing.Point(81, 59);
            this.txtAttachmentsRepo.Name = "txtAttachmentsRepo";
            this.txtAttachmentsRepo.Size = new System.Drawing.Size(221, 20);
            this.txtAttachmentsRepo.TabIndex = 21;
            this.txtAttachmentsRepo.Text = "C:\\Temp";
            this.txtAttachmentsRepo.TextChanged += new System.EventHandler(this.anyRepoSettingChanged_TextChanged);
            this.txtAttachmentsRepo.Leave += new System.EventHandler(this.txtAttachmentsRepo_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Attachments:";
            // 
            // btnPagesRepo
            // 
            this.btnPagesRepo.Location = new System.Drawing.Point(308, 21);
            this.btnPagesRepo.Name = "btnPagesRepo";
            this.btnPagesRepo.Size = new System.Drawing.Size(54, 23);
            this.btnPagesRepo.TabIndex = 19;
            this.btnPagesRepo.Text = "...";
            this.btnPagesRepo.UseVisualStyleBackColor = true;
            this.btnPagesRepo.Click += new System.EventHandler(this.btnPagesRepo_Click);
            // 
            // txtPagesRepo
            // 
            this.txtPagesRepo.Location = new System.Drawing.Point(81, 23);
            this.txtPagesRepo.Name = "txtPagesRepo";
            this.txtPagesRepo.Size = new System.Drawing.Size(221, 20);
            this.txtPagesRepo.TabIndex = 14;
            this.txtPagesRepo.Text = "C:\\Temp";
            this.txtPagesRepo.TextChanged += new System.EventHandler(this.anyRepoSettingChanged_TextChanged);
            this.txtPagesRepo.Leave += new System.EventHandler(this.txtPagesRepo_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Pages:";
            // 
            // tabPrefetch
            // 
            this.tabPrefetch.Controls.Add(this.groupBox2);
            this.tabPrefetch.Location = new System.Drawing.Point(4, 22);
            this.tabPrefetch.Name = "tabPrefetch";
            this.tabPrefetch.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrefetch.Size = new System.Drawing.Size(408, 234);
            this.tabPrefetch.TabIndex = 2;
            this.tabPrefetch.Text = "Prefetch & Update Wiki";
            this.tabPrefetch.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkEnablePrefetch);
            this.groupBox2.Controls.Add(this.txtPrefetchPagesSetSize);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtPrefetchInterval);
            this.groupBox2.Controls.Add(this.lblPollTimeout);
            this.groupBox2.Location = new System.Drawing.Point(8, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(383, 184);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            // 
            // checkEnablePrefetch
            // 
            this.checkEnablePrefetch.AutoSize = true;
            this.checkEnablePrefetch.Location = new System.Drawing.Point(18, 19);
            this.checkEnablePrefetch.Name = "checkEnablePrefetch";
            this.checkEnablePrefetch.Size = new System.Drawing.Size(166, 17);
            this.checkEnablePrefetch.TabIndex = 27;
            this.checkEnablePrefetch.Text = "Enable wiki structure prefetch";
            this.checkEnablePrefetch.UseVisualStyleBackColor = true;
            this.checkEnablePrefetch.CheckedChanged += new System.EventHandler(this.PrefechSettingChanged);
            // 
            // txtPrefetchPagesSetSize
            // 
            this.txtPrefetchPagesSetSize.Location = new System.Drawing.Point(265, 104);
            this.txtPrefetchPagesSetSize.Name = "txtPrefetchPagesSetSize";
            this.txtPrefetchPagesSetSize.Size = new System.Drawing.Size(62, 20);
            this.txtPrefetchPagesSetSize.TabIndex = 25;
            this.txtPrefetchPagesSetSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPrefetchPagesSetSize.TextChanged += new System.EventHandler(this.PrefechSettingChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Number of pages retrieve per request:";
            // 
            // txtPrefetchInterval
            // 
            this.txtPrefetchInterval.Location = new System.Drawing.Point(265, 59);
            this.txtPrefetchInterval.Name = "txtPrefetchInterval";
            this.txtPrefetchInterval.Size = new System.Drawing.Size(62, 20);
            this.txtPrefetchInterval.TabIndex = 23;
            this.txtPrefetchInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPrefetchInterval.TextChanged += new System.EventHandler(this.PrefechSettingChanged);
            // 
            // lblPollTimeout
            // 
            this.lblPollTimeout.AutoSize = true;
            this.lblPollTimeout.Location = new System.Drawing.Point(15, 62);
            this.lblPollTimeout.Name = "lblPollTimeout";
            this.lblPollTimeout.Size = new System.Drawing.Size(137, 13);
            this.lblPollTimeout.TabIndex = 22;
            this.lblPollTimeout.Text = "Get data interval (seconds):";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(248, 266);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 16;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(167, 266);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 17;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(329, 266);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddinSettingsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(416, 298);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddinSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XWord - Settings";
            this.Load += new System.EventHandler(this.ConnectionSettingsForm_Load);
            this.Shown += new System.EventHandler(this.AddinSettingsForm_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AddinSettingsForm_FormClosed);
            this.tabControl.ResumeLayout(false);
            this.tabConnection.ResumeLayout(false);
            this.tabConnection.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabFileRepository.ResumeLayout(false);
            this.grp.ResumeLayout(false);
            this.grp.PerformLayout();
            this.tabPrefetch.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabConnection;
        private System.Windows.Forms.TextBox txtServerURL;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.CheckBox ckRememberMe;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblServerUrl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabFileRepository;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grp;
        private System.Windows.Forms.Button btnPagesRepo;
        private System.Windows.Forms.TextBox txtPagesRepo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAttachmentsRepo;
        private System.Windows.Forms.TextBox txtAttachmentsRepo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkConnectDoc;
        private System.Windows.Forms.ComboBox comboProtocol;
        private System.Windows.Forms.Label lblProtocol;
        private System.Windows.Forms.TabPage tabPrefetch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblPollTimeout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkEnablePrefetch;
        public System.Windows.Forms.TextBox txtPrefetchInterval;
        public System.Windows.Forms.TextBox txtPrefetchPagesSetSize;
        private System.Windows.Forms.CheckBox ckAutoLogin;

    }
}
