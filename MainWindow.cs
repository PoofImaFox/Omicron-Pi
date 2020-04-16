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
using YamlDotNet.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Helpers;
using YamlDotNet.RepresentationModel;

namespace Omicron_Pi
{
    public partial class Form1 : Form
    {
        private int newGroupNo = 1;
        public string configFilePath;
        public string configFile;
        public string[] configFileArray;
        public event EventHandler configSelected;
        public List<Group> groupList = new List<Group>();

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.FileOk += OpenFileDialog1_FileOk;
            configSelected += PopulateConfigOptions;
        }

        private void PopulateConfigOptions(object sender, EventArgs e)
        {
            //var r = new StringReader(configFile);
            //var deserializer = new Deserializer();
            
            // var dict = deserializer.Deserialize<Dictionary<string, string>>(r);

            //MessageBox.Show(dict.Count().ToString());


            // Tick Box Options
            staffAccessCheckBox.Checked      = configFileArray.First(i => i.Contains("enable_staff_access")).Contains("true");
            managerAccessCheckBox.Checked    = configFileArray.First(i => i.Contains("enable_manager_access")).Contains("true");
            banTeamRACheckBox.Checked        = configFileArray.First(i => i.Contains("enable_banteam_access")).Contains("true");
            banTeamSlotCheckBox.Checked      = configFileArray.First(i => i.Contains("enable_banteam_reserved_slots")).Contains("true");
            banTeamBypassGeoCheckBox.Checked = configFileArray.First(i => i.Contains("enable_banteam_bypass_geoblocking")).Contains("true");

            //Define Groups
            int rolesPos = Array.FindIndex(configFileArray, i => i.Contains("Roles")) + 1;
            for(; ; )
            {
                if (!configFileArray[rolesPos].Contains("-")) break;
                var groupName = configFileArray[rolesPos].Remove(0, 3);
                Groups.Items.Add(groupName);
                groupList.Add(new Group
                {
                    name = groupName,
                    badge = "",
                    badgeColour = "",
                    badgeCover = false,
                    badgeHidden = false,
                    kickPower = "",
                    requiredKickPower = "",
                    KickingAndShortTermBanning = false,
                    BanningUpToDay = false,
                    LongTermBanning = false,
                    ForceclassSelf = false,
                    ForceclassToSpectator = false,
                    ForceclassWithoutRestrictions = false,
                    GivingItems = false,
                    WarheadEvents = false,
                    RespawnEvents = false,
                    RoundEvents = false,
                    SetGroup = false,
                    GameplayData = false,
                    Overwatch = false,
                    FacilityManagement = false,
                    PlayersManagement = false,
                    PermissionsManagement = false,
                    ServerConsoleCommands = false,
                    ViewHiddenBadges = false,
                    ServerConfigs = false,
                    Broadcasting = false,
                    PlayerSensitiveDataAccess = false,
                    Noclip = false,
                    AFKImmunity = false
                });
                rolesPos++;
            }

            foreach(var group in groupList)
            {
                group.badge = configFileArray.First(i => i.Contains(group.name + "_badge")).Split(':')[1].Remove(0, 1);
                group.badgeColour = configFileArray.First(i => i.Contains(group.name + "_color")).Split(':')[1].Remove(0, 1);
                group.badgeCover = bool.Parse(configFileArray.First(i => i.Contains(group.name + "_cover")).Split(':')[1].Remove(0, 1));
                group.kickPower = configFileArray.First(i => i.Contains(group.name + "_kick_power")).Split(':')[1].Remove(0, 1);
                group.requiredKickPower = configFileArray.First(i => i.Contains(group.name + "_required_kick_power")).Split(':')[1].Remove(0, 1);
                group.badgeHidden = bool.Parse(configFileArray.First(i => i.Contains(group.name + "_hidden")).Split(':')[1].Remove(0, 1));
            }
        }

        private void Groups_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedGroup = groupList.First(i => i.name == Groups.SelectedItem.ToString());
            badgeNameTextBox.Text = selectedGroup.badge;
            badgeCoverCheckBox.Checked = selectedGroup.badgeCover;
            kickPowerTextBox.Text = selectedGroup.kickPower;
            requiredKickPowerTextBox.Text = selectedGroup.requiredKickPower;
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
                configFileArray = File.ReadAllLines(openFileDialog1.FileName);
                configFile = File.ReadAllText(openFileDialog1.FileName);
                configFilePath = openFileDialog1.FileName;
                EventHandler handler = configSelected;
                handler?.Invoke(this, e);
                groupBox1.Hide();
                tabControl1.Show();
                return;
            }
            MessageBox.Show("Please selecet an existing config_remoteadmin.txt");
        }

        private void PictureBox1_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://github.com/takail/omicron-pi");
        private void PictureBox1_MouseEnter(object sender, EventArgs e) => Cursor = Cursors.Hand;
        private void PictureBox1_MouseLeave(object sender, EventArgs e) => Cursor = Cursors.Arrow;
    }
}