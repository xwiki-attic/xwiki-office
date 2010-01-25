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
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using XWiki.Logging;
using XWiki;

namespace UICommons
{
    /// <summary>
    /// Provides UI for changing the add-in settings.
    /// </summary>
    public partial class AddinSettingsForm : Form
    {
        /// <summary>
        /// Specifies if at least an instance of the form is visible.
        /// </summary>
        public static bool IsShown = false;
        bool connectionSettingsApplied = true;
        bool addinSettingsApplied = true;
        bool loadingDialogFlag = false;
        
        const String connectionDocUrl = "http://xoffice.xwiki.org/xwiki/bin/view/XWord/User_Guide#HConnecttoaXWikiserver";

        #region connectivity
        StringCollection connectMethods = Properties.Settings.Default.ConnectMethods;
        Dictionary<String, XWikiClientType> connectDictionary = new Dictionary<string, XWikiClientType>();
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of connectionSettingsApplied, indicating if
        /// the connections settings are applied.
        /// </summary>
        public bool ConnectionSettingsApplied
        {
            get { return connectionSettingsApplied; }
            set { connectionSettingsApplied = value; }
        }

        /// <summary>
        /// Gets or sets the value of addinSettingsApplied, indicating if
        /// the addin settings (XOffice common settings) are aplied.
        /// </summary>
        public bool AddinSettingsApplied
        {
            get { return addinSettingsApplied; }
            set { addinSettingsApplied = value; }
        }

        /// <summary>
        /// Gets or sets the value of loadingDialogFlag.
        /// </summary>
        public bool LoadingDialogFlag
        {
            get { return loadingDialogFlag; }
            set { loadingDialogFlag = value; }
        }

        /// <summary>
        /// Gets the connect methods.
        /// </summary>
        public StringCollection ConnectMethods
        {
            get { return connectMethods; }
        }
        /// <summary>
        /// Gets the connectDictionary.
        /// </summary>
        public Dictionary<String, XWikiClientType> ConnectDictionary
        {
            get { return connectDictionary; }
        }

        /// <summary>
        /// Gets or sets the URL of the server that the add-in conncets to.
        /// </summary>
        public String ServerURL
        {
            get
            {
                return txtServerURL.Text;
            }
            set
            {
                txtServerURL.Text = value;
            }
        }

        /// <summary>
        /// Gets the user name that was provided.
        /// </summary>
        public String UserName
        {
            get
            {
                return txtUserName.Text;
            }
            set
            {
                txtUserName.Text = value;
            }
        }

