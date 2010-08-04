using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.FixtureClasses;

namespace Kevsoft.LiveControl.FixtureClasses
{
    class FixtureListViewItem : System.Windows.Forms.ListViewItem
    {
        /// <summary>
        /// Constructor
        /// Consturcts a Listview item from a fixture
        /// </summary>
        /// <param name="f"></param>
        public FixtureListViewItem(Fixture f) : base(f.Name)
        {
            //Add a sub item with the fixture addresses
            SubItems.Add(f.Addresses);

            //Set the tooltip
            ToolTipText = f.Name +
                            "\nName: " + f.Personality.Name +
                            "\nManufacturer: " + f.Personality.Manufacturer +
                            "\nAddress: " + f.Addresses;

            //the image index will be index of where the personality is in the list
            ImageIndex = LiveControl.Show.Instance.PersonalitiesBank.IndexOf(f.Personality);
            //set the tag to the fixture
            Tag = f;
        }
        /// <summary>
        /// Gets or Sets the Tag object
        /// The Tag is the Fixture associated with this listview item
        /// </summary>
        public new Fixture Tag
        {
            get { return base.Tag as Fixture; }
            set { base.Tag = value; }
        }
        /// <summary>
        /// Update the subitems and tooltip of this listviewitem
        /// </summary>
        internal void Refresh()
        {
            //Set the addresses
            SubItems[1].Text = Tag.Addresses;

            //Set the tooltip
            ToolTipText = Tag.Name +
                            "\nName: " + Tag.Personality.Name +
                            "\nManufacturer: " + Tag.Personality.Manufacturer +
                            "\nAddress: " + Tag.Addresses;
        }
    }
}
