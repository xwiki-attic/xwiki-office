using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UICommons;

namespace XWord
{
    /// <summary>
    /// Manages the instances and public events handlers for AddPageForm.
    /// </summary>
    public class AddPageFormManager
    {
        private AddPageForm addPageForm;

        /// <summary>
        /// Creates a new AddPageForm.
        /// </summary>
        /// <param name="wiki">A reference to <code>XWiki.WikiSructure</code>.</param>
        /// <returns>New AddPageForm.</returns>
        public AddPageForm NewAddPageForm(ref XWiki.WikiStructure wiki)
        {
            addPageForm = new AddPageForm(ref wiki);
            addPageForm.OnAdd += new EventHandler(this.ActionAdd);
            return addPageForm;
        }

        /// <summary>
        /// Creates a new AddPageForm.
        /// </summary>
        /// <param name="wiki">A reference to <code>XWiki.WikiSructure</code>.</param>
        /// <param name="spaceName">Space name.</param>
        /// <returns>New AddPageForm.</returns>
        public AddPageForm NewAddPageForm(ref XWiki.WikiStructure wiki, string spaceName)
        {
            addPageForm = new AddPageForm(ref wiki,spaceName);
            addPageForm.OnAdd += new EventHandler(this.ActionAdd);
            return addPageForm;
        }

        /// <summary>
        /// Creates a new AddPageForm.
        /// </summary>
        /// <param name="wiki">A reference to <code>XWiki.WikiSructure</code>.</param>
        /// <param name="newSpace">TRUE if it's a new space.</param>
        /// <param name="exportMode">TRUE if export mode.</param>
        /// <returns>New AddPageForm.</returns>
        public AddPageForm NewAddPageForm(ref XWiki.WikiStructure wiki, bool newSpace,bool exportMode)
        {
            addPageForm = new AddPageForm(ref wiki, newSpace, exportMode);
            addPageForm.OnAdd += new EventHandler(this.ActionAdd);
            return addPageForm;
        }

        /// <summary>
        /// Event trigeered when OnAdd event of the AddPageForm instance is raised.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event args.</param>
        private void ActionAdd(object sender, EventArgs e)
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
    }
}
