using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kevsoft.LiveControl.Interfaces;

namespace Kevsoft.LiveControl.GUIs
{
    /// <summary>
    /// Form used for editing ILightProgram objects
    /// </summary>
    public partial class EditLightProgramForm : Form
    {
        /// <summary>
        /// The ILightProgram we are editing
        /// </summary>
        private ILightProgram _lProgram;
        /// <summary>
        /// Default constructor
        /// </summary>
        public EditLightProgramForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor that sets the LightProgram
        /// </summary>
        /// <param name="lProgram"></param>
        public EditLightProgramForm(ILightProgram lProgram) : this ()
        {
            //Set the light program were editing
            LightProgram = lProgram;
        }
        /// <summary>
        /// Gets or Sets the Light Program
        /// </summary>
        public ILightProgram LightProgram
        {
            get { return _lProgram; }
            set
            {
                _lProgram = value;
                SetLightProgramVals();
            }
        }
        /// <summary>
        /// Sets the Light valus on to the form
        /// </summary>
        private void SetLightProgramVals()
        {
            nameTextBox.Text = _lProgram.Name;
            descriptionTextBox.Text = _lProgram.Description;
        }
        /// <summary>
        /// Save the light program values back to the _lprogram object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            //Set values
            _lProgram.Name = nameTextBox.Text;
            _lProgram.Description = descriptionTextBox.Text;
            //Set dialog result to ok
            DialogResult = DialogResult.OK;
            //close the form
            Close();
        }
        /// <summary>
        /// Called when cancel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            //set dialog result to canceled
            DialogResult = DialogResult.Cancel;
            //close the form
            Close();
        }
    }
}
