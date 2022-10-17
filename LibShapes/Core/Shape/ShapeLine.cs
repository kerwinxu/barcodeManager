using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    /// 线段
    /// </summary>
    public class ShapeLine : ShapeEle
    {
        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(getX(), getY(), getX() + getWidth(), getY() + getHeight());
            return path;

            //throw new NotImplementedException();
        }
    }
}
