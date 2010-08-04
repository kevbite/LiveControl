using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace Kevsoft.LiveControl.WFCL
{
	/// <summary>
	/// AVFader.
	/// </summary>
	public class AVFader : System.Windows.Forms.UserControl
	{
		private int	_minValue = 0;
		private int	_maxValue = 100;
		private int _value    = 0;
		private int	_tickFrequency = 10;
		private int	_smallChange = 1;
		private bool _dragModeEnabled = false;
		private Point _startDragPoint;

        private System.Drawing.Color _tickColor = Color.Black;
		private System.Windows.Forms.PictureBox picSlider;
        
		#region Initialise
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AVFader()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            DoubleBuffered = true;
			// Position the slider on the channel
			this.picSlider.Left = (this.Width/2) - (this.picSlider.Width/2);
			this.MoveSlider();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AVFader));
            this.picSlider = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // picSlider
            // 
            this.picSlider.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.picSlider.Image = ((System.Drawing.Image)(resources.GetObject("picSlider.Image")));
            this.picSlider.Location = new System.Drawing.Point(10, 72);
            this.picSlider.Name = "picSlider";
            this.picSlider.Size = new System.Drawing.Size(20, 30);
            this.picSlider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSlider.TabIndex = 0;
            this.picSlider.TabStop = false;
            this.picSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picSlider_MouseMove);
            this.picSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlider_MouseDown);
            this.picSlider.MouseHover += new System.EventHandler(this.AVFader_MouseHover);
            this.picSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picSlider_MouseUp);
            this.picSlider.MouseWheel += new MouseEventHandler(this.AVFader_MouseWheel);
            // 
            // AVFader
            // 
            this.Controls.Add(this.picSlider);
            this.Name = "AVFader";
            this.Size = new System.Drawing.Size(40, 120);
            this.MouseHover += new System.EventHandler(this.AVFader_MouseHover);
            this.MouseWheel += new MouseEventHandler(this.AVFader_MouseWheel);
            ((System.ComponentModel.ISupportInitialize)(this.picSlider)).EndInit();
            this.ResumeLayout(false);

		}

        void AVFader_MouseWheel(object sender, MouseEventArgs e)
        {
            int valToAdd=0;
            if (e.Delta > 0)//scroll up
                valToAdd = SmallChange;
            else if(e.Delta<0)//scrow down
                valToAdd = -SmallChange;

            //Check the value and commit
            if (CheckValue(Value + valToAdd))
                Value += valToAdd;

            // Raise the ValueChanged event
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());

        }
		#endregion

		#region Public Properties
		[Bindable(true), Category("Behavior"),
		DefaultValue(0), Description("The minimum value of the slider")]
		public int Minimum
		{
			get { return this._minValue; }
			set
			{
				this._minValue = value;

				if (this._value < this._minValue)
					this.Value = this._minValue;
				else
				{
					this.MoveSlider();
					this.Invalidate();
				}
			}
		}

		[Bindable(true), Category("Behavior"),
		DefaultValue(0), Description("The value of the slider")]
		public int Value
		{
			get { return this._value; }
			set
			{
                if (!CheckValue(value))
				{
					MessageBox.Show("Value out of bounds");
					return;
				}

				if (value == this._value)
					return;

				int tmp = this._value;
				this._value = value;
				this.MoveSlider();
                this.Invalidate();
			}
		}

        private bool CheckValue(int value)
        {
            return (value >= this._minValue) && (value <= this._maxValue);
        }

		[Bindable(true), Category("Behavior"),
		DefaultValue(100), Description("The maximum value of the slider")]
		public int Maximum
		{
			get { return this._maxValue; }
			set
			{
				this._maxValue = value;

				if (this._value > this._maxValue)
					this.Value = this._maxValue;
				else
				{
					this.MoveSlider();
					this.Invalidate();
				}
			}
		}

		[Bindable(true), Category("Appearance"),
		DefaultValue(5), Description("The number of positions between tick marks")]
		public int TickFrequency
		{
			get { return this._tickFrequency; }
			set
			{
				this._tickFrequency = value;
				this.Invalidate();
			}
		}

		[Bindable(true), Category("Appearance"),
		DefaultValue(1), Description("The number of positions the slider moves in response to keyboard input")]
		public int SmallChange
		{
			get { return this._smallChange; }
			set
			{
				this._smallChange = value;
				this.Invalidate();
			}
		}

		[Bindable(true), Category("Appearance"),
		DefaultValue(typeof(Color), "System.Drawing.Color.Black"), Description("The of the tick marks")]
		public System.Drawing.Color TickColor
		{
			get { return this._tickColor; }
			set
			{ 
				this._tickColor = value;
				this.Invalidate();
			}
		}
		#endregion

		#region Public Events
		[Category("Action"), Description("Occurs when the slider is moved")]
		public event EventHandler ValueChanged;
		#endregion

		#region Overridden events
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);

			// Position the slider on the channel
			this.picSlider.Left = (this.Width/2) - (this.picSlider.Width/2);
            this.MoveSlider();
			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			e.Graphics.Clear(this.BackColor);

			// Init the drawing tools
			Pen penSmallTick = new Pen(this._tickColor, 1);
			Pen penLargeTick = new Pen(this._tickColor, 2);

			// Draw the slider channel up the middle
			e.Graphics.FillRectangle(
				penSmallTick.Brush,
				(this.Width / 2) - 1,
				this.picSlider.Height / 2,
				2,
				this.Height - this.picSlider.Height
				);

			// Draw the ticks
			int tickSpacing   = (this.Height - this.picSlider.Height) / ((Math.Abs(this._minValue) + Math.Abs(this._maxValue)) / this._tickFrequency);
			int startPosition = this.picSlider.Height / 2;
			int endPosition   = this.Height - startPosition;

			int i = 0;
            //check to see if (tickSpacing>0) so that we dont get an infinate loop
			for (int y=startPosition; ((y<=endPosition)&&(tickSpacing>0)); y+=tickSpacing)
			{
				if (i % 2 == 0)
				{
					e.Graphics.DrawLine(penLargeTick, 5, y, (this.Width / 2) - 5, y);
					e.Graphics.DrawLine(penLargeTick, (this.Width / 2) + 5, y, this.Width - 5, y);
				}
				else
				{
					e.Graphics.DrawLine(penSmallTick, 9, y, (this.Width / 2) - 5, y);
					e.Graphics.DrawLine(penSmallTick, (this.Width / 2) + 5, y, this.Width - 9, y);
				}

				i++;
			}
		}

		private void picSlider_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// Set the drag flag for the mousemove event
			this._dragModeEnabled = true;

			// Record the start point for the slider movement
			this._startDragPoint = new Point(e.X, e.Y);
		}

		private void picSlider_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// User isn't dragging the slider, so dont bother moving it
            if (this._dragModeEnabled == true)
            {

                // Calculate the distance the mouse moved
                int delta = e.Y - this._startDragPoint.Y;

                if (delta == 0)
                    return;

                this.MoveSlider(delta);
            }
		}

		private void picSlider_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// Reset the drag flag
			this._dragModeEnabled = false;
		}
		#endregion

		private void MoveSlider(int delta)
		{
			// Move the slider and make sure it stays in the bounds of the control
			if (delta < 0 && (this.picSlider.Top + delta) <= 0)
				this.picSlider.Top = 0;
			else if (delta > 0 && (this.picSlider.Top + this.picSlider.Height + delta) >= this.Height)
				this.picSlider.Top = this.Height - this.picSlider.Height;
			else
				this.picSlider.Top += delta;

			this.CalculateSliderValue();
		}

        private void MoveSlider()
        {
            // distance between tics used in ratio calc
            int distance = Math.Abs(this._maxValue) + Math.Abs(this._minValue);

            // percentage of control travelled
            float percent = (float)this._value / (float)distance;

            // The minimum position of the Slider Pic
            int minPos = this.Height - this.picSlider.Height;

            // New slider location
            this.picSlider.Top = minPos - Convert.ToInt32(percent * (float)(minPos));
        }

		private void CalculateSliderValue()
		{
			// distance between tics used in ratio calc
			int distance  = this.Height - this.picSlider.Height;

			// percentage of control travelled
			float percent = (float)this.picSlider.Top / (float)distance;
			
			// Slider movement in points
			int movement  = Convert.ToInt32(percent * (float)(Math.Abs(this._maxValue) + Math.Abs(this._minValue)));
			
			// New value
			this._value = (this._maxValue >= 0) ? this._maxValue - movement : this._maxValue + movement;

			// Raise the ValueChanged event
            if (ValueChanged!=null)
			    ValueChanged(this, new EventArgs());
		}

        private void AVFader_MouseHover(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void AVFader_MouseMove(object sender, MouseEventArgs e)
        {
        }
        

	}
}
