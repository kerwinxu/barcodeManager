using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.State
{
    public class ShapeRectSelect:State
    {
        public ShapeRectSelect(UserControlCanvas canvas, PointF start_pointF) : base(canvas, start_pointF)
        {

        }
        public ShapeRectSelect(UserControlCanvas canvas) : base(canvas)
        {

        }

        public void Draw(Graphics g)
        {

        }
    }
}
