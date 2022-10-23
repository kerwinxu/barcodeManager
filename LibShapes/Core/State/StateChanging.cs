using Io.Github.Kerwinxu.LibShapes.Core.State.ChangeStrategy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.State
{
    /// <summary>
    /// 更改状态，都是选择后才有的。
    /// </summary>
    public class StateChanging:State
    {
        public StateChanging(UserControlCanvas canvas, PointF start_pointF) : base(canvas, start_pointF)
        {

        }
        public StateChanging(UserControlCanvas canvas) : base(canvas)
        {

        }

        /// <summary>
        ///  当前选择的策略
        /// </summary>
        private IChangeStrategy changeStrategy;

        /// <summary>
        /// 所有的策略
        /// </summary>
        private IChangeStrategy[] changeStrategies = {
            // 先4个角点
            new ChangeStrategy.ResizeModeNorthEast(),
            new ChangeStrategy.ResizeModeNorthWest(),
            new ChangeStrategy.ResizeModeSorthWest(),
            new ChangeStrategy.ResizeModeSouthEast(),
            // 然后4个方向
            new ChangeStrategy.ResizeModeEast(),
            new ChangeStrategy.ResizeModeNorth(),
            new ChangeStrategy.ResizeModeSouth(),
            new ChangeStrategy.ResizeModeWest(),
            // 最后是移动
            new ChangeStrategy.MoveMode()
        };

        public override void LeftMouseDown(PointF pointF)
        {
            // 这个首先看一下是在四面还是八方上，运算是不同的。
            // 这里取得坐标
            var path = new GraphicsPath();
            path.AddRectangle(this.canvas.SelectShape.GetBounds(this.canvas.shapes.GetMatrix()));
            if (path.PointCount == 0) return;
            var points = path.PathPoints;
            foreach (var item in changeStrategies)
            {
                if (item.isRight(points, pointF))
                {
                    changeStrategy = item;
                    return;
                }
            }
            changeStrategy = null;
            //base.LeftMouseDown(pointF);
        }

        public override void LeftMouseMove(PointF pointF)
        {
            if (changeStrategy != null && this.canvas.SelectShape != null)
            {
                // 这里要判断是否对齐网格
                changeStrategy.action(
                    this.canvas.SelectShape, 
                    this.startPoint, 
                    this.canvas.gridAlign(pointF));
                this.canvas.Refresh();
            }
            //base.LeftMouseMove(pointF);
        }

        public override void LeftMouseUp(PointF pointF)
        {
            // 结束更改操作，然后转成选择模式
            if (changeStrategy != null &&  this.canvas.SelectShape != null)
            {
                changeStrategy.action(
                    this.canvas.SelectShape, 
                    this.startPoint,
                    this.canvas.gridAlign(pointF));
                var old = this.canvas.SelectShape.DeepClone();  // 保存旧的
                this.canvas.SelectShape.ChangeComplated();      // 更改状态
                this.canvas.commandRecorder.addCommand(         // 发送命令
                    new Command.CommandResize() { 
                        OldShape = old,
                        NewShape = this.canvas.SelectShape
                    });
            }
            // 转成
            this.canvas.state = new StateSelected(this.canvas);
            this.canvas.Refresh();
            //base.LeftMouseUp(pointF);
        }
    }
}
