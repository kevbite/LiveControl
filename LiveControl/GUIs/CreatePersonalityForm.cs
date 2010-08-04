using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Kevsoft.LiveControl.PersonalityClasses;

namespace Kevsoft.LiveControl.GUIs
{
    /// <summary>
    /// The form for creating and editing Fixture Personalitys
    /// </summary>
    public partial class CreatePersonalityForm : Form
    {
        /// <summary>
        /// The current attribute selected in the list
        /// </summary>
        private PersonalityAttribute _selectedAttribute;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CreatePersonalityForm()
        {
            InitializeComponent();
            //Load the Arribute types
            LoadAttributeTypeValues();
        }
        /// <summary>
        /// Loads the Enum AttributeType into attributeTypeComboBox
        /// </summary>
        private void LoadAttributeTypeValues()
        {
            //clear the combo box incase other items are added
            attributeTypeComboBox.Items.Clear();
            //fill the combo box with the Type enum
            attributeTypeComboBox.DataSource = System.Enum.GetValues(typeof(PersonalityAttribute.AttributeType));
        }
        /// <summary>
        /// Adds a new Attribute to the List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addAttributeButton_Click(object sender, EventArgs e)
        {
            //put a new attribute in to the listbox and select it
            attributesListBox.SelectedIndex =
                attributesListBox.Items.Add(new PersonalityAttribute());
        }

        /// <summary>
        /// Called when the attributes selected in the attributesListBox has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void attributesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if same item is selected again
            if (attributesListBox.SelectedItem == _selectedAttribute)
                //Do nothing
                return;

            //save the previously selected attribute
            if ((_selectedAttribute != null))
                SaveCurrentAttribute();

            //if nothing is selected
            if (attributesListBox.SelectedItem == null)
            {
                //disable remove button and properties groupbox
                removeAttributeButton.Enabled = false;
                attributePropertiesGroupBox.Enabled = false;
                _selectedAttribute = null;
            }
            else//item is selected
            {
                //enable remove button and properties groupbox
                removeAttributeButton.Enabled = true;
                attributePropertiesGroupBox.Enabled = true;

                //set _selectedAttribute
                _selectedAttribute = (PersonalityAttribute)attributesListBox.SelectedItem;
                //load the values
                attributeNameTextBox.Text = _selectedAttribute.Name;
                attribute16bitCheckBox.Checked = (_selectedAttribute.BitSize == AttributeBitSize.Size16);
                attributeTypeComboBox.SelectedItem = _selectedAttribute.Type;
                attributeChannelNumericUpDown.Value = (decimal)_selectedAttribute.OffSetChannel;
                attributeOnValueNumericUpDown.Value = (decimal)_selectedAttribute.OnValue;
                attributeLampOnNumericUpDown.Value = (decimal)_selectedAttribute.LampOnValue;

                //clear the current items from the preset box
                presetListBox.Items.Clear();
                //add the items from the selected attribute
                presetListBox.Items.AddRange(_selectedAttribute.PreSetValues.ToArray());


            }
        }
        /// <summary>
        /// Saves the current Attribute values
        /// </summary>
        private void SaveCurrentAttribute()
        {
            //set the values
            _selectedAttribute.Name = attributeNameTextBox.Text;

            //Set the bitsize
            if (attribute16bitCheckBox.Checked)
                _selectedAttribute.BitSize = AttributeBitSize.Size16;
            else
                _selectedAttribute.BitSize = AttributeBitSize.Size8;
            //set the type
            _selectedAttribute.Type = (PersonalityAttribute.AttributeType)attributeTypeComboBox.SelectedItem;
            //set the channel offset
            _selectedAttribute.OffSetChannel = (short)attributeChannelNumericUpDown.Value;
            //set the on value
            _selectedAttribute.OnValue = (int)attributeOnValueNumericUpDown.Value;
            //set the lamp on value
            _selectedAttribute.LampOnValue = (int)attributeLampOnNumericUpDown.Value;

            //set the presetvalues
            _selectedAttribute.PreSetValues =
                new List<AttributePreSetValue>(presetListBox.Items.Cast<AttributePreSetValue>());


            //reassign the same value to its self, this will update the tostring
            int index = attributesListBox.Items.IndexOf(_selectedAttribute);
            if (index != -1)
                attributesListBox.Items[index] = _selectedAttribute;
        }
        /// <summary>
        /// Called when the 16bit check box value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void position16bitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //tmp newMax
            decimal newMax;

            //16bit position selected
            if (position16bitCheckBox.Checked)
                newMax = 0xFFFF;//65535
            else//8bit position selected
                newMax = 0xFF;//255

