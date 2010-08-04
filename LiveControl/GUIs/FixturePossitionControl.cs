using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kevsoft.LiveControl.FixtureClasses;
using Kevsoft.LiveControl.Interfaces;

namespace Kevsoft.LiveControl.GUIs
{
    public partial class FixturePossitionControl : UserControl, IDmxObserver
    {
        /// <summary>
        /// The fixtures associated with this control
        /// </summary>
        private List<Fixture> _fixtures;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public FixturePossitionControl()
        {
            try
            {
                _fixtures = new List<Fixture>();
                InitializeComponent();
                //fill the speed combobox
                FillSpeedComboBox();
                //fill the offset combox
                FillOffSetComboBox();
                //Add th diffrent shapes to the combobox
                AddShapes();
                DmxOutput.Instance.Attach(this);
            }
            catch { }

        }
        /// <summary>
        /// clears then Fill the shapeOffSetComboBox
        /// </summary>
        private void FillOffSetComboBox()
        {
            //clear the current items
            shapeOffSetComboBox.Items.Clear();
            //add an even item
            shapeOffSetComboBox.Items.Add("Even");
            //add a value all way upto the max
            for (int i = 0; i <= ShapeMovement.OffSetMax; i++)
                shapeOffSetComboBox.Items.Add(i);
        }
        /// <summary>
        /// Clears and fills the shapeSpeedComboBox
        /// </summary>
        private void FillSpeedComboBox()
        {
            //clear current items
            shapeSpeedComboBox.Items.Clear();
            //adds items 0 to speedMax
            for (int i = ShapeMovement.SpeedMin; i <= ShapeMovement.SpeedMax; i++)
                shapeSpeedComboBox.Items.Add(i);
        }
        /// <summary>
        /// Adds the diffrent types of shapes
        /// </summary>
        private void AddShapes()
        {
            List<KeyValuePair<string, Type>> types = new List<KeyValuePair<string,Type>>();
            types.Add(new KeyValuePair<string, Type>("None", typeof(ShapeMovements.NoneShapeMovement)));
            types.Add(new KeyValuePair<string, Type>("Cricle", typeof(ShapeMovements.CircleShapeMovement)));
            types.Add(new KeyValuePair<string, Type>("Line", typeof(ShapeMovements.LineShapeMovement)));
            shapeComboBox.DataSource = types;
            shapeComboBox.DisplayMember = "Key";
        }
        /// <summary>
        /// Gets or Sets the fixtures associated with this control
        /// </summary>
        public List<Fixture> Fixtures
        {
            get
            {
                return _fixtures;
            }
            set
            {
                _fixtures = value;
                setupPossition();
            }
        }

