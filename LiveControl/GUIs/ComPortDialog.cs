using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Kevsoft.LiveControl.GUIs
{
    /// <summary>
    /// A Dialog so that the user can pick a com port on the computer
    /// </summary>
    public partial class ComPortDialog : Form
    {
        public ComPortDialog()
        {
            InitializeComponent();
            //Load ports
            LoadComPorts();
        }
        /// <summary>
        /// Loads the com ports on this machine into the combobox
        /// </summary>
        private void LoadComPorts()
        {
            comPortComboBox.Items.Clear();
            comPortComboBox.Items.AddRange(SerialPort.GetPortNames());
        }
        public string ComPort
        {
            get { return comPortComboBox.SelectedItem.ToString(); }
            set
            {
                //Check that the port given exists on this machine
                if (SerialPort.GetPortNames().Contains(value))
                    //select it in the combobox
                    comPortComboBox.SelectedItem = value;
                else//com port doesnt exist on this machien
                    //thow an exception
                    throw new PortDoesNotExistException();
            }
        }
        private void okButton_Click(object sender, EventArgs e)
        {
            //no item is selected
            if (comPortComboBox.SelectedItem == null)
            {
                //display a message to user
                MessageBox.Show("There is no com port selected to use", "Com Port",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else//item is selected
            {
                //Set result to ok
                DialogResult = DialogResult.OK;
                //Close the window
                Close();
            }
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            //Set the Dialog result
            DialogResult = DialogResult.Cancel;
            //Close window
            Close();
        }
        /// <summary>
        /// Exception for when a port does not exist on the current machine
        /// </summary>
        public class PortDoesNotExistException : Exception
        {
            public PortDoesNotExistException()
                : base()
            { }
            public PortDoesNotExistException(string message)
                : base(message)
            { }
        }


    }
}
