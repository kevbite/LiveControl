using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.PersonalityClasses;

namespace Kevsoft.LiveControl.FixtureClasses
{
    /// <summary>
    /// Stores data on a fixture
    /// </summary>
    [Serializable]
    public class Fixture 
    {
        /// <summary>
        /// Personality of the current fixture
        /// </summary>
        private Personality _personality;

        /// <summary>
        /// The DMX Channel Patch
        /// </summary>
        private Int16 _dmxPatch = 1;
        /// <summary>
        /// The Universe Patch
        /// </summary>
        private byte _universe = 0;

        /// <summary>
        /// Name of the fixture
        /// </summary>
        private String _name = "";

        /// <summary>
        /// Current Pan Value
        /// </summary>
        private int _panValue = 0;
        /// <summary>
        /// Current Tilt Value
        /// </summary>
        private int _tiltValue = 0;

        /// <summary>
        /// Current ShapeMovement
        /// </summary>
        private ShapeMovement _shapeMovement;

        /// <summary>
        /// Attributes
        /// </summary>
        private FixtureAttributeList _attributes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="personality">Personality associated with the fixture</param>
        /// <param name="dmxPatch">channel of fixture</param>
        /// <param name="universe">universe of fixture</param>
        public Fixture(Personality personality, Int16 dmxPatch, byte universe)
        {
            //set the properties of fixture
            _personality = personality;
            _dmxPatch=dmxPatch;
            _universe=universe;
            _name = personality.Name;

            //if there is no possition
            if (_personality.Possition == null)
            {
                //set pan and tilt to -1 
                _panValue = -1;
                _tiltValue = -1;
            }
            //set a none movement
            _shapeMovement = new ShapeMovements.NoneShapeMovement(this);
            //create an attribute list
            _attributes = new FixtureAttributeList();

            //populate the attribute list
            foreach (PersonalityAttribute pAttribute in _personality.Attributes)
            {
                //create a fixutre attribute from the personality
                FixtureAttribute fixtureAttribute = new FixtureAttribute(this, pAttribute);
                //set event handler
                fixtureAttribute.ValueChanged += new FixtureAttribute.ValueChangedEventHandler(ValueChangedEvent);
                //add to attributes 
                _attributes.Add(fixtureAttribute);

            }
        }
        /// <summary>
        /// Gets the Pan Channel
        /// </summary>
        public short PanChannel
        {
            get { return (short)(_dmxPatch + _personality.Possition.PanChannel-1); }
        }
        /// <summary>
        /// Gets the Tilt Channel
        /// </summary>
        public short TiltChannel
        {
            get { return (short)(_dmxPatch + _personality.Possition.TiltChannel-1); }
        }
        /// <summary>
        /// Gets or Sets the Pan Value
        /// </summary>
        public int Pan
        {
            get
            {
                return _panValue;
            }
            set
            {
                //Check the value before setting it
                if (CheckPanTiltValue(value))
                {
                    _panValue = value;
                    
                        //set the channel value
                        ValueChangedEvent(_personality.Possition.PanChannel,
                                            value, _personality.Possition.BitSize);

                    }


                }
            }
        /// <summary>
        /// Gets and Sets the Tilt Value
        /// </summary>
        public int Tilt
        {
            get
            {
                return _tiltValue;
            }
            set
            {
                //Check the value before setting it
                if (CheckPanTiltValue(value))
                {
                    _tiltValue = value;
                  
                        //set the first channel
                    ValueChangedEvent(_personality.Possition.TiltChannel,
                                        value, _personality.Possition.BitSize);
                }

            }
        }
        /// <summary>
        /// Gets the Max Pan Tilt Value for this fixture
        /// </summary>
        public int MaxPanTiltValue
        {
            get { return _personality.Possition.MaxValuePanTilt; }
        }
        /// <summary>
        /// Gets the personality associated with the fixture
        /// </summary>
        public Personality Personality
        {
            get { return _personality; }
        }
        /// <summary>
        /// Gets the fixture attribute list
        /// </summary>
        public FixtureAttributeList Attributes
        {
            get { return _attributes; }
        }
        /// <summary>
        /// Gets or Sets the fixtures name
        /// </summary>
        public string Name
        {
            get{return _name;}
            set{_name = value;}
        }
        /// <summary>
        /// Gets or Sets the universe of fixture
        /// </summary>
        public byte Universe
        {
            get { return _universe; }
            set { _universe = value; }
        }
        /// <summary>
        /// Gets or Sets the Dmx channel of fixture
        /// </summary>
        public Int16 DMX_Patch
        {
            get { return _dmxPatch; }
            set { _dmxPatch = value; }
        }
        /// <summary>
        /// Gets or Sets the ShapeMovement of fixture
        /// </summary>
        public ShapeMovement ShapeMovement
        {
            get { return _shapeMovement; }
            set { _shapeMovement = value; }
        }
        /// <summary>
        /// Returns the Addresses of fixture
        /// </summary>
        public string Addresses
        {
            get { return (_dmxPatch + "-" + (_dmxPatch+_personality.ChannelsUsed-1)); }
        }
        /// <summary>
        /// Called when any value of the fixture is changed
        /// after called it updates the output with the required value
        /// </summary>
        /// <param name="ChannelOffSet">Channel offset of attribute</param>
        /// <param name="value">value to send</param>
        /// <param name="size">bit size of attribute</param>
        void ValueChangedEvent(Int16 ChannelOffSet, int value, AttributeBitSize size)
        {
            //if the value = -1 then is hasnt been set so just send 0 out
            if (value == -1) value = 0;
            //get ref to instance of the output
            DmxOutput output = DmxOutput.Instance;
            //set the first channel
            if (size == AttributeBitSize.Size8)
                output.SetDmx(_universe, (Int16)(_dmxPatch + ChannelOffSet - 2), (byte)value);
            else if (size == AttributeBitSize.Size16)//16bit
            {
                output.Set16BitDmx(_universe, (Int16)(_dmxPatch + ChannelOffSet - 2), value);
            
            }
        }

        /// <summary>
        /// Locates the current fixture
        /// </summary>
        public void LocateFixture()
        {
            //if the fixture has a possition value
            if (_personality.Possition != null)
            {
                //set shape to none movement
                _shapeMovement = new ShapeMovements.NoneShapeMovement(this);
                //set them to there on possition
                Pan = _personality.Possition.PanOnValue;
                Tilt = _personality.Possition.TiltOnValue;
            }

            //Locate Attributes
            _attributes.LocateAttributes();
        }

        /// <summary>
        /// Checks the Pan, Tilt value if in range
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool CheckPanTiltValue(int value)
        {
            return ((value >= -1) && (value <= _personality.Possition.MaxValuePanTilt));
        }

        /// <summary>
        /// Turns the lamp on the fixture
        /// </summary>
        public void LampOn()
        {
            //Set "Lamp On" on  Attributes
            _attributes.SetLampOn();
        }
        /// <summary>
        /// Clears all the fixture values
        /// </summary>
        public void ClearValues()
        {
            //if the fixture has a possition value
            if (_personality.Possition != null)
            {
                //set to none movement
                _shapeMovement = new ShapeMovements.NoneShapeMovement(this);

                //set them to -1
                Pan = -1;
                Tilt = -1;
            }
            //clear Attributes
            _attributes.ClearValues();

        }
    }
}