        private void setupPossition()
        {
            //Everytime we setup the values we need to remove the event handles first
            RemoveEventHandles();
            //if the fixtures dont have the same max value
            if ((_fixtures == null) )
            {
                //disable the possition control
                this.Enabled = false;

            }else if ((_fixtures.Count==0) || (!CheckFixtureSameBitSize()))
            {
                //disable the possition control
                this.Enabled = false;
            }

            else//if the fixtures have the same max value
            {
                //enable the possition control
                this.Enabled = true;
                //set the max values
                possition1.MaxXValue = _fixtures[0].Personality.Possition.MaxValuePanTilt;
                possition1.MaxYValue = _fixtures[0].Personality.Possition.MaxValuePanTilt;

                setupShapeAttributes();
                //Attach this object to the outputs now we have some fixture

            }
            AddEventHandles();
        }
        /// <summary>
        /// Removes the events from the controls
        /// </summary>
        private void RemoveEventHandles()
        {
            //Remove Event Handles
            possition1.ValueChanged -= new Kevsoft.LiveControl.WFCL.Possition.ValueChangedEventHandler(position1_ValueChanged);
            shapeComboBox.SelectedIndexChanged -= new EventHandler(shapeComboBox_SelectedIndexChanged);
            shapeSizeYNumericUpDown.ValueChanged -= new EventHandler(shapeSizeYNumericUpDown_ValueChanged);
            shapeSizeXNumericUpDown.ValueChanged -= new EventHandler(shapeSizeXNumericUpDown_ValueChanged);
            shapeSpeedComboBox.SelectedIndexChanged -= new EventHandler(shapeSpeedComboBox_SelectedIndexChanged);
            shapeOffSetComboBox.SelectedIndexChanged -= new EventHandler(shapeOffSetComboBox_SelectedIndexChanged);
        }
        /// <summary>
        /// Adds the events back to the controls
        /// </summary>
        private void AddEventHandles()
        {
            //Adds event handles to controls
            possition1.ValueChanged += new Kevsoft.LiveControl.WFCL.Possition.ValueChangedEventHandler(position1_ValueChanged);
            shapeComboBox.SelectedIndexChanged += new EventHandler(shapeComboBox_SelectedIndexChanged);
            shapeSizeYNumericUpDown.ValueChanged += new EventHandler(shapeSizeYNumericUpDown_ValueChanged);
            shapeSizeXNumericUpDown.ValueChanged += new EventHandler(shapeSizeXNumericUpDown_ValueChanged);
            shapeSpeedComboBox.SelectedIndexChanged += new EventHandler(shapeSpeedComboBox_SelectedIndexChanged);
            shapeOffSetComboBox.SelectedIndexChanged += new EventHandler(shapeOffSetComboBox_SelectedIndexChanged);

        }
        /// <summary>
        /// Setups the Shape Attribute values
        /// </summary>
        private void setupShapeAttributes()
        {
            //loop though each item in the combobox
            foreach (KeyValuePair<string, Type> t in shapeComboBox.Items)
            {
                //if the movement types are the same
                if (_fixtures[0].ShapeMovement.GetType() == t.Value)
                    //set the combobox selection to that key
                    shapeComboBox.SelectedItem = t;
            }
            //set the Min Values
            shapeSizeXNumericUpDown.Minimum = -(decimal)_fixtures[0].Personality.Possition.MaxValuePanTilt;
            shapeSizeYNumericUpDown.Minimum = -(decimal)_fixtures[0].Personality.Possition.MaxValuePanTilt;
            
            //set the max values
            shapeSizeXNumericUpDown.Maximum = (decimal)_fixtures[0].Personality.Possition.MaxValuePanTilt;
            shapeSizeYNumericUpDown.Maximum = (decimal)_fixtures[0].Personality.Possition.MaxValuePanTilt;
            
            //set the width and height
            shapeSizeXNumericUpDown.Value = (decimal)_fixtures[0].ShapeMovement.Width;
            shapeSizeYNumericUpDown.Value = (decimal)_fixtures[0].ShapeMovement.Height;

            //set the speed and offset
            shapeSpeedComboBox.SelectedItem = _fixtures[0].ShapeMovement.Speed;
            shapeOffSetComboBox.SelectedItem = _fixtures[0].ShapeMovement.OffSet;
        }
        /// <summary>
        /// checks the bitsize of the
        /// </summary>
        /// <returns></returns>
        private bool CheckFixtureSameBitSize()
        {
            //set PersonallitySame to true
            bool PersonallitySame = true;

            //loop though each fixture in array
            foreach (Fixture pF in _fixtures)
            {
                //if has not possition attribute
                if (pF.Personality.Possition == null)
                {    //set same to false
                    PersonallitySame = false;
                    //come out of for
                    break;
                }
                //if the bitsize are diffrent
                if (pF.Personality.Possition.BitSize != _fixtures[0].Personality.Possition.BitSize)
                {
                    //set same to false
                    PersonallitySame = false;
                    //come out of for
                    break;
                }

            }
            //return result
            return PersonallitySame;
        }
        /// <summary>
        /// Called when the position control changes value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void position1_ValueChanged(object sender, EventArgs e)
        {

            //loop though each fixture
            foreach (Fixture pF in _fixtures)
            {
                if ( pF.ShapeMovement is ShapeMovements.NoneShapeMovement)
                {
                    //set the pan and tilit values
                    pF.Pan = possition1.Value.X;
                    pF.Tilt = possition1.Value.Y;
                }
                else
                {
                    pF.ShapeMovement.ShapePosition =
                        new Point((possition1.Value.X - (pF.ShapeMovement.Width/2)),
                            (possition1.Value.Y - (pF.ShapeMovement.Height/2)));
                    possition1.Refresh();
                }
            }

        }

        private void lockXCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //set the X lock to the check box value
            possition1.LockX = lockXCheckBox.Checked;
        }

