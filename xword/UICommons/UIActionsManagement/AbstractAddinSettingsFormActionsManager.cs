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
