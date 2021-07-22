using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace ProgramManagerVC
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
            label2.Text = String.Format("Version {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/veselcraft/progman");
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {

        }
    }
}
