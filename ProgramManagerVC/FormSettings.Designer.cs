namespace ProgramManagerVC
{
    partial class FormSettings
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkBoxShell = new System.Windows.Forms.CheckBox();
            this.checkBoxUsername = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(172, 65);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxShell
            // 
            this.checkBoxShell.AutoSize = true;
            this.checkBoxShell.Enabled = false;
            this.checkBoxShell.Location = new System.Drawing.Point(12, 12);
            this.checkBoxShell.Name = "checkBoxShell";
            this.checkBoxShell.Size = new System.Drawing.Size(127, 17);
            this.checkBoxShell.TabIndex = 1;
            this.checkBoxShell.Text = "Setup as default shell";
            this.checkBoxShell.UseVisualStyleBackColor = true;
            // 
            // checkBoxUsername
            // 
            this.checkBoxUsername.AutoSize = true;
            this.checkBoxUsername.Location = new System.Drawing.Point(12, 35);
            this.checkBoxUsername.Name = "checkBoxUsername";
            this.checkBoxUsername.Size = new System.Drawing.Size(154, 17);
            this.checkBoxUsername.TabIndex = 2;
            this.checkBoxUsername.Text = "Show username above title";
            this.checkBoxUsername.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 100);
            this.Controls.Add(this.checkBoxUsername);
            this.Controls.Add(this.checkBoxShell);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBoxShell;
        private System.Windows.Forms.CheckBox checkBoxUsername;
    }
}