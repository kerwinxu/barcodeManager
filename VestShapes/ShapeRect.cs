using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
//using System.Linq;
using System.Text;

namespace VestShapes
{

    [Serializable]
    //[ProtoContract]
    public class ShapeRect : ShapeEle
    {
        public override ShapeEle DeepClone()
        {
            ShapeRect shapeEle = new ShapeRect();
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

            //如下是子类单独的

            return shapeEle;
            //throw new NotImplementedException();
        }
        /**
        public override void Draw(Graphics g)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //旋转图形

            if (Route != 0)
            {
                PointF pZhongXin = new PointF(_X + _XAdd + (_Width + _WidthAdd) / 2, _Y + _YAdd + (_Height + _HeightAdd) / 2);
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            RectangleF rect = getRect();

            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;
            g.DrawRectangle(_myPen, rect.X, rect.Y,rect.Width,rect.Height);
            //throw new NotImplementedException();
            if (_isFill)
            {
                g.FillRectangle(new SolidBrush(_FillColor), rect.X, rect.Y, rect.Width, rect.Height);

            }

            g.ResetTransform();

        }
         * */


    }
}
