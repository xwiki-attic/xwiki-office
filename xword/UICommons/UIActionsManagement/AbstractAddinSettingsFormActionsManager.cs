using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICommons.UIActionsManagement
{
    /// <summary>
    /// Abstract actions manager for <code>AddinSettingsForm</code> instances.
    /// </summary>
    public abstract class AbstractAddinSettingsFormActionsManager:IActionsManager<AddinSettingsForm>
    {
        /// <summary>
        /// Action to perform when OnFormLoad event is raised.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected abstract void ActionFormLoad(object sender, EventArgs e);

        /// <summary>
        /// Action to perform when OnApply event is raised.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected abstract void ActionApply(object sender, EventArgs e);

        /// <summary>
        /// Action to perform when OnOK event is raised.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected abstract void ActionOK(object sender, EventArgs e);

        /// <summary>
        /// Action to perform when OnProtocolChanged event is raised.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected abstract void ActionProtocolChanged(object sender, EventArgs e);

        /// <summary>
        /// Action to perform when OnCancel event is raised.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected abstract void ActionCancel(object sender, EventArgs e);


        #region IActionsManager<AddinSettingsForm> Members

        /// <summary>
        /// Enqueues all (known/custom) event handlers defined for an <code>AddinSettingsForm</code> instance.
        /// </summary>
        public abstract void EnqueueAllHandlers();

        #endregion
    }
}
