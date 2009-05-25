using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XWiki.Logging
{
    /// <summary>
    /// Simple utility class for user notifications.
    /// </summary>
    public class UserNotifier
    {
        private static string MESSAGE_TITLE = "XWord";

        /// <summary>
        /// Displays a simple message to the user.
        /// </summary>
        /// <param name="text">Text message to display.</param>
        public static void Message(string text)
        {
            Show(text, MESSAGE_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Displays a message to the user and two Retry/Cancel buttons.
        /// </summary>
        /// <param name="text">Message to display.</param>
        /// <returns>A <code>DialogResult</code> containing the button clicked by the user.</returns>
        public static DialogResult RetryCancelQuestion(string text)
        {
            return Show(text, MESSAGE_TITLE, MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Asks the user a question and provides Yes/No buttons.
        /// </summary>
        /// <param name="text">Question to ask.</param>
        /// <returns>A <code>DialogResult</code> containing the button clicked by the user.</returns>
        public static DialogResult YesNoQuestion(string text)
        {
            return Show(text, MESSAGE_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Asks the user a question and provides Yes/No/Cancel buttons.
        /// </summary>
        /// <param name="text">Question to ask.</param>
        /// <returns>A <code>DialogResult</code> containing the button clicked by the user.</returns>
        public static DialogResult YesNoCancelQuestion(string text)
        {
            return Show(text, MESSAGE_TITLE, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Displays an exclamation message.
        /// </summary>
        /// <param name="text">Text to be displayed</param>
        public static void Exclamation(string text)
        {
            Show(text, MESSAGE_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        /// <summary>
        /// Shows a warning message.
        /// </summary>
        /// <param name="text">Warning message text.</param>
        /// <returns>A <code>DialogResult</code> containing the button clicked by the user.</returns>
        public static DialogResult Warning(string text)
        {
            return Show(text, MESSAGE_TITLE, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Shows an error message.
        /// Used to inform the user that an error has occured.
        /// </summary>
        /// <param name="text">Error message text.</param>
        public static void Error(string text)
        {
            Show(text, MESSAGE_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a stop message.
        /// Used to inform the user that an action will not execute.
        /// </summary>
        /// <param name="text">Stop message text</param>
        public static void StopHand(string text)
        {
            Show(text,MESSAGE_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private static DialogResult Show(string text)
        {
            return MessageBox.Show(text);
        }

        private static DialogResult Show(string text, string caption)
        {
            return MessageBox.Show(text, caption);
        }

        private static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(text, caption, buttons);
        }

        private static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(text, caption, buttons, icon);
        }

        private static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return MessageBox.Show(text, caption, buttons, icon, defaultButton);
        }

        private static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return MessageBox.Show(text, caption, buttons, icon, defaultButton, options);
        }

        /// <summary>
        /// Private constructor to avoid instancing and inheritance.
        /// </summary>
        private UserNotifier()
        {

        }
    }
}