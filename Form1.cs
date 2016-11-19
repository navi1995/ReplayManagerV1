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
        private OpenFileDialog leagueBrowserDialog;
        private FolderBrowserDialog replayBrowserDialog;
        private MainMenu mainMenu1;
        private MenuItem fileMenuItem, leagueDirectoryMenuItem, replayDirectoryMenuItem;
        private List<FileInfo> markedItems = new List<FileInfo>();
        private Dictionary<string, FileInfo> replayFiles = new Dictionary<string, FileInfo>();

        private string leagueDirectory = (string) Properties.Settings.Default["LeagueDirectory"];
        private string replayDirectory = (string)Properties.Settings.Default["ReplayDirectory"];
        private string username = "navi1995";

        private bool fileOpened = false;

        public Form1()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.leagueDirectoryMenuItem = new System.Windows.Forms.MenuItem();
            this.replayDirectoryMenuItem = new System.Windows.Forms.MenuItem();

            this.leagueBrowserDialog = new System.Windows.Forms.OpenFileDialog();
            this.replayBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            this.mainMenu1.MenuItems.Add(this.fileMenuItem);
            this.fileMenuItem.MenuItems.AddRange(
                                new System.Windows.Forms.MenuItem[] {this.leagueDirectoryMenuItem,
                                                                 this.replayDirectoryMenuItem});
            this.fileMenuItem.Text = "Settings";

            this.leagueDirectoryMenuItem.Text = "Set League Directory...";
            this.leagueDirectoryMenuItem.Click += new System.EventHandler(this.leagueDirectoryMenuItem_Click);

            this.replayDirectoryMenuItem.Text = "Set Replay Directory...";
            this.replayDirectoryMenuItem.Click += new System.EventHandler(this.replayDirectoryMenuItem_Click);

            // Set the help text description for the FolderBrowserDialog.
            this.replayBrowserDialog.Description = "Select location of your replay (.rofl) files";
            this.replayBrowserDialog.SelectedPath = Environment.SpecialFolder.MyDocuments.ToString();
            // Do not allow the user to create new files via the FolderBrowserDialog.
            this.replayBrowserDialog.ShowNewFolderButton = false;
            this.leagueBrowserDialog.Filter = "League of Legends.exe (*.exe)|League Of Legends.exe";

            this.ClientSize = new System.Drawing.Size(296, 360);
            //this.Controls.Add(this.richTextBox1);
            this.Menu = this.mainMenu1;
            this.Text = "League of Legends replay manager";
            InitializeComponent();
            refreshReplays();
            listView1.MouseDoubleClick += new MouseEventHandler(listView1_MouseDoubleClick);
        }

        private void refreshReplays()
        {
            counter++;
            try
            {
                string test = Path.GetFullPath(replayDirectory);
                DirectoryInfo dinfo = new DirectoryInfo(Path.GetFullPath(replayDirectory));
                FileInfo[] Files = dinfo.GetFiles("*.rofl");
                listView1.Clear();
                replayFiles.Clear();
                listView1.Columns.Add("Delete", 50);
                listView1.Columns.Add("File Name", 200);
                listView1.Columns.Add("Date Created", 100);

                foreach (FileInfo file in Files)
                {
                    replayFiles.Add(file.Name, file);
                    string[] details = new string[3];
                    details[0] = "  "; details[1] = file.Name; details[2] = file.CreationTime.ToString("HH:mm dd/MM/yyyy");
                    ListViewItem item = new ListViewItem(details);
                    listView1.Items.Add(item);
                }
                this.label1.Text = "Double click a row to launch replay!";
            }
            catch (Exception e)
            {
                this.label1.Text = "No replay files (.rofl) found, perhaps check Replay directory settings?";
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

        // Bring up a dialog to open a file.
        private void leagueDirectoryMenuItem_Click(object sender, System.EventArgs e)
        {
            // Show the FolderBrowserDialog.
            DialogResult result = leagueBrowserDialog.ShowDialog();
            if (result == DialogResult.OK && Path.GetExtension(leagueBrowserDialog.FileName) == ".exe" && Path.GetFileName(leagueBrowserDialog.FileName) != "League of Legends")
            {
                //Implement STRICT checks that it's lol_game_client
                leagueDirectory = leagueBrowserDialog.FileName;
                Properties.Settings.Default["LeagueDirectory"] = leagueDirectory;
                Properties.Settings.Default.Save();
                refreshReplays();
            } else
            {
                MessageBox.Show("Invalid file chosen, please find the League of Legends.exe file!");
            }
        }

        // Bring up a dialog to chose a folder path in which to open or save a file.
        private void replayDirectoryMenuItem_Click(object sender, System.EventArgs e)
        {
            // Show the FolderBrowserDialog.
            DialogResult result = replayBrowserDialog.ShowDialog();
            if (result == DialogResult.OK && Path.GetFileName(replayBrowserDialog.SelectedPath) == "Replays")
            {
                //Implement REPLAY directory checks
                replayDirectory = replayBrowserDialog.SelectedPath;
                Properties.Settings.Default["ReplayDirectory"] = replayDirectory;
                Properties.Settings.Default.Save();
                refreshReplays();
            } else
            {
                MessageBox.Show("Invalid folder chosen, please choose the folder named \"Replays\"!");
            }
        }
    }
}