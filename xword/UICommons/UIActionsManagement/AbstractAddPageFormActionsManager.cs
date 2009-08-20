using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICommons.UIActionsManagement
{
    /// <summary>
    /// Abstract actions manager for <code>AddPageForm</code> instances.
    /// </summary>
    public abstract class AbstractAddPageFormActionsManager : IActionsManager<AddPageForm>
    {
        /// <summary>
        /// Action to perform when OnAdd event is raised.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        protected abstract void ActionAdd(object sender, EventArgs e);


        #region IActionsManager<AddPageForm> Members

        /// <summary>
        /// Enqueues all event handlers for an AddPageForm.
        /// </summary>
        public abstract void EnqueueAllHandlers();

        #endregion IActionsManager<AddPageForm> Members
    }
}