        /// <summary>
        /// Gets the password that was provided.
        /// </summary>
        public String Password
        {
            get
            {
                return txtPassword.Text;
            }
            set
            {
                txtPassword.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the datasource of the comboProtocol combobox.
        /// </summary>
        public object ComboProtocolDataSource
        {
            get { return comboProtocol.DataSource; }
            set { comboProtocol.DataSource = value; }
        }

        /// <summary>
        /// Gets or sets the selected index of the comboProtocol combobox.
        /// </summary>
        public int ComboProtocolSelectedIndex
        {
            get { return comboProtocol.SelectedIndex; }
            set { comboProtocol.SelectedIndex = value; }
        }

        /// <summary>
        /// Gets the selected value of the comboProtocol combobox.
        /// </summary>
        public object ComboProtocolSelectedValue
        {
            get { return comboProtocol.SelectedValue; }
        }

        /// <summary>
        /// Gets the checked value of the ckRememberMe checkbox.
        /// </summary>
        public bool IsCkRememberMeChecked
        {
            get { return ckRememberMe.Checked; }
        }

        /// <summary>
        /// Returns TRUE if connection tab is selected.
        /// </summary>
        public bool IsTabConnectionSelected
        {
            get { return tabControl.SelectedTab == tabConnection; }
        }

        /// <summary>
        /// Return TRUE if repository tab is selected.
        /// </summary>
        public bool IsTabFileRepositorySelected
        {
            get { return tabControl.SelectedTab == tabFileRepository; }
        }

        /// <summary>
        /// Gets the text value from the txtPagesRepo textbox.
        /// </summary>
        public string TxtPagesRepoText
        {
            get { return txtPagesRepo.Text; }
            set { txtPagesRepo.Text = value; }
        }

        /// <summary>
        /// Gets the text value of the txtAttachmentsRepo textbox.
        /// </summary>
        public string TxtAttachmentsRepoText
        {
            get { return txtAttachmentsRepo.Text; }
            set { txtAttachmentsRepo.Text = value; }
        }

        /// <summary>
        /// Gets or sets the text value of groupBox1.
        /// </summary>
        public string GroupBox1Text
        {
            get { return groupBox1.Text; }
            set { groupBox1.Text = value; }
        }

        #endregion Properties

        #region Public Event Handlers
        
        /// <summary>
        /// Actions to perform when on form load.
        /// </summary>
        public EventHandler OnFormLoad;

        /// <summary>
        /// Actions to perform when applying settings.
        /// </summary>
        public EventHandler OnApply;

        /// <summary>
        /// Actions to perform when OK button was pressed.
        /// </summary>
        public EventHandler OnOK;


        /// <summary>
        /// Actions to perform when user changes the protocol.
        /// </summary>
        public EventHandler OnProtocolChange;

        /// <summary>
        /// Actions to perform on canceling settings.
        /// </summary>
        public EventHandler OnCancel;

        #endregion Public Event Handlers

        /// <summary>
        /// Default constructor. Initializes all components.
        /// </summary>
        public AddinSettingsForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Event triggered when the "Apply" button is clicked.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            this.OnApply(sender, e);
        }


        /// <summary>
        /// Event triggered when the "Cancel button is pressed.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.OnCancel(sender,e);
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Event that is automatically triggered the Form is loading.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void ConnectionSettingsForm_Load(object sender, EventArgs e)
        {
            this.OnFormLoad(sender,e);
        }
        
        /// <summary>
        /// Event triggered when a connection setting is changed.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void txtAnyConnectionSetting_TextChanged(object sender, EventArgs e)
        {
            connectionSettingsApplied = false;
        }

        /// <summary>
        /// Event triggered when the OK button is pressed.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.OnOK(sender, e);
        }

        /// <summary>
        /// Event triggered when the select pages repository is clicked.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void btnPagesRepo_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtPagesRepo.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// Event triggered when the select downloaded attachments repository is clicked.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void btnAttachmentsRepo_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtAttachmentsRepo.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// Checks if a path is valid
        /// </summary>
        /// <param name="path">The value of the path.</param>
        /// <returns>True if the path is valid. False if the path is not valid.</returns>
        public bool ValidatePath(String path)
        {
            bool valid = true;
            try
            {
                if (!Path.IsPathRooted(path))
                {
                    valid = false;
                }
            }
            catch (Exception)
            {
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Checks if a url is valid.
        /// </summary>
        /// <param name="url">The value of the url to be checked.</param>
        /// <returns>True if the url is valid. False if the url is not valid.</returns>
        private bool ValidateURL(String url)
        {
            bool valid = true;
            try
            {
                Uri uri = new Uri(url, UriKind.Absolute);
            }
            catch (UriFormatException)
            {
                valid = false;
            }
            return valid;
        }


        /// <summary>
        /// Event triggered when the textbox loses the focus.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void txtPagesRepo_Leave(object sender, EventArgs e)
        {
            bool isValid = ValidatePath(txtPagesRepo.Text);
            if (!isValid)
            {
                UserNotifier.Error("The path you provided is not valid. Please select a valid path");
            }
        }

        /// <summary>
        /// Event triggered when the textbox loses the focus.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void txtAttachmentsRepo_Leave(object sender, EventArgs e)
        {
            bool isValid = ValidatePath(txtAttachmentsRepo.Text);
            if (!isValid)
            {
                UserNotifier.Error("The path you provided is not valid. Please select a valid path");
            }
        }

        /// <summary>
        /// Event triggered when a repository setting is changed.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event paramaters.</param>
        private void anyRepoSettingChanged_TextChanged(object sender, EventArgs e)
        {
            addinSettingsApplied = false;
        }

        private void comboProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnProtocolChange(sender, e);
        }

        private void linkConnectDoc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(connectionDocUrl);
                p.Start();
            }
        }

        private void AddinSettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsShown = false;
        }

        private void AddinSettingsForm_Shown(object sender, EventArgs e)
        {
            IsShown = true;
        }
    }
}