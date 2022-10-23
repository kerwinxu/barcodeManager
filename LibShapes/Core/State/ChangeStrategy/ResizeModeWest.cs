using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using Io.Github.Kerwinxu.LibShapes.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.State.ChangeStrategy
{
    public class ResizeModeWest : IChangeStrategy
    {
        public void action(ShapeEle shape, PointF start_pointF, PointF end_pointF)
        {
            // 右边的话，是更改x，或者width，
            RectangleF rect = new RectangleF();
            var diff = end_pointF.X - start_pointF.X;
            if (shape.Width < 0)
            {
                rect.Width = -diff;
            }
            else
            {
                rect.X = diff;
                rect.Width = -diff;
            }
            shape.Change(rect);
        }

        public bool isRight(PointF[] pointFs, PointF start_pointF)
        {
            return DistanceCalculation.pointToLine(start_pointF, pointFs[0], pointFs[3]) <= DistanceCalculation.select_tolerance;
        }
    }
}
