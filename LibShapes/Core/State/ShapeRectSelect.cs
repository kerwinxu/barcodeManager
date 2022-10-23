using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.State
{
    public class ShapeRectSelect:State
    {

        protected PointF endPoint;

        private Pen penSelectShape = new Pen(Color.Black)
        {      // 选择框的画笔
            DashStyle = DashStyle.Dash,
            Width = 0.2f
        };

        public ShapeRectSelect(UserControlCanvas canvas, PointF start_pointF) : base(canvas, start_pointF)
        {

        }
        public ShapeRectSelect(UserControlCanvas canvas) : base(canvas)
        {

        }

        public void Draw(Graphics g)
        {
            // 这个其实是绘制一个矩形。
            g.DrawRectangle(
                penSelectShape, 
                startPoint.X,
                startPoint.Y, 
                endPoint.X-startPoint.X,
                endPoint.Y - startPoint.Y);
        }

        public override void LeftMouseDown(PointF pointF)
        {
            startPoint = pointF; // 只是保存开始地址
            base.LeftMouseDown(pointF);
        }

        public override void LeftMouseMove(PointF pointF)
        {
            // 保存
            endPoint = pointF;
            this.canvas.Refresh(); // 刷新。

            //base.LeftMouseMove(pointF);
        }

        public override void LeftMouseUp(PointF pointF)
        {
            // 这里看一下是否有选择图形
            endPoint = pointF;
            var _rect = new RectangleF(startPoint.X, startPoint.Y, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
            var _shapes = this.canvas.shapes.getSelectShapes(_rect);
            if (_shapes!=null && _shapes.Count > 0)
            {
                // 说明选择了图形
                this.canvas.state = new StateSelected(this.canvas);
                var group = new Shape.ShapeGroup();
                group.shapes.AddRange(_shapes);
                this.canvas.changeSelect(group);
            }
            else
            {
                // 说明没有选择图形
                this.canvas.changeSelect(null);
                this.canvas.state = new StateStandby(this.canvas);
            }

            this.canvas.Refresh();

            //base.LeftMouseUp(pointF);
        }
    }
}
