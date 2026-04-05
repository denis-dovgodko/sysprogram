using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sysprogramming
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Button btnStart = new Button() { Text = "Start", Top = 10, Left = 10 };
        Button btnCancel = new Button() { Text = "Cancel", Top = 40, Left = 10 };
        Label lblResult = new Label() { Text = "", Top = 70, Left = 10, Width = 200 };
        ProgressBar progressBar1 = new ProgressBar() { Top = 100, Left = 10, Width = 200 };
        CancellationTokenSource cts;
        private async void btnStart_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
            progressBar1.Value = 0;
            lblResult.Text = "Running...";

            var progress = new Progress<int>(value =>
            {
                progressBar1.Value = value;
                lblResult.Text = $"Progress: {value}%";
            });

            try
            {
                int result = await LongRunningTask(100, progress, cts.Token);
                lblResult.Text = $"Done! Result: {result}";
            }
            catch (OperationCanceledException)
            {
                lblResult.Text = "Canceled";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
        }

        private Task<int> LongRunningTask(int count, IProgress<int> progress, CancellationToken token)
        {
            return Task.Run(() =>
            {
                int sum = 0;
                for (int i = 1; i <= count; i++)
                {
                    token.ThrowIfCancellationRequested();
                    sum += i;
                    progress.Report(i);
                    Thread.Sleep(50);
                }
                return sum;
            }, token);
        }
    }
}
