using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Omicron_Pi
{
    public partial class createNewConfigButton : Form
    {
        private int newGroupNo = 1;
        public string configFilePath;
        public string[] configFile;
        public event EventHandler configSelected;
        public createNewConfigButton()
        {
            InitializeComponent();
            openFileDialog1.FileOk += OpenFileDialog1_FileOk;
            configSelected += PopulateConfigOptions;
        }

        private void PopulateConfigOptions(object sender, EventArgs e)
        {
            // Tick Box Options
            staffAccessCheckBox.Checked      = configFile.First(i => i.Contains("enable_staff_access")).Contains("true")               ? true : false;
            managerAccessCheckBox.Checked    = configFile.First(i => i.Contains("enable_manager_access")).Contains("true")             ? true : false;
            banTeamRACheckBox.Checked        = configFile.First(i => i.Contains("enable_banteam_access")).Contains("true")             ? true : false;
            banTeamSlotCheckBox.Checked      = configFile.First(i => i.Contains("enable_banteam_reserved_slots")).Contains("true")     ? true : false;
            banTeamBypassGeoCheckBox.Checked = configFile.First(i => i.Contains("enable_banteam_bypass_geoblocking")).Contains("true") ? true : false;

            //Define Groups
            int rolesPos = Array.FindIndex(configFile, i => i.Contains("Roles")) + 1;
            for(; ; )
            {
                if (!configFile[rolesPos].Contains("-")) break;
                Groups.Items.Add(configFile[rolesPos].Remove(0, 3));
                rolesPos++;
            }
        }

        private void Groups_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.Hide();
        }

        private void GroupsRemoveButton_Click(object sender, EventArgs e)
        {
            if(Groups.Items.Count == 1)
            {
                MessageBox.Show("You can't have less than 1 group");
                return;
            }
            int newIndex = Groups.SelectedIndex - 1 == -1 ? Groups.SelectedIndex : Groups.SelectedIndex - 1;
            Groups.Items.Remove(Groups.SelectedItem);
            Groups.SelectedIndex = newIndex;
        }

        private void GroupsUpButton_Click(object sender, EventArgs e)
        {
            object item = Groups.Items[Groups.SelectedIndex];
            int newIndex = Groups.SelectedIndex - 1;
            Groups.Items.RemoveAt(Groups.SelectedIndex);
            Groups.Items.Insert(newIndex, item);
            Groups.SelectedItem = Groups.Items[newIndex];
        }
        private void GroupsDownButton_Click(object sender, EventArgs e)
        {
            object item = Groups.Items[Groups.SelectedIndex];
            int newIndex = Groups.SelectedIndex + 1;
            Groups.Items.RemoveAt(Groups.SelectedIndex);
            Groups.Items.Insert(newIndex, item);
            Groups.SelectedItem = Groups.Items[newIndex];
        }

        private void GroupsAddButton_Click(object sender, EventArgs e)
        {
            Groups.BeginUpdate();
            var newItem = Groups.Items.Add($"New Group{newGroupNo}");
            newGroupNo++;
            Groups.EndUpdate();
            Groups.SelectedItem = Groups.Items[newItem];
        }
        private void Button1_Click(object sender, EventArgs e) => openFileDialog1.ShowDialog();

        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (openFileDialog1.FileName.Contains("config_remoteadmin.txt"))
            {
                configFile = File.ReadAllLines(openFileDialog1.FileName);
                configFilePath = openFileDialog1.FileName;
                EventHandler handler = configSelected;
                handler?.Invoke(this, e);
                groupBox1.Hide();
                tabControl1.Show();
                return;
            }
            MessageBox.Show("Please selecet an existing config_remoteadmin.txt");
        }

        private void Label8_Click(object sender, EventArgs e)
        {

        }

        private void TabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
