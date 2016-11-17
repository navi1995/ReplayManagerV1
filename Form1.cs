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
        private FolderBrowserDialog folderBrowserDialog1;
        

        private MainMenu mainMenu1;
        private MenuItem fileMenuItem, leagueDirectoryMenuItem, replayDirectoryMenuItem;

        private string leagueDirectory = @"D:\Riot Games\League of Legends\RADS\solutions\lol_game_client_sln\releases\0.0.1.152\deploy\League of Legends.exe";
        private string replayDirectory = @"D:\Users\Navi Jador\Documents\League of Legends\Replays\";

        private bool fileOpened = false;

        public Form1()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.leagueDirectoryMenuItem = new System.Windows.Forms.MenuItem();
            this.replayDirectoryMenuItem = new System.Windows.Forms.MenuItem();
            
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();

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
            this.folderBrowserDialog1.Description =
                "Select the directory that you want to use as the default.";

            // Do not allow the user to create new files via the FolderBrowserDialog.
            this.folderBrowserDialog1.ShowNewFolderButton = false;

            // Default to the My Documents folder.

            this.ClientSize = new System.Drawing.Size(296, 360);
           //this.Controls.Add(this.richTextBox1);
            this.Menu = this.mainMenu1;
            this.Text = "RTF Document Browser";
            InitializeComponent();

            DirectoryInfo dinfo = new DirectoryInfo(Path.GetDirectoryName(replayDirectory));
            FileInfo[] Files = dinfo.GetFiles("*.rofl");

            listView1.Columns.Add("File Name", -2);
            listView1.Columns.Add("Date Created", -2);

            foreach (FileInfo file in Files)
            {
                string[] details = new string[2];
                details[0] = file.Name; details[1] = file.CreationTime.ToString("HH:mm dd/MM/yyyy");
                ListViewItem item = new ListViewItem(details);
                listView1.Items.Add(item);
            }

            listView1.MouseDoubleClick += new MouseEventHandler(listView1_MouseDoubleClick);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            if (item != null)
            {
                Process process = new Process();
                string leaguePath = @"D:\Riot Games\League of Legends\RADS\solutions\lol_game_client_sln\releases\0.0.1.152\deploy\League of Legends.exe";
                process.StartInfo.FileName = "League of Legends.exe";
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(leaguePath);
                process.StartInfo.Arguments = "\"League Of Legends.exe\" \"D:\\Users\\Navi Jador\\Documents\\League of Legends\\Replays\\" + item.Text;
                process.Start();
            }

        }

        // Bring up a dialog to open a file.
        private void leagueDirectoryMenuItem_Click(object sender, System.EventArgs e)
        {
            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                leagueDirectory = folderBrowserDialog1.SelectedPath;
                if (!fileOpened)
                {
                    leagueDirectoryMenuItem.PerformClick();
                }
            }
        }

        // Bring up a dialog to chose a folder path in which to open or save a file.
        private void replayDirectoryMenuItem_Click(object sender, System.EventArgs e)
        {
            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                replayDirectory = folderBrowserDialog1.SelectedPath;
                if (!fileOpened)
                {
                    leagueDirectoryMenuItem.PerformClick();
                }
            }
        }
    }
}
