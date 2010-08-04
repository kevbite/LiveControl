using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Kevsoft.LiveControl.FixtureClasses;

namespace Kevsoft.LiveControl.Tests
{
    [TestFixture]
    public class FixtureTest
    {
        Fixture _fixWith8bitAttribute;
        Fixture _fixWith16bitAttribute;
        Fixture _fixWith8bitPosition;
        Fixture _fixWith16bitPosition;

        #region "Startup and Stopping Methods"

        /// <summary>
        /// Called before running test on fixture
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            //Create some fixtures with the patch of
            // 1 to 1 = _fixWith8bitAttribute
            // 2 to 3 = _fixWith16bitAttribute
            // 4 to 5 = _fixWith8bitPosition
            // 6 to 9 = _fixWith16bitPosition
            _fixWith8bitAttribute = new Fixture(create8bitAttributePersonality(), 1, 0);
            _fixWith16bitAttribute = new Fixture(create16bitAttributePersonality(), 2, 0);
            _fixWith8bitPosition = new Fixture(create8bitPositionPersonality(), 4, 0);
            _fixWith16bitPosition= new Fixture(create16bitPositionPersonality(), 6, 0);
        }

        /// <summary>
        /// After Tests have finished we need to stop everything
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            //Close the output Thread
            DmxOutput.Instance.StopOutputThread();
        }


        private Personality create16bitPositionPersonality()
        {
            //Create a personality
            Personality per = new Personality();

            //create a position for the personality
            PersonalityPosition position = new PersonalityPosition();
            position.BitSize = Kevsoft.LiveControl.PersonalityClasses.AttributeBitSize.Size16;
            position.PanChannel = 1;
            position.TiltChannel = 3;
            position.PanOnValue = 32767;
            position.TiltOnValue=32767;
            
            //set the position
            per.Possition = position;
            
            return per;     
        }

        private Personality create8bitPositionPersonality()
        {
            //Create a personality
            Personality per = new Personality();

            //create a position for the personality
            PersonalityPosition position = new PersonalityPosition();
            position.BitSize = Kevsoft.LiveControl.PersonalityClasses.AttributeBitSize.Size8;
            position.PanOnValue=127;
            position.TiltOnValue=127;
            
            //set the position
            per.Possition = position;
            
            return per;
        }

        private Personality create16bitAttributePersonality()
        {
            //Create a personality
            Personality per = new Personality();

            //Create an Personality Attribute
            PersonalityAttribute att1 = new PersonalityAttribute();
            //Set the properties of Attribute
            att1.BitSize = Kevsoft.LiveControl.PersonalityClasses.AttributeBitSize.Size16;
            att1.OffSetChannel = 1;
            att1.OnValue = 10;
            att1.LampOnValue = 200;
            att1.Type= PersonalityAttribute.AttributeType.Dimmer;

            //add the attribute
            per.Attributes.Add(att1);

            //return personality
            return per;
        }

        private Personality create8bitAttributePersonality()
        {
            //Create a personality
            Personality per = new Personality();

            //Create an Personality Attribute
            PersonalityAttribute att1 = new PersonalityAttribute();
            //Set the properties of Attribute
            att1.BitSize = Kevsoft.LiveControl.PersonalityClasses.AttributeBitSize.Size8;
            att1.OffSetChannel = 1;
            att1.LampOnValue = 200;
            att1.OnValue = 10;
            att1.Type = PersonalityAttribute.AttributeType.Dimmer;

            //add the attribute
            per.Attributes.Add(att1);

            //return personality
            return per;

        }

        #endregion



        /// <summary>
        /// Test to see if value changes when fixture is located
        /// </summary>
        [Test]
        public void Locate8BitFixtureTest()
        {
            _fixWith8bitAttribute.LocateFixture();
            Assert.AreEqual(_fixWith8bitAttribute.Attributes[0].PersonalityAttribute.OnValue,
                             _fixWith8bitAttribute.Attributes[0].Value, "Locate 8bit Fixture Failed");
        }

        /// <summary>
        /// Test to see if value changes when fixture is located
        /// </summary>
        [Test]
        public void Locate16BitFixtureTest()
        {
            _fixWith16bitAttribute.LocateFixture();
            Assert.AreEqual(_fixWith16bitAttribute.Attributes[0].PersonalityAttribute.OnValue,
                             _fixWith16bitAttribute.Attributes[0].Value, "Locate 16bit Fixture Failed");
        }

        /// <summary>
        /// Test to see if value changes when fixture is located
        /// </summary>
        [Test]
        public void Locate8BitPositionFixtureTest()
        {
            _fixWith8bitPosition.LocateFixture();
            Assert.AreEqual(_fixWith8bitPosition.Personality.Possition.PanOnValue,
                             _fixWith8bitPosition.Pan, "Locate 8bit Fixture Position Failed - Pan");

            Assert.AreEqual(_fixWith8bitPosition.Personality.Possition.TiltOnValue,
                             _fixWith8bitPosition.Tilt, "Locate 8bit Fixture Position Failed - Tilt");
        }

        /// <summary>
        /// Test to see if value changes when fixture is located
        /// </summary>
        [Test]
        public void Locate16BitPositionFixtureTest()
        {
            _fixWith16bitPosition.LocateFixture();
            Assert.AreEqual(_fixWith16bitPosition.Personality.Possition.PanOnValue,
                             _fixWith16bitPosition.Pan, "Locate 16bit Fixture Position Failed - Pan");

            Assert.AreEqual(_fixWith16bitPosition.Personality.Possition.TiltOnValue,
                             _fixWith16bitPosition.Tilt, "Locate 16bit Fixture Position Failed - Tilt");
        }   
                
        /// <summary>
        /// Test to see if value changes when fixture is "LampOn"
        /// </summary>
        [Test]
        public void LampOn8BitFixtureTest()
        {
            _fixWith8bitAttribute.LampOn();
            Assert.AreEqual(_fixWith8bitAttribute.Attributes[0].PersonalityAttribute.LampOnValue,
                             _fixWith8bitAttribute.Attributes[0].Value, "LampOn 8bit Fixture Failed");
        }

        /// <summary>
        /// Test to see if value changes when fixture is "LampOn"
        /// </summary>
        [Test]
        public void LampOn16BitFixtureTest()
        {
            _fixWith16bitAttribute.LampOn();
            Assert.AreEqual(_fixWith16bitAttribute.Attributes[0].PersonalityAttribute.LampOnValue,
                             _fixWith16bitAttribute.Attributes[0].Value, "LampOn 16bit Fixture Failed");
        }

        /// <summary>
        /// Test to see if value changes when fixture is ValueCleared
        /// </summary>
        [Test]
        public void ClearValue8BitFixtureTest()
        {
            _fixWith8bitAttribute.ClearValues();
            Assert.AreEqual(-1,
                             _fixWith8bitAttribute.Attributes[0].Value, "LampOn 8bit Fixture Failed");
        }

        /// <summary>
        /// Test to see if value changes when fixture is ValueCleared
        /// </summary>
        [Test]
        public void ClearValue16BitFixtureTest()
        {
            _fixWith16bitAttribute.ClearValues();
            Assert.AreEqual(-1,
                             _fixWith16bitAttribute.Attributes[0].Value, "LampOn 16bit Fixture Failed");
        }

        /// <summary>
        /// Test to see if setting a valid position on a 16bit fixture works
        /// </summary>
        [Test]
        public void SetValid16BitPositionTest()
        {
            _fixWith16bitPosition.Pan = 400;
            _fixWith16bitPosition.Tilt = 500;

            Assert.AreEqual(400, _fixWith16bitPosition.Pan, "Setting Pan Value failed");
            Assert.AreEqual(500, _fixWith16bitPosition.Tilt, "Setting Tilt Value failed");
        }

        /// <summary>
        /// Test to see if setting a invalid position on a 16bit fixture doesnt change the value
        /// </summary>
        [Test]
        public void SetInValid16BitPositionTest()
        {
            _fixWith16bitPosition.Pan = 70000;
            _fixWith16bitPosition.Tilt = 70000;

            Assert.AreNotEqual(70000, _fixWith16bitPosition.Pan, "Setting Pan Value failed");
            Assert.AreNotEqual(70000, _fixWith16bitPosition.Tilt, "Setting Tilt Value failed");
        }

        /// <summary>
        /// Test to see if setting a valid position on a 8bit fixture works
        /// </summary>
        [Test]
        public void SetValid8BitPositionTest()
        {

            _fixWith8bitPosition.Pan = 100;
            _fixWith8bitPosition.Tilt = 200;

            Assert.AreEqual(100, _fixWith8bitPosition.Pan, "Setting Pan Value failed");
            Assert.AreEqual(200, _fixWith8bitPosition.Tilt, "Setting Tilt Value failed");
        }

        /// <summary>
        /// Test to see if setting a invalid position on a 8bit fixture doesnt change the value
        /// </summary>
        [Test]
        public void SetInValid8BitPositionTest()
        {
            _fixWith8bitPosition.Pan = 500;
            _fixWith8bitPosition.Tilt = 500;

            Assert.AreNotEqual(500, _fixWith8bitPosition.Pan, "Setting Pan Value failed");
            Assert.AreNotEqual(500, _fixWith8bitPosition.Tilt, "Setting Tilt Value failed");
        }

        /// <summary>
        /// Test to see if the DmxOutput channel changes when a fixture value changes
        /// </summary>
        [Test] 
        public void TestDmxOutputWith8bitAttribute()
        {
            _fixWith8bitAttribute.Attributes[0].Value = 100;
            Assert.AreEqual(100, DmxOutput.Instance.GetDMXValue(0,0));
        }
        /// <summary>
        /// Test to see if the DmxOutput channel changes when a fixture value changes
        /// </summary>
        [Test] 
        public void TestDmxOutputWith16bitAttribute()
        {
            //As its a 16bit take up 2 channels each
            //max for each channel is 255
            //channel output should be
            // Ch | Val
            //----+-------
            // 2  | 244
            // 3  | 1

            _fixWith16bitAttribute.Attributes[0].Value = 500;
            //Check first channel
            Assert.AreEqual(244, DmxOutput.Instance.GetDMXValue(0, 1));
            //Check second channel
            Assert.AreEqual(1, DmxOutput.Instance.GetDMXValue(0, 2));
        }
        /// <summary>
        /// Test to see if the DmxOutput channel changes when a fixture value changes
        /// </summary>
        [Test] 
        public void TestDmxOutputWith8bitPosition()
        {
            _fixWith8bitPosition.Pan = 100;
            _fixWith8bitPosition.Tilt = 100;
            //Check first channel
            Assert.AreEqual(100, DmxOutput.Instance.GetDMXValue(0, 3));
            //Check second channel
            Assert.AreEqual(100, DmxOutput.Instance.GetDMXValue(0, 4));
        }
        /// <summary>
        /// Test to see if the DmxOutput channel changes when a fixture value changes
        /// </summary>
        [Test]
        public void TestDmxOutputWith16bitPosition()
        {
            _fixWith16bitPosition.Pan = 1000;
            _fixWith16bitPosition.Tilt = 1000;

            //As its a 16bit position pan and tilt take up 2 channels each
            //max for each channel is 255
            //channel output should be
            // Ch | Val
            //----+-------
            // 6  | 232
            // 7  | 3
            // 8  | 232
            // 9  | 3

            //Check first channel pan
            Assert.AreEqual(232, DmxOutput.Instance.GetDMXValue(0, 5));
            //Check second channel fine pan
            Assert.AreEqual(3, DmxOutput.Instance.GetDMXValue(0, 6));
            //Check first channel tilt
            Assert.AreEqual(232, DmxOutput.Instance.GetDMXValue(0, 7));
            //Check second channel fine tilt
            Assert.AreEqual(3, DmxOutput.Instance.GetDMXValue(0, 8));
        }
    }
}
