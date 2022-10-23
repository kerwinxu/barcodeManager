using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using Io.Github.Kerwinxu.LibShapes.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.State.ChangeStrategy
{
    /// <summary>
    /// 东北方向更改
    /// </summary>
    public class ResizeModeNorthEast : IChangeStrategy
    {
        public void action(ShapeEle shape, PointF start_pointF, PointF end_pointF)
        {
            RectangleF rect = new RectangleF();
            var diffx = end_pointF.X - start_pointF.X;
            var diffy = end_pointF.Y - start_pointF.Y;
            if (shape.Width < 0)
            {
                rect.X = diffx;
                rect.Width = -diffx;
            }
            else
            {
                rect.Width = diffx;
            }

            if (shape.Height < 0)
            {
                rect.Y = -diffy;
                rect.Height = diffy;
            }
            else
            {
                rect.Y = diffy;
                rect.Height = -diffy;
            }
            Trace.WriteLine($"更改:{rect}");
            shape.Change(rect);
        }

        public bool isRight(PointF[] pointFs, PointF start_pointF)
        {
            return DistanceCalculation.distance(start_pointF, pointFs[1]) <= DistanceCalculation.select_tolerance * 2;
            
        }
    }
}
