namespace ProgramManagerVC
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));

            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.FileMenu = new System.Windows.Forms.MenuItem();
            this.newGroupMenuItem = new System.Windows.Forms.MenuItem();
            this.newItemMenuItem = new System.Windows.Forms.MenuItem();
            this.deleteItemMenuItem = new System.Windows.Forms.MenuItem();
            this.propertiesMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.executeMenuItem = new System.Windows.Forms.MenuItem();
            this.settingsMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.WindowsMenu = new System.Windows.Forms.MenuItem();
            this.tileVerticalMenuItem = new System.Windows.Forms.MenuItem();
            this.tileHorizontalMenuItem = new System.Windows.Forms.MenuItem();
            this.cascadeMenuItem = new System.Windows.Forms.MenuItem();
            this.HelpMenu = new System.Windows.Forms.MenuItem();
            this.aboutMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileMenu,
            this.WindowsMenu,
            this.HelpMenu});
            // 
            // FileMenu
            // 
            this.FileMenu.Index = 0;
            this.FileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.newGroupMenuItem,
            this.newItemMenuItem,
            this.deleteItemMenuItem,
            this.propertiesMenuItem,
            this.menuItem8,
            this.executeMenuItem,
            this.settingsMenuItem,
            this.menuItem11,
            this.exitMenuItem});
            this.FileMenu.Text = "File";
            this.FileMenu.Select += new System.EventHandler(this.FileMenu_Select);
            // 
            // newGroupMenuItem
            // 
            this.newGroupMenuItem.Index = 0;
            this.newGroupMenuItem.Text = "New Group...";
            this.newGroupMenuItem.Click += new System.EventHandler(this.newGroupMenuItem_Click);
            // 
            // newItemMenuItem
            // 
            this.newItemMenuItem.Enabled = false;
            this.newItemMenuItem.Index = 1;
            this.newItemMenuItem.Text = "New Item...";
            this.newItemMenuItem.Click += new System.EventHandler(this.newItemMenuItem_Click);
            // 
            // deleteItemMenuItem
            // 
            this.deleteItemMenuItem.Enabled = false;
            this.deleteItemMenuItem.Index = 2;
            this.deleteItemMenuItem.Text = "Delete";
            this.deleteItemMenuItem.Click += new System.EventHandler(this.deleteItemMenuItem_Click);
            // 
            // propertiesMenuItem
            // 
            this.propertiesMenuItem.Enabled = false;
            this.propertiesMenuItem.Index = 3;
            this.propertiesMenuItem.Text = "Properties";
            this.propertiesMenuItem.Click += new System.EventHandler(this.propertiesMenuItem_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 4;
            this.menuItem8.Text = "-";
            // 
            // executeMenuItem
            // 
            this.executeMenuItem.Index = 5;
            this.executeMenuItem.Text = "Execute...";
            this.executeMenuItem.Click += new System.EventHandler(this.executeMenuItem_Click);
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.Index = 6;
            this.settingsMenuItem.Text = "Settings";
            this.settingsMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 7;
            this.menuItem11.Text = "-";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 8;
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // WindowsMenu
            // 
            this.WindowsMenu.Index = 1;
            this.WindowsMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.tileVerticalMenuItem,
            this.tileHorizontalMenuItem,
            this.cascadeMenuItem,
            this.menuItem1});
            this.WindowsMenu.Text = "Windows";
            // 
            // tileVerticalMenuItem
            // 
            this.tileVerticalMenuItem.Index = 0;
            this.tileVerticalMenuItem.Text = "Tile Vertical";
            this.tileVerticalMenuItem.Click += new System.EventHandler(this.tileVerticalMenuItem_Click);
            // 
            // tileHorizontalMenuItem
            // 
            this.tileHorizontalMenuItem.Index = 1;
            this.tileHorizontalMenuItem.Text = "Tile Horizontal";
            this.tileHorizontalMenuItem.Click += new System.EventHandler(this.tileHorizontalMenuItem_Click);
            // 
            // cascadeMenuItem
            // 
            this.cascadeMenuItem.Index = 2;
            this.cascadeMenuItem.Text = "Cascade";
            this.cascadeMenuItem.Click += new System.EventHandler(this.cascadeMenuItem_Click);
            // 
            // HelpMenu
            // 
            this.HelpMenu.Index = 2;
            this.HelpMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.aboutMenuItem});
            this.HelpMenu.Text = "Help";
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Index = 0;
            this.aboutMenuItem.Text = "About";
            this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "-";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(199, 6);
            // 
            // convertFolderToGroupToolStripMenuItem
            // 
            this.convertFolderToGroupToolStripMenuItem.Name = "convertFolderToGroupToolStripMenuItem";
            this.convertFolderToGroupToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.convertFolderToGroupToolStripMenuItem.Text = "Convert Folder to Group";
            this.convertFolderToGroupToolStripMenuItem.Click += new System.EventHandler(this.convertFolderToGroupToolStripMenuItem_Click);
            // 
            // folderBrowserDialogCovnerter
            // 
            this.folderBrowserDialogCovnerter.Description = "Select folder you want to convert to group";
            this.folderBrowserDialogCovnerter.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(733, 450);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(180, 96);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Program Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem FileMenu;
        private System.Windows.Forms.MenuItem WindowsMenu;
        private System.Windows.Forms.MenuItem HelpMenu;
        private System.Windows.Forms.MenuItem newGroupMenuItem;
        private System.Windows.Forms.MenuItem newItemMenuItem;
        private System.Windows.Forms.MenuItem deleteItemMenuItem;
        private System.Windows.Forms.MenuItem propertiesMenuItem;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem executeMenuItem;
        private System.Windows.Forms.MenuItem settingsMenuItem;
        private System.Windows.Forms.MenuItem menuItem11;
        private System.Windows.Forms.MenuItem exitMenuItem;
        private System.Windows.Forms.MenuItem tileVerticalMenuItem;
        private System.Windows.Forms.MenuItem tileHorizontalMenuItem;
        private System.Windows.Forms.MenuItem cascadeMenuItem;
        private System.Windows.Forms.MenuItem aboutMenuItem;
        private System.Windows.Forms.MenuItem menuItem1;
    }
}

