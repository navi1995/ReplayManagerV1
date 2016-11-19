using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReplayManagerv1
{
    public partial class SettingsPage : Form
    {
        private string leagueDirectory = (string)Properties.Settings.Default["LeagueDirectory"];
        private string replayDirectory = (string)Properties.Settings.Default["ReplayDirectory"];

        private OpenFileDialog leagueBrowserDialog;
        private FolderBrowserDialog replayBrowserDialog;
        private bool verified = true;

        public bool getVerified()
        {
            return verified;
        }
        public SettingsPage()
        {
            InitializeComponent();
            this.leagueBrowserDialog = new System.Windows.Forms.OpenFileDialog();
            this.replayBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox1.Text = leagueDirectory;
            this.textBox2.Text = replayDirectory;

            this.replayBrowserDialog.Description = "Select location of your replay (.rofl) files";
            this.replayBrowserDialog.SelectedPath = replayDirectory;
            this.replayBrowserDialog.ShowNewFolderButton = false;

            this.leagueBrowserDialog.Filter = "League of Legends.exe (*.exe)|League Of Legends.exe";
        }


        //Browse league
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = leagueBrowserDialog.ShowDialog();
            if (result == DialogResult.OK && Path.GetExtension(leagueBrowserDialog.FileName) == ".exe" && Path.GetFileName(leagueBrowserDialog.FileName) == "League of Legends.exe")
            {
                //Implement STRICT checks that it's lol_game_client
                leagueDirectory = leagueBrowserDialog.FileName;
                this.textBox1.Text = leagueDirectory;
                Properties.Settings.Default["LeagueDirectory"] = leagueDirectory;
                Properties.Settings.Default.Save();
            }
            else if (result != DialogResult.Cancel)
            {
                MessageBox.Show("Invalid file chosen, please find the League of Legends.exe file!");
                verified = false;
            }
        }

        //Browse replay
        private void button4_Click(object sender, EventArgs e)
        {
            // Show the FolderBrowserDialog.
            DialogResult result = replayBrowserDialog.ShowDialog();
            if (result == DialogResult.OK && Path.GetFileName(replayBrowserDialog.SelectedPath) == "Replays")
            {
                //Implement REPLAY directory checks
                replayDirectory = replayBrowserDialog.SelectedPath;
                this.textBox2.Text = replayDirectory;
                Properties.Settings.Default["ReplayDirectory"] = replayDirectory;
                Properties.Settings.Default.Save();
            }
            else if (result != DialogResult.Cancel)
            {
                MessageBox.Show("Invalid folder chosen, please choose the folder named \"Replays\"!");
                verified = false;
            }
        }

        //Save
        private void button1_Click(object sender, EventArgs e)
        {
            if (verified && isValid())
            {
                Properties.Settings.Default.Save();
                this.DialogResult = DialogResult.OK;
            } else
            {
                MessageBox.Show("Invalid directories input!");
            }
        }

        //Cancel
        private void button2_Click(object sender, EventArgs e)
        {

        }

        public bool isValid()
        {
            if (Path.GetFileName(this.textBox2.Text) == "Replays" && Path.GetExtension(this.textBox1.Text) == ".exe" 
                && Path.GetFileName(this.textBox1.Text) == "League of Legends.exe")
            {
                Properties.Settings.Default["LeagueDirectory"] = this.textBox1.Text;
                Properties.Settings.Default["ReplayDirectory"] = this.textBox2.Text;
                Properties.Settings.Default.Save();

                return true;
            } else
            {
                return false;
            }
        }
    }
}
