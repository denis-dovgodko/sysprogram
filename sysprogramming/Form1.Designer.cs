namespace sysprogramming
{
    partial class Form1
    {
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listView1 = new System.Windows.Forms.ListView();
            this.btnCreateFolder = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Size = new System.Drawing.Size(250, 426);
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);

            this.listView1.Location = new System.Drawing.Point(268, 12);
            this.listView1.Size = new System.Drawing.Size(520, 426);
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.Columns.Add("Name", 300);
            this.listView1.Columns.Add("Size/Type", 100);
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
  
            this.btnCreateFolder.Location = new System.Drawing.Point(12, 444);
            this.btnCreateFolder.Size = new System.Drawing.Size(90, 30);
            this.btnCreateFolder.Text = "Create Folder";
            this.btnCreateFolder.Click += new System.EventHandler(this.btnCreateFolder_Click);

            this.btnDelete.Location = new System.Drawing.Point(108, 444);
            this.btnDelete.Size = new System.Drawing.Size(75, 30);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.btnRename.Location = new System.Drawing.Point(189, 444);
            this.btnRename.Size = new System.Drawing.Size(75, 30);
            this.btnRename.Text = "Rename";
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);

            this.btnCopy.Location = new System.Drawing.Point(270, 444);
            this.btnCopy.Size = new System.Drawing.Size(75, 30);
            this.btnCopy.Text = "Copy";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);

            this.btnSearch.Location = new System.Drawing.Point(361, 444);
            this.btnSearch.Size = new System.Drawing.Size(75, 30);
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            this.ClientSize = new System.Drawing.Size(800, 486);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnCreateFolder);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnSearch);
            this.Text = "File Manager";
            this.ResumeLayout(false);
        }
    }
}