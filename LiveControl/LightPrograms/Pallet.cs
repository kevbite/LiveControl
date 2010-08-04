using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.FixtureClasses;

namespace Kevsoft.LiveControl.LightPrograms
{
    /// <summary>
    /// stores values for a pallet
    /// </summary>
    [Serializable]
    public class Pallet : Kevsoft.LiveControl.Interfaces.ILightProgram
    {
        /// <summary>
        /// Values of the pallet
        /// </summary>
        private List<StoredFixtureValues> _fixturesValues;
        /// <summary>
        /// Name of the Pallet
        /// </summary>
        private string _name = "untitled";
        /// <summary>
        /// Description of the pallet
        /// </summary>
        private string _description;
        /// <summary>
        /// Constructor to create a pallet
        /// </summary>
        /// <param name="patch"></param>
        public Pallet(FixtureClasses.FixturePatch patch)
        {
            _fixturesValues = new List<StoredFixtureValues>();
            //loop though the whole patch
            foreach (Fixture fix in patch)
                //add Fixture Values for each fixture
                _fixturesValues.Add(new StoredFixtureValues(fix));

        }
        /// <summary>
        /// Gets or Sets the name of the pallet
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Gets or Sets the Description of the pallet
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// runs the pallet
        /// </summary>
        /// <param name="aType"></param>
        public void Run(PersonalityAttribute.AttributeType aType)
        {
            //Go though all the fix and execute them
            foreach (StoredFixtureValues fixVals in _fixturesValues)
                fixVals.ExecuteValues(aType);

        }
        /// <summary>
        /// Runs the pallet on certain fixtures
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="fixtures">the fixture to run the pallet on</param>
        public void Run(PersonalityAttribute.AttributeType attributeType, List<Fixture> fixtures)
        {
            //Go though all the fix and execute them
            foreach (StoredFixtureValues fixVals in _fixturesValues)
                if (fixtures.Contains(fixVals.Fixture))
                    fixVals.ExecuteValues(attributeType);

        }
        /// <summary>
        /// Does nothing as a pallet cannot be stopped
        /// </summary>
        public void Stop()
        {
            //Pallets dont need stopping
        }

        #region ILightProgram Members




        #endregion
    }
}
