using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.PersonalityClasses;

namespace Kevsoft.LiveControl
{
    [Serializable]
    public class PersonalityAttribute
    {
        /// <summary>
        /// Max value for 8bit Attribute
        /// </summary>
        private const int MAX_SIZE8 = 0xFF;
        /// <summary>
        /// Max value for 16bit Attribute
        /// </summary>
        private const int MAX_SIZE16 = 0xFFFF;

        /// <summary>
        /// offset channel 
        /// </summary>
        private Int16 _offSetChannel;

        /// <summary>
        /// on value
        /// </summary>
        private int _onValue;

        /// <summary>
        /// Lamp on Value
        /// </summary>
        private int _lampOnValue;

        /// <summary>
        /// name of Attribute
        /// </summary>
        private String _name;

        /// <summary>
        /// Attribute type
        /// </summary>
        private AttributeType _type;

        /// <summary>
        /// Pre Set Values
        /// </summary>
        private List<AttributePreSetValue> _preSetValues;

        /// <summary>
        /// bit size
        /// </summary>
        private AttributeBitSize _bitSize;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PersonalityAttribute()
        {
            _offSetChannel = 1;
            _onValue = 255;
            _lampOnValue = -1;
            _name = "Untitled Attribute";
            _type= AttributeType.Dimmer;
            _preSetValues = new List<AttributePreSetValue>();
            _bitSize = AttributeBitSize.Size8;
        }

        #region "Properties"

        /// <summary>
        /// Gets the max value based on the bitsize
        /// </summary>
        public int MaxValue
        {
            get
            {
                if (BitSize == AttributeBitSize.Size8)
                    return MAX_SIZE8;
                else
                    return MAX_SIZE16;

            }
        }

        /// <summary>
        /// Gets and sets the preset values
        /// </summary>
        public List<AttributePreSetValue> PreSetValues
        {
            get{return _preSetValues;}
            set { _preSetValues = value; }
        }
  
        /// <summary>
        /// Gets and sets the name of this attribute
        /// </summary>
        public string Name
        {
            get{return _name;}
            set { _name = value; }
        }

        /// <summary>
        /// Gets and sets the on value used on this attribute
        /// </summary>
        public int OnValue
        {
            get
            {
                return _onValue;
            }
            set
            {
                if (CheckValue(value))
                    _onValue = value;
            }
        }
        /// <summary>
        /// Gets and sets the lamp on value
        /// </summary>
        public int LampOnValue
        {
            get
            {
                return _lampOnValue;
            }
            set
            {
                if (CheckValue(value))
                    _lampOnValue = value;
            }
        }

        /// <summary>
        /// Gets and sets the off set channel
        /// </summary>
        public Int16 OffSetChannel
        {
            get
            {
                return _offSetChannel;
            }
            set
            {
                _offSetChannel = value;
            }
        }

        /// <summary>
        /// Gets and sets the Attribute Type
        /// </summary>
        public AttributeType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        /// <summary>
        /// Gets and sets the bitsize of this attribute
        /// </summary>
        public AttributeBitSize BitSize
        {
            get
            {
                return _bitSize;
            }
            set
            {
                _bitSize = value;
            }
        }

        #endregion

        /// <summary>
        /// Validates the value
        /// 8bit = 0x0-0xFF
        /// 16bit = 0x0-0xFFFF
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool CheckValue(int value)
        {
                return ((value >= 0) && (value <= MaxValue));
        }

        /// <summary>
        /// to string bring back the name and channel
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _name + " (" + _offSetChannel + ")";
        }
        /// <summary>
        /// Different Attribute Types
        /// </summary>
        [Serializable]
        public enum AttributeType : int
        {
            Dimmer=0,
            Position,
            Shutter,
            Color,
            Gobo,
            Iris,
            Gobo_Function,
            Effect,
            Other
        }
    }
}
