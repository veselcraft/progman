using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ProgramManagerVC
{
    public partial class FormExecute : Form
    {
        public FormExecute()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Process.Start(textBoxPath.Text);
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialogBrowse.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = openFileDialogBrowse.InitialDirectory + openFileDialogBrowse.FileName;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void TextBoxPath_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxPath.Text))
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void FormExecute_Load(object sender, EventArgs e)
        {

        }
    }
}
