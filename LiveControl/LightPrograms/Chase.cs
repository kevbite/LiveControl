using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Kevsoft.LiveControl.FixtureClasses;

namespace Kevsoft.LiveControl.LightPrograms
{
    /// <summary>
    /// Stores values for a chase
    /// </summary>
    [Serializable]
    class Chase : Kevsoft.LiveControl.Interfaces.ILightProgram
    {
        /// <summary>
        /// The Steps of the current chase
        /// </summary>
        private List<List<StoredFixtureValues>> _steps;
        /// <summary>
        /// The last step run in the chase
        /// </summary>
        private List<StoredFixtureValues> _lastStep;
        /// <summary>
        /// Name of the chase
        /// </summary>
        private string _name = "untitled";
        /// <summary>
        /// Description of the chase
        /// </summary>
        private string _description;
        /// <summary>
        /// The interval between steps
        /// </summary>
        private TimeSpan _intervalTimeSpan;
        /// <summary>
        /// the current step
        /// </summary>
        private int _currentStep;
        /// <summary>
        /// timer used for changing steps
        /// </summary>
        [NonSerialized]
        private Timer _timer;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Chase()
        {
            _steps = new List<List<StoredFixtureValues>>();
            _intervalTimeSpan = new TimeSpan(0,0,0,0,500);
        }
        /// <summary>
        /// Gets or Sets the name of the chase
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Gets or Sets the description of the chase
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// Gets the number of steps in the chase
        /// </summary>
        public int NumberOfSteps
        {
            get { return _steps.Count; }
        }
        /// <summary>
        /// Add the step to the chase
        /// </summary>
        /// <param name="patch">the current output to copy</param>
        public void AddStep(FixtureClasses.FixturePatch patch)
        {
            //Create a list of the current values
            List<StoredFixtureValues> stepValues = new List<StoredFixtureValues>();
            //loop though the whole patch
            foreach (Fixture fix in patch)
                //add Fixture Values for each fixture
                stepValues.Add(new StoredFixtureValues(fix));
            AddStep(stepValues);
        }
        /// <summary>
        /// Adds a step to chase
        /// </summary>
        /// <param name="stepValues"></param>
        public void AddStep(List<StoredFixtureValues> stepValues)
        {
            _steps.Add(stepValues);
        }
        /// <summary>
        /// Gets or Sets the Interval time
        /// </summary>
        public TimeSpan IntervalSpeed
        {
            get{return _intervalTimeSpan;}
            set { _intervalTimeSpan = value; if(_timer!=null) _timer.Change(new TimeSpan(), value); }
        }
        /// <summary>
        /// Runs the Chase
        /// </summary>
        /// <param name="aType"></param>
        public void Run(PersonalityAttribute.AttributeType aType)
        {
            //set current step to 0
            _currentStep = 0;
            //set last step to null
            _lastStep = null;
            //start the timer
            _timer = new Timer(RunNextStep, null, new TimeSpan(), _intervalTimeSpan);
        }

        /// <summary>
        /// Runs the Chase
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="fixtures"></param>
        public void Run(PersonalityAttribute.AttributeType attributeType, List<Fixture> fixtures)
        {
            //Call the other Run method as the fixture select
            //makes no diffrence to a chase
            Run(attributeType);
        }
        /// <summary>
        /// Runs the next step
        /// </summary>
        /// <param name="stateInfo"></param>
        public void RunNextStep(Object stateInfo)
        {
            //Go though all the fix in the current step and execute them
            for (int i = 0; i <= _steps[_currentStep].Count - 1; i++)
            {
                StoredFixtureValues fixVals = _steps[_currentStep][i];
                if (_lastStep == null)
                    fixVals.ExecuteValues();
                else
                    fixVals.ExecuteValues(_lastStep[i]);
            }
            //Adds the next step
            AddNextStep();
        }
        /// <summary>
        /// Adds the next step to the chase
        /// </summary>
        private void AddNextStep()
        {
            //set this current step as last step
            _lastStep = _steps[_currentStep];

            //if on last step
            if (_currentStep >= _steps.Count-1)
            {
                //set back to start
                _currentStep = 0;
            }
            else//if not last step in list
            {
                //Add one to the current step
                _currentStep += 1;
            }
        }
        /// <summary>
        /// Stops the chase
        /// </summary>
        public void Stop()
        {
            //check see if timer is not null
            if(_timer!=null)
                _timer.Dispose();
            _timer = null;
        }

        /// <summary>
        /// Removes the last step from the chase
        /// </summary>
        internal void RemoveLastStep()
        {
            //Remove the last step from the list
            _steps.RemoveAt(_steps.Count - 1);
        }


    }
}
