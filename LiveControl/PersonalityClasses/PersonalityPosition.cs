using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.PersonalityClasses;

namespace Kevsoft.LiveControl
{
    /// <summary>
    /// stores the data for a personalitys possition
    /// </summary>
    [Serializable]
    public class PersonalityPosition
    {
        /// <summary>
        /// max size for a 8bit channel
        /// </summary>
        private const int MAX_SIZE8 = 0xFF;
        /// <summary>
        /// max size for a 16bit channel
        /// </summary>
        private const int MAX_SIZE16 = 0xFFFF;

        /// <summary>
        /// Pan channel
        /// </summary>
        private Int16 _panChannel = 1;
        /// <summary>
        /// Tilt channel
        /// </summary>
        private Int16 _tiltChannel = 2;

        /// <summary>
        /// pan on value
        /// </summary>
        private int _panOnValue = 0;
        /// <summary>
        /// tilt on value
        /// </summary>
        private int _tiltOnValue = 0;

        /// <summary>
        /// the bit size 8bit or 16bit
        /// </summary>
        private AttributeBitSize _bitSize = AttributeBitSize.Size8;

        /// <summary>
        /// Default constructor needed for serialzation
        /// </summary>
        public PersonalityPosition() { }

        #region "Properties"

        /// <summary>
        /// Gets the max Pan or Tilt Value for this position
        /// </summary>
        public int MaxValuePanTilt
        {
            get
            {
                if (BitSize == AttributeBitSize.Size8)
                    return MAX_SIZE8;
                else
                    return MAX_SIZE16;

            }
        }
        /// <summary>
        /// Gets or Sets the Pan On value
        /// </summary>
        public int PanOnValue
        {
            get
            {
                return _panOnValue;
            }
            set
            {
                _panOnValue = value;
            }
        }
        /// <summary>
        /// Gets or Sets the Tilt on value
        /// </summary>
        public int TiltOnValue
        {
            get
            {
                return _tiltOnValue;
            }
            set
            {
                _tiltOnValue = value;
            }
        }
        /// <summary>
        /// Gets or sets the Pan Channel
        /// </summary>
        public Int16 PanChannel
        {
            get
            {
                return _panChannel;
            }
            set
            {
                _panChannel = value;
            }
        }
        /// <summary>
        /// Gets or Sets the tilt channel
        /// </summary>
        public Int16 TiltChannel
        {
            get
            {
                return _tiltChannel;
            }
            set
            {
                _tiltChannel = value;
            }
        }
        /// <summary>
        /// Gets or Sets the Bitsize for this position
        /// </summary>
        public AttributeBitSize BitSize
        {
            get
            {
                return _bitSize;
            }
            set
            {
                _bitSize = value;
            }
        }

        #endregion

    }
}
