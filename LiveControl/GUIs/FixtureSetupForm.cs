using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using Kevsoft.LiveControl.FixtureClasses;

namespace Kevsoft.LiveControl.GUIs
{
    /// <summary>
    /// Form used for patching fixtures
    /// </summary>
    public partial class FixtureSetupForm : Form
    {
        //Declare variable named groupTables of type Hashtable.
        private Hashtable groupTables;
        public FixtureSetupForm()
        {
            InitializeComponent();
            // Create the groupsTable HashTable array object.
            groupTables = new Hashtable(personalitiesListView.Columns.Count) { };
            //fills the Universe ComboBox
            FillUniverseComboBox();

        }
        /// <summary>
        /// fills the universeComboBox with 0 to max universes
        /// </summary>
        private void FillUniverseComboBox()
        {
            //Get the number of max univserses
            int maxUniverses = DmxOutput.MaxUniverses;
            //clear the combobox
            universeComboBox.Items.Clear();
            //loop though each universe until end 
            for (byte i = 0; i < maxUniverses; i++)
                //add the universe to combobox
                universeComboBox.Items.Add(i);

            //check if the combobox has some items
            if (universeComboBox.Items.Count > 0)
                //select the first item
                universeComboBox.SelectedItem
                    = universeComboBox.Items[0];
        }

        /// <summary>
        /// Set the grouping according to the column number.
        /// </summary>
        /// <param name="column"></param>
        private void SetGroups(int column)
        {
            // Clear the current grouping.
            personalitiesListView.Groups.Clear();

            //Retrieve the hash table corresponding to the column clicked.
            Hashtable groupsHashTable = (Hashtable)groupTables[column];

            //Copy the groups for the column from the groupsHastTable to an array.
            ListViewGroup[] groupsArray = new ListViewGroup[groupsHashTable.Count];

            groupsHashTable.Values.CopyTo(groupsArray, 0);

            //Sort the groups and add them to personalitiesListView.
            Array.Sort(groupsArray, new ListViewGroupSorter(personalitiesListView.Sorting));

            personalitiesListView.Groups.AddRange(groupsArray);

            //Iterate through the items in personalitiesListView, assigning each
            //one to the appropriate group.
            foreach (ListViewItem item in personalitiesListView.Items)
            {
                //Retrieve the subitem text corresponding to the column.
                string subItemText = item.SubItems[column].Text;

                //For the Title column, use only the first letter.
                if (column == 0)
                    subItemText = subItemText.Substring(0, 1);

                //Assign the item to the matching group.

                item.Group = (ListViewGroup)groupsHashTable[subItemText];
            }
        }




        private Hashtable CreateGroupsHashTable(int column)
        {

            //Create a Hashtable object.
            Hashtable groupsHashTable = new Hashtable();

            //Iterate through the items in personalitiesListView.

            foreach (ListViewItem item in personalitiesListView.Items)
            {
                //Retrieve the text value for the column.
                string subItemText = item.SubItems[column].Text;

                //Use the initial letter instead if it is the first column.

                if (column == 0)
                    subItemText = subItemText.Substring(0, 1);


                if (!groupsHashTable.Contains(subItemText))
                    groupsHashTable.Add(subItemText, new ListViewGroup(subItemText,
                        HorizontalAlignment.Left));
            }



            return groupsHashTable;
        }

        private void FixtureSetupForm_Load(object sender, EventArgs e)
        {
            //loads the personalities that are in the show
            LoadPersonalities();
        }

        /// <summary>
        /// Loads the patched fixtures into list listview
        /// </summary>
        /// <param name="universe">univse which to load</param>
        private void LoadPatchedFixtures(byte universe)
        {
            //clear the list first
            fixturesListView.Items.Clear();
            //loop though each patched fixture in the show
            foreach (Fixture fixture in LiveControl.Show.Instance.FixPatch.GetUniverse(universe))
            {
                //add to listview
                fixturesListView.Items.Add(new FixtureListViewItem(fixture));
            }
        }
        /// <summary>
        /// Loads the Personalities from the current show
        /// </summary>
        private void LoadPersonalities()
        {
            //get instance of show
            LiveControl.Show show = LiveControl.Show.Instance;

            //clear the current items
            personalitiesListView.Items.Clear();

            //loup though each Personality and add it to the list
            foreach (Personality personality in show.PersonalitiesBank)
            {
                personalitiesListView.Items.Add(new PersonalityListViewItem(personality));
            }

            // Create a HashTable for each column in personalitiesListView.
            for (int column = 0; column < personalitiesListView.Columns.Count; ++column)
                groupTables[column] = CreateGroupsHashTable(column);

            // Start with the CustomerName group.
            SetGroups(1);
        }

