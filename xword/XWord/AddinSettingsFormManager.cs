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
using System.Linq;
using System.Text;
using XWiki.Clients;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using XWiki;
using XWiki.Logging;
using UICommons;
using UICommons.UIActionsManagement;
using XOffice;

namespace XWord
{
    /// <summary>
    /// Manages the public events handlers for <code>AddinSettingsForm</code> instances.
    /// </summary>
    public class AddinSettingsFormManager : AbstractAddinSettingsFormActionsManager
    {
        private AddinSettingsForm addinSettingsForm;
        private XWikiAddIn addin;
        private XWikiClientType currentClientType;

        private XOfficeCommonSettings AddinSettings
        {
            get { return addin.AddinSettings; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="addinSettingsForm">A reference to an <code>AddinSettingsForm</code> instance.</param>
        public AddinSettingsFormManager(ref AddinSettingsForm addinSettingsForm)
        {
            this.addinSettingsForm = addinSettingsForm;
            this.addin = Globals.XWikiAddIn;
            this.currentClientType = addin.AddinSettings.ClientType;
        }

        #region AbstractAddinSettingsFormActionsManager Members

        /// <summary>
        /// Enqueues all (known/custom) event handlers defined for an <code>AddinSettingsForm</code> instance:
        /// OnApply, OnCancel, OnFormLoad, OnOK, OnProtocolChange. 
        /// </summary>
        public override void EnqueueAllHandlers()
        {
            addinSettingsForm.OnApply = this.ActionApply;
            addinSettingsForm.OnCancel = this.ActionCancel;
            addinSettingsForm.OnFormLoad = this.ActionFormLoad;
            addinSettingsForm.OnOK = this.ActionOK;
            addinSettingsForm.OnProtocolChange = this.ActionProtocolChanged;
        }

        /// <summary>
        /// Action to perform when OnFormLoad event is raised.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void ActionFormLoad(object sender, EventArgs e)
        {
            addinSettingsForm.LoadingDialogFlag = true;

            if (addin.serverURL == "" || addin.serverURL == null)
            {
                addinSettingsForm.GroupBox1Text = "Test server settings";
            }
            else
            {
                addinSettingsForm.GroupBox1Text = "Settings";
                addinSettingsForm.ServerURL = addin.serverURL;
                addinSettingsForm.UserName = addin.username;
                addinSettingsForm.Password = addin.password;
            }
            addinSettingsForm.CkRememberMe = addin.RememberCredentials;
            addinSettingsForm.CkAutoLogin = AddinSettings.AutoLogin;

            //repository tab
            addinSettingsForm.TxtPagesRepoText = addin.AddinSettings.PagesRepository;
            addinSettingsForm.TxtAttachmentsRepoText = addin.AddinSettings.DownloadedAttachmentsRepository;
            //prefetch tab
            addinSettingsForm.IsPrefetchEnabled = addin.AddinSettings.PrefetchSettings.PrefetchEnabled;
            addinSettingsForm.txtPrefetchInterval.Text = addin.AddinSettings.PrefetchSettings.PollingInterval.ToString();
            addinSettingsForm.txtPrefetchPagesSetSize.Text = addin.AddinSettings.PrefetchSettings.ResultSetSize.ToString();            

            //init protocol settings
            addinSettingsForm.ConnectDictionary.Add(addinSettingsForm.ConnectMethods[0], XWikiClientType.XML_RPC);
            addinSettingsForm.ConnectDictionary.Add(addinSettingsForm.ConnectMethods[1], XWikiClientType.HTTP_Client);
            addinSettingsForm.ComboProtocolDataSource = addinSettingsForm.ConnectMethods;
            switch (addin.AddinSettings.ClientType)
            {
                case XWikiClientType.XML_RPC:
                    addinSettingsForm.ComboProtocolSelectedIndex = 0;
                    break;
                case XWikiClientType.HTTP_Client:
                    addinSettingsForm.ComboProtocolSelectedIndex = 1;
                    break;
            }
            addinSettingsForm.LoadingDialogFlag = false;
        }

        /// <summary>
        /// Action to perform when OnApply event is raised.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void ActionApply(object sender, EventArgs e)
        {
            if (addinSettingsForm.IsTabConnectionSelected)
            {
                ApplyConnectionSettings();
                addinSettingsForm.ConnectionSettingsApplied = true;
            }
            else if (addinSettingsForm.IsTabFileRepositorySelected)
            {
                ApplyRepositoriesSettings();
                addinSettingsForm.AddinSettingsApplied = true;
            }
            else if (addinSettingsForm.IsTabPrefetchSelected)
            {
                ApplyPrefetchSettings();
                addinSettingsForm.AddinSettingsApplied = true;
            }
        }

        /// <summary>
        /// Action to perform when OnOK event is raised.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void ActionOK(object sender, EventArgs e)
        {
            //If settings changes were made or there is no client instance
            if (!addinSettingsForm.ConnectionSettingsApplied || (addin.Client == null))
            {
                ApplyConnectionSettings();
            }
            if (!addinSettingsForm.AddinSettingsApplied)
            {
                ApplyRepositoriesSettings();
                ApplyPrefetchSettings();
            }
            addinSettingsForm.DialogResult = DialogResult.OK;
            addinSettingsForm.Close();
        }

        /// <summary>
        /// Action to perform when OnProtocolChanged event is raised.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void ActionProtocolChanged(object sender, EventArgs e)
        {
            //If user generated event
            if (!addinSettingsForm.LoadingDialogFlag)
            {
                String selectedValue = (String)addinSettingsForm.ComboProtocolSelectedValue;
                if (addinSettingsForm.ConnectDictionary.Keys.Contains(selectedValue))
                {
                    AddinSettings.ClientType = addinSettingsForm.ConnectDictionary[selectedValue];
                    addin.AddinSettings.ClientType = AddinSettings.ClientType;
                    addinSettingsForm.ConnectionSettingsApplied = false;
                }
                else
                {
                    UserNotifier.StopHand("The selected value is not valid.");
                }
            }
        }

        /// <summary>
        /// Action to perform when OnCancel event is raised.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void ActionCancel(object sender, EventArgs e)
        {
            //rollback to initial value of addin client type
            addin.AddinSettings.ClientType = currentClientType;
        }

        #endregion AbstractAddinSettingsFormActionsManager

        /// <summary>
        /// Sets the new connection settings for the addin,
        /// connects to the server and refreshes the active wiki explorer.
        /// </summary>
        private void ApplyConnectionSettings()
        {
            Cursor c = addinSettingsForm.Cursor;
            addinSettingsForm.Cursor = Cursors.WaitCursor;
            if (addinSettingsForm.ServerURL.EndsWith("/"))
            {
                addinSettingsForm.ServerURL = addinSettingsForm.ServerURL.Substring(0, addinSettingsForm.ServerURL.Length - 1);
            }
            addin.serverURL = addinSettingsForm.ServerURL;
            addin.username = addinSettingsForm.UserName;
            addin.password = addinSettingsForm.Password;
            addin.RememberCredentials = addinSettingsForm.CkRememberMe;
            addin.AddinSettings.AutoLogin = addinSettingsForm.CkAutoLogin;
            LoginData loginData = new LoginData(LoginData.XWORD_LOGIN_DATA_FILENAME);
            addin.Client = XWikiClientFactory.CreateXWikiClient(addin.AddinSettings.ClientType,
                addin.serverURL, addin.username, addin.password);

            if (addinSettingsForm.CkRememberMe)
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
            XOfficeCommonSettingsHandler.WriteRepositorySettings(AddinSettings);

            addinSettingsForm.Cursor = c;
        }


        /// <summary>
        /// Sets the pages and attachments repositories.
        /// </summary>
        private void ApplyRepositoriesSettings()
        {
            Cursor c = addinSettingsForm.Cursor;
            addinSettingsForm.Cursor = Cursors.WaitCursor;
            if (addinSettingsForm.ValidatePath(addinSettingsForm.TxtPagesRepoText))
            {
                addin.AddinSettings.PagesRepository = addinSettingsForm.TxtPagesRepoText;
            }
            else
            {
                addin.AddinSettings.PagesRepository = Path.GetTempPath();
            }
            if (addinSettingsForm.ValidatePath(addinSettingsForm.TxtAttachmentsRepoText))
            {
                addin.AddinSettings.DownloadedAttachmentsRepository = addinSettingsForm.TxtAttachmentsRepoText;
            }
            else
            {
                addin.AddinSettings.DownloadedAttachmentsRepository = Path.GetTempPath();
            }
            XOfficeCommonSettingsHandler.WriteRepositorySettings(AddinSettings);
            addinSettingsForm.AddinSettingsApplied = true;
            Thread.Sleep(500);
            addinSettingsForm.Cursor = c;
        }

        /// <summary>
        /// Sets the settings for the active prefetcher and 
        /// </summary>
        private void ApplyPrefetchSettings()
        {
            Cursor c = addinSettingsForm.Cursor;
            addinSettingsForm.Cursor = Cursors.WaitCursor;
            //replace active settings
            try
            {
                //parse the input
                double prefetchInterval = Double.Parse(addinSettingsForm.txtPrefetchInterval.Text);
                int resultSetSize = Int32.Parse(addinSettingsForm.txtPrefetchPagesSetSize.Text);
                AddinSettings.PrefetchSettings.PollingInterval = prefetchInterval;
                AddinSettings.PrefetchSettings.ResultSetSize = resultSetSize;
            }
            catch (FormatException fe)
            {
                Log.Exception(fe);
                //return to initial values
                addinSettingsForm.txtPrefetchInterval.Text = AddinSettings.PrefetchSettings.PollingInterval.ToString();
                addinSettingsForm.txtPrefetchPagesSetSize.Text = AddinSettings.PrefetchSettings.ResultSetSize.ToString();
            }
            AddinSettings.PrefetchSettings.PrefetchEnabled = addinSettingsForm.IsPrefetchEnabled;
            //write the settings to storage
            XOfficeCommonSettingsHandler.WriteRepositorySettings(AddinSettings);
            addinSettingsForm.Cursor = c;
        }
    }
}
