using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using data = ProgramManagerVC.data;

namespace ProgramManagerVC
{
    public partial class FormCreateGroup : Form
    {
        string id_group;
        public FormCreateGroup(string id = "0")
        {
            InitializeComponent();
            id_group = id;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (id_group == "0")
            {
                data.SendQueryWithoutReturn("INSERT INTO groups (id,name,status) VALUES (NULL,\"" + textBoxName.Text + "\",1)");
            }
            else
            {
                data.SendQueryWithoutReturn("UPDATE groups SET name = \"" + textBoxName.Text + "\" WHERE id = " + id_group);
            }
            this.Close();
        }

        private void FormCreateGroup_Load(object sender, EventArgs e)
        {
            if(id_group != "0")
            {
                DataTable dTable = new DataTable();
                dTable = data.SendQueryWithReturn("SELECT * FROM groups WHERE id = " + id_group);
                textBoxName.Text = dTable.Rows[0][1].ToString();
            }
        }

        private void TextBoxName_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = !string.IsNullOrEmpty(textBoxName.Text);
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && !string.IsNullOrEmpty(textBoxName.Text))
            {
                buttonOK.PerformClick();
            }
        }
    }
}
