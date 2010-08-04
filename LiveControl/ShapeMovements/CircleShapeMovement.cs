using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.FixtureClasses;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Kevsoft.LiveControl.ShapeMovements
{
    [Serializable]
    public class CircleShapeMovement : ShapeMovement
    {
        /// <summary>
        /// Keeps track of the current Degrees
        /// </summary>
        float _currentDegrees = 0;
        public CircleShapeMovement(Fixture parent) : base(parent) {}
        public CircleShapeMovement(ShapeMovement shape): base(shape) { }
        public override void Move()
        {
            //if speed == 0 mean stopped, so just return
            if (Speed == 0) return;
           //Get the Deg in which we will use
            float deg = AddOffSet();
            //Workout the Values of X and Y
            Point p = WorkoutValues(deg);

            //Set the possition with the new values
            Position = p;

            //Add the next Step
            AddDegStep();
        }
        /// <summary>
        /// Works out the point for a given degrees
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        private Point WorkoutValues(float deg)
        {
            return new Point((int)((Math.Cos(deg * Math.PI / 180) * ((Width) - 1)) + ((Width - 1)) + (float)ShapePosition.X-(Width * 0.5F)),
                        (int)((Width - 1) - (Math.Sin(deg * Math.PI / 180) * (Width - 1)) + (float)ShapePosition.Y-(Width * 0.5F)));
        }

        /// <summary>
        /// Adds the Step to the degrees
        /// </summary>
        private void AddDegStep()
        {
            //get the DegreesToAdd from the speed and min/max speed
            float DegToAdd = ((float)Speed / (float)(SpeedMax-SpeedMin) * 359F);
            //set the new current Degrees
            _currentDegrees = AddDeg(DegToAdd);
            System.Diagnostics.Debug.WriteLine("current Deg: " + _currentDegrees.ToString());
        }

        /// <summary>
        /// returns offset and Current value
        /// </summary>
        /// <returns></returns>
        private float AddOffSet()
        {
            //Get the Deg to add because offset is out of OffSetMax
            float DegToAdd = (int)((float)OffSet / (float)OffSetMax * 360);
            //Add the Deg to current and return
            return (AddDeg(DegToAdd));
        }

        /// <summary>
        /// Adds to Current Deg
        /// </summary>
        /// <param name="DegreesToAdd">Degrees to add</param>
        /// <returns></returns>
        private float AddDeg(float DegreesToAdd)
        {
            float deg;
            //if over the max Degrees
            if ((_currentDegrees + DegreesToAdd) >= 360)
            {
                //takeway the max and return what we have left
                deg = (_currentDegrees + DegreesToAdd) - 360;
                if(deg<0)
                System.Diagnostics.Debug.Print("New Deg:" + deg.ToString());
            }
            //if over the min size
            else if ((_currentDegrees + DegreesToAdd) <= -360)
            {
                //add the max and return what we have left
                deg = (_currentDegrees + DegreesToAdd) + 360;
            }
            else
            {
                //add the current and DegreesToAdd together
                deg = DegreesToAdd + _currentDegrees;
            }
            return deg;
        }

        /// <summary>
        /// Draws the path of the current circle
        /// </summary>
        /// <param name="g"></param>
        public override void DrawPath(Graphics g)
        {
            float width = g.Clip.GetBounds(g).Width;
            float height = g.Clip.GetBounds(g).Height;
            //Array of Points
            Point[] pts = new Point[360];
            byte[] ptrTypes = new byte[360];
            GraphicsPath graphPath = new GraphicsPath();
            //Go though every value
            for(int i=0;i<360;i++)
            {
                //Work out the X Y positions
                pts[i] = WorkoutValues(i);
                //Scale the position down
                pts[i].X = (int)((float)pts[i].X / (float)ParentFixture.Personality.Possition.MaxValuePanTilt * width);
                pts[i].Y =(int)((float)pts[i].Y / (float)ParentFixture.Personality.Possition.MaxValuePanTilt * height);

                ptrTypes[i] = (byte)(System.Drawing.Drawing2D.PathPointType.Bezier
                    /*| System.Drawing.Drawing2D.PathPointType.CloseSubpath*/);
            }
            graphPath.AddCurve(pts);
            //Draw the Path
            g.DrawPath(Pens.Red,
                graphPath);
        }
        /// <summary>
        /// Overrides the heigh as its the same as the width
        /// </summary>
        public override int Height
        {
            get
            {
                return Width;
            }
            set
            {
                Width = value;
            }
        }
        /// <summary>
        /// Overrides the width so that it sets the height to the same value
        /// </summary>
        public override int Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
                base.Height = value;
            }
        }
    }
}