        private void lockYCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //set the Y lock to the check box value
            possition1.LockY = lockYCheckBox.Checked;
        }

        private void shapeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get the selected movement Type
            Type ShapeMovementType = ((KeyValuePair<string, Type>)((ComboBox)sender).SelectedItem).Value;
            //if None shapemovement is selected then make the pointer aut move
            possition1.AutoMovePointer = (ShapeMovementType.Equals(typeof(ShapeMovements.NoneShapeMovement)));
            //loop though each fixture
            foreach (Fixture pF in _fixtures)
            {
                if (pF.ShapeMovement.GetType() != ShapeMovementType)
                    //Create an instance of the selected movement type
                    pF.ShapeMovement = (ShapeMovement)Activator.CreateInstance(ShapeMovementType, pF.ShapeMovement);

            }
        }

        private void possition1_Paint(object sender, PaintEventArgs e)
        {
            if (_fixtures == null) return;
            if (_fixtures.Count == 0) return;
            //if the first fixture has a ShapeMovement 
            if (!(_fixtures[0].ShapeMovement is ShapeMovements.NoneShapeMovement))
            {
                //Draw the shapes path
                _fixtures[0].ShapeMovement.DrawPath(e.Graphics);
            }
        }

        private void shapeSizeXNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            foreach (Fixture pF in _fixtures)
            {
                //Set the shape width
                pF.ShapeMovement.Width = (int)shapeSizeXNumericUpDown.Value;
            }
            //Refresh possition control
            possition1.Refresh();
        }

        private void shapeSizeYNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            foreach (Fixture pF in _fixtures)
            {
                //Set the shape width
                pF.ShapeMovement.Height = (int)shapeSizeYNumericUpDown.Value;
            }
            //Refresh possition control
            possition1.Refresh();
        
        }

        #region IDmxObserver Members
        /// <summary>
        /// the output this observer will use 
        /// </summary>
        private DmxOutput _output;
        /// <summary>
        /// Gets or Sets the dmx output for this observer
        /// </summary>
        public DmxOutput Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
            }
        }
        /// <summary>
        /// Updates the values on this control
        /// </summary>
        public void UpdateValues()
        {
            try
            {

                int X;
                int Y; 
                //if 16bit position
                if (_fixtures[0].Personality.Possition.BitSize ==
                    Kevsoft.LiveControl.PersonalityClasses.AttributeBitSize.Size16)
                {
                    //Get the X and Y Values
                    X = _output.Get16BitDMXValue(_fixtures[0].Universe, (short)(_fixtures[0].PanChannel - 1));
                    Y = _output.Get16BitDMXValue(_fixtures[0].Universe, (short)(_fixtures[0].TiltChannel - 1));
                }
                else//8bit
                {
                    //Get the X and Y Values
                    X = _output.GetDMXValue(_fixtures[0].Universe, (short)(_fixtures[0].PanChannel - 1));
                    Y = _output.GetDMXValue(_fixtures[0].Universe, (short)(_fixtures[0].TiltChannel - 1));
                }
                //Set the position
                SetPositionValue(new Point(X, Y));

            }
            catch { }
        }

        public bool CloseConnection()
        {
            //Do not need to close any connections
            return true;
        }

        #endregion
        /// <summary>
        /// Call back for setting position value
        /// </summary>
        /// <param name="value"></param>
        delegate void SetPositionValueCallback(Point value);
        /// <summary>
        /// Sets the position value
        /// </summary>
        /// <param name="value"></param>
        public void SetPositionValue(Point value)
        {
            if (possition1.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetPositionValueCallback d = new SetPositionValueCallback(SetPositionValue);
                    this.Invoke
                        (d, new object[] { value });
            }
            else
            {
                // It's on the same thread, no need for Invoke
                possition1.Value = value;
            } 
        }

        private void shapeSpeedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //loop though all the fixtures
            foreach (Fixture pF in _fixtures)
            {
                //Set the shape width
                pF.ShapeMovement.Speed = (int)shapeSpeedComboBox.SelectedItem;
            }
        }

        private void shapeOffSetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Even is selected as its the only string
            if (shapeOffSetComboBox.SelectedItem is string)//="Even"
            {
                //get the spacing value
                float spacing = ((float)ShapeMovement.OffSetMax / (float)_fixtures.Count);
                for (int i = 0; i < _fixtures.Count; i++)
                {
                    //set the offset
                    _fixtures[i].ShapeMovement.OffSet = (int)(spacing * i);
                }
  

            }
            else//number has been selected
            {
                foreach (Fixture pF in _fixtures)
                {
                    //Set the shape offset
                    pF.ShapeMovement.OffSet = (int)shapeOffSetComboBox.SelectedItem;
                }
            }
        }

    }
}
