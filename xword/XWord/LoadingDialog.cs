using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XWriter
{
    public partial class LoadingDialog : Form
    {

        private bool show;
        /// <summary>
        /// Displays a continuous progress bar while an operation is in progress.
        /// </summary>
        /// <param name="message"></param>
        public LoadingDialog(String message)
        {
            InitializeComponent();
            label.Text = message;
            show = false;
        }
        
        /// <summary>
        /// Shows the form.
        /// </summary>
        /// <remarks>Recomended for cross tread calls.</remarks>
        /// <param name="obj">Extra parameter to match the wait callback signature.</param>
        public void ShowSyncDialog(Object obj)
        {
            show = true;
            ShowDialog();
            timer.Start();
            //Show();
        }

        delegate void closeDialogDelegate();

        /// <summary>
        /// Closes the dialog in a safe cross thread manner.
        /// </summary>
        public void CloseSyncDialog()
        {
            closeDialogDelegate closeDelegate = new closeDialogDelegate(CloseSyncDialog);
            if (this.InvokeRequired)
            {
                this.Invoke(closeDelegate);
            }
            else
            {
                show = false;
                Close();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (show)
            {
                try
                {
                    this.ShowDialog();
                }
                catch (Exception) { };
            }
        }

        private void LoadingDialog_Shown(object sender, EventArgs e)
        {
            show = false;
        }
    }
}
