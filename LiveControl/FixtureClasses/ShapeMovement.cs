using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Kevsoft.LiveControl.FixtureClasses;

namespace Kevsoft.LiveControl.FixtureClasses
{
    /// <summary>
    /// Abstract Class for a ShapeMovement
    /// </summary>
    [Serializable]
    public abstract class ShapeMovement
    {
        /// <summary>
        /// Max value for the Off Set value
        /// </summary>
        public const int OffSetMax = 100;
        /// <summary>
        /// Max value for the speed
        /// </summary>
        public const int SpeedMin = -300;
        /// <summary>
        /// Max value for the speed
        /// </summary>
        public const int SpeedMax = 300;
        /// <summary>
        /// off set of the playback
        /// </summary>
        int _offSet;
        /// <summary>
        /// Speed value for the playback
        /// </summary>
        int _speed;
         /// <summary>
        /// Size of the shape
        /// </summary>
        Size _size;
       /// <summary>
        /// Position of the shape
        /// </summary>
        Point _shapePosition;
        /// <summary>
        /// The parent of the shape movement
        /// </summary>
        Fixture _parentFix;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ShapeMovement()
        {
            //set default values
            _offSet = 0;
            _speed = 127;
            _size = new Size();
            _shapePosition = new Point();
            _parentFix = null;
        }
        /// <summary>
        /// Constructor for adding shape with a parent
        /// </summary>
        /// <param name="parent"></param>
        public ShapeMovement(Fixture parent)
        {
            //Set the parent
            _parentFix = parent;
            _offSet = 0;
            _speed = 0;
            _size = new Size(parent.MaxPanTiltValue / 2,
                                parent.MaxPanTiltValue / 2);
            _shapePosition = new Point(0,0);
            _parentFix = parent;
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="shape"></param>
        public ShapeMovement(ShapeMovement shape)
        {
            //Copy the data from shape
            _offSet = shape.OffSet;
            _speed = shape.Speed;
            _size = shape.Size;
            _shapePosition = shape.ShapePosition;
            _parentFix = shape.ParentFixture;
        }

        /// <summary>
        /// Gets and sets the size of the shape
        /// </summary>
        public Size Size
        {
            get { return _size; }
            set { _size = value; }
        }
        /// <summary>
        /// Gets and Sets the Width of the Shape
        /// </summary>
        public virtual int Width
        {
            get { return _size.Width; }
            set { _size.Width = value; }
        }
        /// <summary>
        /// Get and Sets the Height of the Shape
        /// </summary>
        public virtual int Height
        {
            get { return _size.Height; }
            set { _size.Height = value; }
        }
        /// <summary>
        /// Gets and Sets the Position of the Shape
        /// </summary>
        public virtual Point ShapePosition
        {
            get { return _shapePosition; }
            set { _shapePosition = value; }
        }
        /// <summary>
        /// Gets the X Position of the Shape
        /// </summary>
        public virtual int ShapeX
        {
            get { return _shapePosition.X; }
        }
        /// <summary>
        /// Gets the Y positions of the Shape
        /// </summary>
        public virtual int ShapeY
        {
            get { return _shapePosition.Y; }
        }
        /// <summary>
        /// Gets and Sets the ParentFixture for the shape
        /// </summary>
        public virtual Fixture ParentFixture
        {
            get { return _parentFix; }
            set { _parentFix = value; }
        }
        /// <summary>
        /// Gets and Sets the Off Set position of the Playback
        /// OffSet value can range from 0 to 100
        /// </summary>
        public virtual int OffSet
        {
            get { return _offSet; }
            set { if (value >= 0 && value <= OffSetMax) _offSet = value; }
        }
        /// <summary>
        /// Gets and Sets the Speed Value of the Playback
        /// Speed can range from 0 to SpeedMax
        /// </summary>
        public virtual int Speed
        {
            get { return _speed; }
            set { if (value >= SpeedMin && value <= SpeedMax) _speed = value; }
        }
        /// <summary>
        /// Gets and Sets the Position of the fixture
        /// </summary>
        public virtual Point Position
        {
            get { return new Point(_parentFix.Pan, _parentFix.Tilt); }
            set
            {
                //if parent is not null
                if (_parentFix != null)
                {
                    //set the fixtures pan and tilt values
                    _parentFix.Pan = value.X;
                    _parentFix.Tilt = value.Y;
                }

            }
        }
        /// <summary>
        /// Overrides Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            //cast obj to ShapeMovement
            ShapeMovement temp = obj as ShapeMovement;
            bool equals = false;

            //Check Properties equil
            if ((temp != null)
                && (temp.GetType() == this.GetType())
                && (temp.OffSet == this._offSet)
                && (temp.Speed == this._speed)
                && (temp.Size == this._size)
                && (temp.ShapePosition == this._shapePosition))
                equals = true;
            return equals;
        }
        /// <summary>
        /// Overrides hashcode of object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Moves the the current value of the shape to next set of values
        /// </summary>
        public abstract void Move();
        /// <summary>
        /// Draws the ShapeMovement Path
        /// </summary>
        /// <param name="g"></param>
        public abstract void DrawPath(Graphics g);
    
    }
}
