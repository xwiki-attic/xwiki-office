using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using XWiki;

namespace XWriter
{
    /// <summary>
    /// UI for adding a page or a space.
    /// </summary>
    public partial class AddPageForm : Form
    {

        WikiStructure wiki;
        Space selectedSpace;
        String spaceName;
        String pageName;
        String pageTitle;

        /// <param name="_wiki">A reference to the currently loaded wiki structure.</param>
        public AddPageForm(ref WikiStructure _wiki)
        {
            InitializeComponent();
            this.wiki = _wiki;
            LoadSpaces();            
        }

        /// <param name="_wiki">A reference to the currently loaded wiki structure.</param>
        /// <param name="_spaceName">
        /// The default space name in which the new page will be created
        /// </param>
        public AddPageForm(ref WikiStructure _wiki, String _spaceName)
        {
            InitializeComponent();
            this.wiki = _wiki;
            LoadSpaces();
            foreach (Space space in wiki.spaces)
            {
                if (space.name == _spaceName)
                {
                    radioButtonExistingSpace.Checked = true;
                    foreach (Object item in comboBoxSpaceName.Items)
                    {
                        if (item.ToString() == _spaceName)
                        {
                            comboBoxSpaceName.SelectedItem = item;
                        }
                    }
                }
            }
            txtPageName.Focus();
        }

        /// <param name="_wiki">A reference to the currently loaded wiki structure.</param>
        /// <param name="newSpace">Specifies if a new space will be created.</param>
        public AddPageForm(ref WikiStructure _wiki, bool newSpace)
        {
            InitializeComponent();
            this.wiki = _wiki;
            LoadSpaces();
            if (newSpace)
            {
                comboBoxSpaceName.Visible = false;
                txtSpaceName.Visible = true;
                radioButtonNewSpace.Checked = true;
                txtPageName.Text = "WebHome";
                txtPageTitle.Text = "WebHome";
            }
        }

        /// <summary>
        /// Event triggered when the form is loading.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">The event parameters.</param>
        private void AddPageForm_Load(object sender, EventArgs e)
        {
            comboBoxSpaceName.Top = txtSpaceName.Top;
        }

        /// <summary>
        /// Loads the spaces into the combo.
        /// </summary>
        private void LoadSpaces()
        {
            foreach (Space space in wiki.spaces)
            {
                comboBoxSpaceName.Items.Add(space.name);
            }
            if (comboBoxSpaceName.Items.Count > 0)
            {
                comboBoxSpaceName.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Refreshes the UI to the coresponding option regarding the space.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonExistingSpace.Checked)
            {               
                comboBoxSpaceName.Visible = true;
                comboBoxSpaceName.BringToFront();
                txtSpaceName.SendToBack();
                txtSpaceName.Visible = false;
            }
            else if (radioButtonNewSpace.Checked)
            {
                txtSpaceName.Visible = true;
                txtSpaceName.BringToFront();
                comboBoxSpaceName.SendToBack();
                comboBoxSpaceName.Visible = false;
            }
        }

        /// <summary>
        /// Event triggered when the "Add page" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPage_Click(object sender, EventArgs e)
        {
            if (ValidateFormData())
            {
                try
                {
                    this.Close();
                    Globals.XWikiAddIn.AddinActions.AddNewPage(spaceName, pageName, pageTitle, this);
                }
                catch (COMException) { }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "XWord", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Validates the data.
        /// </summary>
        /// <returns>True if data is valid. False if data is invalid.</returns>
        private bool ValidateFormData()
        {
            bool isValid = true;
            String err = "";
            if (comboBoxSpaceName.Text.Length == 0 && radioButtonExistingSpace.Checked)
            {
                err = err + Environment.NewLine + " - The space name is not valid";
                isValid = false;
            }
            if(txtSpaceName.Text.Length == 0 && radioButtonNewSpace.Checked)
            {
                err = err + Environment.NewLine + " - The space name is not valid.";
                isValid = false;
            }
            if (txtPageName.Text.Length == 0)
            {
                err = err + Environment.NewLine + " - The page name connot be empty.";
                isValid = false;
            }
            if (radioButtonExistingSpace.Checked && (selectedSpace != null))
            {
                foreach (XWikiDocument doc in selectedSpace.documents)
                {
                    if (doc.name == txtPageName.Text)
                    {
                        err = err + " - The page name is not valid. A page named '" + doc.name + "' already exists. Please choose another name.";
                    }
                }
            }
            if (isValid)
            {
                if(radioButtonExistingSpace.Checked)
                {
                    spaceName = comboBoxSpaceName.Text;
                }
                else
                {
                    spaceName = txtSpaceName.Text;
                }
                pageName = txtPageName.Text;
                pageTitle = txtPageTitle.Text;
            }
            else
            {
                MessageBox.Show(err, "XWord", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return isValid;
        }

        /// <summary>
        /// Sets the instance of the selected space when the combo box selection is changed
        /// </summary>
        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSpace = wiki.spaces[comboBoxSpaceName.SelectedIndex];
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
