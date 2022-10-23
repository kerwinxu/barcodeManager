using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.State.ChangeStrategy
{
    /// <summary>
    /// 移动
    /// </summary>
    public class MoveMode : IChangeStrategy
    {
        public void action(ShapeEle shape, PointF start_pointF, PointF end_pointF)
        {
            // 这个是更改xy的
            RectangleF rect = new RectangleF() { 
                X = end_pointF.X-start_pointF.X,
                Y = end_pointF.Y - start_pointF.Y
            };
            shape.Change(rect);
            //throw new NotImplementedException();
        }

        public bool isRight(PointF[] pointFs, PointF start_pointF)
        {
            return true;// 一般是最后一项。
            throw new NotImplementedException();
        }
    }
}