            //set the new max value
            panOnValueNumericUpDown.Maximum = newMax;
            tiltOnValueNumericUpDown.Maximum = newMax;
        }

        private void attribute16bitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            decimal newMax;

            //16bit position selected
            if (attribute16bitCheckBox.Checked)
                newMax = 0xFFFF;//65535
            else//8bit position selected
                newMax = 0xFF;//255

            //set the new max value
            attributeOnValueNumericUpDown.Maximum = newMax;
            attributeLampOnNumericUpDown.Maximum = newMax;
            presetValueNumericUpDown.Maximum = newMax;

        }
        /// <summary>
        /// Called when possition checkbox value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void possitionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //get if the position enabled or not
            bool positionEnabled = possitionCheckBox.Checked;

            //enabled or disable
            position16bitCheckBox.Enabled = positionEnabled;
            panChannelNumericUpDown.Enabled = positionEnabled;
            tiltChannelNumericUpDown.Enabled = positionEnabled;
            panOnValueNumericUpDown.Enabled = positionEnabled;
            tiltOnValueNumericUpDown.Enabled = positionEnabled;
        }
        /// <summary>
        /// Adds a preset item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addPreSetButton_Click(object sender, EventArgs e)
        {
            //Create a new Preset value
            AttributePreSetValue preset =
                new AttributePreSetValue(presetNameTextBox.Text,
                                            (int)presetValueNumericUpDown.Value);
            //add it to the list
            presetListBox.Items.Add(preset);
            //Clear the text boxes
            presetNameTextBox.Text = "";
            presetValueNumericUpDown.Value = 0;
        }
        /// <summary>
        /// Removes the selected preset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removePreSetButton_Click(object sender, EventArgs e)
        {
            //no item selected to remove
            if (presetListBox.SelectedItem == null)
            {
                //display a message to the user
                MessageBox.Show("There is currently no items selected to remove",
                                "Remove preset", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            }
            else//an item has been selected
            {
                //remove the selected item
                presetListBox.Items.Remove(presetListBox.SelectedItem);
            }
        }

        /// <summary>
        /// Removes the attribute selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeAttributeButton_Click(object sender, EventArgs e)
        {
            //check to see if we have a selected item
            if (attributesListBox.SelectedItem != null)
            {
                //remove it from the list
                attributesListBox.Items.Remove(attributesListBox.SelectedItem);

            }
        }
        /// <summary>
        /// Called when the selected index changes on the preset list
        /// Updates the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void presetListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check to see if we have an item selected
            if (presetListBox.SelectedItem != null)
            {
                //set the image
                presetPictureBox.Image =
                    ((AttributePreSetValue)presetListBox.SelectedItem).GetImage();
            }
        }

        private void setPresetImageButton_Click(object sender, EventArgs e)
        {
            //check to see if we have an item selected
            if (presetListBox.SelectedItem != null)
            {
                //set the image to the loaded image
                presetPictureBox.Image = LoadImage();
                ((AttributePreSetValue)presetListBox.SelectedItem).SetImage(presetPictureBox.Image);
            }
        }

        /// <summary>
        /// Loads a image from file
        /// </summary>
        /// <returns></returns>
        private Image LoadImage()
        {
            
            Bitmap newBmp  = null;
            //Create Open File Dialog
            OpenFileDialog dlg = new OpenFileDialog();
            //Set its properties
            dlg.Title = "Open Picture";
            dlg.Filter = "Jpeg|*.jpg|Bitmap|*.bmp|All Files|*.*";
            dlg.AddExtension = true;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;

            //Displays it and if everything was ok
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    //create the new bitmap from the file steeam
                    Bitmap loadedBmp = new Bitmap(fs);
                    //Create a copy of the loaded bitmap
                    newBmp = new Bitmap(loadedBmp);
                    //close the file stream
                    loadedBmp.Dispose();
                    fs.Close();
                }
                catch (Exception ex)//if there was any problems loading the image
                {
                    //Display an error to the user
                    MessageBox.Show("There was an error while loading image file" +
                                    Environment.NewLine + ex.Message, "Picture",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return newBmp;
        }
        /// <summary>
        /// called when then change image button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeImageButton_Click(object sender, EventArgs e)
        {
            //set the personality image to the loaded image
            personalityPictureBox.Image = LoadImage();

        }
        /// <summary>
        /// Called when cancel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            //Closes the form
            Close();
        }
        /// <summary>
        /// Called when save button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            //Creates and Saves the Personality
            CreatePersonalityAndSave();
        }

        /// <summary>
        /// Creates an Personality Object then Saves it
        /// </summary>
        /// <returns></returns>
        private bool CreatePersonalityAndSave()
        {
            //set the selected item to null so it updates the currently selected values
            attributesListBox.SelectedItem = null;

            //create new Personality
            Personality newPersonality = new Personality();
            //add the values
            newPersonality.Name = nameTextBox.Text;
            newPersonality.Manufacturer = manuTextBox.Text;
            newPersonality.Description = descTextBox.Text;
            newPersonality.SetImage(personalityPictureBox.Image);

            //if Personality has position values
            if (possitionCheckBox.Checked)
            {
                //create new possition
                newPersonality.Possition = new PersonalityPosition();
                //set if 16bit or 8bit
                if (position16bitCheckBox.Checked)
                    newPersonality.Possition.BitSize = AttributeBitSize.Size16;
                else
                    newPersonality.Possition.BitSize = AttributeBitSize.Size8;
                //set pan and tilt values
                newPersonality.Possition.PanChannel = (short)panChannelNumericUpDown.Value;
                newPersonality.Possition.TiltChannel = (short)tiltChannelNumericUpDown.Value;
                newPersonality.Possition.PanOnValue = (int)panOnValueNumericUpDown.Value;
                newPersonality.Possition.TiltOnValue = (int)tiltOnValueNumericUpDown.Value;
            }
            //set the Attributes from the listbox
            newPersonality.Attributes =
                new List<PersonalityAttribute>(attributesListBox.Items.Cast<PersonalityAttribute>());

            return SavePersonality(newPersonality);
        }

        /// <summary>
        /// Saves the Personality object to file
        /// </summary>
        /// <param name="newPersonality"></param>
        /// <returns></returns>
        private bool SavePersonality(Personality newPersonality)
        {
            //Create new Save File Dialog
            SaveFileDialog dlg = new SaveFileDialog();
            //Set Properties
            dlg.Title = "Save Personality";
            dlg.Filter = "Personality File|*.xml|All Files|*.*";

            //Show the Dialog and if everythign went ok
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try//there maybe some exceptions
                {
                    //save the new Personality
                    newPersonality.Save(dlg.FileName);
                    return true;
                }
                catch (Exception ex)//catch the exceptions
                {
                    //display an error message to the user
                    MessageBox.Show("There was an error while creating Personality File"
                                    + Environment.NewLine + ex.Message, "Save Personality",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else//user canceled
            {
                //return false
                return false;
            }

        }

        private void saveAndCloseButton_Click(object sender, EventArgs e)
        {
            //If saving went ok
            if (CreatePersonalityAndSave())
                //close the window
                Close();
        }

        /// <summary>
        /// Loads a personality form file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void laodButton_Click(object sender, EventArgs e)
        {
            //Create Open File Dialog
            OpenFileDialog dlg = new OpenFileDialog();
            //Set its properties
            dlg.Title = "Open Personality File";
            dlg.Filter = "Personality File|*.xml|All Files|*.*";
            dlg.AddExtension = true;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.Multiselect = false;

            //show the dialog and if everything ok
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //get Personalities List
                    Personality p = Personality.FromFile(dlg.FileName);
                    LoadPersonalityValues(p);
                }
                catch (Exception ex)//if something went wrong
                {
                    //display a message to the user 
                    MessageBox.Show("There was an error loading Personality \"" + dlg.FileName + "\""
                                        + Environment.NewLine + ex.Message, "Loading Personality",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }

        }

        /// <summary>
        /// Loads the personality values back on to the form
        /// </summary>
        /// <param name="p">Personality to load</param>
        private void LoadPersonalityValues(Personality p)
        {
            //add the values
            nameTextBox.Text = p.Name;
            manuTextBox.Text = p.Manufacturer;
            descTextBox.Text = p.Description;
            personalityPictureBox.Image = p.GetImage();

            //set the position checkbox
            possitionCheckBox.Checked = (p.Possition != null);
            if (possitionCheckBox.Checked)
            {
                //set if 16bit or 8bit
                position16bitCheckBox.Checked = (p.Possition.BitSize == AttributeBitSize.Size16);

                //set pan and tilt values
                panChannelNumericUpDown.Value = (decimal)p.Possition.PanChannel;
                tiltChannelNumericUpDown.Value = (decimal)p.Possition.TiltChannel;
                panOnValueNumericUpDown.Value = (decimal)p.Possition.PanOnValue;
                tiltOnValueNumericUpDown.Value = (decimal)p.Possition.TiltOnValue;
            }
            //clear the attributes list box
            attributesListBox.Items.Clear();
            //add the items from the personality
            attributesListBox.Items.AddRange(p.Attributes.ToArray());
            //if the listbox now has something inside
            if(attributesListBox.Items.Count>0)
                //select the first item
                attributesListBox.SelectedItem = attributesListBox.Items[0];

        }
    }
}
