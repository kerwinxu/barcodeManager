using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Io.Github.Kerwinxu.LibShapes.Core.State
{
    public class StateStandby : State
    {

        public StateStandby(UserControlCanvas canvas, PointF start_pointF):base(canvas,start_pointF)
        {
            
        }

        public StateStandby(UserControlCanvas canvas) : base(canvas)
        {
            this.canvas.changeSelect(null);// null表示没有选择，然后就是纸张。
        }

        public override void LeftMouseDown(PointF pointF)
        {
            // 首先看看是否有选择的图形
            var shape = this.canvas.shapes.getSelectShape(pointF);
            if (shape != null)
            {
                this.canvas.changeSelect(shape);                                // 选择这个
                this.canvas.state = new StateSelected(this.canvas, pointF);      // 改变状态
                this.canvas.state.LeftMouseDown(pointF);                         // 调用他的处理
            }
            else
            {
                this.canvas.changeSelect(null);
                this.canvas.state = new ShapeRectSelect(this.canvas, pointF);
                this.canvas.state.LeftMouseDown(pointF);
            }

            this.canvas.Refresh();
            //base.LeftMouseDown(e);
        }

    }
}
