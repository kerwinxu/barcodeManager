using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.State
{
    public class StateSelected:State
    {
        public StateSelected(UserControlCanvas canvas, PointF start_pointF) : base(canvas, start_pointF)
        {

        }
        public StateSelected(UserControlCanvas canvas) : base(canvas)
        {

        }
    }
}
