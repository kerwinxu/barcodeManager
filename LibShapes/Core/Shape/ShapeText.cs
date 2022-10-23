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
    // todo 这个以后支持拉伸。

    /// <summary>
    /// 文本
    /// </summary>
    public class ShapeText : ShapeVar
    {
        #region 文本的属性
        [DescriptionAttribute("前缀"), DisplayName("前缀"), CategoryAttribute("文本")]
        public string Piefix { get; set; }


        [DescriptionAttribute("后缀"), DisplayName("后缀"), CategoryAttribute("文本")]
        public string Suffix { get; set; }
        #endregion

        #region 字体方面的属性

        [DescriptionAttribute("字体"), DisplayName("字体"), CategoryAttribute("字体")]
        public Font Font { get; set; }

        [DescriptionAttribute("水平对齐方式"), DisplayName("水平对齐方式"), CategoryAttribute("字体")]
        public StringAlignment Alignment { get; set; }

        [DescriptionAttribute("垂直对齐方式"), DisplayName("垂直对齐方式"), CategoryAttribute("字体")]
        public StringAlignment LineAlignment { get; set; }

        #endregion



        public override ShapeEle DeepClone()
        {
            // 这里用json的方式
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShapeText>(json);
            //throw new NotImplementedException();
        }

        public override  string getText()
        {
            return Piefix + (string.IsNullOrEmpty(this.VarName) ? StaticText : this.VarValue) + Suffix;
        }

        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddString(
                getText(),
                Font.FontFamily,
                (int)Font.Style,
                Font.Size,
                new RectangleF() {
                    X = getX(),
                    Y = getY(),
                    Width = getWidth(),
                    Height = getWidth(),
                },
                new StringFormat() { Alignment=Alignment, LineAlignment=LineAlignment}
                );
            
            return path;
            //throw new NotImplementedException();
        }

        public override RectangleF GetBounds(Matrix matrix)
        {
            // 这个应该调用的是ShapeEle中的,而不是上边的GetGraphicsPathWithAngle，
            // 上边的实际上只是文字范围内的图形，而这个是要
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
            // 返回这个路径
            return path.GetBounds();
            //return base.GetBounds(matrix);
        }
    }
}
