using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Drawing;

namespace Kevsoft.LiveControl
{
    /// <summary>
    /// Stores a preset attribute value
    /// </summary>
    [Serializable]
    public class AttributePreSetValue
    {
        /// <summary>
        /// Image of the attribute at that value
        /// </summary>
        private System.Drawing.Image _image;

        /// <summary>
        /// Constructor with a name, value and image
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="image"></param>
        public AttributePreSetValue(string name, int value, Image image)
        {
            Name = name;
            Value = value;
            _image = image;
        } 
        /// <summary>
        /// constructor with a name and value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public AttributePreSetValue(string name, int value)
        {
            Name = name;
            Value = value;
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AttributePreSetValue()
        {
            Name = "No Name";
            Value = 0;
        }
        /// <summary>
        /// Get or Sets the Name of the Preset
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or Sets the Value of the Preset
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// Gets the image of the preset
        /// </summary>
        /// <returns></returns>
        public Image GetImage()
        {
            return _image;
        }
        /// <summary>
        /// Sets the image of the preset
        /// </summary>
        /// <param name="value"></param>
        public void SetImage(Image value)
        {
            _image = value; 
        }
        /// <summary>
        /// Gets or Sets the PictureByteArray
        /// Used for Serializing
        /// </summary>
        public byte[] PictureByteArray
        {
            get
            {
                if (_image != null)//if has an image
                {
                    //convert the image to byte array
                    TypeConverter BitmapConverter =
                         TypeDescriptor.GetConverter(_image.GetType());
                    return (byte[])
                         BitmapConverter.ConvertTo(_image, typeof(byte[]));
                }
                else//if no image
                    return null;
            }

            set
            {
                if (value != null)//has an image
                    //create new image from the byte array
                    _image = new Bitmap(new MemoryStream(value));
                else//no image
                    //set image to null
                    _image = null;
            }
        }
        /// <summary>
        /// returns Name + Value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + "\t(" + Value.ToString() + ")";
        }
    }
}
