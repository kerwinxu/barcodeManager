using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.State
{
    public  class StateCreate:State
    {
        public StateCreate(UserControlCanvas canvas, PointF start_pointF) : base(canvas, start_pointF)
        {

        }
        public StateCreate(UserControlCanvas canvas) : base(canvas)
        {

        }
    }
}
