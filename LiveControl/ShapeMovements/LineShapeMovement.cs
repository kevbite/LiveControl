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
    public class LineShapeMovement : ShapeMovement
    {

        private Direction _currentDirection;

        int _currentStep = 0;
        public LineShapeMovement(Fixture parent) : base(parent) { }
        public LineShapeMovement(ShapeMovement shape) : base(shape) { }
        public override void Move()
        {
            //if speed == 0 mean stopped, so just return
            if (Speed == 0) return;

            Point p = GetNextValue();
            System.Diagnostics.Debug.WriteLine(p);
            //set the new position
            Position = p;
            //Add the next Step
        }


        private Point GetNextValue()
        {
            int diffX = SecondShapePosition.X - ShapePosition.X;
            int diffY = SecondShapePosition.Y - ShapePosition.Y;

            float stepX = (float)Speed / (float)SpeedMax * (float)diffX;
            float stepY = (float)Speed / (float)SpeedMax * (float)diffY;

            int maxSteps = (int)(diffY / stepY);

            int step = AddOffSetSteps(maxSteps);


            Point p = new Point((int)((stepX * step) + ShapePosition.X), (int)(
                                    (stepY * step) + ShapePosition.Y));

            if (_currentDirection == Direction.Forward)
                _currentStep = AddSteps(1, maxSteps);
            else
                _currentStep = AddSteps(-1, maxSteps);

            return p;
        }
        public override int Speed
        {
            get
            {
                return base.Speed;
            }
            set
            {
                //for a line speed has to always be a possitive
                if (value < 0) value *= -1;
                base.Speed = value;
            }
        }
        private int AddOffSetSteps(int maxSteps)
        {
            int offSetSteps = (int)((float)OffSet / (float)OffSetMax * (float)maxSteps);
            return AddSteps(offSetSteps, maxSteps);
        }


        private int AddSteps(int stepsToAdd, int maxSteps)
        {
            int result = 0;
            if (stepsToAdd == 0)
            {
                result = _currentStep;
            }
            else if (_currentStep + stepsToAdd >= maxSteps)
            {
                int nextstep = (_currentStep + stepsToAdd) - maxSteps;
                result = nextstep;
                _currentDirection = Direction.Forward;

            }
            else if (_currentStep + stepsToAdd <= 0)
            {
                int nextstep = (_currentStep + stepsToAdd) * -1;
                result = nextstep;
                _currentDirection = Direction.Backwards;
            }
            else
            {
                result = _currentStep + stepsToAdd;
            }


            return result;

        }
        public Point SecondShapePosition
        {
            get
            {
                return new Point((ShapePosition.X + Width),
                                    (ShapePosition.Y + Height));
            }
        }

        /// <summary>
        /// Draws the path of the current circle
        /// </summary>
        /// <param name="g"></param>
        public override void DrawPath(Graphics g)
        {
            float width = g.Clip.GetBounds(g).Width;
            float height = g.Clip.GetBounds(g).Height;

            int x1 = (int)((float)ShapePosition.X / (float)ParentFixture.Personality.Possition.MaxValuePanTilt * width);
            int y1 = (int)((float)ShapePosition.Y / (float)ParentFixture.Personality.Possition.MaxValuePanTilt * height);

            int x2 = (int)((float)SecondShapePosition.X / (float)ParentFixture.Personality.Possition.MaxValuePanTilt * width);
            int y2 = (int)((float)SecondShapePosition.Y / (float)ParentFixture.Personality.Possition.MaxValuePanTilt * height);

            g.DrawLine(Pens.Red, x1, y1, x2, y2);

        }

        private enum Direction
        {
            Forward,
            Backwards
        }

    }
}
