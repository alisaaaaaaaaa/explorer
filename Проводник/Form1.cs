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
            listView.SmallImageList = imageList;
            PopulatetreeView();
        }
        private void PopulatetreeView()
        {
            TreeNode rootNode;
            DirectoryInfo info = new DirectoryInfo(@"../..");
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDiretories(info.GetDirectories(), rootNode);
                FileTree.Nodes.Add(rootNode);
            }
        }
        private void GetDiretories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            try
            {
                foreach (DirectoryInfo subDir in subDirs)
                {
                    aNode = new TreeNode(subDir.Name, 0, 0);
                    aNode.Tag = subDir;
                    aNode.ImageKey = "folder";
                    subSubDirs = subDir.GetDirectories();
                    if(subSubDirs.Length != 0)
                    {
                        GetDiretories(subSubDirs, aNode);
                    }
                    nodeToAddTo.Nodes.Add(aNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FileTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            listView.Items.Clear();
            DirectoryInfo nodeDI = (DirectoryInfo)newSelected.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;
            foreach (DirectoryInfo dir in nodeDI.GetDirectories())
            {
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                { new ListViewItem.ListViewSubItem(item, "Directory"),
                  new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToShortDateString())};
                item.SubItems.AddRange(subItems);
                listView.Items.Add(item);
            }
            foreach(FileInfo file in nodeDI.GetFiles())
            {
                item = new ListViewItem(file.Name, 1);
                subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, "File"),
                    new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString())};
                item.SubItems.AddRange(subItems);
                listView.Items.Add(item);
            }
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

    }
}
