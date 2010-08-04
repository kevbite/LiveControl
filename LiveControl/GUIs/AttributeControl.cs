using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PreSetValues = System.Collections.Generic.List<Kevsoft.LiveControl.AttributePreSetValue>;
using Kevsoft.LiveControl.Interfaces;
using Kevsoft.LiveControl.FixtureClasses;
using Kevsoft.LiveControl.PersonalityClasses;

namespace Kevsoft.LiveControl.GUIs 
{
    /// <summary>
    /// Controls the values of Fixture Attributes
    /// </summary>
    public partial class AttributeControl : UserControl, IDmxObserver
    {
        /// <summary>
        /// Fixture Attributes associated with this control
        /// </summary>
        private List<FixtureAttribute> _attributes;
        /// <summary>
        /// Ref to the preset popup window
        /// </summary>
        private AttributePreSetValuePopup _popupWindow = null;
        public AttributeControl(List<FixtureAttribute> attributes)
        {
            //set the attributes
            _attributes = attributes;
            //initilize stuff on control
            InitializeComponent();

            //set name
            nameLabel.Text = _attributes[0].PersonalityAttribute.Name;

            //set the faders max value
            avFader1.Maximum = _attributes[0].MaxValue;
            avFader1.TickFrequency = (_attributes[0].MaxValue / 20);

            //set the value to the first attribute in the array#
            if (_attributes[0].Value == -1)//if not set treat as -1
                avFader1.Value = 0;
            else
                avFader1.Value = _attributes[0].Value;

            valueLabel.Text = _attributes[0].Value.ToString();

            //if theres only one attribute then add it to the Ouputs so it updates
            //if (_attributes.Count == 1)
            //    DmxOutput.Instance.Attach(this);
            //add hander for av fader
            //need to do this last as if more than one attribute is in array it will change all the lights
            this.avFader1.ValueChanged += new System.EventHandler(this.avFader1_ValueChanged);


        }

        public List<FixtureAttribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        /// <summary>
        /// Called when the fader value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void avFader1_ValueChanged(object sender, EventArgs e)
        {
            //set the label value
            valueLabel.Text = avFader1.Value.ToString();
            //loop though each attribute and set the value
            foreach (FixtureAttribute a in _attributes)
                a.Value = avFader1.Value;
        }

        /// <summary>
        /// When the PreSet Button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void preSetsButton_Click(object sender, EventArgs e)
        {
            //if theres PreSetValues on this Attribute
            if (_attributes[0].PersonalityAttribute.PreSetValues.Count > 0)
            {
                //if no window has been opened yet
                if (_popupWindow == null)
                {
                    //create a new AttributePreSetValuePopup
                    _popupWindow = new AttributePreSetValuePopup(_attributes[0].PersonalityAttribute.PreSetValues);
                    //possition the popup
                    _popupWindow.Top = Cursor.Position.Y;
                    _popupWindow.Left = Cursor.Position.X;
                    _popupWindow.ValueChangedEvent += new AttributePreSetValuePopup.ValueChangedEventHandler(popup_ValueChangedEvent);
                    _popupWindow.Show();
                }
                else//window is still ope
                {
                    //close the window
                    _popupWindow.Close();
                    //set ref to null
                    _popupWindow = null;
                }
            }
        }
        /// <summary>
        /// Called when an item on the PreSet popup window is selected
        /// </summary>
        /// <param name="preSet"></param>
        void popup_ValueChangedEvent(AttributePreSetValue preSet)
        {
            //loop though each attribute and set the value
            foreach (FixtureAttribute a in _attributes)
                a.Value = preSet.Value;

            //set the popupwindow ref to null
            _popupWindow = null;
        }

        #region IDmxObserver Members

        /// <summary>
        /// the output this observer is using
        /// </summary>
        private DmxOutput _output;
        /// <summary>
        /// Gets or Sets the dmx output associated with this observer
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
        /// Needed to be a IDmxObserver but does nothing
        /// </summary>
        /// <returns></returns>
        public bool CloseConnection()
        {
            //Dont need to do anything for closed connections
            return true;
        }
        /// <summary>
        /// updates the dmx values
        /// </summary>
        void IDmxObserver.UpdateValues()
        {
            int value = 0;
            if (_output == null) return;
            //if a 8bit attribute
            if (_attributes[0].PersonalityAttribute.BitSize == AttributeBitSize.Size8)
                value = _output.GetDMXValue(_attributes[0].Universe, (Int16)(_attributes[0].Channel - 1));
            //if a 16bit attribute
            else if (_attributes[0].PersonalityAttribute.BitSize == AttributeBitSize.Size16)
                value = _output.Get16BitDMXValue(_attributes[0].Universe, (Int16)(_attributes[0].Channel - 1));

            //set the fader and label
            SetValues(value);

        }

        #endregion
        /// <summary>
        /// Set values call back
        /// </summary>
        /// <param name="fullValue"></param>
        delegate void SetValuesCallback(int fullValue);

        /// <summary>
        /// Sets the Fader and valueLabel to the value given
        /// Thread Safe
        /// </summary>
        /// <param name="fullValue"></param>
        private void SetValues(int fullValue)
        { 

            if (avFader1.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetValuesCallback d = new SetValuesCallback(SetValues);
                this.Invoke
                    (d, new object[] { fullValue });
            }
            else
            {
                // It's on the same thread, no need for Invoke
                avFader1.Value = fullValue;
                valueLabel.Text = _attributes[0].Value.ToString();
            } 
        }   
    }


}

