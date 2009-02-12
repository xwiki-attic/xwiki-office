using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XWordTrustManager
{
    public partial class TrustForm : Form
    {
        public TrustForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\MICROSOFT\\.NETFramework\\Security\\TrustManager\\PromptingLevel");
            key.SetValue("MyComputer", "Enabled");
            key.SetValue("LocalIntranet", "Enabled");
            key.SetValue("Internet", "Enabled");
            key.SetValue("TrustedSites", "Enabled");
            key.SetValue("UntrustedSites", "Disabled");
            key.Close();
            MessageBox.Show("Done.", "XWord", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }        
    }
}
