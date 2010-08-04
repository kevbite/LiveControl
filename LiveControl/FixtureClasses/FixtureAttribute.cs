using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.PersonalityClasses;

namespace Kevsoft.LiveControl.FixtureClasses
{
    /// <summary>
    /// Stores data on a attribute of a fixture
    /// </summary>
    [Serializable]
    public class FixtureAttribute
    {
        /// <summary>
        /// Handler for value changed event
        /// </summary>
        /// <param name="ChannelOffSet">Chanel offset</param>
        /// <param name="value">value of channel</param>
        /// <param name="size">bitsize of attribute</param>
        public delegate void ValueChangedEventHandler(Int16 ChannelOffSet, int value, AttributeBitSize size);
        /// <summary>
        /// Event for when a value changes
        /// </summary>
        public event ValueChangedEventHandler ValueChanged;
        /// <summary>
        /// Value of the attribute
        /// </summary>
        private int _value = 0;
        /// <summary>
        /// The Personality Attribute this Attribute is associated with
        /// </summary>
        private PersonalityAttribute _pAttribute;
        /// <summary>
        /// The fixture this attribute is associated with
        /// </summary>
        private Fixture _parentFixture;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parentFixture">Parent of this Attribute</param>
        /// <param name="pAttribute">Attribute Personality</param>
        public FixtureAttribute(Fixture parentFixture, PersonalityAttribute pAttribute)
        {
            //set the objects values
            _parentFixture = parentFixture;
            _pAttribute = pAttribute;
        }
        /// <summary>
        /// Gets or Sets the Attributes Personality
        /// </summary>
        public PersonalityAttribute PersonalityAttribute
        {
            get { return _pAttribute; }
            set { _pAttribute = value; }
        }
        /// <summary>
        /// Gets the Channel of the fixture attribute
        /// </summary>
        public Int16 Channel
        {
            get { return (Int16)(_pAttribute.OffSetChannel + _parentFixture.DMX_Patch - 1) ; }
        }
        /// <summary>
        /// Gets the universe of the attribute
        /// </summary>
        public byte Universe
        {
            get { return (byte)(_parentFixture.Universe); }
        }
        /// <summary>
        /// Gets the Type of the attribute
        /// </summary>
        public PersonalityAttribute.AttributeType Type
        {
            get { return _pAttribute.Type; }
        }
        /// <summary>
        /// Gets the max value of the attribute
        /// </summary>
        public int MaxValue
        {
            get { return _pAttribute.MaxValue; }
        }
        /// <summary>
        /// Gets or Sets the value of the attribute
        /// </summary>
        public int Value
        {
            get
            {
                //return the Attribute Value
                return _value;
            }
            set
            {
                //set Attribute value
                _value = value;
                //Call ValueChanged events
                if (ValueChanged != null)
                    ValueChanged(_pAttribute.OffSetChannel, _value, _pAttribute.BitSize);
            }
        }

        /// <summary>
        /// Locates the attribute value
        /// </summary>
        internal void LocateAttribute()
        {
            //set the attribute value to its on state
            Value = _pAttribute.OnValue;
        }
        /// <summary>
        /// Turns the lamp on 
        /// </summary>
        internal void LampOn()
        {
            //if lamp on value is set
            if(_pAttribute.LampOnValue!=-1)
                //set the value to lamp on
                Value = _pAttribute.LampOnValue;

        }
        /// <summary>
        /// Clears the attribute value
        /// </summary>
        internal void ClearValue()
        {
            //Set the value to -1
            Value = -1;
        }
    }
}
