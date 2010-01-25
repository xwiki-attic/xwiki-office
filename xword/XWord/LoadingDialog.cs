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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XWord
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
