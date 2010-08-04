using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.FixtureClasses;
using Kevsoft.LiveControl.Interfaces;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kevsoft.LiveControl
{
    /// <summary>
    /// Hold data about a show
    /// </summary>
    [Serializable]
    class Show
    {
        /// <summary>
        /// The instance of Show
        /// </summary>
        private static Show _instance = new Show();
        /// <summary>
        /// The Title of the show
        /// </summary>
        private String _title;
        /// <summary>
        /// Description of the show
        /// </summary>
        private String _description;
        /// <summary>
        /// Personalities used within the show
        /// </summary>
        public PersonalitiesBank PersonalitiesBank { get; set; }
        /// <summary>
        /// the fixture patch of the show
        /// </summary>
        public FixturePatch FixPatch { get; set; }
        /// <summary>
        /// the Light Programs list
        /// </summary>
        public LightProgramList ProgramList { get; set; }

        /// <summary>
        /// Gets the Instance of Show
        /// </summary>
        public static Show Instance
        {
            get{return _instance;}
        }

        /// <summary>
        /// Private Constructor
        /// </summary>
        private Show()
        {
            //setup a default show
            _title = "Untitled Show";
            _description = "";
            FixPatch = new FixturePatch();
            ProgramList = new LightProgramList();
            PersonalitiesBank = new PersonalitiesBank();
        }

        /// <summary>
        /// Gets or Sets the Title of the Show
        /// </summary>
        public String Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }
        /// <summary>
        /// Gets or Sets the Description of the show
        /// </summary>
        public String Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        /// <summary>
        /// Loads a show from file
        /// </summary>
        /// <param name="filePath">the file path</param>
        static internal void LoadShow(string filePath)
        {
            //if patch is not null
            if (_instance.FixPatch != null)
                //stop the movement thread first
                _instance.FixPatch.StopShapeMovementsThread();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            _instance = (Show)formatter.Deserialize(stream);
            stream.Close();
            //Start the new ShapeMovement Timer
            _instance.FixPatch.StartMovementThread();
        }
        /// <summary>
        /// Creates a new Show
        /// </summary>
        internal void CreateNewShow()
        {
            //if patch is not null
            if (FixPatch != null)
                //stop the movement thread first
                FixPatch.StopShapeMovementsThread();
            _instance = new Show();
        }
        /// <summary>
        /// Saves a show to file
        /// </summary>
        /// <param name="filePath">the file path</param>
        internal void SaveShow(string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this);
            stream.Close();
        }
        /// <summary>
        /// Stops all the Light Programs running
        /// </summary>
        internal void StopPrograms()
        {
            //for each program
            foreach (ILightProgram program in ProgramList)
                //Stop it
                program.Stop();
        }
    }
}
