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

        public ShapeText() : base()
        {
            // 设置摩尔默认的字体。
            Font = new Font("Arial", 8);
            Piefix = "前缀";
            StaticText = "文本";
            Suffix = "后缀";
            PenWidth = 0;
            IsFill = true;
            FillColor = Color.Black;
        }


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

        public override bool isVisible(Matrix matrix, PointF mousePointF)
        {
            // todo 这个是判断文本框的内部。
            return base.isVisible(matrix, mousePointF);
        }

        public override bool isOutlineVisible(Matrix matrix, PointF mousePointF)
        {
            // 这个会
            return base.isOutlineVisible(matrix, mousePointF) || isVisible(matrix, mousePointF);
        }

        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            GraphicsPath path = new GraphicsPath();
            var rect = new RectangleF()
            {
                X = getX(),
                Y = getY(),
                Width = getWidth(),
                Height = getWidth(),
            };
            path.AddString(
                getText(),
                Font.FontFamily,
                (int)Font.Style,
                Font.Size,
                rect,
                new StringFormat() { Alignment=Alignment, LineAlignment=LineAlignment}
                );
            
            return path;
            //throw new NotImplementedException();
        }

        public override bool isBeContains(Matrix matrix, RectangleF rect)
        {
            // 这个要判断是否整个文本内容是否在这个矩形内，而不是单独的看这个文本的框。
            // 这里要判断是否有文字，有时候没有文字，就按照基类的吧
            RectangleF rect2; 
            if(getText() != string.Empty)
            {
                // 如果有文字，返回文字的范围
                rect2 = GetTrueBounds(matrix);
            }
            else
            {
                // 如果没有文字，就选择外边边框的范围
                rect2 = GetBounds(matrix);
            }
            
            return rect.Contains(rect2);
            //return base.isBeContains(matrix, rect);
        }

        /// <summary>
        /// 这个返回的是矩形边框
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public override RectangleF GetBounds(Matrix matrix)
        {
            // 这里不用这个文本图形的GetGraphicsPathWithAngle，而是调用矩形的。
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
            // 返回这个矩形
            return path.GetBounds();
            //return base.GetBounds(matrix);
        }

        /// <summary>
        /// 返回的是实际文字的边框
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public RectangleF GetTrueBounds(Matrix matrix)
        {
            
            GraphicsPath path = GetGraphicsPathWithAngle();
            // 这里加上旋转
            Matrix matrix1 = new Matrix();
            // 这里按照中心点旋转,
            var rect = path.GetBounds();
            // 我这里做一个判断，如果这里上边的全是0，那么就手动计算宽度和高度吧
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
