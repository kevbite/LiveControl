using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl;
using System.Runtime.Serialization.Formatters;
using System.Xml.Serialization;
using System.IO;
namespace Kevsoft.LiveControl
{
    [Serializable]
    class PersonalitiesBank : List<Personality>
    {
        public int AddFromFile(string filePath)
        {
            //Get Personality form file
            Personality personality = Personality.FromFile(filePath);
            //add the personality to the list
            Add(personality);
            //return the index
            return Count-1;

        }
        /// <summary>
        /// Gets a list of images of all the personalities holded by this lis
        /// </summary>
        /// <returns></returns>
        internal System.Drawing.Image[] GetImages()
        {
            //create a temp array
            System.Drawing.Image[] tmpImgs = new System.Drawing.Image[this.Count];
            //loop though each item in this bank
            for (int i = 0; i < this.Count; i++)
            {
                //fill the array
                tmpImgs[i] = this[i].GetImage();
                if (tmpImgs[i] == null)//if no image for fixture
                    //set a default image
                    tmpImgs[i] = (System.Drawing.Image)Kevsoft.LiveControl.Properties.Resources.dimmer;
            }
            //return the array
            return tmpImgs;
        }
    }
}
