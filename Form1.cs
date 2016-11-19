using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReplayManagerv1
{
    public partial class Form1 : Form
    {
        private int counter = 0;
        private List<FileInfo> markedItems = new List<FileInfo>();
        private Dictionary<string, FileInfo> replayFiles = new Dictionary<string, FileInfo>();

        private string leagueDirectory = (string) Properties.Settings.Default["LeagueDirectory"];
        private string replayDirectory = (string)Properties.Settings.Default["ReplayDirectory"];

        public Form1()
        {

            this.ClientSize = new System.Drawing.Size(296, 360);
            this.Text = "League of Legends replay manager";
            InitializeComponent();
            listView1.OwnerDraw = true;
            listView1.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(listView1_DrawColumnHeader);
            listView1.DrawItem += new DrawListViewItemEventHandler(listView1_DrawItem);
            refreshReplays();
            listView1.MouseDoubleClick += new MouseEventHandler(listView1_MouseDoubleClick);
        }

        private void refreshReplays()
        {
            this.label3.Visible = false;
            try
            {
                string test = Path.GetFullPath(replayDirectory);
                DirectoryInfo dinfo = new DirectoryInfo(Path.GetFullPath(replayDirectory));
                FileInfo[] Files = dinfo.GetFiles("*.rofl");
                listView1.Clear();
                replayFiles.Clear();
                listView1.Columns.Add("Delete", 50);
                listView1.Columns.Add("File Name", 200);
                listView1.Columns.Add("Date Created", -2);

                foreach (FileInfo file in Files)
                {
                    replayFiles.Add(file.Name, file);
                    string[] details = new string[3];
                    details[0] = "  "; details[1] = file.Name; details[2] = file.CreationTime.ToString("HH:mm dd/MM/yyyy");
                    ListViewItem item = new ListViewItem(details);
                    listView1.Items.Add(item);
                }

                this.label1.Text = "Double click a row to launch replay!";

                if (Files.Length == 0)
                {
                    this.label3.Visible = true;
                }
            }
            catch (Exception e)
            {
                this.label3.Visible = true;
            }
        }

        private void listView1_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            ListViewItem current = listView1.Items[e.Index];
            FileInfo currentFile = replayFiles[current.SubItems[1].Text];

            if (e.NewValue == CheckState.Checked)
            {
                markedItems.Add(currentFile);
            } else if (e.NewValue == CheckState.Unchecked)
            {
                if (markedItems.Contains(currentFile))
                {
                    markedItems.Remove(currentFile);
                }
            }
        }

        private void listView1_DrawColumnHeader(object sender,
                                            DrawListViewColumnHeaderEventArgs e)
        {
            var brush = new SolidBrush(Color.FromArgb(255, (byte)30, (byte)35, (byte)40));
            var brush1 = new SolidBrush(Color.FromArgb(255, (byte)135, (byte)132, (byte)121));

            using (var headerFont = new Font("Microsoft Sans Serif", 9, FontStyle.Bold))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
                e.Graphics.DrawString(e.Header.Text, headerFont,
                    brush1, e.Bounds);
            }
        }

        private void listView1_DrawItem(object sender,
                                    DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            //ListViewItem item = info.Item;
            ListViewItem item = listView1.FocusedItem;
            FileInfo selectedItem = replayFiles[item.SubItems[1].Text];

            string test = "\"League Of Legends.exe\" \"" + selectedItem.FullName; 

            if (item != null)
            {
                Process process = new Process();
                string leaguePath = leagueDirectory;
                process.StartInfo.FileName = "League of Legends.exe";
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(leaguePath);
                process.StartInfo.Arguments = "\"League Of Legends.exe\" \"" + selectedItem.FullName;
                process.Start();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            refreshReplays();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string test = "";
            //int counter = 0;
            foreach (FileInfo file in markedItems)
            {
                file.Delete();
            }

            refreshReplays();
            //MessageBox.Show(test + counter);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SettingsPage modal = new SettingsPage();
            DialogResult result = modal.ShowDialog(this);

            if (result == DialogResult.Cancel)
            {
                modal.Close();
            } else if (result == DialogResult.OK)
            {
                if (modal.getVerified() && modal.isValid())
                {
                    leagueDirectory = (string)Properties.Settings.Default["LeagueDirectory"];
                    replayDirectory = (string)Properties.Settings.Default["ReplayDirectory"];
                    modal.Close();
                    refreshReplays();
                } else
                {
                    MessageBox.Show("Invalid directories chosen.");
                }
            }
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            refreshReplays();
        }
    }
}