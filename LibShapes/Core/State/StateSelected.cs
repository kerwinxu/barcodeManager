using Io.Github.Kerwinxu.LibShapes.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        public override void LeftMouseDown(PointF pointF)
        {
            // 首先判断一下是否在这个选择矩形的范围内
            // 在的话就是更改模式了，比如更改尺寸和移动。我这个在选择框上是更改尺寸，而在内部是移动
            // 如果不在，就转成待机模式
            // 1. 这里判断一下是否在矩形框的范围内
            GraphicsPath path = new GraphicsPath();
            // 这里要判断一下是否是ShapeMultiSelect
            if (this.canvas.SelectShape is Shape.ShapeMultiSelect)
            {
                // 那么最简单的方式是取消所有的选择，然后运行待机模式的选择
                this.canvas.changeSelect(null);
                this.canvas.state = new StateStandby(this.canvas);
                this.canvas.state.LeftMouseDown(pointF);
            }
            else
            {
                // 如果选择的图形是其他类型
                // 看看是否被选择了吧。
                var rect = this.canvas.SelectShape.GetBounds(this.canvas.shapes.GetMatrix());
                // 放大范围，
                rect.X -= DistanceCalculation.select_tolerance;
                rect.Y -= DistanceCalculation.select_tolerance;
                rect.Width += DistanceCalculation.select_tolerance * 2;
                rect.Height += DistanceCalculation.select_tolerance * 2;
                path.AddRectangle(rect);
                // 判断是否
                if (path.IsVisible(pointF))
                {
                    this.canvas.state = new StateChanging(this.canvas, pointF);
                    this.canvas.state.LeftMouseDown(pointF);
                }
                else
                {
                    // 转成待机模式
                    this.canvas.changeSelect(null);
                    this.canvas.state = new StateStandby(this.canvas);
                    this.canvas.state.LeftMouseDown(pointF);
                }
            }


            //base.LeftMouseDown(pointF);
        }
    }
}
