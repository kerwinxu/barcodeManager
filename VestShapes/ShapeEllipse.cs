using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
////using System.Linq;
using System.Text;

namespace VestShapes
{

    [Serializable]
    //[ProtoContract]
    public class ShapeEllipse : ShapeEle
    {
        public override ShapeEle DeepClone()
        {
            ShapeEllipse shapeEle = new ShapeEllipse();
            shapeEle.Zoom = Zoom;
            shapeEle.X = X;
            shapeEle.Y = Y;
            shapeEle.Width = Width;
            shapeEle.Height = Height;
            shapeEle.isFill = isFill;
            shapeEle.PenColor = PenColor;
            shapeEle.PenWidth = PenWidth;
            shapeEle.PenDashStyle = PenDashStyle;
            shapeEle.Route = Route;

            return shapeEle;
            //throw new NotImplementedException();
        }

        public override GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(getRect());
            return path;
            //return base.getGraphicsPath();
        }

    }
}
