using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sysprogramming
{
    public partial class Form1 : Form
    {
        private TreeView treeView1;
        private Button btnShow;
        private Button btnExit;
        public Form1()
        {
            InitializeControls();
        }


        private void InitializeControls()
        {
            this.Text = "Динамічна ідентифікація типів";
            this.Size = new System.Drawing.Size(600, 400);

            treeView1 = new TreeView();
            treeView1.Dock = DockStyle.Top;
            treeView1.Height = 300;
            this.Controls.Add(treeView1);

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 50;
            this.Controls.Add(panel);

            btnShow = new Button();
            btnShow.Text = "Показати властивості";
            btnShow.Click += BtnShow_Click;
            panel.Controls.Add(btnShow);

            btnExit = new Button();
            btnExit.Text = "Вихід";
            btnExit.Click += (s, e) => this.Close();
            panel.Controls.Add(btnExit);
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            Doctor doc = new Doctor("Олександр", 35, "Терапевт", new List<string> { "Хірургія", "Терапія" });
            ShowProperties(doc);
        }

        private void ShowProperties(object obj)
        {
            Type t = obj.GetType();
            TreeNode root = new TreeNode(t.Name);
            PropertyInfo[] props = t.GetProperties();

            foreach (var p in props)
            {
                object value = p.GetValue(obj, null);
                string display = value is System.Collections.IEnumerable && !(value is string)
                    ? string.Join(", ", (System.Collections.IEnumerable)value)
                    : value?.ToString();
                root.Nodes.Add($"{p.PropertyType.Name} {p.Name} = {display}");
            }

            TreeNode methodsNode = new TreeNode("Методи");
            MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var m in methods)
            {
                string signature = m.Name + "(";
                ParameterInfo[] pars = m.GetParameters();
                for (int i = 0; i < pars.Length; i++)
                {
                    signature += $"{pars[i].ParameterType.Name} {pars[i].Name}";
                    if (i < pars.Length - 1) signature += ", ";
                }
                signature += $") : {m.ReturnType.Name}";
                methodsNode.Nodes.Add(signature);
            }
            root.Nodes.Add(methodsNode);

            TreeNode ctorsNode = new TreeNode("Конструктори");
            ConstructorInfo[] ctors = t.GetConstructors();
            foreach (var c in ctors)
            {
                string signature = t.Name + "(";
                ParameterInfo[] pars = c.GetParameters();
                for (int i = 0; i < pars.Length; i++)
                {
                    signature += $"{pars[i].ParameterType.Name} {pars[i].Name}";
                    if (i < pars.Length - 1) signature += ", ";
                }
                signature += ")";
                ctorsNode.Nodes.Add(signature);
            }
            root.Nodes.Add(ctorsNode);

            treeView1.Nodes.Add(root);
            root.Expand();
        }
  
    }


    public class Doctor
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Specialty { get; set; }       
        public List<string> Skills { get; set; } 
        public Doctor() { }

        public Doctor(string name, int age, string specialty, List<string> skills)
        {
            Name = name;
            Age = age;
            Specialty = specialty;
            Skills = skills;
        }

        public void ShowInfo()
        {
            MessageBox.Show($"{Name}, {Age} років, спеціальність: {Specialty}");
        }

        public int AddExperience(int years)
        {
            Age += years;
            return Age;
        }

        public string GetSummary()
        {
            return $"{Name}, {Age} років, {Specialty}, навички: {string.Join(", ", Skills)}";
        }
    }

}