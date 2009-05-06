namespace XWriter
{
    partial class AddPageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddPageForm));
            this.lblSpaceName = new System.Windows.Forms.Label();
            this.txtSpaceName = new System.Windows.Forms.TextBox();
            this.groupBoxSpace = new System.Windows.Forms.GroupBox();
            this.comboBoxSpaceName = new System.Windows.Forms.ComboBox();
            this.radioButtonNewSpace = new System.Windows.Forms.RadioButton();
            this.radioButtonExistingSpace = new System.Windows.Forms.RadioButton();
            this.groupBoxPage = new System.Windows.Forms.GroupBox();
            this.txtPageTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPageName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddPage = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBoxSpace.SuspendLayout();
            this.groupBoxPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSpaceName
            // 
            this.lblSpaceName.AutoSize = true;
            this.lblSpaceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpaceName.Location = new System.Drawing.Point(7, 55);
            this.lblSpaceName.Name = "lblSpaceName";
            this.lblSpaceName.Size = new System.Drawing.Size(88, 16);
            this.lblSpaceName.TabIndex = 0;
            this.lblSpaceName.Text = "Space name:";
            // 
            // txtSpaceName
            // 
            this.txtSpaceName.Location = new System.Drawing.Point(117, 55);
            this.txtSpaceName.Name = "txtSpaceName";
            this.txtSpaceName.Size = new System.Drawing.Size(284, 21);
            this.txtSpaceName.TabIndex = 2;
            // 
            // groupBoxSpace
            // 
            this.groupBoxSpace.Controls.Add(this.comboBoxSpaceName);
            this.groupBoxSpace.Controls.Add(this.txtSpaceName);
            this.groupBoxSpace.Controls.Add(this.radioButtonNewSpace);
            this.groupBoxSpace.Controls.Add(this.lblSpaceName);
            this.groupBoxSpace.Controls.Add(this.radioButtonExistingSpace);
            this.groupBoxSpace.Location = new System.Drawing.Point(14, 14);
            this.groupBoxSpace.Name = "groupBoxSpace";
            this.groupBoxSpace.Size = new System.Drawing.Size(419, 108);
            this.groupBoxSpace.TabIndex = 2;
            this.groupBoxSpace.TabStop = false;
            this.groupBoxSpace.Text = "Wiki Space";
            // 
            // comboBoxSpaceName
            // 
            this.comboBoxSpaceName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSpaceName.FormattingEnabled = true;
            this.comboBoxSpaceName.Location = new System.Drawing.Point(117, 77);
            this.comboBoxSpaceName.Name = "comboBoxSpaceName";
            this.comboBoxSpaceName.Size = new System.Drawing.Size(284, 23);
            this.comboBoxSpaceName.TabIndex = 2;
            this.comboBoxSpaceName.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // radioButtonNewSpace
            // 
            this.radioButtonNewSpace.AutoSize = true;
            this.radioButtonNewSpace.Location = new System.Drawing.Point(125, 22);
            this.radioButtonNewSpace.Name = "radioButtonNewSpace";
            this.radioButtonNewSpace.Size = new System.Drawing.Size(88, 19);
            this.radioButtonNewSpace.TabIndex = 1;
            this.radioButtonNewSpace.TabStop = true;
            this.radioButtonNewSpace.Text = "New Space";
            this.radioButtonNewSpace.UseVisualStyleBackColor = true;
            // 
            // radioButtonExistingSpace
            // 
            this.radioButtonExistingSpace.AutoSize = true;
            this.radioButtonExistingSpace.Location = new System.Drawing.Point(7, 22);
            this.radioButtonExistingSpace.Name = "radioButtonExistingSpace";
            this.radioButtonExistingSpace.Size = new System.Drawing.Size(106, 19);
            this.radioButtonExistingSpace.TabIndex = 0;
            this.radioButtonExistingSpace.TabStop = true;
            this.radioButtonExistingSpace.Text = "Existing Space";
            this.radioButtonExistingSpace.UseVisualStyleBackColor = true;
            this.radioButtonExistingSpace.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBoxPage
            // 
            this.groupBoxPage.Controls.Add(this.txtPageTitle);
            this.groupBoxPage.Controls.Add(this.label2);
            this.groupBoxPage.Controls.Add(this.txtPageName);
            this.groupBoxPage.Controls.Add(this.label1);
            this.groupBoxPage.Location = new System.Drawing.Point(14, 129);
            this.groupBoxPage.Name = "groupBoxPage";
            this.groupBoxPage.Size = new System.Drawing.Size(419, 114);
            this.groupBoxPage.TabIndex = 3;
            this.groupBoxPage.TabStop = false;
            this.groupBoxPage.Text = "Wiki Page";
            // 
            // txtPageTitle
            // 
            this.txtPageTitle.Location = new System.Drawing.Point(117, 63);
            this.txtPageTitle.Name = "txtPageTitle";
            this.txtPageTitle.Size = new System.Drawing.Size(284, 21);
            this.txtPageTitle.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Page title:";
            // 
            // txtPageName
            // 
            this.txtPageName.Location = new System.Drawing.Point(117, 33);
            this.txtPageName.Name = "txtPageName";
            this.txtPageName.Size = new System.Drawing.Size(284, 21);
            this.txtPageName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Page name:";
            // 
            // btnAddPage
            // 
            this.btnAddPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPage.AutoSize = true;
            this.btnAddPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPage.Location = new System.Drawing.Point(276, 254);
            this.btnAddPage.Name = "btnAddPage";
            this.btnAddPage.Size = new System.Drawing.Size(78, 28);
            this.btnAddPage.TabIndex = 5;
            this.btnAddPage.Text = "Add page";
            this.btnAddPage.UseVisualStyleBackColor = true;
            this.btnAddPage.Click += new System.EventHandler(this.btnAddPage_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(359, 254);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 28);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddPageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(448, 294);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddPage);
            this.Controls.Add(this.groupBoxPage);
            this.Controls.Add(this.groupBoxSpace);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddPageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XWord - Add new page to the wiki";
            this.Load += new System.EventHandler(this.AddPageForm_Load);
            this.groupBoxSpace.ResumeLayout(false);
            this.groupBoxSpace.PerformLayout();
            this.groupBoxPage.ResumeLayout(false);
            this.groupBoxPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSpaceName;
        private System.Windows.Forms.TextBox txtSpaceName;
        private System.Windows.Forms.GroupBox groupBoxSpace;
        private System.Windows.Forms.RadioButton radioButtonNewSpace;
        private System.Windows.Forms.RadioButton radioButtonExistingSpace;
        private System.Windows.Forms.GroupBox groupBoxPage;
        private System.Windows.Forms.TextBox txtPageName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPageTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxSpaceName;
        private System.Windows.Forms.Button btnAddPage;
        private System.Windows.Forms.Button btnCancel;
    }
}