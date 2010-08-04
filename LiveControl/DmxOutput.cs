using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kevsoft.LiveControl
{
    /// <summary>
    /// DmxOutput stores the dmx values and updates the DmxObserver attached to the output
    /// </summary>
    public class DmxOutput
    {
        /// <summary>
        /// lock used for output
        /// </summary>
        private object _outputPadlock = new object();
        /// <summary>
        /// The observers to add on next output loop
        /// </summary>
        private List<Interfaces.IDmxObserver> _observersToAdd = new List<Interfaces.IDmxObserver>();
        /// <summary>
        /// The observers to remove on next output loop
        /// </summary> 
        private List<Interfaces.IDmxObserver> _observersToRemove = new List<Interfaces.IDmxObserver>();
        /// <summary>
        /// The instance of DmxOutput
        /// </summary>
        private static DmxOutput _instance =  new DmxOutput();
        /// <summary>
        /// Max number of universe on this output
        /// </summary>
        public const int MaxUniverses = 4;
        /// <summary>
        /// List of all the observers that are connected to the output
        /// </summary>
        private List<Interfaces.IDmxObserver> _observers = new List<Interfaces.IDmxObserver>();
        /// <summary>
        /// Dmx Data
        /// </summary>
        private byte[][] _dmxValues;
        /// <summary>
        /// Thread which tells the Observers to update there values
        /// </summary>
        private Thread _updateObserverThread;
        /// <summary>
        /// the interval value between updates to the observers
        /// </summary>
        private int _updateObserverThreadInterval;

        #region "Thread Worker"

        /// <summary>
        /// Lock covering stopping and stopped
        /// </summary>
        readonly object stopLock = new object();
        /// <summary>
        /// Whether or not the worker thread has been asked to stop
        /// </summary>
        bool stopping = false;
        /// <summary>
        /// Whether or not the worker thread has stopped
        /// </summary>
        bool stopped = false;

        /// <summary>
        /// Returns whether the worker thread has been asked to stop.
        /// This continues to return true even after the thread has stopped.
        /// </summary>
        public bool Stopping
        {
            get
            {
                lock (stopLock)
                {
                    return stopping;
                }
            }
        }

        /// <summary>
        /// Returns whether the worker thread has stopped.
        /// </summary>
        public bool Stopped
        {
            get
            {
                lock (stopLock)
                {
                    return stopped;
                }
            }
        }

        /// <summary>
        /// Tells the worker thread to stop, typically after completing its 
        /// current work item. (The thread is *not* guaranteed to have stopped
        /// by the time this method returns.)
        /// </summary>
        public void StopOutputThread()
        {
            lock (stopLock)
            {
                stopping = true;
            }
        }

        /// <summary>
        /// Called by the worker thread to indicate when it has stopped.
        /// </summary>
        void SetStopped()
        {
            lock (stopLock)
            {
                stopped = true;
            }
        }

        #endregion

        /// <summary>
        /// Static constructor
        /// </summary>
        static DmxOutput() 
        {}
        /// <summary>
        /// Default constructor
        /// </summary>
        private DmxOutput()
        {
            //set the thread interval from settings file
            _updateObserverThreadInterval = Properties.Settings.Default.OutputThreadSpeed;
            //create the new byte array to store output
            _dmxValues = new byte[MaxUniverses][];
            //loup though each array and create an array of 512
            for(int i=0;i<_dmxValues.Length; i++)
                _dmxValues[i] = new byte[512];

            //create the update observer thread
            _updateObserverThread = new Thread(UpdateObservers);
            //set the name so its easier to debug
            _updateObserverThread.Name = "Update Observers Thread";
            //start the thread
            _updateObserverThread.Start();
        }
        /// <summary>
        /// Gets the Instance of DmxOutput
        /// </summary>
        public static DmxOutput Instance
        {
            get{ return _instance; }
        }
        /// <summary>
        /// Set a value of a dmx channel
        /// </summary>
        /// <param name="universe">universe to set value on</param>
        /// <param name="channel">channel to set value on</param>
        /// <param name="value">value of channel</param>
        public void SetDmx(byte universe, Int16 channel, byte value)
        {
            //set the dmx chased output
            _dmxValues[universe][channel] = value;
        }
        
        /// <summary>
        /// Gets all the dmx values
        /// </summary>
        public Byte[][] DmxValues
        {
            get{return _dmxValues;}
        }
        /// <summary>
        /// Gets a certain universe of dmx data
        /// </summary>
        /// <param name="universe">Universe wish to get</param>
        /// <returns></returns>
        public Byte[] GetDmxUniverse(byte universe)
        {
            return _dmxValues[universe];
        }
        /// <summary>
        /// Gets a dmx channel value value
        /// </summary>
        /// <param name="universe">universe of the channel</param>
        /// <param name="channel">the channel</param>
        /// <returns></returns>
        public byte GetDMXValue(byte universe, Int16 channel)
        {
            return _dmxValues[universe][channel];
        }


        /// <summary>
        /// Attaches an IDmxObserver object
        /// </summary>
        /// <param name="observer">the observer to attach</param>
        public void Attach(Interfaces.IDmxObserver observer)
        {
            _observersToAdd.Add(observer);
        }
        /// <summary>
        /// Detaches an observer from the output
        /// </summary>
        /// <param name="observer">The observer we wish to detach</param>
        public void Detach(Interfaces.IDmxObserver observer)
        {
            //if item hasnt been added yet
            if(_observersToAdd.Contains(observer))
                //remove it from to add list
                _observersToAdd.Remove(observer);
            else
                //add it to the remove list
                _observersToRemove.Add(observer);

        }
        /// <summary>
        /// Tells all the observers in the list to update them selves
        /// </summary>
        public void UpdateObservers()
        {
           try
            {
                while (!Stopping)//while we not wanting to stop
                {
                    try
                    {
                        lock (_outputPadlock)
                        {
                            //loop though each observer
                            foreach (Interfaces.IDmxObserver o in _observers)
                            {
                                //tell the observer to update
                                o.UpdateValues();
                            }
                            //atach the waiting observers
                            AttachWaitingObservers();
                            //detach the waiting observers
                            DetachWaitingObservers();
                        }
                    }
                    catch (InvalidOperationException)
                    { }
                    //Sleep
                    Thread.Sleep(_updateObserverThreadInterval);
                }
            }
           finally
           {
               //set the thread so it has been stopped
               SetStopped();
           }
        }
        /// <summary>
        /// Attaches all waiting observers
        /// </summary>
        private void DetachWaitingObservers()
        {
            while(_observersToRemove.Count != 0)
            {
                //Remove this object from observer
                _observersToRemove[0].Output = null;
                //Remove from list
                _observers.Remove(_observersToRemove[0]);

                _observersToRemove.RemoveAt(0);
            }

        }
        /// <summary>
        /// Detaches waiting observers
        /// </summary>
        private void AttachWaitingObservers()
        {
            while(_observersToAdd.Count!=0)
            {
                //Attach this object to the observer
                _observersToAdd[0].Output = this;
                //Add to list
                _observers.Add(_observersToAdd[0]);

                _observersToAdd.RemoveAt(0);
            }
        }

        internal int Get16BitDMXValue(byte universe, short channel)
        {
            int fullValue;
            //set the fullValue to the fist channel value value
            fullValue = GetDMXValue(universe, (Int16)(channel+1));
            //add the second channel to the value (ch2 is * 256
            fullValue += (GetDMXValue(universe, channel) * 256);

            return fullValue;
        }

        internal void Set16BitDmx(byte universe, short channel, int value)
        {
            SetDmx(universe, channel, (byte)(value / 256));
            SetDmx(universe, (short)(channel + 1), (byte)(value % 256));
        }
    }
}
