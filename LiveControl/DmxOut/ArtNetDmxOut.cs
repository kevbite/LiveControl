using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.IDmxOut.ArtNet;
using Kevsoft.LiveControl.Interfaces;
namespace Kevsoft.LiveControl.DmxOut
{
    /// <summary>
    /// The output for ArtNet
    /// </summary>
    public class ArtNetDmxOut : IDmxObserver
    {
        /// <summary>
        /// ArtNet object needed for sending data to artnet
        /// </summary>
        ArtNet artnet;
        /// <summary>
        /// The DMX output object that the observer is connect to
        /// </summary>
        DmxOutput _output;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ArtNetDmxOut()
        {
            //Creates an artnet object without an input
            artnet = new ArtNet(false);
        }

        #region IDmxObserver Members
        /// <summary>
        /// Gets and Sets the DmxOutput object which this output uses
        /// </summary>
        public DmxOutput Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
            }
        }
        /// <summary>
        /// Releases the connections uses for an ArtNet output
        /// </summary>
        /// <returns></returns>
        public bool CloseConnection()
        {
            //sets the artnet object to null
            artnet = null;
            //return true as everything was ok
            return true;
        }
        /// <summary>
        /// Updates the output with the current values in _output
        /// </summary>
        public void UpdateValues()
        {

            //Loop though every universe
            for (byte i = 0; i < DmxOutput.MaxUniverses; i++)
            {
                //Get the 2D array of the universe 
                byte[] DMXData = _output.GetDmxUniverse(i);
                //Send out the universe data to artnet
                artnet.SendDMX(i, DMXData, (short)DMXData.Length);

            }
        }

        #endregion

    }
}
