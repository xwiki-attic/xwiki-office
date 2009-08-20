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
