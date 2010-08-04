using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kevsoft.LiveControl
{
    /// <summary>
    /// Creates an ListViewitem from a Personality
    /// </summary>
    class PersonalityListViewItem : System.Windows.Forms.ListViewItem
    {
        public PersonalityListViewItem(Personality p)
            : base(p.Name)
        {
            //Adds sub items
            SubItems.Add(p.Manufacturer);
            SubItems.Add(p.ChannelsUsed.ToString());
            //adds tooltip
            ToolTipText = p.Description;
            //set the tag to the personality
            Tag = p;
        }
    }
}
