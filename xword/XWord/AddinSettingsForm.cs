using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using XWiki.Clients;
using XWiki.Logging;

namespace XWord
{
    /// <summary>
    /// Provides UI for changing the add-in settings.
    /// </summary>
    public partial class AddinSettingsForm : Form
    {
        bool connectionSettingsApplied = true;
        bool addinSettingsApplied = true;
        bool loadingDialogFlag = false;
        XWordSettings addinSettings = new XWordSettings();
        const String connectionDocUrl = "http://xoffice.xwiki.org/xwiki/bin/view/XWord/User_Guide#HConnecttoaXWikiserver";

        #region connectivity
        StringCollection connectMethods = Properties.Settings.Default.ConnectMethods;
        Dictionary<String, XWikiClientType> connectDictionary = new Dictionary<string, XWikiClientType>();
        #endregion
        /// <summary>
        /// Default constructor. Initializes all components.
        /// </summary>
        public AddinSettingsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the instance of the addin.
        /// </summary>
        public XWikiAddIn Addin
        {
            get
            {
                return Globals.XWikiAddIn;
            }
        }

        /// <summary>
        /// Gets or sets the URL of the server that the add-in conncets to.
        /// </summary>
        private String ServerURL
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
        private String UserName
        {
            get
            {
                return txtUserName.Text;
            }
        }

        /// <summary>
        /// Gets the password that was provided.
        /// </summary>
        private String Password
        {
            get
            {
                return txtPassword.Text;
            }
        }

        /// <summary>
        /// Event triggered when the "Apply" button is clicked.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabConnection)
            {
                ApplyConnectionSettings();
                connectionSettingsApplied = true;
            }
            else if (tabControl.SelectedTab == tabFileRepository)
            {
                ApplyRepositoriesSettings();
                addinSettingsApplied = true;
            }
        }

        /// <summary>
        /// Sets the new connection settings for the addin,
        /// connects to the server and refreshes the active wiki explorer.
        /// </summary>
        private void ApplyConnectionSettings()
        {
            Cursor c = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            if (ServerURL.EndsWith("/"))
            {
                ServerURL = ServerURL.Substring(0, ServerURL.Length - 1);
            }
            Addin.serverURL = ServerURL;
            Addin.username = UserName;
            Addin.password = Password;
            LoginData loginData = new LoginData();
            Addin.Client = XWikiClientFactory.CreateXWikiClient(Addin.ClientType, Addin.serverURL, Addin.username, Addin.password);
            // refreshes the ribbon buttons
            // which allow the user to work with the documents from XWiki server
            Globals.Ribbons.XWikiRibbon.Refresh(null, null);            
            //TODO if login fails then...
            //AddTaskPanes(); TODO: Sync all taskpanes
            if (Addin.XWikiTaskPane != null)
            {
                Addin.XWikiTaskPane.RefreshWikiExplorer();
            }
            
            if (ckRememberMe.Checked)
            {
                String[] credentials = new String[3];
                credentials[0] = Globals.XWikiAddIn.serverURL;
                credentials[1] = Globals.XWikiAddIn.username;
                credentials[2] = Globals.XWikiAddIn.password;
                loginData.WriteCredentials(credentials);
            }
            else
            {
                loginData.ClearCredentials();
            }
            //Write the settings to isolated storage. 
            XWordSettingsHandler.WriteRepositorySettings(addinSettings);

            this.Cursor = c;
        }

        /// <summary>
        /// Sets the pages and attachments repositories.
        /// </summary>
        private void ApplyRepositoriesSettings()
        {
            Cursor c = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            if (ValidatePath(txtPagesRepo.Text))
            {
                Addin.PagesRepository = txtPagesRepo.Text;
            }
            else
            {
                Addin.PagesRepository = Path.GetTempPath();
            }
            if (ValidatePath(txtAttachmentsRepo.Text))
            {
                Addin.DownloadedAttachmentsRepository = txtAttachmentsRepo.Text;
            }
            else
            {
                Addin.DownloadedAttachmentsRepository = Path.GetTempPath();
            }            
            XWordSettingsHandler.WriteRepositorySettings(addinSettings);
            addinSettingsApplied = true;
            Thread.Sleep(500);
            this.Cursor = c;
        }

        /// <summary>
        /// Event triggered when the "Cancel button is pressed.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
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
            loadingDialogFlag = true;
            if (Addin.serverURL == "" || Addin.serverURL == null)
            {
                groupBox1.Text = "Test server settings";
            }
            else
            {
                groupBox1.Text = "Settings";
                txtServerURL.Text = Addin.serverURL;
                txtUserName.Text = Addin.username;
                txtPassword.Text = Addin.password;
            }
            addinSettings = new XWordSettings();
            addinSettings.PagesRepository = Addin.PagesRepository;
            addinSettings.DownloadedAttachmentsRepository = Addin.DownloadedAttachmentsRepository;
            addinSettings.ClientType = Addin.ClientType;
            txtPagesRepo.Text = Addin.PagesRepository;
            txtAttachmentsRepo.Text = Addin.DownloadedAttachmentsRepository;
            //init protocol settings
            connectDictionary.Add(connectMethods[0], XWikiClientType.XML_RPC);
            connectDictionary.Add(connectMethods[1], XWikiClientType.HTTP_Client);
            comboProtocol.DataSource = connectMethods;
            switch(Addin.ClientType)
            {
                case XWikiClientType.XML_RPC:
                    comboProtocol.SelectedIndex = 0;
                    break;
                case XWikiClientType.HTTP_Client:
                    comboProtocol.SelectedIndex = 1;
                    break;
            }
            loadingDialogFlag = false;
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
            //If settings changes were made or there is no client instance
            if (!connectionSettingsApplied || (Addin.Client == null))
            {
                ApplyConnectionSettings();
            }
            if (!addinSettingsApplied)
            {
                ApplyRepositoriesSettings();
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
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
        /// Checks if a path is valid
        /// </summary>
        /// <param name="path">The value of the path.</param>
        /// <returns>True if the path is valid. False if the path is not valid.</returns>
        private bool ValidatePath(String path)
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
            //If user generated event
            if (!loadingDialogFlag)
            {
                String selectedValue = (String)comboProtocol.SelectedValue;
                if (connectDictionary.Keys.Contains(selectedValue))
                {
                    addinSettings.ClientType = connectDictionary[selectedValue];
                    Addin.ClientType = addinSettings.ClientType;
                    connectionSettingsApplied = false;
                }
                else
                {
                    UserNotifier.StopHand("The selected value is not valid.");
                }
            }
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
    }
}