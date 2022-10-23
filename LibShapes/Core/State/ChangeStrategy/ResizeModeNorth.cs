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
    /// 北方更改大小
    /// </summary>
    public class ResizeModeNorth : IChangeStrategy
    {
        public void action(ShapeEle shape, PointF start_pointF, PointF end_pointF)
        {
            // 右边的话，是更改x，或者width，
            RectangleF rect = new RectangleF();
            var diff = end_pointF.Y - start_pointF.Y;
            if (shape.Height < 0)
            {
                rect.Height = -diff;
                rect.Height = diff;
                Trace.WriteLine($"修改h:{rect}");
            }
            else
            {
                rect.Y = diff;
                rect.Height = -diff;
                Trace.WriteLine($"修改y:{rect}");
            }
            shape.Change(rect);

            //throw new NotImplementedException();
        }

        public bool isRight(PointF[] pointFs, PointF start_pointF)
        {

            return DistanceCalculation.pointToLine(start_pointF, pointFs[0], pointFs[1]) <= DistanceCalculation.select_tolerance;

            //throw new NotImplementedException();
        }
    }
}
