using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Kevsoft.LiveControl.PersonalityClasses;

namespace Kevsoft.LiveControl
{
    /// <summary>
    /// Holds a Personality of a Fixture
    /// </summary>
    [Serializable]
    public class Personality
    {
        /// <summary>
        /// Fixture Name
        /// </summary>
        private string _fixtureName;
        /// <summary>
        /// Fixture Manufacture
        /// </summary>
        private string _fixtureManufacturer;
        /// <summary>
        /// Description of Personality
        /// </summary>
        private string _description;
        /// <summary>
        /// Image of Fixture
        /// </summary>
        private System.Drawing.Image _image;
        /// <summary>
        /// Position value of personality
        /// </summary>
        private PersonalityPosition _position = null;
        /// <summary>
        /// Attributes of personality
        /// </summary>
        private List<PersonalityAttribute> _attributes = null;


        /// <summary>
        /// Default Constructor - used for serialization
        /// </summary>
        public Personality()
        {
            //Set object values
            _fixtureName = "Name Not Set";
            _fixtureManufacturer = "Manufacturer Not Set";
            _description = "";
            _position = null;
            _attributes = new List<PersonalityAttribute>();
        }

        #region "Properties"
        /// <summary>
        /// Gets or Sets the Name of the Personality
        /// </summary>
        public String Name
        {
            get { return _fixtureName; }
            set { _fixtureName = value; }
        }
        /// <summary>
        /// Gets or Sets the Manufacture of the Personality
        /// </summary>
        public String Manufacturer
        {
            get { return _fixtureManufacturer; }
            set { _fixtureManufacturer = value; }
        }
        /// <summary>
        /// Gets or Sets the Description of the Personality
        /// </summary>
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// Gets the Channels used by this fixture personality
        /// </summary>
        public Int16 ChannelsUsed
        {
            get
            { 
                //somewhere to store the channels
                Int16 ch = 0;

                if(Possition!=null)//has possition attributes
                    if (Possition.BitSize == AttributeBitSize.Size8)//8bit
                        ch+=2;//add 2chs pan/tilt
                    else//16bit
                        ch+=4;//add 4 chs pan/tilt, fine pan/tilt

                //loop though each Attribute
                foreach(PersonalityAttribute attribute in Attributes)
                    if (attribute.BitSize == AttributeBitSize.Size8)//8bit
                        ch+=1;//add 1 chs 
                    else//16bit
                        ch+=2;//add 2 chs

                //return the amount of channels
                return ch;
            }
        }
        /// <summary>
        /// Gets or Sets the Possition value of this personality
        /// </summary>
        public PersonalityPosition Possition
        {
            get { return _position; }
            set { _position = value; }
        }
        /// <summary>
        /// Gets or Sets the Attribute values of this personality
        /// </summary>
        public List<PersonalityAttribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }
        /// <summary>
        /// Gets or Sets the PictureByteArray for this personality
        /// Used for Serializing
        /// </summary>
        public byte[] PictureByteArray
        {
            get
            {
                //if has an image
                if (_image != null)
                {
                    //convert to byte array
                    TypeConverter BitmapConverter =
                         TypeDescriptor.GetConverter(_image.GetType());
                    return (byte[])
                         BitmapConverter.ConvertTo(_image, typeof(byte[]));
                }
                else//no image
                    //return null
                    return null;
            }

            set
            {
                //has an image
                if (value != null)
                    //create an image from byte array
                    _image = new Bitmap(new MemoryStream(value));
                else//had no image
                    //set image to null
                    _image = null;
            }
        }

        #endregion 

        /// <summary>
        /// Get the image of the personality
        /// </summary>
        /// <returns></returns>
        public Image GetImage()
        {
            return _image;
        }
        /// <summary>
        /// Sets the image for the personality
        /// </summary>
        /// <param name="value"></param>
        public void SetImage(Image value)
        {
            _image = value;
        }
        /// <summary>
        /// Saves the persaonlity to a file
        /// </summary>
        /// <param name="filePath">file to save to</param>
        internal void Save(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            Stream stream = new FileStream(filePath, FileMode.Create,
                   FileAccess.Write, FileShare.None);
            serializer.Serialize(stream, this);
            stream.Close();
        }
        /// <summary>
        /// Creates an personality from a file
        /// </summary>
        /// <param name="filePath">file to load</param>
        /// <returns></returns>
        internal static Personality FromFile(string filePath)
        {
            //create an XmlSerializer
            XmlSerializer deser = new XmlSerializer(typeof(Personality));
            //create a stream to the filepath
            StreamReader stream = new StreamReader(filePath);

            Personality personality = null;
            //Deserialize
            personality = (Personality)deser.Deserialize(stream);
            //return personality
            return personality;
        }
    }
}
