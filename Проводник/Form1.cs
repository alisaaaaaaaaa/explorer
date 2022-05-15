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
            string[] drivesArray = Directory.GetLogicalDrives();
            foreach (string s in drivesArray)
                Console.Write("{0} ", s);
            DirectoryInfo[] diArray;
            string fullPath = "C:\\";

            DirectoryInfo di = new DirectoryInfo(fullPath);

            try
            {
                diArray = di.GetDirectories();
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
            DriveTreeInit();
        }
        string fullPath;
        public void DriveTreeInit()
        {
            string[] drivesArray = Directory.GetLogicalDrives();

            FileTree.BeginUpdate();
            FileTree.Nodes.Clear();

            foreach (string s in drivesArray)
            {
                TreeNode drive = new TreeNode(s, 0, 0);
                FileTree.Nodes.Add(drive);

                GetDirs(drive);
            }


            FileTree.EndUpdate();
        }
        public void GetDirs(TreeNode node)
        {
            DirectoryInfo[] diArray;

            node.Nodes.Clear();

            string fullPath = node.FullPath;
            DirectoryInfo di = new DirectoryInfo(fullPath);

            try
            {
                diArray = di.GetDirectories();
            }
            catch
            {
                return;
            }

            foreach (DirectoryInfo dirinfo in diArray)
            {
                TreeNode dir = new TreeNode(dirinfo.Name, 0, 0);
                node.Nodes.Add(dir);
            }
        }

        private void FileTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            FileTree.BeginUpdate();

            foreach (TreeNode node in e.Node.Nodes)
            {
                GetDirs(node);
            }

            FileTree.EndUpdate();
        }

        private void FileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            fullPath = selectedNode.FullPath;

            DirectoryInfo di = new DirectoryInfo(fullPath);
            FileInfo[] fiArray;
            DirectoryInfo[] diArray;

            try
            {
                fiArray = di.GetFiles();
                diArray = di.GetDirectories();
            }
            catch
            {
                return;
            }

            listView.Items.Clear();

            foreach (DirectoryInfo dirInfo in diArray)
            {
                ListViewItem lvi = new ListViewItem(dirInfo.Name);
                lvi.SubItems.Add("0");
                lvi.SubItems.Add(dirInfo.LastWriteTime.ToString());
                lvi.ImageIndex = 0;

                listView.Items.Add(lvi);
            }


            foreach (FileInfo fileInfo in fiArray)
            {
                ListViewItem lvi = new ListViewItem(fileInfo.Name);
                lvi.SubItems.Add(fileInfo.Length.ToString());
                lvi.SubItems.Add(fileInfo.LastWriteTime.ToString());

                string filenameExtension =
                  Path.GetExtension(fileInfo.Name).ToLower();

                switch (filenameExtension)
                {
                    case ".com":
                        {
                            lvi.ImageIndex = 2;
                            break;
                        }
                    case ".exe":
                        {
                            lvi.ImageIndex = 2;
                            break;
                        }
                    case ".hlp":
                        {
                            lvi.ImageIndex = 3;
                            break;
                        }
                    case ".txt":
                        {
                            lvi.ImageIndex = 4;
                            break;
                        }
                    case ".doc":
                        {
                            lvi.ImageIndex = 5;
                            break;
                        }
                    default:
                        {
                            lvi.ImageIndex = 1;
                            break;
                        }
                }

                listView.Items.Add(lvi);
            }
        }

        private void listView_ItemActivate(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView.SelectedItems)
            {
                string ext = Path.GetExtension(lvi.Text).ToLower();
                if (ext == ".txt" || ext == ".htm" || ext == ".html" || ext == ".docx")
                {
                    try
                    {
                        showfileBox.LoadFile(Path.Combine(fullPath, lvi.Text),
                          RichTextBoxStreamType.PlainText);

                        selectedfileSB.Text = lvi.Text;
                    }
                    catch
                    {
                        return;
                    }
                }
                else if (ext == ".rtf")
                {
                    try
                    {
                        showfileBox.LoadFile(Path.Combine(fullPath, lvi.Text),
                          RichTextBoxStreamType.RichText);

                        selectedfileSB.Text = lvi.Text;
                    }
                    catch
                    {
                        return;
                    }
                }
            }
        }

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            FileTree.BackColor = colorDialog.Color;
            listView.BackColor = colorDialog.Color;
            showfileBox.BackColor = colorDialog.Color;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count < 1)
                return;
            string path;
            foreach (ListViewItem item in listView.SelectedItems)
            {
                path = path_txt.Text + item.SubItems[0].Text;
                if (Directory.Exists(path))
                {
                    try
                    {
                        Directory.Delete(path);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"Не получается удалить эту папку: {item.SubItems[0].Text}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"Не получается удалить эту папку: {item.SubItems[0].Text}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void размерШрифтаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog.ShowDialog();
            showfileBox.Font = fontDialog.Font;
        }

    }
}

