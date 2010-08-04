using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kevsoft.LiveControl.WFCL
{
    public partial class Possition : Control
    {
        public delegate void ValueChangedEventHandler(object sender, EventArgs e);
        public event ValueChangedEventHandler ValueChanged; 

        /// <summary>
        /// Min X Value
        /// </summary>
        private int _minXValue;
        /// <summary>
        /// Min Y Value
        /// </summary>
        private int _minYValue;

        /// <summary>
        /// Max X Value
        /// </summary>
        private int _maxXValue;
        /// <summary>
        /// Max Y Value
        /// </summary>
        private int _maxYValue;

        /// <summary>
        /// Lock X axis
        /// </summary>
        private bool _lockX;
        /// <summary>
        /// Lock Y Axis
        /// </summary>
        private bool _lockY;

        /// <summary>
        /// Current Values
        /// </summary>
        private Point _value;

        /// <summary>
        /// Pointer Size
        /// </summary>
        private Size _pointerSize;
        /// <summary>
        /// Pointer Pen
        /// </summary>
        private Pen _valuePen;
        /// <summary>
        /// If Pointer should auto move
        /// </summary>
        private bool _autoMovePointer;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Possition()
        {
            //set default values
            _minXValue = 0;
            _minYValue = 0;
            _maxXValue = 100;
            _maxYValue = 100;

            _lockX = false;
            _lockY = false;

            _pointerSize = new Size(10, 10);
            _value = new Point(50, 50);

            _valuePen = new Pen(Brushes.Navy);

            _autoMovePointer = true;
            InitializeComponent();
        }

        #region "Properties"
        /// <summary>
        /// Get or Set Min X
        /// </summary>
        public int MinXValue
        {
            get { return _minXValue; }
            set { _minXValue = value; Refresh(); }
        }
        /// <summary>
        /// Get or Set Min Y
        /// </summary>
        public int MinYValue
        {
            get { return _minYValue; }
            set { _minYValue = value; Refresh(); }
        }
        /// <summary>
        /// Get or Set Max X
        /// </summary>
        public int MaxXValue
        {
            get { return _maxXValue; }
            set { _maxXValue = value; Refresh(); }
        }
        /// <summary>
        /// Get or Set Max Y
        /// </summary>
        public int MaxYValue
        {
            get { return _maxYValue; }
            set { _maxYValue = value; Refresh(); }
        }
        /// <summary>
        /// Get or Set pointer size
        /// </summary>
        public Size PointerSize
        {
            get { return _pointerSize; }
            set { _pointerSize = value; Refresh(); }
        }
        /// <summary>
        /// Get or Set pointer thickness
        /// </summary>
        public float PointerThickness
        {
            get { return _valuePen.Width; }
            set { _valuePen.Width= value; Refresh(); }
        }
        /// <summary>
        /// Get or Set pointer color
        /// </summary>
        public Color PointerColor
        {
            get { return _valuePen.Color; }
            set { _valuePen.Color = value; Refresh(); }
        }
        /// <summary>
        /// Get or Set the Value
        /// </summary>
        public Point Value
        {
            get { return _value; }
            set
            {
                _value = value;//set the value
                Refresh();//Forces Redraw
            }

        }
        /// <summary>
        /// Get or Set X Value
        /// </summary>
        public int XValue
        {
            get { return _value.X; }
            set
            {
                _value.X = value;//set the value
                Refresh();//Forces Redraw
            }

        }
        /// <summary>
        /// Get or Set Y value
        /// </summary>
        public int YValue
        {
            get { return _value.Y; }
            set
            {
                _value.Y = value;//set the value
                Refresh();//Forces Redraw
            }

        }   
        /// <summary>
        /// Get or Set lock X axis
        /// </summary>
        public bool LockX
        {
            get { return _lockX; }
            set { _lockX = value; }
        }
        /// <summary>
        /// Get or Set lock Y axis
        /// </summary>
        public bool LockY
        {
            get { return _lockY; }
            set { _lockY = value; }
        }
        /// <summary>
        /// Get or Set if the pointer should automatically move when mouse click
        /// </summary>
        public bool AutoMovePointer
        {
            get { return _autoMovePointer; }
            set { _autoMovePointer = value; }

        }

        #endregion


        protected override void OnPaint(PaintEventArgs e)
        {
            //get the graphics obj
            Graphics g = e.Graphics;
            //draw grid lines
            DrawGridLines(g);
            //draw the pointer possition
            DrawPointer(g);
            //draw value text
            DrawValueText(g);
            base.OnPaint(e);
        }

        /// <summary>
        /// Draws the text at bottom of control
        /// </summary>
        /// <param name="g"></param>
        private void DrawValueText(Graphics g)
        {
            //create a string formatter
            StringFormat sf = new StringFormat();
            //set to far side
            sf.Alignment = StringAlignment.Far;

            //create the rectangle where the text will be possitioned
            Rectangle rec = new Rectangle(0 ,(int)(Height - Font.GetHeight()), Width, Height);

            g.DrawString(_value.ToString(),Font, Brushes.Black,rec,sf);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //change the Values
                if(!_lockX)
                    _value.X = (int)((double)e.X / (double)Width * (double)(_maxXValue - _minXValue)) + _minXValue;
                if(!_lockY)
                    _value.Y = (int)((double)e.Y / (double)Height * (double)(_maxYValue - _minYValue)) + _minYValue;

                //if any of the values are out of range use the nearist
                if (_value.X > _maxXValue) _value.X = _maxXValue;
                if (_value.X < _minXValue) _value.X = _minXValue;
                if (_value.Y > _maxYValue) _value.Y = _maxYValue;
                if (_value.Y < _minYValue) _value.Y = _minYValue;


                //refresh the control
                if (_autoMovePointer)
                    Refresh();

                //call the ValueChanged Event
                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());

            }
            base.OnMouseMove(e);
        }
        /// <summary>
        /// Draws the pointer on the control
        /// </summary>
        /// <param name="g"></param>
        private void DrawPointer(Graphics g)
        {
            double x = 0;
            double y = 0;
            //get the possition on the control
            if (_value.X != 0)
                x = (double)_value.X / (double)(_maxXValue - _minXValue) * (double)Width;
            if (_value.Y != 0)
                y = (double)_value.Y / (double)(_maxYValue - _minYValue) * (double)Height;

            Rectangle rec = new Rectangle((int)x - (_pointerSize.Width / 2),
                (int)y - (_pointerSize.Height / 2), _pointerSize.Width, _pointerSize.Height);

            //Draw the circle
            g.DrawEllipse(_valuePen, rec);

        }
        /// <summary>
        /// Draws the grid lines
        /// </summary>
        /// <param name="g"></param>
        private void DrawGridLines(Graphics g)
        {
            //find the center
            int verticalCenter = Height / 2;
            int horizontalCenter = Width / 2;

            Pen pen = new Pen(Brushes.Gray, 2);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            //Draw the Horziontal Line
            g.DrawLine(pen, 0, verticalCenter, Width, verticalCenter);
            //Draw the Vertical Line
            g.DrawLine(pen, horizontalCenter, 0, horizontalCenter, Height);



        }
        /// <summary>
        /// on resize of control redraw
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            //on resize force redraw 
            Refresh();
            base.OnResize(e);
        }

    }
}
