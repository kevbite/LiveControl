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
    public partial class ShowSetupForm : Form
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ShowSetupForm()
        {
            InitializeComponent();
        }

        private void ShowSetupForm_Load(object sender, EventArgs e)
        {
            //get the current instance of "Show"
            LiveControl.Show show = LiveControl.Show.Instance;
            //set the title and description
            showNameTextBox.Text = show.Title;
            showDescriptionTextBox.Text = show.Description;

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            //close the form
            Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            //set name and description for current instance of "Show"
            LiveControl.Show show = LiveControl.Show.Instance;
            //set the title and description to new values
            show.Title = showNameTextBox.Text;
            show.Description = showDescriptionTextBox.Text;

            //close the form
            Close();
        }
    }
}
