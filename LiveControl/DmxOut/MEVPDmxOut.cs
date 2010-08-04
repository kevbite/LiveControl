using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.IDmxOut.ArtNet;
using Kevsoft.LiveControl.Interfaces;
using System.Runtime.InteropServices;
using Kevsoft.LiveControl.Properties;
using System.IO;
using System.Diagnostics;
using System.Reflection;
namespace Kevsoft.LiveControl
{
    /// <summary>
    /// Output for Sunlite Magic 3D EasyView
    /// </summary>
    public class MEVPDmxOut : IDmxObserver
    {
        #region "Constants"
        // DasMevCommand parameters
        /// <summary>
        /// Closes the visualiser
        /// </summary>
        const int MEVP_CLOSE_VISUALIZER = 0;
        /// <summary>
        /// Sets the Language
        /// </summary>
        const int MEVP_SET_LANGUAGE= 1;
        /// <summary>
        /// Read from patch
        /// </summary>
        const int MEVP_READ_PATCH = 2;

        #endregion

        #region "API calls"
        /// <summary>
        /// Loads the specified module into the address space of the calling process.
        /// </summary>
        /// <param name="lpFileName">Module to load</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr LoadLibrary(string lpFileName);
        /// <summary>
        /// Starts Magic 3D EasyView Visuliser
        /// </summary>
        /// <param name="sController"></param>
        /// <param name="sPassWrd"></param>
        /// <returns></returns>
        [DllImport("MEVP.dll")]
        static extern int DasMevStart(string sController, string sPassWrd);
        /// <summary>
        /// Sends commands to the Dll
        /// </summary>
        /// <param name="iType"></param>
        /// <param name="iParam"></param>
        /// <returns></returns>
        [DllImport("MEVP.dll")]
        static extern int DasMevCommand(int iType, int iParam);
        /// <summary>
        /// Writes a universe to Magic 3D EasyView
        /// </summary>
        /// <param name="iUniverse">Universe</param>
        /// <param name="DmxArray">Dmx Values</param>
        /// <returns></returns>
        [DllImport("MEVP.dll")]
        static extern int DasMevWriteDmx(int iUniverse, byte[] DmxArray);
        /// <summary>
        /// Gets the Dll Version
        /// </summary>
        /// <returns></returns>
        [DllImport("MEVP.dll")]
        static extern int DasMevGetVersion();

        #endregion

        /// <summary>
        /// Stores the output in which this dmx observer is attached to
        /// </summary>
        DmxOutput _output;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MEVPDmxOut()
        {
            //if the file doesnt exist then throw File Not Found Exception
            if (!File.Exists(Settings.Default.MEVP_Path)) throw new FileNotFoundException("Cannot find MEVP.dll"); 
            //Load the libary path of MEVP
            LoadLibrary(Settings.Default.MEVP_Path);
            //Change current Director to the MEVP path as MEVP wont start
            Environment.CurrentDirectory = Path.GetDirectoryName(Settings.Default.MEVP_Path);
            //Start the Application
            if (!StartVisualizer()) throw new Exception("MEVP not started");
            //set the path back to the normal app path
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


        }
        /// <summary>
        /// Starts the Visualizer
        /// </summary>
        /// <returns></returns>
        private static bool StartVisualizer()
        {
            return DasMevStart("LiveControl", "HkjArk") != 0;
        }
        /// <summary>
        /// Gets the DLL Version
        /// </summary>
        public int DllVersion
        {
            get{return DasMevGetVersion();}

        }
        /// <summary>
        /// Closes the Visualizer
        /// </summary>
        /// <returns></returns>
        public bool CloseVisualizer()
        {
            return (DasMevCommand(MEVP_CLOSE_VISUALIZER, 0)==1);
        }

        #region IDmxObserver Members

        /// <summary>
        /// Gets or Sets the dmx output that this class will use
        /// </summary>
        public DmxOutput Output
        {
            get{return _output;}
            set{_output = value;}
        }

        /// <summary>
        /// Closes the visualizer
        /// </summary>
        /// <returns></returns>
        public bool CloseConnection()
        {
            return CloseVisualizer();
        }

        /// <summary>
        /// Updates the values to this device
        /// </summary>
        public void UpdateValues()
        {
            //Loops though every universe
            for (byte universe = 0; universe < DmxOutput.MaxUniverses; universe++)
            {
                //get the universes data
                byte[] DMXData = _output.GetDmxUniverse(universe);
                //Send out the data to MEV
                if (DasMevWriteDmx(universe, DMXData) == 0)
                {
                    //if it could not be send write to trace
                    Trace.Write("Universe " + universe + " could not be sent to MEVP");
                }
            }
        }
        #endregion
    }
}
