namespace sysprogramming
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btnShow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Size = new System.Drawing.Size(360, 400);
            this.btnShow.Location = new System.Drawing.Point(12, 420);
            this.btnShow.Size = new System.Drawing.Size(100, 30);
            this.btnShow.Text = "Show";
            this.btnShow.Click += new System.EventHandler(this.BtnShow_Click);
            this.ClientSize = new System.Drawing.Size(384, 461);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.btnShow);
            this.ResumeLayout(false);
        }
    }
}