using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.FixtureClasses;

namespace Kevsoft.LiveControl.ShapeMovements
{
    [Serializable]
    public class NoneShapeMovement : ShapeMovement
    {
        public NoneShapeMovement(Fixture parent) : base(parent) { }
        public NoneShapeMovement(ShapeMovement shape) : base(shape) { }
        public override void Move()
        {
            //This is just a shape that does nothing
        }

        public override void DrawPath(System.Drawing.Graphics g)
        {
            //This is just a shape that does nothing
        }

    }
}
