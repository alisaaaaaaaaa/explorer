using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Проводник
{
    public partial class Explorer : Form
    {
        public Explorer()
        {
            InitializeComponent();
            var rootC = new TreeNode() { Text = "C:", Tag = "c:\\" };
            FileTree.Nodes.Add(rootC);
            Build(rootC);
            rootC.Expand();
            var rootD = new TreeNode() { Text = "D:", Tag = "d:\\" };
            FileTree.Nodes.Add(rootD);
            Build(rootD);
            rootD.Expand();
            listView.SmallImageList = imageList;
            findtb.Text = @"C:\Users\Е\Desktop";
        }
        private void Build(TreeNode parent)
        {
            var str = parent.Tag as string;
            parent.Nodes.Clear();
            try
            {
                foreach (var dir in Directory.GetDirectories(str))
                {
                    parent.Nodes.Add(new TreeNode(Path.GetFileName(dir), new[] { new TreeNode("...") }) { Tag = dir });
                }
                foreach(var file in Directory.GetFiles(str))
                {
                    parent.Nodes.Add(new TreeNode(Path.GetFileName(file), 1, 1) { Tag = file });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FileTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            Build(e.Node);
        }

        private void findbutton_Click(object sender, EventArgs e)
        {
            string path = findtb.Text;
            string[] files = Directory.GetFiles(path);
            foreach(string file in files)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = file.Remove(0, file.LastIndexOf('\\') + 1);
                lvi.ImageIndex = 0;
                listView.Items.Add(lvi);
            }
        }
    }
}
