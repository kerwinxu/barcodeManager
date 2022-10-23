using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    /// 椭圆弧
    /// </summary>
    public class ShapeArc : ShapeRectangle
    {
        public ShapeArc()
        {
            // 设置默认的角度
            StartAngle = 0;
            SweepAngle = 90;
        }

        [DescriptionAttribute("弧线的起始角度"), DisplayName("弧线的起始角度"), CategoryAttribute("外观")]
        public float StartAngle { get; set; }


        [DescriptionAttribute("startAngle 和弧线末尾之间的角度"), DisplayName("夹角"), CategoryAttribute("外观")]
        public float SweepAngle { get; set; }

        public override ShapeEle DeepClone()
        {
            // 这里用json的方式
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShapeArc>(json);
            //throw new NotImplementedException();
        }

        public override System.Drawing.RectangleF GetBounds(Matrix matrix)
        {
            // 这个实际上是矩形的
            GraphicsPath path = base.GetGraphicsPathWithAngle();
            // 这里加上旋转
            Matrix matrix1 = new Matrix();
            // 这里按照中心点旋转,
            var rect = path.GetBounds();
            var centerPoint = new PointF() { X = rect.X + rect.Width / 2, Y = rect.Y + rect.Height / 2 };
            matrix1.RotateAt(Angle, centerPoint);
            Matrix matrix2 = matrix.Clone();
            matrix2.Multiply(matrix1);
            // 应用这个转换
            path.Transform(matrix2);
            return path.GetBounds();
            //return base.GetBounds(matrix);
        }

        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            GraphicsPath path = new GraphicsPath();
            var rect = new System.Drawing.RectangleF()
            {
                X = getX(),
                Y = getY(),
                Width = getWidth(),
                Height = getHeight(),
            };

            var rect2 = correctRectangle(rect);
            path.AddArc(
                   rect2,
                   StartAngle,
                   SweepAngle);
  
            return path;
            //return base.GetGraphicsPathWithAngle();
        }
    }
}
