using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace sysprogramming
{
    public partial class Form1 : Form
    {
        private TreeView treeView1;
        private ListView listView1;
        private Button btnRename, btnDelete, btnCreateFolder, btnCopy, btnSearch;
        public Form1()
        {
            InitializeComponent();
            LoadDrives();
        }

        private void LoadDrives()
        {
            treeView1.Nodes.Clear();
            foreach (string drive in Directory.GetLogicalDrives())
            {
                TreeNode node = new TreeNode(drive) { Tag = drive };
                treeView1.Nodes.Add(node);
                LoadSubDirectories(node);
            }
        }

        private void LoadSubDirectories(TreeNode node, int depth = 2)
        {
            if (depth == 0) return;

            string path = node.Tag.ToString();
            try
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    TreeNode subNode = new TreeNode(Path.GetFileName(dir)) { Tag = dir };
                    node.Nodes.Add(subNode);
                    LoadSubDirectories(subNode, depth - 1);
                }
            }
            catch { }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listView1.Items.Clear();
            string path = e.Node.Tag.ToString();
            try
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    ListViewItem item = new ListViewItem(Path.GetFileName(dir));
                    item.SubItems.Add("<DIR>");
                    listView1.Items.Add(item);
                }
                foreach (var file in Directory.GetFiles(path))
                {
                    FileInfo fi = new FileInfo(file);
                    ListViewItem item = new ListViewItem(fi.Name);
                    item.SubItems.Add(fi.Length.ToString());
                    listView1.Items.Add(item);
                }
            }
            catch { }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string fileName = ShowInputDialog("Enter file name to search:");
            if (string.IsNullOrEmpty(fileName)) return;

            listView1.Items.Clear();
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("Select a folder to start search");
                return;
            }
            string startPath = treeView1.SelectedNode.Tag.ToString();
            SearchFilesRecursive(startPath, fileName);
        }

        private void SearchFilesRecursive(string path, string fileName)
        {
            try
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    if (Path.GetFileName(file).IndexOf(fileName, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        FileInfo fi = new FileInfo(file);
                        ListViewItem item = new ListViewItem(fi.FullName);
                        item.SubItems.Add(fi.Length.ToString());
                        listView1.Items.Add(item);
                    }
                }

                foreach (var dir in Directory.GetDirectories(path))
                {
                    SearchFilesRecursive(dir, fileName);
                }
            }
            catch (UnauthorizedAccessException) { }
            catch (IOException) { }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "...")
            {
                e.Node.Nodes.Clear();
                string path = e.Node.Tag.ToString();
                try
                {
                    foreach (var dir in Directory.GetDirectories(path))
                    {
                        TreeNode subNode = new TreeNode(Path.GetFileName(dir)) { Tag = dir };
                        if (Directory.GetDirectories(dir).Length > 0)
                            subNode.Nodes.Add("...");
                        e.Node.Nodes.Add(subNode);
                    }
                }
                catch { }
            }
        }

        private void btnCreateFolder_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            string path = treeView1.SelectedNode.Tag.ToString();
            string newFolder = Path.Combine(path, "NewFolder");
            try
            {
                Directory.CreateDirectory(newFolder);
                treeView1.SelectedNode.Nodes.Add(new TreeNode("NewFolder") { Tag = newFolder });
                treeView1.SelectedNode.Expand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            string path = treeView1.SelectedNode.Tag.ToString();
            try
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                else if (File.Exists(path))
                    File.Delete(path);
                treeView1.SelectedNode.Remove();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            string oldPath = treeView1.SelectedNode.Tag.ToString();
            string parent = Path.GetDirectoryName(oldPath);
            string newName = ShowInputDialog("Enter new name:");
            if (string.IsNullOrEmpty(newName)) return;
            string newPath = Path.Combine(parent, newName);
            try
            {
                if (Directory.Exists(oldPath))
                    Directory.Move(oldPath, newPath);
                else if (File.Exists(oldPath))
                    File.Move(oldPath, newPath);
                treeView1.SelectedNode.Text = newName;
                treeView1.SelectedNode.Tag = newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            string source = treeView1.SelectedNode.Tag.ToString();
            string dest = ShowInputDialog("Enter destination path:");
            if (string.IsNullOrEmpty(dest)) return;
            if (!Path.IsPathRooted(dest))
                dest = Path.Combine(@"C:\", dest);
            try
            {
                if (Directory.Exists(source))
                    CopyDirectory(source, dest);
                else if (File.Exists(source))
                    File.Copy(source, dest, true);
                MessageBox.Show("Copied!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CopyDirectory(string sourceDir, string destDir)
        {
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(dir));
                CopyDirectory(dir, destSubDir);
            }
        }

        private string ShowInputDialog(string prompt)
        {
            Form promptForm = new Form()
            {
                Width = 400,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Input",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = prompt, AutoSize = true };
            TextBox inputBox = new TextBox() { Left = 20, Top = 50, Width = 340 };
            Button confirmation = new Button() { Text = "OK", Left = 280, Width = 80, Top = 80, DialogResult = DialogResult.OK };
            promptForm.Controls.Add(textLabel);
            promptForm.Controls.Add(inputBox);
            promptForm.Controls.Add(confirmation);
            promptForm.AcceptButton = confirmation;
            return promptForm.ShowDialog() == DialogResult.OK ? inputBox.Text : "";
        }
    }
}