using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Kevsoft.LiveControl.DmxOut
{
    /// <summary>
    /// The Output for Das Hard Device
    /// </summary>
    class DasHardDmxOut : Interfaces.IDmxObserver
    {
        #region "Constants"

        /// <summary>
        /// Device ID 0
        /// </summary>
        const int DHC_SIUDI0 = 0;	// COMMAND
        /// <summary>
        /// Device ID 1
        /// </summary>
        const int DHC_SIUDI1 = 100;	// COMMAND
        /// <summary>
        /// Device ID 2
        /// </summary>
        const int DHC_SIUDI2 = 200;	// COMMAND
        /// <summary>
        /// Device ID 3
        /// </summary>
        const int DHC_SIUDI3 = 300;	// COMMAND
        /// <summary>
        /// Device ID 4
        /// </summary>
        const int DHC_SIUDI4 = 400;	// COMMAND
        /// <summary>
        /// Device ID 5
        /// </summary>
        const int DHC_SIUDI5 = 500;	// COMMAND
        /// <summary>
        /// Device ID 6
        /// </summary>
        const int DHC_SIUDI6 = 600;	// COMMAND
        /// <summary>
        /// Device ID 7
        /// </summary>
        const int DHC_SIUDI7 = 700;	// COMMAND
        /// <summary>
        /// Device ID 8
        /// </summary>
        const int DHC_SIUDI8 = 800;	// COMMAND
        /// <summary>
        /// Device ID 9
        /// </summary>
        const int DHC_SIUDI9 = 900;	// COMMAND

        const int DHC_OPEN = 1; // COMMAND
        const int DHC_CLOSE = 2;	// COMMAND
        const int DHC_DMXOUTOFF = 3;	// COMMAND
        const int DHC_DMXOUT = 4;		// COMMAND
        const int DHC_PORTREAD = 5;	// COMMAND
        const int DHC_PORTCONFIG = 6;		// COMMAND
        const int DHC_VERSION = 7;		// COMMAND
        const int DHC_DMXIN = 8;	// COMMAND
        const int DHC_INIT = 9;	// COMMAND
        const int DHC_EXIT = 10;	// COMMAND
        const int DHC_DMXSCODE = 11;	// COMMAND
        const int DHC_DMX2ENABLE = 12;	// COMMAND
        const int DHC_DMX2OUT = 13;	// COMMAND
        const int DHC_SERIAL = 14;	// COMMAND
        const int DHC_TRANSPORT = 15;	// COMMAND


        const int DHC_WRITEMEMORY = 21;// COMMAND
        const int DHC_READMEMORY = 22;	// COMMAND
        const int DHC_SIZEMEMORY = 23;		// COMMAND

        /// <summary>
        /// Command was ok
        /// </summary>
        const int DHE_OK = 1;	// RETURN NO ERROR
        /// <summary>
        /// Nothing for the device to do
        /// </summary>
        const int DHE_NOTHINGTODO = 2;	// RETURN NO ERROR

        /// <summary>
        /// Error on command
        /// </summary>
        const int DHE_ERROR_COMMAND = -1;	// RETURN ERROR
        /// <summary>
        /// Device not open
        /// </summary>
        const int DHE_ERROR_NOTOPEN = -2;	// RETURN ERROR
        /// <summary>
        /// Device already open
        /// </summary>
        const int DHE_ERROR_ALREADYOPEN = -12;// RETURN ERROR

        #endregion

        #region "API calls"
        /// <summary>
        /// Sends commands to the Das Hard DMX output driver
        /// </summary>
        /// <param name="command"></param>
        /// <param name="param"></param>
        /// <param name="bloc"></param>
        /// <returns></returns>
        [DllImport("DasHard2006.dll")]
        static extern int DasUsbCommand(int command, int param, byte[] bloc);

        #endregion
        /// <summary>
        /// Cached Dll Version
        /// </summary>
        private int _dllVersion;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DasHardDmxOut()
        {
            //Start the Dmx interface (ret is dll version)
            _dllVersion = DasUsbCommand(DHC_INIT, 0, null);
            //if _dllversion=0 means the device has not started
            if (_dllVersion == 0) throw new Exception("Device not started");

            //Open the connection
            if (DasUsbCommand(DHC_OPEN, 0, null) != DHE_OK)
                //If something went wrong throw an exception
                throw new Exception("Device could not open");

            //try to swop the second input to and output
            if (DasUsbCommand(DHC_DMX2ENABLE, 1, null) != DHE_OK)
                Trace.Write("DasHardDmxOut : DasUsbCommand(DHC_DMX2ENABLE, 1, null) : problem changing in to out");
        }

        /// <summary>
        /// Gets the firmware version of the device
        /// </summary>
        /// <returns></returns>
        public int GetFirmwareVersion()
        {
            return DasUsbCommand(DHC_VERSION, 0, null);
        }

        /// <summary>
        /// Gets the serial number of the device
        /// </summary>
        /// <returns></returns>
        public int GetSerialNumber()
        {
            return DasUsbCommand(DHC_SERIAL, 0, null);
        }
        /// <summary>
        /// Gets the DLL version
        /// </summary>
        /// <returns></returns>
        public int GetDLLVersion()
        {
            return _dllVersion;
        }

        #region IDmxObserver Members
        /// <summary>
        /// the dmx output that the object is connected to
        /// </summary>
        private DmxOutput _output;

        /// <summary>
        /// Gets or Sets the Dmx output that this class with use
        /// </summary>
        public DmxOutput Output
        {
            get{ return _output;}
            set{_output = value;}
        }

        /// <summary>
        /// Updates the values to the Das Hard Device
        /// </summary>
        public void UpdateValues()
        {
            int ret;
            //output to the first output on the device
            if ((ret = DasUsbCommand(DHC_DMXOUT, 512, _output.GetDmxUniverse(0))) != DHE_OK)
            {
                Trace.Write("Error outputing Das USB - DasUsbCommand ret " + ret);
            }

            //output to the second port
            if ((ret = DasUsbCommand(DHC_DMX2OUT, 512, _output.GetDmxUniverse(1))) != DHE_OK)
            {
                Trace.Write("Error output Das USB - DasUsbCommand ret " + ret);
            }

        }
        /// <summary>
        /// Closes the connection to the device
        /// </summary>
        /// <returns>True if close was ok</returns>
        public bool CloseConnection()
        {
            return (DasUsbCommand(DHC_CLOSE, 0, null) == DHE_OK);
        }

        #endregion
    }
}
