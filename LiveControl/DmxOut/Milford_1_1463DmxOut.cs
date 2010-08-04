using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Kevsoft.LiveControl.Properties;
namespace Kevsoft.LiveControl.DmxOut
{
    /// <summary>
    /// Dmx Output for Milford instrument 1-1463 Com port device
    /// </summary>
    class Milford_1_1463DmxOut : Interfaces.IDmxObserver
    {
        /// <summary>
        /// The maximum channels the milford 1-1463 supports
        /// </summary>
        public const int MaxChannels = 112;
        /// <summary>
        /// Serial port for communicating to the milford device
        /// </summary>
        private SerialPort _serialPort;
        /// <summary>
        /// Default constructor
        /// </summary>
        public Milford_1_1463DmxOut()
        {
            //gets the port from the settings
            string port = Settings.Default.Milford1463Port;
            //create a new Serial Port Object
            _serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
            //Open the com port
            _serialPort.Open();

        }
        /// <summary>
        /// Closes the Comport
        /// </summary>
        /// <returns>if the com port is not open</returns>
        private bool CloseComPort()
        {
            _serialPort.Close();
            return !_serialPort.IsOpen;
        }
        private void SendDmxToCom()
        {

            //Get the first universe
            byte[] data = _output.GetDmxUniverse(0);

            //write data upto max channels
            for (int i = 0; i < MaxChannels; i++)
            {
                //write data (ch,val)
                _serialPort.Write(new char[]{
                    Convert.ToChar(i + 1),Convert.ToChar(data[i]) },
                                    0,2);
            }
            

        }
        #region IDmxObserver Members
        private DmxOutput _output;
        /// <summary>
        /// Gets or Sets the output associated with this device
        /// </summary>
        public DmxOutput Output
        {
            get{ return _output; }
            set{_output = value;}
        }
        /// <summary>
        /// Update the values on thsi device
        /// </summary>
        public void UpdateValues()
        {
            //if the com port is open
            if (_serialPort.IsOpen)
                //send dmx data to com port
                SendDmxToCom();
        }
        /// <summary>
        /// Closes the connection to this device
        /// </summary>
        /// <returns></returns>
        public bool CloseConnection()
        {
            return CloseComPort();
        }

        #endregion
    }
}
