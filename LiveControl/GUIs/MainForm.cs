using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kevsoft.LiveControl.GUIs;
using System.IO;
using Kevsoft.LiveControl.Properties;
using Kevsoft.LiveControl.Interfaces;
using Kevsoft.LiveControl.DmxOut;
using System.Collections;
using Kevsoft.LiveControl.FixtureClasses;
using Kevsoft.LiveControl.LightPrograms;

namespace Kevsoft.LiveControl
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The current show path which is loaded
        /// </summary>
        private string _currentShowPath = null;
        /// <summary>
        /// The current selected attribute type
        /// </summary>
        private RadioButton _selectedAttributeType;
        /// <summary>
        /// The current selected fixtures
        /// </summary>
        List<Fixture> _selectedFixtures = new List<Fixture>();
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Loads the Attribute Types Option Buttons
        /// </summary>
        private void LoadTypesOptionButtons()
        {
            //create a radio button for each attributeType
            foreach (int value in System.Enum.GetValues(typeof(PersonalityAttribute.AttributeType)))
            {
                RadioButton btn = new RadioButton();//create the button
                btn.Tag = value;//set the tag as value
                //set the text replacing _ with spaces
                btn.Text = System.Enum.GetName(typeof(PersonalityAttribute.AttributeType), value).Replace("_", " ");
                btn.Width = 70;//set width
                btn.Height = 50;//set height
                btn.Anchor = AnchorStyles.Top;//set anchor to top
                btn.TextAlign = ContentAlignment.MiddleCenter;//align txt
                btn.Appearance = Appearance.Button;//button appearance
                btn.Click += new EventHandler(typeBtn_Click);//add event
                typesflowLayoutPanel.Controls.Add(btn);//dd the control to the table
                if (value == 0)//is the first button
                {
                    btn.Checked = true;//check it
                    _selectedAttributeType = btn;//set select type option button
                }

            }
        }

        void typeBtn_Click(object sender, EventArgs e)
        {
            //set the ref to the selected type button
            _selectedAttributeType = (RadioButton)sender;

        }
        /// <summary>
        /// Called when the form first loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //loads the Attribute Type Buttons
            LoadTypesOptionButtons();
            //Loads the Fixture list
            LoadFixtureList();
            //Load the LightPrograms
            LoadProgramData();
        }
        
        /// <summary>
        /// Loads the program data buttons
        /// </summary>
        private void LoadProgramData()
        {
            //clear the current controls
            programsFlowLayoutPanel.Controls.Clear();

            //loop though all the pallets
            foreach (ILightProgram p in LiveControl.Show.Instance.ProgramList)
            {
                AddProgramButton(p);
            }
        }

        /// <summary>
        /// Adds a Light Program Button
        /// </summary>
        /// <param name="p">The ILightProgram to add</param>
        private void AddProgramButton(ILightProgram p)
        {
            //Create checkbox
            CheckBox programmedButton = new CheckBox();
            //set the properties
            programmedButton.Text = p.Name;
            programmedButton.Tag = p;
            programmedButton.Appearance = Appearance.Button;
            programmedButton.AutoCheck = false;
            programmedButton.UseMnemonic = false;
            //add click handle
            programmedButton.Click += new EventHandler(programmedButton_Click);
            //add tooltip
            toolTip1.SetToolTip(programmedButton, p.Description);
            //add to flow pannel
            programsFlowLayoutPanel.Controls.Add(programmedButton);
        }
        /// <summary>
        /// Called when a program button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void programmedButton_Click(object sender, EventArgs e)
        {
            CheckBox pButton = (CheckBox)sender;
            //Get ref to the light program
            ILightProgram lProgram = (ILightProgram)pButton.Tag;
            //if user wishes to edit the clicked program
            if (editProgramToolStripButton.Checked)
            {
                EditLightProgram(pButton, lProgram);
            }
            //user wishes to remove an item
            else if (removeProgramToolStripButton.Checked)
            {
                RemoveLightProgram(pButton, lProgram);
            }else//program just required to be run or stopped
            {
                RunOrStopLightProgram(pButton, lProgram);
            }
        }
        /// <summary>
        /// Displays the edit window for Editing a ILightProgram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="lProgram"></param>
        private void EditLightProgram(CheckBox sender, ILightProgram lProgram)
        {
            //Create and Show the edit Form
            GUIs.EditLightProgramForm frm = new EditLightProgramForm(lProgram);
            frm.ShowDialog();
            //Set the new name
            sender.Text = lProgram.Name;
            //set the tooltip of the button
            toolTip1.SetToolTip(sender, lProgram.Description);
            //uncheck the edit button
            editProgramToolStripButton.Checked = false;
        }
        /// <summary>
        /// Removes a ILightProgram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="lProgram"></param>
        private void RemoveLightProgram(CheckBox sender, ILightProgram lProgram)
        {
            //ask the user if they wish to remove the selected
            DialogResult r = MessageBox.Show("Are you sure you wish to remove " + lProgram.Name + "?",
                                        "Remove Program", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if user wish to carry on with removing program
            if (r == DialogResult.Yes)
            {
                //make sure the program is stopped
                lProgram.Stop();
                //remove the program from the show
                LiveControl.Show.Instance.ProgramList.Remove(lProgram);
                //remove the button
                programsFlowLayoutPanel.Controls.Remove(sender);
            }
            //set the remove program button to unchecked
            removeProgramToolStripButton.Checked = false;
        }
        /// <summary>
        /// Runs or stops a ILightProgram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="lProgram"></param>
        private void RunOrStopLightProgram(object sender, ILightProgram lProgram)
        {
            //if program already running
            if (((CheckBox)sender).Checked)
            {
                //Stop the program
                lProgram.Stop();
            }
            else//program not running
            {
                //Execute the Program in the button
                if(_selectedFixtures.Count==0)
                    lProgram.Run((PersonalityAttribute.AttributeType)_selectedAttributeType.Tag);
                else
                    lProgram.Run((PersonalityAttribute.AttributeType)_selectedAttributeType.Tag, _selectedFixtures);

                if (lProgram is Chase)
                {
                    //set last chase to this one
                    _lastRunChase = (Chase)lProgram;
                    //set the interval time
                    TimeSpan interval = _lastRunChase.IntervalSpeed;
                    currentChaseSpeedToolStripLabel.Text =
                        (int)interval.TotalSeconds + ":" + interval.Milliseconds;

                }
            }
            //if program is a case
            if (lProgram is Chase)
                //invert the checked state
                ((CheckBox)sender).Checked = !((CheckBox)sender).Checked;
        }
        /// <summary>
        /// Loads the fixture list in to the listview
        /// </summary>
        private void LoadFixtureList()
        {
            //clear the current items first.
            fixturesListView.Items.Clear();

            CreateImageList();            
            
            foreach (Fixture f in LiveControl.Show.Instance.FixPatch)
            {
                ListViewItem l = new FixtureListViewItem(f);
                fixturesListView.Items.Add(l);

            }
        }
        /// <summary>
        /// Creates the image list for the fixturesListView
        /// </summary>
        private void CreateImageList()
        {
            //clear the image lists
            fixtureImageList.Images.Clear();
            fixtureSmallImageList.Images.Clear();

            //get the images from the Personalities Bank
            Image[] imgs = LiveControl.Show.Instance.PersonalitiesBank.GetImages();

            //add images to the lists
            fixtureImageList.Images.AddRange(imgs);
            fixtureSmallImageList.Images.AddRange(imgs);

        }
        /// <summary>
        /// Called when the selected index changes of the fixture listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fixturesListView_SelectedIndexChanged(object sender, EventArgs e)  
        {
            _selectedFixtures = new List<Fixture>();

            //if no item was selected
            if (fixturesListView.SelectedItems.Count != 0)
                //Collapses the position panel if the fixture personality has no position
                possitionAttributesSplit.Panel1Collapsed =
                    (((Fixture)fixturesListView.SelectedItems[0].Tag).Personality.Possition == null);

            //loop though each selected item
            foreach (FixtureListViewItem item in fixturesListView.SelectedItems)
                //add the items to the selected items
                _selectedFixtures.Add(item.Tag);

            //set the attribute controls fixture to the selected
            fixtureAttributesControl1.Fixtures = _selectedFixtures;
            //set the position controls fixture to the selected
            fixturePossitionControl1.Fixtures = _selectedFixtures;

        }
        /// <summary>
        /// Called after the user edits the fixture label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fixturesListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            //set the fixtures new name
            ((Fixture)fixturesListView.Items[e.Item].Tag).Name = e.Label;
        }

        private void setupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create a show setup form
            ShowSetupForm frm = new ShowSetupForm();
            //display it
            frm.ShowDialog();
        }

        private void fixtureSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create Fixture Setup Form
            FixtureSetupForm frm = new FixtureSetupForm();
            //Show the Form
            frm.ShowDialog();
            //Refresh the Fixture List
            LoadFixtureList();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveShow();//Call Save
        }
        /// <summary>
        /// Saves the current Show
        /// </summary>
        /// <returns></returns>
        private bool SaveShow()
        {
            if (_currentShowPath != null)//if path has been set
            {
                try
                {
                    //Try saving the show
                    LiveControl.Show.Instance.SaveShow(_currentShowPath);
                    return true;//returns true if everything went ok
                }
                catch (Exception ex)//if there was any exceptions raised
                {
                    //Display an error to the user
                    MessageBox.Show("There was an error while saving the show file" +
                                    Environment.NewLine + ex.Message, "Saving Show File",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;//return false
                }
            }
            else return SaveShowAs();//if a path has not been set, call SaveShowAs

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveShowAs();//Call Save As
        }
        /// <summary>
        /// Save the current show under a diffrent filename
        /// </summary>
        /// <returns></returns>
        private bool SaveShowAs()
        {
            //Create new Save File Dialog
            SaveFileDialog dlg = new SaveFileDialog();
            //Set Properties
            dlg.Title = "Save Show File As";
            dlg.Filter = "Live Control File|*.shw|All Files|*.*";

            //Show the Dialog and if everythign went ok
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //set the path to the new path
                _currentShowPath = dlg.FileName;
                //Save the Show
                return SaveShow();
            }
            else//user canceled
            {
                //return false
                return false;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewShow();//Call New Show
        }
        /// <summary>
        /// Creates a new show
        /// </summary>
        private void NewShow()
        {
            if (CheckSaveShow())//Check if user wishes to save current show
            {
                //create a new show
                LiveControl.Show.Instance.CreateNewShow();
                //Clear the Show File Path Location
                _currentShowPath = null;
                //Load the fixture list
                LoadFixtureList();
                LoadProgramData();

            }

        }
        /// <summary>
        /// Checks if the show requires to be saved
        /// </summary>
        /// <returns></returns>
        private bool CheckSaveShow()
        {
            //ask the user if they wish to save current show
            DialogResult r = MessageBox.Show("Do you wish to save the current show?", "Save Current Show",
                                 MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            //if user "clicked No" or the "Show was save ok"
            if (r == DialogResult.No || ((r == DialogResult.Yes) && SaveShow()))
                return true;
            else
                return false;

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenShow();
        }
        /// <summary>
        /// Opens a show file
        /// </summary>
        private void OpenShow()
        {
            if (CheckSaveShow())//Check if user wishes to save current show
            {
                //Create Open File Dialog
                OpenFileDialog dlg = new OpenFileDialog();
                //Set its properties
                dlg.Title = "Open Show File";
                dlg.Filter = "Live Control File|*.shw|All Files|*.*";
                dlg.AddExtension = true;
                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;

                //Displays it and if everything was ok
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _currentShowPath = dlg.FileName;
                        //Load the show
                        LiveControl.Show.LoadShow(_currentShowPath);
                        //Load the fixture list
                        LoadFixtureList();
                        LoadProgramData();
                    }
                    catch (Exception ex)//if there was any problems loading the show
                    {
                        //Display an error to the user
                        MessageBox.Show("There was an error while Opening the show file" +
                                        Environment.NewLine + ex.Message, "Open Show File",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }

        }

        private void magic3DEasyViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if magic easy view not started
            if (magic3DEasyViewToolStripMenuItem.Tag == null)
            {
                StartMEVP(magic3DEasyViewToolStripMenuItem);
            }
            else//Magic 3D EasyView is already been started
            {
                StopOutputDevice(magic3DEasyViewToolStripMenuItem);
                startMEVPToolStripButton.Checked = false;
            }

        }

        /// <summary>
        /// Starts the MEVP (Sunlite Magic Easy View)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartMEVP(object sender)
        {
            try//we my encounter an error
            {
                //create the MEVP dmx out
                MEVPDmxOut mevp = new MEVPDmxOut();
                //attach it to the output
                DmxOutput.Instance.Attach(mevp);
                //set the tag of this object to the mevp
                ((ToolStripItem)sender).Tag = mevp;
                //set the toolmenu and button to checked
                startMEVPToolStripButton.Checked = true;
                magic3DEasyViewToolStripMenuItem.Checked = true;
            }
            catch (FileNotFoundException)//cannot find the MEVP.dll
            {
                //ask the user if they wish to find it there self
                if (MessageBox.Show("Could not find MEVP.dll, Would you like to try and find this file?",
                                    "File not found", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
                                    == DialogResult.Yes)
                {
                    //Set the MEVP Path
                    if (SetMEVPPath())
                        //if everything went ok then recall this method with same values
                        StartMEVP(sender);

                }

            }
            catch (Exception ex)//if there was a problem
            {
                //Display a message to the user
                MessageBox.Show("There was a problem starting Sunlite Magic 3D EasyView"
                                    + Environment.NewLine + ex.Message, "Magic 3D EasyView",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Gets the user to select the MEVP file to use
        /// </summary>
        /// <returns>If everything went ok</returns>
        private bool SetMEVPPath()
        {
            bool methodResult = false;
            //create a Open File Dlg
            OpenFileDialog dlg = new OpenFileDialog();
            //Set its properties
            dlg.Title = "MEVP.dll";
            dlg.Filter = "MEVP|MEVP.dll|All Files|*.*";
            dlg.AddExtension = true;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;

            //Displays it and if everything was ok
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //Change the MEVP_Path Setting file
                Settings.Default.MEVP_Path = dlg.FileName;

                //set the result to true
                methodResult = true;
            }
            //return the result
            return methodResult;

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close the form
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //check if requires a save if ok then close
            e.Cancel = !CheckSaveShow();
            if (!e.Cancel)//if not canceled close
            {
                //stop the output thread
                DmxOutput.Instance.StopOutputThread();
                //Wait until the output thread stopped before closing the form
                while (!DmxOutput.Instance.Stopped)
                {
                    System.Threading.Thread.Sleep(0);
                    Application.DoEvents();
                }
            }
        }

        private void FixtureViewTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //uncheck all the view states
            largeIconsToolStripMenuItem.CheckState = CheckState.Unchecked;
            detailedToolStripMenuItem.CheckState = CheckState.Unchecked;
            tiledToolStripMenuItem.CheckState = CheckState.Unchecked;
            listToolStripMenuItem.CheckState = CheckState.Unchecked;
            smallIconsToolStripMenuItem.CheckState = CheckState.Unchecked;

            //change the view
            fixturesListView.View = (View)((ToolStripMenuItem)sender).Tag;
            //check the item the user clicked on
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Select all the items in the listview
            foreach (ListViewItem item in fixturesListView.Items)
                item.Selected = true;
        }

        private void oddFixturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Select the items that are odd (1 at start)
            SelectItemBitwise(1);
        }

        private void SelectItemBitwise(int rBitwise)
        {
            //create list that we will put ListView Items in
            IList items;
            //if theres items already selected
            if(fixturesListView.SelectedItems.Count>0)
                //set items to the selected items
                items = fixturesListView.SelectedItems;
            else//no items already selected
                //set items to all items in list
                items = fixturesListView.Items;
            
            //loop though all the items
            for (int i = 0; i < items.Count; i++)
            {
                //Select the item if the rbitwise = firstbit
                ((ListViewItem)items[i]).Selected = ((i & 1) == rBitwise);
            }
        }

        private void evenFixturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Select the items that are even (0 at start)
            SelectItemBitwise(0);
        }

        private void byPersonalityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void invertSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //loop though each item in fixture list
            foreach (ListViewItem item in fixturesListView.Items)
                //invert the selected
                item.Selected = !item.Selected;
        }

        private void artNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if ArtNet is not started
            if (((ToolStripMenuItem)sender).Tag == null)
            {
                StartOutputDevice(sender, typeof(ArtNetDmxOut));
            }
            else
            {
                StopOutputDevice(sender);
            }
        }

        private static void StopOutputDevice(object sender)
        {
            //Get the ref to the dmx observer
            IDmxObserver dmxDevice = (IDmxObserver)((ToolStripItem)sender).Tag;
            //remove it from the output
            DmxOutput.Instance.Detach(dmxDevice);
            //tell the device to close connection
            dmxDevice.CloseConnection();
            //Set the Tag as null
            ((ToolStripMenuItem)sender).Tag = null;
            dmxDevice = null;
            //setArtnet toolmenu to unchecked 
            ((ToolStripMenuItem)sender).Checked = false;
            
        }

        private static void StartOutputDevice(object sender, Type type)
        {
            try//we my encounter an error
            {
                //create the DmxObserver type
                IDmxObserver observer = (IDmxObserver)Activator.CreateInstance(type);
                //attach it to the output
                DmxOutput.Instance.Attach(observer);
                //set the tag to the DmxObserver object
                ((ToolStripItem)sender).Tag = observer;
                //setArtnet toolmenu to checked 
                ((ToolStripMenuItem)sender).Checked = true;
            }
            catch (Exception ex)//if there was a problem
            {
                //Display a message to the user
                MessageBox.Show("There was a problem starting Device output"
                        + Environment.NewLine + ex.Message, "Starting Dmx Output",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void slminiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if slmini is not started
            if (((ToolStripMenuItem)sender).Tag == null)
            {
                //Start slmini
                StartOutputDevice(sender, typeof(DasHardDmxOut));
            }
            else//slmini already running
            {
                //stop slmini
                StopOutputDevice(sender);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Pallet p = LiveControl.Show.Instance.ProgramList.AddPallet();
            EditLightProgramForm frm = new EditLightProgramForm(p);
            frm.ShowDialog();
            AddProgramButton(p);
        }

        private void dMXOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if window not already open
            if (((ToolStripMenuItem)sender).Tag == null)
            {
                StartOutputDevice(sender, typeof(GuiDmxOut));
            }
            else
            {
                StopOutputDevice(sender);
            }
        }   

        private void locateFixtureStripButton_Click(object sender, EventArgs e)
        {
            //locate the fixtures
            LocateSelectedFixtures();
        }

        private void LocateSelectedFixtures()
        {
            //loop though each selected item
            foreach (ListViewItem item in fixturesListView.SelectedItems)
                //Locate the fixture
                ((Fixture)item.Tag).LocateFixture();
        }

        private void lampOnStripButton_Click(object sender, EventArgs e)
        {
            //turn the lamps on
            TurnLampOnSelectedFixtures();
        }

        private void TurnLampOnSelectedFixtures()
        {
            //loop though each selected item
            foreach (ListViewItem item in fixturesListView.SelectedItems)
                //Locate the fixture
                ((Fixture)item.Tag).LampOn();
        }

        private void createFixturePersonalityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create and show the CreatePersonalityForm 
            CreatePersonalityForm frm = new CreatePersonalityForm();
            frm.Show();
        }

        private void programChaseToolStripButton_Click(object sender, EventArgs e)
        {
            //disable this button and enable next chase and end chase buttons
            programChaseToolStripButton.Enabled = false;
            programChaseNextToolStripButton.Enabled = true;
            programChaseStopToolStripButton.Enabled = true;
            removeLastStepToolStripButton.Enabled = true;

            stepNoToolStripLabel.Text = "0";
            stepNoToolStripLabel.Visible = true;
            //Create a tmp chase
            tmpChase = new Chase();
        }
        /// <summary>
        /// The ref to the chase we use when creating a new chase
        /// </summary>
        private Chase tmpChase;
        /// <summary>
        /// Called when End chase proramming button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void programChaseStopToolStripButton_Click(object sender, EventArgs e)
        {
            //if some steps have been added
            if (tmpChase.NumberOfSteps != 0)
            {
                EditLightProgramForm frm = new EditLightProgramForm(tmpChase);
                frm.ShowDialog();
                LiveControl.Show.Instance.ProgramList.Add(tmpChase);
                AddProgramButton(tmpChase);

            }
            //disable this and next chase button and enable program chase button
            programChaseToolStripButton.Enabled = true;
            programChaseNextToolStripButton.Enabled = false;
            programChaseStopToolStripButton.Enabled = false;
            removeLastStepToolStripButton.Enabled = false;
            stepNoToolStripLabel.Visible = false;
        }

        private void programChaseNextToolStripButton_Click(object sender, EventArgs e)
        {
            //Add a step to the current chase program
            tmpChase.AddStep(LiveControl.Show.Instance.FixPatch);
            //Update the number of steps
            stepNoToolStripLabel.Text = tmpChase.NumberOfSteps.ToString();
        }
        /// <summary>
        /// ref to the last chase run
        /// </summary>
        private Chase _lastRunChase;
        private void setChaseSpeedToolStripButton_Click(object sender, EventArgs e)
        {
            //we are storing the first time in the tag
            //if no first time set or gap is greather than 1
            if (setChaseSpeedToolStripButton.Tag == null ||
                (DateTime.Now-(DateTime)setChaseSpeedToolStripButton.Tag).TotalMinutes > 1
                )
            {
                //set the time now
                setChaseSpeedToolStripButton.Tag = DateTime.Now;
            }
            else
            {
                //get the first time
                DateTime first = (DateTime)setChaseSpeedToolStripButton.Tag;
                DateTime second = DateTime.Now;
                //find diffrence
                TimeSpan diff = second - first;
                SetLastChaseSpeed(diff);
                //set the tag back to null
                setChaseSpeedToolStripButton.Tag = null;
            }
        }
        /// <summary>
        /// Sets the chase speed of last run chase
        /// </summary>
        /// <param name="diff"></param>
        private void SetLastChaseSpeed(TimeSpan diff)
        {
            //send last running chase to interval
            if (_lastRunChase != null)
                _lastRunChase.IntervalSpeed = diff;
            currentChaseSpeedToolStripLabel.Text =
                (int)diff.TotalSeconds + ":" + diff.Milliseconds.ToString("000");
            //set the trackbar
            currentChaseSpeedToolStripTrackBar.Value = (int)diff.TotalMilliseconds;
            //set the tag of value to TimeSpan
            currentChaseSpeedToolStripLabel.Tag = diff;
        }

        private void clearValuesToolStripButton_Click(object sender, EventArgs e)
        {
            //clears the selected fixture values
            ClearSelectedFixturesValues();
        }
        /// <summary>
        /// Clears the currently selected fixture values
        /// </summary>
        private void ClearSelectedFixturesValues()
        {
            //loop though each selected item
            foreach (ListViewItem item in fixturesListView.SelectedItems)
                //Locate the fixture
                ((Fixture)item.Tag).ClearValues();
        }

        private void programSceneToolStripButton_Click(object sender, EventArgs e)
        {
            //Program Scene is not implemented yet
            MessageBox.Show("Not Implemented!", "Program Scene",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void removeLastStepToolStripButton_Click(object sender, EventArgs e)
        {
            //if theres a step to remove
            if (tmpChase.NumberOfSteps > 0)
            {
                //Remove Last Step
                tmpChase.RemoveLastStep();
                //Update the number of steps
                stepNoToolStripLabel.Text = tmpChase.NumberOfSteps.ToString();
            }
            else//No steps added yet
            {
                //Display a message to the user
                MessageBox.Show("There are no steps left to remove", "Remove Steps",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void currentChaseSpeedToolStripTrackBar_ValueChanged(object sender, EventArgs e)
        {
            //create a timespan of the current time selected on the currentChaseSpeedToolStripTrackBar
            TimeSpan t = new TimeSpan(0, 0, 0, 0, currentChaseSpeedToolStripTrackBar.Value);
            //set the new time
            SetLastChaseSpeed(t);
        }

        private void milford11463ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if milford11463 is not started
            if (((ToolStripMenuItem)sender).Tag == null)
            {
                //Create a new Comport Dialog
                ComPortDialog dlg = new ComPortDialog();
                try
                {
                    //try and set the comport on the dialog
                    dlg.ComPort = Settings.Default.Milford1463Port;
                }
                catch { }
                //if the dlg result was ok
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //Set the comport in the settings
                    Settings.Default.Milford1463Port = dlg.ComPort;
                    //Start milford11463
                    StartOutputDevice(sender, typeof(Milford_1_1463DmxOut));
                }
            }
            else//milford11463 already running
            {
                //stop milford11463
                StopOutputDevice(sender);
            }
        }

        private void aboutToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Written by: Kevin Smith", "Live Control",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
