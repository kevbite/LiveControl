using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kevsoft.LiveControl.Interfaces;

namespace Kevsoft.LiveControl.DmxOut
{
    public partial class GuiDmxOut : Form, IDmxObserver
    {
        /// <summary>
        /// Value label Array
        /// </summary>
        private Label[,] _valLabels;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public GuiDmxOut()
        {
            //create array of labels for ref
            _valLabels = new Label[4, 512];

            InitializeComponent();
            //create the channel labels for each universe
            CreateChannelLabels();  

            //Show the form
            Show();
            //Put focus on the form
            Focus();
        }

        private void CreateChannelLabels()
        {
            //loop though all the universes
            for (int universe = 0; universe < DmxOutput.MaxUniverses; universe++)
            {
                //Create a tab page for the universe
                TabPage universeTab = new TabPage("DMX Universe " + (universe+1).ToString());
                //add the tabe to the tab container
                tabControl1.TabPages.Add(universeTab);
                //Create the table for the universe
                TableLayoutPanel universeTableLayoutPanel = new TableLayoutPanel();
                //add the table to the tab
                universeTab.Controls.Add(universeTableLayoutPanel);
                //Set the dock style
                universeTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
                //set the column count
                universeTableLayoutPanel.ColumnCount = 32;
                //format each colum
                for (int i = 0; i < universeTableLayoutPanel.ColumnCount; i++)
                    universeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                //set the row count
                universeTableLayoutPanel.RowCount = 16;
                //format each row
                for(int i=0; i< universeTableLayoutPanel.RowCount; i++)
                    universeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent,50F));
                //for each channel in the universe (0-511)
                for (int channel = 0; channel < 512; channel++)
                {
                    //create a channel panel to hold val and channel num
                    TableLayoutPanel chPanel = new TableLayoutPanel();
                    //set column count to 1
                    chPanel.ColumnCount = 1;
                    //set so has no margins
                    chPanel.Margin = new System.Windows.Forms.Padding(0);
                    //set 2 rows and format them
                    chPanel.RowCount = 2;
                    chPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    chPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                    //set dock style
                    chPanel.Dock = DockStyle.Fill;

                    //create a label to the channel number
                    Label chLabel = new Label();
                    chLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
                    chLabel.AutoSize = true;
                    chLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    chLabel.Text = (channel + 1).ToString();
                    chPanel.Controls.Add(chLabel);

                    //create the label for the value
                    Label chValLabel = new Label();
                    //set the ref to it so we can change it later
                    _valLabels[universe, channel] = chValLabel;
                    chValLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
                    chValLabel.AutoSize = true;
                    chValLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    chValLabel.Text = "0";
                    chPanel.Controls.Add(chValLabel);


                    //Add the channel to the universe table
                    universeTableLayoutPanel.Controls.Add(chPanel);
                }
            }
        }

        private void UpdateAllLabels()
        {
            //loop though all the universes
            for (byte universe = 0; universe < DmxOutput.MaxUniverses; universe++)
            {
                //loop though all the channels in universe
                for (short channel = 0; channel < 512; channel++)
                {
                    //set the label value
                    SetLabelText(_valLabels[universe, channel],
                        _output.GetDMXValue(universe, channel).ToString());
                }
            }
        }
        #region IDmxObserver Members

        /// <summary>
        /// The dmxoutput that this class will use
        /// </summary>
        DmxOutput _output;
        /// <summary>
        /// Gets and sets the dmx output which this class uses
        /// </summary>
        public DmxOutput Output
        {
            get{return _output;}
            set
            {
                //set the output value
                _output = value;
                //update the values labels
                if (value!=null)
                    UpdateAllLabels();
            }
        }
        /// <summary>
        /// Updates the values on the form
        /// </summary>
        public void UpdateValues()
        {
            UpdateAllLabels();
        }
        /// <summary>
        /// Closes the form
        /// </summary>
        /// <returns></returns>
        public bool CloseConnection()
        {
            //close the form
            Close();
            return true;
        }

        #endregion

        delegate void SetDmxCallback(Label control, string text);

        /// <summary>
        /// Sets the Label Text on this form
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        void SetLabelText(Label control, string text)
        {
            if (control.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetDmxCallback d = new SetDmxCallback(SetLabelText);
                this.Invoke
                    (d, new object[] { control, text });
            }
            else
            {
                // It's on the same thread, no need for Invoke
                control.Text = text;
            }
        }


    }
}
