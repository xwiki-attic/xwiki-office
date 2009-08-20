using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace UICommons.UIActionsManagement
{
    /// <summary>
    /// Generic actions manager interface implemented by all specific actions managers.
    /// Defines a method of enqueuing event handlers for a generic control.
    /// </summary>
    /// <typeparam name="T">A reference to a generic control.</typeparam>
    public interface IActionsManager<T> where T:Control
    {
        /// <summary>
        /// Enqueues all (known) event handlers defined for a control.
        /// </summary>
        void EnqueueAllHandlers();
    }
}