using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
////using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VestShapes
{

    [Serializable]
    //[ProtoContract]
    public class ShapeArc : ShapeEle
    {
        //这个圆弧是根据椭圆画的，只是添加了两个监督角度。

        private float _startAngle = 0f;
        private float _endAngle = 90f;

        [DescriptionAttribute("开始角度"), DisplayName("开始角度"), CategoryAttribute("布局")]
        [XmlElement]
        public float StartAngle
        {
            get
            {
                return _startAngle;
            }
            set
            {
                _startAngle = value;
            }
        }
        [DescriptionAttribute("结束角度"), DisplayName("结束角度"), CategoryAttribute("布局")]
        [XmlElement]
        public float EndAngle
        {
            get
            {
                return _endAngle;
            }
            set
            {
                _endAngle = value;
            }
        }

        public override ShapeEle DeepClone()
        {
            ShapeArc shapeEle = new ShapeArc();
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
            shapeEle.StartAngle = StartAngle;
            shapeEle.EndAngle = EndAngle;

            return shapeEle;
            //throw new NotImplementedException();
        }

        public override GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            GraphicsPath path = new GraphicsPath();
            try
            {
                path.AddArc(getRect(), StartAngle, EndAngle);
            }
            catch (Exception ex)
            {
                RectangleF rect = new RectangleF();

                rect.X = _X + _XAdd;
                rect.Y = _Y + _YAdd;
                rect.Width = 10;
                rect.Height = 10;
                path.AddArc(rect, 0, 90);
                ////ClsErrorFile.WriteLine("这里是一个圆弧出现参数错误，异常处理是构造一个默认宽和高都是10，角度为0和90的扇形", ex);
                //throw;
            }

            return path;
            //return base.getGraphicsPath();
        }



    }
}
