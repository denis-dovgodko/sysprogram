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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();

            this.btnStart.Location = new System.Drawing.Point(30, 20);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 30);
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);

            this.btnCancel.Location = new System.Drawing.Point(150, 20);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.progressBar1.Location = new System.Drawing.Point(30, 90);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(220, 20);

            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(30, 130);
            this.lblResult.Name = "lblResult";
            this.lblResult.Text = "Result:";

            this.ClientSize = new System.Drawing.Size(284, 181);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "Async Task Example";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

