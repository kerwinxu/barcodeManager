using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
//using System.Linq;
using System.Text;

namespace VestShapes
{

    //绘制圆角矩形。
    [Serializable]
    //[ProtoContract]
    public class ShapeRoundRect : ShapeEle
    {
        protected float _fltCornerRadius = 2f;

        [DescriptionAttribute("圆角的角度"), DisplayName("圆角的角度"), CategoryAttribute("设计")]
        public float CornerRadius
        {
            get
            {
                return _fltCornerRadius / Zoom;
            }
            set
            {
                if (value > 0)
                {
                    _fltCornerRadius = value * Zoom;
                }
            }
        }

        public override float Zoom
        {
            get
            {
                return base.Zoom;
            }
            set
            {
                float fR = CornerRadius;
                base.Zoom = value;
                CornerRadius = fR;
            }
        }
        public override ShapeEle DeepClone()
        {
            ShapeRoundRect shapeEle = new ShapeRoundRect();
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
            shapeEle.CornerRadius = CornerRadius;

            return shapeEle;
            //throw new NotImplementedException();
        }



        public override GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            return CreateRoundedRectanglePath(getRect(), _fltCornerRadius);
            //return base.getGraphicsPath();
        }



        protected void DrawRoundRectangle(Graphics g, Pen pen, RectangleF rect, float cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                g.DrawPath(pen, path);
            }
        }
        protected void FillRoundRectangle(Graphics g, Brush brush, RectangleF rect, float cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                g.FillPath(brush, path);
            }
        }
        protected GraphicsPath CreateRoundedRectanglePath(RectangleF rect, float cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }

    }
}
