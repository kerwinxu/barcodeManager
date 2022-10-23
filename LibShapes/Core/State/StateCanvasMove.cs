using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.State
{
    /// <summary>
    /// 画布整体的移动
    /// </summary>
    public class StateCanvasMove:State
    {
        public StateCanvasMove(UserControlCanvas canvas, PointF start_pointF) : base(canvas, start_pointF)
        {

        }
        public StateCanvasMove(UserControlCanvas canvas) : base(canvas)
        {

        }

        // 移动的话，我这个类会保存原先的偏移
        private float old_offsetX, old_offsetY;

        public override void LeftMouseDown(PointF pointF)
        {
            // 保存偏移
            old_offsetX = this.canvas.shapes.pointTransform.OffsetX;
            old_offsetY = this.canvas.shapes.pointTransform.OffsetY;
            startPoint = pointF;

            base.LeftMouseDown(pointF);
        }

        public override void LeftMouseMove(PointF pointF)
        {
            float diffx = pointF.X - startPoint.X;
            float diffy = pointF.Y - startPoint.Y;
            // 然后修改偏移
            this.canvas.shapes.pointTransform.OffsetX = old_offsetX + diffx;
            this.canvas.shapes.pointTransform.OffsetY = old_offsetY + diffy;
            
            //base.LeftMouseMove(pointF);
        }

        public override void LeftMouseUp(PointF pointF)
        {
            // 保存命令，这里好像不用保存。
            //base.LeftMouseUp(pointF);
        }
    }
}
