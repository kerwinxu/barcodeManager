using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using Io.Github.Kerwinxu.LibShapes.Core.State.ChangeStrategy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.State
{
    /// <summary>
    /// 创建状态。
    /// </summary>
    public  class StateCreate:State
    {
        public StateCreate(UserControlCanvas canvas, PointF start_pointF) : base(canvas, start_pointF)
        {

        }
        public StateCreate(UserControlCanvas canvas) : base(canvas)
        {

        }

        private Shapes oldShapes;

        public ShapeEle shape { get; set; }

        public StateCreate(UserControlCanvas canvas, ShapeEle shape):base(canvas)
        {
            this.shape = shape; // 保存要创建的图形
        }

        private IChangeStrategy strategy = new ResizeModeSouthEast();  // 更改的策略
        private ShapeEle newshape; // 建立的图形。

        public override void LeftMouseDown(PointF pointF)
        {
            oldShapes = this.canvas.shapes.DeepClone();
            // 这里先创建一个拷贝
            newshape = shape.DeepClone();
            // 判断是否需要对齐
            startPoint = this.canvas.gridAlign(pointF);
            // 这里的坐标要转换成虚拟中的坐标
            var point3 = cantosPointToVirtualPoint(startPoint);
            // 这个x和y就有了
            newshape.X = point3.X;
            newshape.Y = point3.Y;
            // 然后计算id
            newshape.ID = this.canvas.shapes.getNextId();
            Trace.WriteLine($"增加了一个新的图形，id:{newshape.ID}");
            // 然后添加到图形中
            this.canvas.addShape(newshape);
            // 刷新
            this.canvas.Refresh();
            
            //base.LeftMouseDown(pointF);
        }

        public override void LeftMouseMove(PointF pointF)
        {
            // 判断是否需要对齐
            var point2 = cantosPointToVirtualPoint(this.canvas.gridAlign(pointF));
            strategy.action(newshape, cantosPointToVirtualPoint(startPoint), point2);
            // 刷新
            this.canvas.Refresh();
            base.LeftMouseMove(pointF);
        }

        public override void LeftMouseUp(PointF pointF)
        {
            // 这里有一个特殊的情况，就是宽和高都是0的情况下，会死机

            var point2 = cantosPointToVirtualPoint(this.canvas.gridAlign(pointF));
            strategy.action(newshape, cantosPointToVirtualPoint(startPoint), point2);
            newshape.ChangeComplated();

            if (newshape.Width == 0 && newshape.Height == 0)
            {
                this.canvas.deleteShapes(newshape);
            }
            else
            {
                // 更改成选择模式
                this.canvas.state = new StateSelected(this.canvas);
                // 需要发送命令
                this.canvas.commandRecorder.addCommand(
                    new Command.CommandShapesChanged()
                    {
                        canvas=this.canvas,
                        OldShapes = this.oldShapes,
                        NewShapes = this.canvas.shapes.DeepClone()
                    }
                    ) ;
                // 这里还是发出改变通知
                this.canvas.changeSelect(newshape);
            }

            //base.LeftMouseUp(pointF);
            // 刷新
            this.canvas.Refresh();
        }

    }
}
