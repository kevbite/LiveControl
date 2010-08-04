using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.FixtureClasses;

namespace Kevsoft.LiveControl
{
    /// <summary>
    /// stores values of a current fixture
    /// </summary>
    [Serializable]
    class StoredFixtureValues
    {
        /// <summary>
        /// the fixture the values associate with
        /// </summary>
        private Fixture _fixture;
        /// <summary>
        /// the fixture movement
        /// </summary>
        private ShapeMovement _shapeMovement;
        /// <summary>
        /// the values of the attributes
        /// </summary>
        private int[] _attributesVals;
        /// <summary>
        /// the pan value
        /// </summary>
        private int _pan;
        /// <summary>
        /// the tilt value
        /// </summary>
        private int _tilt;

        #region "Properties"
        /// <summary>
        /// Gets or Sets the Fixture that connected with these values
        /// </summary>
        public Fixture Fixture
        {
            get { return _fixture; }
            set { _fixture = value; }
        }
        /// <summary>
        /// Gets or Sets the Shape Movement
        /// </summary>
        public ShapeMovement ShapeMovement
        {
            get { return _shapeMovement; }
            set { _shapeMovement = value; }
        }
        /// <summary>
        /// Gets or Sets the Attribute Values
        /// </summary>
        public int[] AttributesVals
        {
            get { return _attributesVals; }
            set { _attributesVals = value; }
        }
        /// <summary>
        /// Gets or Sets the Pan values
        /// </summary>
        public int Pan
        {
            get { return _pan; }
            set { _pan = value; }
        }
        /// <summary>
        /// Gets or Sets the Tilt Values
        /// </summary>
        public int Tilt
        {
            get { return _tilt; }
            set { _tilt = value; }
        }

        #endregion
        /// <summary>
        /// Constructor for creating FixtureValues with a given Fixture
        /// </summary>
        /// <param name="fix"></param>
        public StoredFixtureValues(Fixture fix)
        {
            //get fixture ref
            _fixture = fix;
            //create new array for values
            _attributesVals = new int[_fixture.Attributes.Count];

            //loop though each 
            for(int i = 0; i<=_attributesVals.Length-1; i++)
                _attributesVals[i] = _fixture.Attributes[i].Value;
            
            //Copy the shape movement using the Copy Constructor
            _shapeMovement = (ShapeMovement)Activator.CreateInstance(fix.ShapeMovement.GetType(), fix.ShapeMovement);

            //set the pan/tilt values (-1 if no possition)
            _pan = _fixture.Pan;
            _tilt = _fixture.Tilt;

        }
        /// <summary>
        /// Executes all the values
        /// </summary>
        public void ExecuteValues()
        {
            //loop though _attributesVals and change the value 
            for(int i = 0; i<=_attributesVals.Length-1; i++)
                //if value has been set
                if(_attributesVals[i]!=-1)
                    //Change the value
                    _fixture.Attributes[i].Value = _attributesVals[i];

            if (_pan != -1 && _shapeMovement is ShapeMovements.NoneShapeMovement)//if pan is set
                //Change the Pan Value
                _fixture.Pan = _pan;

            if (_tilt != -1 && _shapeMovement is ShapeMovements.NoneShapeMovement)//if tilt is set
                //Change the Tilt Value
                _fixture.Tilt = _tilt;

            //if diffrent from the shape that is playing
            if (!_shapeMovement.Equals(_fixture.ShapeMovement) && !(_shapeMovement is ShapeMovements.NoneShapeMovement))
                //Set the movement using a copy constructor
                _fixture.ShapeMovement = (ShapeMovement)Activator.CreateInstance(_shapeMovement.GetType(), _shapeMovement); ;


        }
        /// <summary>
        /// Executes the values with a given attributeType
        /// </summary>
        /// <param name="aType"></param>
        internal void ExecuteValues(PersonalityAttribute.AttributeType aType)
        {
            //loop though _attributesVals and change the value 
            for(int i = 0; i<=_attributesVals.Length-1; i++)
                //if value has been set
                if(_attributesVals[i]!=-1 && 
                    (aType == PersonalityAttribute.AttributeType.Dimmer 
                                 || _fixture.Attributes[i].Type == aType))
                    //Change the value
                    _fixture.Attributes[i].Value = _attributesVals[i];

            if (aType == PersonalityAttribute.AttributeType.Dimmer ||
                aType == PersonalityAttribute.AttributeType.Position)
            {
                if (_pan != -1 && _shapeMovement is ShapeMovements.NoneShapeMovement)//if pan is set
                    //Change the Pan Value
                    _fixture.Pan = _pan;

                if (_tilt != -1 && _shapeMovement is ShapeMovements.NoneShapeMovement)//if tilt is set
                    //Change the Tilt Value
                    _fixture.Tilt = _tilt;
                //if diffrent from the shape that is playing
                if (!_shapeMovement.Equals(_fixture.ShapeMovement))
                    //Set the movement using a copy constructor
                    _fixture.ShapeMovement = (ShapeMovement)Activator.CreateInstance(_shapeMovement.GetType(), _shapeMovement); ;
            }
        }

        /// <summary>
        /// Executes the values that are not the same as the compare values
        /// </summary>
        /// <param name="compareValues"></param>
        internal void ExecuteValues(StoredFixtureValues compareValues)
        {
            //if has no compare values
            if (compareValues == null)
                ExecuteValues();//just execute them all
            else
            {
            //loop though _attributesVals and change the value 
            for(int i = 0; i<=_attributesVals.Length-1; i++)
                //if value has been set and there not same value as compare values
                if (_attributesVals[i] != -1 && _attributesVals[i] != compareValues.AttributesVals[i])
                    //Change the value
                    _fixture.Attributes[i].Value = _attributesVals[i];

            //if pan is set and not same as compare values
            if (_pan != -1 && _pan != compareValues.Pan && _shapeMovement is ShapeMovements.NoneShapeMovement)
                //Change the Pan Value
                _fixture.Pan = _pan;
 
            //if tilt is set and not same as compare values
            if (_tilt != -1 && _tilt != compareValues.Tilt && _shapeMovement is ShapeMovements.NoneShapeMovement)
                //Change the Tilt Value
                _fixture.Tilt = _tilt;

            //if not the same shape that is playing and is diffrent from the compare values
            if((!_shapeMovement.Equals(_fixture.ShapeMovement)) 
                    && (!_shapeMovement.Equals(compareValues.ShapeMovement))
                    && !(_shapeMovement is ShapeMovements.NoneShapeMovement))
                //Set the movement using a copy constructor
                _fixture.ShapeMovement = (ShapeMovement)Activator.CreateInstance(_shapeMovement.GetType(), _shapeMovement); ;





            }
        }
    }
}
