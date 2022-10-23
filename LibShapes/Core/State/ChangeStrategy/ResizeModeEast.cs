using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using Io.Github.Kerwinxu.LibShapes.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Io.Github.Kerwinxu.LibShapes.Core.State.ChangeStrategy
{
    public class ResizeModeEast : IChangeStrategy
    {
        public void action(ShapeEle shape, PointF start_pointF, PointF end_pointF)
        {
            // 右边的话，是更改x，或者width，
            RectangleF rect = new RectangleF();
            var diff = end_pointF.X - start_pointF.X;
            if (shape.Width < 0)
            {
                rect.X = diff;
            }
            else
            {
                rect.Width = diff;
            }
            shape.Change(rect);
            //throw new NotImplementedException();
        }

        public Cursor changeCursor()
        {
            return Cursors.PanEast;
            //throw new NotImplementedException();
        }

        public bool isRight(PointF[] pointFs, PointF start_pointF)
        {
            // 判断一句是跟右边的线足够的近。
            return DistanceCalculation.pointToLine(start_pointF, pointFs[1], pointFs[2]) <= DistanceCalculation.select_tolerance;

            //throw new NotImplementedException();
        }
    }
}
