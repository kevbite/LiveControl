using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kevsoft.LiveControl.GUIs
{
    /// <summary>
    /// Control used for selecting preset values
    /// </summary>
    public partial class AttributePreSetValuePopup : Form
    {
        /// <summary>
        /// Value Changed Event Handler
        /// </summary>
        /// <param name="preSet"></param>
        public delegate void ValueChangedEventHandler(AttributePreSetValue preSet);
        /// <summary>
        /// Value Changed event
        /// </summary>
        public event ValueChangedEventHandler ValueChangedEvent;
        /// <summary>
        /// list of Pre Set Values
        /// </summary>
        List<AttributePreSetValue> _preSetVals;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="preSetVals">Attribute PreSet Values to use</param>
        public AttributePreSetValuePopup(List<AttributePreSetValue> preSetVals)
        {
            //set the values
            _preSetVals = preSetVals;
            InitializeComponent();
            flowLayoutPanel1.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width/3,0);
        }

        private void AttributePreSetValuePopup_Load(object sender, EventArgs e)
        {
            //Load the preset buttons
            LoadPreSetValueButtons();
        }

        private void LoadPreSetValueButtons()
        {
            //loop though all the preset values
            foreach (AttributePreSetValue PreSet in _preSetVals)
            {
                //create a button for each preset
                Button preSetButton = new Button();
                preSetButton.Text = PreSet.Name;
                preSetButton.Image = PreSet.GetImage();
                preSetButton.Size = new Size(70, 60);
                preSetButton.ImageAlign = ContentAlignment.TopCenter;
                preSetButton.TextAlign = ContentAlignment.BottomCenter;
                preSetButton.Click += new EventHandler(preSetButton_Click);
                preSetButton.Tag = PreSet;
                //add it to the flow panel
                flowLayoutPanel1.Controls.Add(preSetButton);

            }
        }
        /// <summary>
        /// called when a click event on a preset button happends
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void preSetButton_Click(object sender, EventArgs e)
        {
            //if there is events associated
            if (ValueChangedEvent != null)
                //called them
                ValueChangedEvent((AttributePreSetValue)((Button)sender).Tag);
            //dispose the form
            Dispose();
        }
    }
}
