using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.State
{
    /// <summary>
    ///  画布的放大和缩小
    /// </summary>
    public  class StateCanvasZoom:State
    {
        public StateCanvasZoom(UserControlCanvas canvas, PointF start_pointF) : base(canvas, start_pointF)
        {

        }
        public StateCanvasZoom(UserControlCanvas canvas) : base(canvas)
        {

        }

        public override void RightMouseClick(PointF pointF)
        {
            this.canvas.reduce(pointF);
            //base.RightMouseClick(pointF);
        }

        public override void LeftMouseUp(PointF pointF)
        {
            this.canvas.zoom(pointF);
            //base.LeftMouseUp(pointF);
        }
    }
}
