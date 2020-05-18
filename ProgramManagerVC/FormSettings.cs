using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProgramManagerVC
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UsernameInTitle = checkBoxUsername.Checked ? 1 : 0;
            Properties.Settings.Default.Save();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            checkBoxUsername.Checked = Properties.Settings.Default.UsernameInTitle == 1 ? true : false;
        }
    }
}
