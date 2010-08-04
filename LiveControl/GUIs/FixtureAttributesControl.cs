using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kevsoft.LiveControl.Interfaces;
using Kevsoft.LiveControl.FixtureClasses;

namespace Kevsoft.LiveControl.GUIs
{
    /// <summary>
    /// Loads all the attribute of a fixture so the user can easly change them
    /// </summary>
    public partial class FixtureAttributesControl : UserControl
    {
        /// <summary>
        /// List of fixtures associated with this control
        /// </summary>
        private List<Fixture> _fixtures;
        /// <summary>
        /// Default constructor
        /// </summary>
        public FixtureAttributesControl()
        {
            _fixtures = new List<Fixture>();
            InitializeComponent();
        }
        /// <summary>
        /// Gets or Sets the Fixtures associated with this control
        /// </summary>
        public List<Fixture> Fixtures
        {
            get
            {
                return _fixtures;
            }
            set
            {
                if (value != null)
                {
                    if (!SamePersonalities(_fixtures, value))
                    {
                        _fixtures = value;
                        //reSetup the table
                        setupTable();
                    }
                    else
                    {
                        _fixtures = value;
                        UpdateAttributeControls();
                    }
                }
            }
        }

        private bool SamePersonalities(List<Fixture> fixtures1, List<Fixture> fixtures2)
        {
            bool result = true;
            if(fixtures1.Count==0 || fixtures2.Count==0)
            {
                result = false;
            }
            foreach(Fixture fixture1 in fixtures1)
                foreach(Fixture fixture2 in fixtures2)
                    if (fixture1.Personality != fixture2.Personality)
                    {
                        result = false;
                        break;
                    }

            return result;
        }

        private void UpdateAttributeControls()
        {
            if (_fixtures.Count == 0) return;
            //loup though all the attributes
            for (int i = 0; i < _fixtures[0].Attributes.Count; i++)
            {
                //create a list of Attribute
                List<FixtureAttribute> att = new List<FixtureAttribute>();
                foreach (Fixture pF in _fixtures)
                {//add every fixtures to attributes to list
                    //Example all colors from each fixture
                    att.Add(pF.Attributes[i]);
                }
                if(tableLayoutPanel1.Controls.Count-1>=i)
                    ((AttributeControl)tableLayoutPanel1.Controls[i]).Attributes = att;
            }

        }
        private void setupTable()
        {
            //clear the table
            while (tableLayoutPanel1.Controls.Count > 0)
            {
                //Remove from the output so we dont get a null ref
                DmxOutput.Instance.Detach((IDmxObserver)tableLayoutPanel1.Controls[0]);
                //dispose of the control
                tableLayoutPanel1.Controls[0].Dispose();
            }

            //if there is no fixture then return
            if (_fixtures.Count == 0) return;

            //if the personallitys are not the same
            if (!CheckFixtureSamePersonality()) return;
            tableLayoutPanel1.ColumnStyles.Clear();

            //loup though all the attributes
            for(int i=0; i<_fixtures[0].Attributes.Count;i++)
            {
                //create a list of Attribute
                List<FixtureAttribute> att = new List<FixtureAttribute>();
                foreach (Fixture pF in _fixtures)
                {//add every fixtures to attributes to list
                 //Example all colors from each fixture
                    att.Add(pF.Attributes[i]);
                }

                AttributeControl aCtrl = new AttributeControl(att);
                tableLayoutPanel1.Controls.Add(aCtrl);
                aCtrl.Dock = DockStyle.None;
                //aCtrl.Anchor = 
                //    ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left 
                //                                            | System.Windows.Forms.AnchorStyles.Bottom)));
                //tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60f));
	        }
            //Loop though all the styles and change size style and width
            foreach (ColumnStyle cs in tableLayoutPanel1.ColumnStyles)
            {
                cs.SizeType = SizeType.Absolute;
                cs.Width = 60;
            }
        }
        /// <summary>
        /// Check if all the fixture personalitys are the same
        /// </summary>
        /// <returns></returns>
        private bool CheckFixtureSamePersonality()
        {
            //set PersonallitySame to true
            bool PersonallitySame = true;

            //loop though each fixture in array
            foreach (Fixture pF in _fixtures)
            {
                //if the personalitys are diffrent
                if (pF.Personality != _fixtures[0].Personality)
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
    }
}