        private void Patchbutton_Click(object sender, EventArgs e)
        {
            //Patches selected personality
            PatchFixture();
        }
        /// <summary>
        /// Patches the selected Personality to next dmx channel
        /// </summary>
        private void PatchFixture()
        {
            //if no personality is selected
            if (personalitiesListView.SelectedItems.Count == 0)
            {
                //dispay a message to the user
                MessageBox.Show("No personality selected", "Patch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else//personality is selected
            {
                //get the personality instance
                Personality personality = (Personality)personalitiesListView.SelectedItems[0].Tag;
                Int16 dmxPatch = GetNextDMX();//set the dmxpatch
                //check see if the patch goes over 512 channels
                if ((dmxPatch + personality.ChannelsUsed) > 512)
                {
                    MessageBox.Show("Next patch will go over 512 channels, please try using a diffrent universe",
                                    "Patch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else//we have enough channels to patch
                {
                    //create the new fixture
                    Fixture fixture = new Fixture(personality, dmxPatch, (byte)universeComboBox.SelectedItem);
                    //add the fixture to the fixture patch
                    LiveControl.Show.Instance.FixPatch.Add(fixture);
                    //add to listview
                    fixturesListView.Items.Add(new FixtureListViewItem(fixture));
                }
            }
        }

        /// <summary>
        /// returns the next dmx value
        /// </summary>
        /// <returns></returns>
        private short GetNextDMX()
        {
            //if theres no current fixtures then first patch is 1
            if (fixturesListView.Items.Count == 0) return 1;
            //get the fixture last in the list
            Fixture f = (Fixture)fixturesListView.Items[fixturesListView.Items.Count - 1].Tag;
            //work out the end dmx channel
            return (Int16)(f.DMX_Patch + f.Personality.ChannelsUsed);
        }

        private void personalitiesListView_DoubleClick(object sender, EventArgs e)
        {
            //patches the selected personality
            PatchFixture();
        }

        private void fixturesListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            //set the fixtures new name
            ((Fixture)fixturesListView.Items[e.Item].Tag).Name = e.Label;
        }

        private void removePatchButton_Click(object sender, EventArgs e)
        {
            //remove a selected fixture from the patch
            removePatchFixture();
        }

        private void removePatchFixture()
        {

            if (fixturesListView.SelectedItems.Count == 0)//if theres no fixture selected
            {
                MessageBox.Show("No fixture selected to remove", "Fixture", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else//if theres fixtures selected to remove
            {
                //loop though each fixture
                foreach (FixtureListViewItem item in fixturesListView.SelectedItems)
                {
                    //remove from show
                    LiveControl.Show.Instance.FixPatch.Remove((Fixture)item.Tag);
                    //remove from listview
                    fixturesListView.Items.Remove((ListViewItem)item);
                }
            }
        }

        private void createPersonalityButton_Click(object sender, EventArgs e)
        {
            //Creates and displays CreatePersonalityForm
            CreatePersonalityForm frm = new CreatePersonalityForm();
            frm.Show();
        }

        private void addPersonalityButton_Click(object sender, EventArgs e)
        {
            //adds a personality to the show
            AddPersonality();
            //reloads the personalities
            LoadPersonalities();
        }

        /// <summary>
        /// Loads a Personality from file
        /// </summary>
        private void AddPersonality()
        {
            //Create Open File Dialog
            OpenFileDialog dlg = new OpenFileDialog();
            //Set its properties
            dlg.Title = "Open Personality File";
            dlg.Filter = "Personality File|*.xml|All Files|*.*";
            dlg.AddExtension = true;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.Multiselect = true;

            //show the dialog and if everything ok
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //loup though files selected
                foreach (string file in dlg.FileNames)
                {
                    try
                    {
                        //get Personalities List
                        PersonalitiesBank Personalities = LiveControl.Show.Instance.PersonalitiesBank;
                        //try to load the file
                        int index = Personalities.AddFromFile(file);
                        //add the Personality to the listview
                        personalitiesListView.Items.Add(new PersonalityListViewItem(Personalities[index]));
                    }
                    catch (Exception ex)//if something went wrong
                    {
                        //display a message to the user 
                        MessageBox.Show("There was an error loading Personality \"" + file + "\""
                                            + Environment.NewLine + ex.Message, "Loading Personality",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }

            }
        }

        private void universeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (universeComboBox.SelectedItem == null) return;
            //Load the patched fixtures for the selected universe
            LoadPatchedFixtures((byte)universeComboBox.SelectedItem);
        }

        private void setButton_Click(object sender, EventArgs e)
        {
            if (fixturesListView.SelectedItems.Count != 1)
            {
                //display a message to user
                MessageBox.Show("You can only set one DMX channel at once", "Set DMX",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else//one item is selected
            {
                //get ref to the selected listview item
                FixtureListViewItem item = (FixtureListViewItem)fixturesListView.SelectedItems[0];
                //set the new dmx patch
                ((Fixture)item.Tag).DMX_Patch = (Int16)setDMXchNumericUpDown.Value;
                //refresh the data
                item.Refresh();

            }

        }




    }
    public class ListViewGroupSorter : IComparer
    {

        private SortOrder currentSortOrder;

        public ListViewGroupSorter(SortOrder theSortOrder)
        {
            currentSortOrder = theSortOrder;
        }

        // Compare the groups by header value.
        public int Compare(object x, object y)
        {
            int result = string.Compare(((ListViewGroup)x).Header, ((ListViewGroup)y).Header);
            if (currentSortOrder == SortOrder.Ascending)
            {
                return result;
            }
            else
            {
                return -result;
            }
        }
    }


}
