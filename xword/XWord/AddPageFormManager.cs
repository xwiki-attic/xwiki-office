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
using System.Windows.Forms;
using UICommons;
using UICommons.UIActionsManagement;

namespace XWord
{
    /// <summary>
    /// Manages the public events handlers for AddPageForm.
    /// </summary>
    public class AddPageFormManager : AbstractAddPageFormActionsManager
    {
        AddPageForm addPageForm;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="addPageForm">A reference to an <code>AddPageForm</code> instance.</param>
        public AddPageFormManager(ref AddPageForm addPageForm)
        {
            this.addPageForm = addPageForm;
        }

        /// <summary>
        /// Event trigerred when OnAdd event of the AddPageForm instance is raised.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event args.</param>
        protected override void ActionAdd(object sender, EventArgs e)
        {
            if (!addPageForm.ExportMode)
            {
                Globals.XWikiAddIn.AddinActions.AddNewPage(addPageForm.SpaceName, addPageForm.PageName, addPageForm.PageTitle, addPageForm);
            }
            else
            {
                Globals.XWikiAddIn.currentPageFullName = addPageForm.SpaceName + "." + addPageForm.PageName;
                Globals.XWikiAddIn.AddinActions.SaveToServer();
            }
        }

        #region IActionsManager<AddPageForm> Members

        /// <summary>
        /// Enqueues all event handlers for an AddPageForm.
        /// </summary>
        public override void EnqueueAllHandlers()
        {
            addPageForm.OnAdd += new EventHandler(this.ActionAdd);
        }

        #endregion IActionsManager<AddPageForm> Members
    }
}
