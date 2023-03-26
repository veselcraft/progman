using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using data = ProgramManagerVC.data;

namespace ProgramManagerVC
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form createForm = new FormCreateGroup();
            if (createForm.ShowDialog() == DialogResult.OK)
            {
                CloseAllMDIWindows();
                InitializeMDI();
            } 
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("This program will be closed.", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            else if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                e.Cancel = false;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            InitializeMDI();
            InitializeTitle();


        }

        private void InitializeTitle()
        {
            if (Properties.Settings.Default.UsernameInTitle == 1)
            {
                Text = $"Program Manager - {Environment.MachineName}\\{Environment.UserName}";
            }
            else
            {
                Text = "Program Manager";
            }
        }

        private void InitializeMDI()
        {
            data.SendQueryWithoutReturn("CREATE TABLE IF NOT EXISTS \"groups\" (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, status INTEGER)");
            data.SendQueryWithoutReturn("CREATE TABLE IF NOT EXISTS \"items\" (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, path TEXT, workingdir TEXT, icon TEXT, groups INTEGER)");
            DataTable groups = new DataTable();
            groups = data.SendQueryWithReturn("SELECT * FROM groups");
            if (groups.Rows.Count > 0)
            {
                for (int i = 0; i < groups.Rows.Count; i++)
                {
                    Form child = new FormChild();
                    child.Text = groups.Rows[i][1].ToString();
                    child.Tag = groups.Rows[i][0].ToString();
                    child.MdiParent = this;
                    switch (groups.Rows[i][2].ToString())
                    {
                        case "0":
                            child.WindowState = FormWindowState.Minimized;
                            break;
                        case "1":
                            child.WindowState = FormWindowState.Normal;
                            break;
                        case "2":
                            child.WindowState = FormWindowState.Maximized;
                            break;
                    }
                    child.Show();
                }
            }
        }

        private void CloseAllMDIWindows()
        {
            foreach (Form frm in this.MdiChildren)
            {
                frm.Visible = false;
                frm.Dispose();
            }
        }

        private void newGroupMenuItem_Click(object sender, EventArgs e)
        {
            Form createForm = new FormCreateGroup();
            if (createForm.ShowDialog() == DialogResult.OK)
            {
                CloseAllMDIWindows();
                InitializeMDI();
            }
        }

        private void newItemMenuItem_Click(object sender, EventArgs e)
        {
            using (FormCreateItem createform = new FormCreateItem(this.ActiveMdiChild.Tag.ToString()))
            {
                if (createform.ShowDialog() == DialogResult.OK)
                {
                    ((FormChild)this.ActiveMdiChild).InitializeItems();
                }
            }
        }

        private void deleteItemMenuItem_Click(object sender, EventArgs e)
        {
            if (((FormChild)this.ActiveMdiChild).listViewMain.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Do you really want to delete the \"" + ((FormChild)this.ActiveMdiChild).listViewMain.SelectedItems[0].Text + "\" item?",
                                   "Confirm",
                                   MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    data.SendQueryWithoutReturn("DELETE FROM \"items\" WHERE id = " + ((FormChild)this.ActiveMdiChild).listViewMain.SelectedItems[0].Tag);
                    ((FormChild)this.ActiveMdiChild).InitializeItems();
                }
            }
            else
            {
                if (MessageBox.Show("Do you really want to delete \"" + this.ActiveMdiChild.Text + "\" group?",
                               "Confirm",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    data.SendQueryWithoutReturn("DELETE FROM \"groups\" WHERE id = " + this.ActiveMdiChild.Tag);
                    ((FormChild)this.ActiveMdiChild).Hide();
                }
            }
        }

        private void propertiesMenuItem_Click(object sender, EventArgs e)
        {
            if (((FormChild)this.ActiveMdiChild).listViewMain.SelectedItems.Count > 0)
            {
                using (FormCreateItem createform = new FormCreateItem(this.ActiveMdiChild.Tag.ToString(),
                    ((FormChild)this.ActiveMdiChild).listViewMain.SelectedItems[0].Tag.ToString()))
                {
                    if (createform.ShowDialog() == DialogResult.OK)
                    {
                        ((FormChild)this.ActiveMdiChild).InitializeItems();
                    }
                }
            }
            else
            {
                Form createForm = new FormCreateGroup(this.ActiveMdiChild.Tag.ToString());
                if (createForm.ShowDialog() == DialogResult.OK)
                {
                    CloseAllMDIWindows();
                    InitializeMDI();
                }
            }
        }

        private void executeMenuItem_Click(object sender, EventArgs e)
        {
            FormExecute ex = new FormExecute();
            ex.ShowDialog();
        }

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            Form formSettings = new FormSettings();
            formSettings.ShowDialog();
            if (formSettings.DialogResult == DialogResult.OK)
                InitializeTitle();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This program will be closed.", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                Environment.Exit(0);
            }
        }

        private void tileVerticalMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void tileHorizontalMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void cascadeMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout about = new FormAbout();
            about.ShowDialog();
        }

        private void FileMenu_Select(object sender, EventArgs e)
        {
            if (((FormChild)this.ActiveMdiChild) == null)
            {
                newItemMenuItem.Enabled = false;
                deleteItemMenuItem.Enabled = false;
                propertiesMenuItem.Enabled = false;
            }
            else
            {
                newItemMenuItem.Enabled = true;
                deleteItemMenuItem.Enabled = true;
                propertiesMenuItem.Enabled = true;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formSettings = new FormSettings();
            formSettings.ShowDialog();
            if(formSettings.DialogResult == DialogResult.OK)
            InitializeTitle();
        }

        private void convertFolderToGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialogCovnerter.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = folderBrowserDialogCovnerter.SelectedPath;
                string[] list = Directory.GetFiles(path, "*.lnk");
                string name = path.Replace(Path.GetDirectoryName(path) + Path.DirectorySeparatorChar, "");

                data.SendQueryWithoutReturn("INSERT INTO groups (id,name,status) VALUES (NULL,\"" + name + "\",1)");

                DataTable group = new DataTable();
                group = data.SendQueryWithReturn("SELECT * FROM groups WHERE name = \"" + name + "\";");

                foreach (string Link in list)
                {
                    string itemName = Path.GetFileNameWithoutExtension(Link);
                    data.SendQueryWithoutReturn("INSERT INTO \"items\"(id,name,path,icon,groups) VALUES (NULL,'" + itemName + "','" + Link + "','" + Link + "','" + group.Rows[0][0].ToString() + "');");
                }

                CloseAllMDIWindows();
                InitializeMDI();
            }
        }
    }
}
