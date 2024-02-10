using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using MinimizeToIcon;

namespace ProgramManagerVC
{
    public partial class FormChild : MinimizableForm
    {
        public FormChild() : base(64)
        {
            InitializeComponent();
        }

        private void FormChild_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        private void FormChild_Load(object sender, EventArgs e)
        {
            InitializeItems();
            if (System.Environment.OSVersion.Version.Major < 6) {
                runAsAdministratorToolStripMenuItem.Visible = false;
            } else {
                runAsAdministratorToolStripMenuItem.Image = SystemIcons.Shield.ToBitmap();
            }
        }

        private void ListViewMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Process.Start(listViewMain.SelectedItems[0].ToolTipText.ToString());
        }

        public void InitializeItems()
        {
            listViewMain.Items.Clear();
            imageListIcons.Images.Clear();
            DataTable items = new DataTable();
            items = data.SendQueryWithReturn("SELECT * FROM items WHERE groups = " + this.Tag);
            if (items.Rows.Count > 0)
            {
                for (int i = 0; i < items.Rows.Count; i++)
                {
                    try
                    {
                        imageListIcons.Images.Add(Icon.ExtractAssociatedIcon(items.Rows[i][3].ToString()).ToBitmap());
                        ListViewItem item = new ListViewItem();
                        item.Text = items.Rows[i][1].ToString();
                        item.ImageIndex = i;
                        item.ToolTipText = items.Rows[i][2].ToString();
                        item.Tag = items.Rows[i][0].ToString();
                        listViewMain.Items.Add(item);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("File \"" + items.Rows[i][3].ToString() + "\" cannot be found. Icon will be deleted.", 
                            "Error", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);

                    }
                }
            }
        }

        private void FormChild_ResizeEnd(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                data.SendQueryWithoutReturn("UPDATE groups SET status=0 WHERE id="+this.Tag);
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                data.SendQueryWithoutReturn("UPDATE groups SET status=1 WHERE id=" + this.Tag);
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                data.SendQueryWithoutReturn("UPDATE groups SET status=2 WHERE id=" + this.Tag);
            }
        }

        private void listViewMain_MouseDown(object sender, MouseEventArgs e) 
        {
            if (e.Button == MouseButtons.Right) 
            {
                if (listViewMain.FocusedItem == null)
                {
                    ListMenu.Show(Cursor.Position);
                }
                else if (listViewMain.FocusedItem.Bounds.Contains(e.Location)) 
                {
                    FileMenu.Show(Cursor.Position);
                } 
                else 
                {
                    ListMenu.Show(Cursor.Position);
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            Process.Start(listViewMain.SelectedItems[0].ToolTipText.ToString());
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            Process.Start(new ProcessStartInfo ("explorer.exe", "/select, " + listViewMain.SelectedItems[0].ToolTipText.ToString()));
        }

        private void runAsAdministratorToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            if (System.Environment.OSVersion.Version.Major >= 6) 
            {
                try 
                {
                    Process proc = new Process();
                    proc.StartInfo.FileName = listViewMain.SelectedItems[0].ToolTipText.ToString();
                    proc.StartInfo.UseShellExecute = true;
                    proc.StartInfo.Verb = "runas";
                    proc.Start();
                }
                catch 
                { }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            if (MessageBox.Show("Do you really want to delete the \"" + listViewMain.SelectedItems[0].Text + "\" item?",
                                   "Confirm",
                                   MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Question) == DialogResult.Yes) 
            {
                data.SendQueryWithoutReturn("DELETE FROM \"items\" WHERE id = " + listViewMain.SelectedItems[0].Tag);
                this.InitializeItems();
            }
        }

        private void newItemToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            using (FormCreateItem createform = new FormCreateItem(this.Tag.ToString())) 
            {
                if (createform.ShowDialog() == DialogResult.OK) 
                {
                    this.InitializeItems();
                }
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
                using (FormCreateItem createform = new FormCreateItem(this.Tag.ToString(),
                    this.listViewMain.SelectedItems[0].Tag.ToString()))
                {
                    if (createform.ShowDialog() == DialogResult.OK)
                    {
                        InitializeItems();
                    }
                }
           
        }

        private void propertiesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form createForm = new FormCreateGroup(this.Tag.ToString());
            if (createForm.ShowDialog() == DialogResult.OK)
            {
                DataTable db = new DataTable();
                db = data.SendQueryWithReturn("SELECT * FROM groups WHERE id = '" + this.Tag.ToString() + "';");
                this.Text = db.Rows[0][1].ToString();
                InitializeItems();
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete \"" + this.Text + "\" group?",
                               "Confirm",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question) == DialogResult.Yes)
            {
                data.SendQueryWithoutReturn("DELETE FROM \"groups\" WHERE id = " + this.Tag);
                this.Hide();
                this.DestroyHandle();
            }
        }
    }
}
