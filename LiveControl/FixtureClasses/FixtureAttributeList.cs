using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kevsoft.LiveControl.FixtureClasses
{
    /// <summary>
    /// Stores a list of Fixture Attributes
    /// </summary>
    [Serializable]
    public class FixtureAttributeList : List<FixtureAttribute>
    {
        /// <summary>
        /// Locates all the attributes in the list
        /// </summary>
        internal void LocateAttributes()
        {
            //for each item in this list
            foreach (FixtureAttribute att in this)
                //locate the attribute
                att.LocateAttribute();
        }
        /// <summary>
        /// Sends the lamp on command to all the arribute in the list
        /// </summary>
        internal void SetLampOn()
        {
            //for each item in this list
            foreach (FixtureAttribute att in this)
                //Lamp On
                att.LampOn();
        }

        /// <summary>
        /// Clears all the attribute values in the list
        /// </summary>
        internal void ClearValues()
        {
            //for each item in this list
            foreach (FixtureAttribute att in this)
                //Clear the Values
                att.ClearValue();
        }
    }
}
