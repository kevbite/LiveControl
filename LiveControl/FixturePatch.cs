using System;
using System.Collections;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl;
using System.Collections.Generic;
using System.Threading;
namespace Kevsoft.LiveControl.FixtureClasses
{
    [Serializable]
    public class FixturePatch : List<Fixture>
    {
        /// <summary>
        /// the timer that is used for do the shape movements
        /// </summary>
        [NonSerialized]
        private Timer _shapeMovementTimer;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FixturePatch()
            : base()
        {
            StartMovementThread();
        }

        public void StartMovementThread()
        {
            //Start the timer
            _shapeMovementTimer = new Timer(RunFixtureMovements, null,
                        1000, Properties.Settings.Default.ShapeThreadSpeed);
        }
        /// <summary>
        /// Return a certain universe from the patch
        /// </summary>
        /// <param name="universe"></param>
        /// <returns></returns>
        internal System.Collections.Generic.IEnumerable<Fixture> GetUniverse(byte universe)
        {
            //Create a temp list
            List<Fixture> tmpList =  new List<Fixture>();
            //loop though each items in the current list
            foreach (Fixture f in this)
                //if has the same universe
                if (f.Universe == universe)
                    //add to the tmp list
                    tmpList.Add(f);

            //return the tmpList
            return tmpList;

        }

        /// <summary>
        /// runs all the fixtures move command in the show patch
        /// </summary>
        /// <param name="stateInfo"></param>
        public void RunFixtureMovements(Object stateInfo)
        {
            foreach (Fixture pF in this)
            {
                pF.ShapeMovement.Move();
            }
        }
        /// <summary>
        /// Stops the Shapemovement Timer
        /// </summary>
        internal void StopShapeMovementsThread()
        {
            if(_shapeMovementTimer!=null)
                //Dispose the Timer Thread
                _shapeMovementTimer.Dispose();
        }
    }
}
