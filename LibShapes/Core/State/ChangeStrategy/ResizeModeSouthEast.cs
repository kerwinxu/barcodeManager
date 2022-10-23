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
    /// <summary>
    /// 东南方向更改
    /// </summary>
    public class ResizeModeSouthEast : IChangeStrategy
    {
        public void action(ShapeEle shape, PointF start_pointF, PointF end_pointF)
        {
            RectangleF rect = new RectangleF();
            var diffx = end_pointF.X - start_pointF.X;
            var diffy = end_pointF.Y - start_pointF.Y;
            if (shape.Width < 0)
            {
                rect.X = diffx;
            }
            else
            {
                rect.Width = diffx;
            }
            if (shape.Height < 0)
            {
                rect.Y = diffy;
            }
            else
            {
                rect.Height = diffy;
            }
            shape.Change(rect);
        }

        public Cursor changeCursor()
        {
            return Cursors.PanSE;
            throw new NotImplementedException();
        }

        public bool isRight(PointF[] pointFs, PointF start_pointF)
        {
            return DistanceCalculation.distance(start_pointF, pointFs[2]) <= DistanceCalculation.select_tolerance * 2;
        }
    }
}
