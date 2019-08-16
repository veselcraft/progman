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
    public partial class FormChild : Form
    {
        public FormChild()
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
                    catch (Exception ex)
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
    }
}
