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
    public partial class FormCreateItem : Form
    {
        string id_group;
        string id_item;
        public FormCreateItem(string id, string iditem = "0")
        {
            InitializeComponent();
            id_group = id;
            id_item = iditem;
        }

        private void ButtonBrowser_Click(object sender, EventArgs e)
        {
            if(openFileDialogPath.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = openFileDialogPath.InitialDirectory + openFileDialogPath.FileName;
            }
        }

        private void FormCreateItem_Load(object sender, EventArgs e)
        {
            if (id_item != "0")
            {
                DataTable dTable = new DataTable();
                dTable = data.SendQueryWithReturn("SELECT * FROM items WHERE id = " + id_item);
                textBoxName.Text = dTable.Rows[0][1].ToString();
                textBoxPath.Text = dTable.Rows[0][2].ToString();
                textBoxWdir.Text = dTable.Rows[0][3].ToString();
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (id_item == "0")
            {
                data.SendQueryWithoutReturn("INSERT INTO \"items\"(id,name,path,workingdir,icon,groups) VALUES (NULL,'" + textBoxName.Text + "','" + textBoxPath.Text + "','" + textBoxWdir.Text + "','" + textBoxPath.Text + "','" + id_group + "');");
            }
            else
            {
                data.SendQueryWithoutReturn("UPDATE items SET name = \"" + textBoxName.Text + "\", path = \"" + textBoxPath.Text + "\", workingdir = \"" + textBoxWdir.Text + "\", icon = \"" + textBoxPath.Text + "\" WHERE id = " + id_item);
            }
            this.Close();
        }

        private void CheckTextBoxes()
        {
            /* TJournal закрылся, поэтому оставлю ссылку на оригинал :D
             * https://t.me/temablog/337
             */
            if (!string.IsNullOrEmpty(textBoxName.Text) && !string.IsNullOrEmpty(textBoxPath.Text))
            {
                buttonOK.Enabled = true;
            }
            else
            {
                buttonOK.Enabled = false;
            }
        }

        private void TextBoxName_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void TextBoxPath_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void textBoxPath_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Any())
                textBoxPath.Text = files.First();
        }

        private void textBoxPath_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
