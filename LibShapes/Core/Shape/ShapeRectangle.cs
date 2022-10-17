using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    /// 矩形
    /// </summary>
    public class ShapeRectangle : ShapeEle
    {
        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new System.Drawing.RectangleF() {
                X = getX(),
                Y = getY(),
                Width = getWidth(),
                Height = getHeight()
            });
            return path;
            //throw new NotImplementedException();
        }
    }
}
