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
            var oldShapes = this.canvas.shapes.DeepClone();
            this.canvas.reduce(pointF);
            // 保存命令，
            this.canvas.commandRecorder.addCommand(new Command.CommandShapesChanged()
            {
                canvas = this.canvas,
                OldShapes = oldShapes,
                NewShapes = this.canvas.shapes.DeepClone(),
            });
            //base.RightMouseClick(pointF);
        }

        public override void LeftMouseUp(PointF pointF)
        {
            var oldShapes = this.canvas.shapes.DeepClone();
            this.canvas.zoom(pointF);
            // 保存命令，
            this.canvas.commandRecorder.addCommand(new Command.CommandShapesChanged()
            {
                canvas =this.canvas,
                OldShapes = oldShapes,
                NewShapes = this.canvas.shapes.DeepClone(),
            });
            //base.LeftMouseUp(pointF);
        }
    }
}
