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
    /// 形状的基类
    /// </summary>
    public abstract class ShapeEle
    {
        #region 一堆属性
        #region  设计
        /// <summary>
        /// 唯一的标识符
        /// </summary>
        [CategoryAttribute("设计")]
        public int ID { get; set; }

        [DescriptionAttribute("变量名"), DisplayName("变量名"), CategoryAttribute("布局")]
        public string VarName { get; set; }


        [Browsable(false)]//不在PropertyGrid上显示
        public string VarValue { get; set; }

        #endregion

        #region 布局

        [CategoryAttribute("布局")]
        public float X { get; set; }

        [CategoryAttribute("布局")]
        public float Y { get; set; }

        [DescriptionAttribute("宽度"), DisplayName("宽度"), CategoryAttribute("布局")]
        public float Width { get; set; }

        [DescriptionAttribute("高度"), DisplayName("高度"), CategoryAttribute("布局")]
        public float Height { get; set; }

        [DescriptionAttribute("角度"), DisplayName("角度"), CategoryAttribute("外观")]
        public float Angle { get; set; }


        #endregion

        #region 外观

        [DescriptionAttribute("边框颜色"), DisplayName("边框颜色"), CategoryAttribute("外观")]
        public Color PenColor { get; set; }

        [DescriptionAttribute("边框粗细"), DisplayName("边框粗细"), CategoryAttribute("外观")]
        public float PenWidth { get; set; }

        [DescriptionAttribute("虚线的样式"), DisplayName("虚线的样式"), CategoryAttribute("外观")]
        public DashStyle PenDashStyle { get; set; }

        [DescriptionAttribute("是否填充"), DisplayName("是否填充"), CategoryAttribute("外观")]
        public bool IsFill { get; set; }

        [DescriptionAttribute("填充颜色"), DisplayName("填充颜色"), CategoryAttribute("外观")]
        public Color FillColor { get; set; }



        #region 如下的几个是为了更改大小或者移动的时候用的
        float x_add, y_add, width_add, height_add;
        #endregion

        #endregion

        #endregion

        #region 操作

        public virtual void Draw(Graphics g, Matrix matrix) {
            // 首先取得绘图路径
            var path = GetGraphicsPath(matrix);
            // 定义画笔
            Pen pen = new Pen(PenColor);
            pen.Width = PenWidth;           // 画笔的粗细
            pen.DashStyle = PenDashStyle;   // 虚线的样式
            g.DrawPath(pen, path);          // 画边框
            if (IsFill) // 如果填充
            {
                Brush brush = new SolidBrush(FillColor);
                g.FillPath(brush, path);
            }
            path.Dispose();
        }


        /// <summary>
        /// 这个返回的是屏幕画布上的真实的坐标体系
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public virtual GraphicsPath GetGraphicsPath(Matrix matrix)
        {
            GraphicsPath path = GetGraphicsPathWithAngle();
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
            return path;

        }

        /// <summary>
        /// 返回不包括旋转的路径，且这个是虚拟世界的坐标。
        /// </summary>
        /// <returns></returns>
        public abstract GraphicsPath GetGraphicsPathWithAngle();

        /// <summary>
        /// 返回外接矩形
        /// </summary>
        /// <returns></returns>
        public virtual RectangleF GetBounds(Matrix matrix)
        {
            // 请注意，这里是以旋转后的。
            return GetGraphicsPath(matrix).GetBounds();
        }

        /// <summary>
        ///  这个点是否在这个图形内部，文字需要重写这个方法
        /// </summary>
        /// <param name="mousePointF"></param>
        /// <returns></returns>
        public virtual bool isContains(Matrix matrix, PointF mousePointF)
        {
            return GetGraphicsPath(matrix).IsVisible(mousePointF);
        }

        /// <summary>
        /// 这个表示是否被包含在这个矩形内
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool isBeContains(Matrix matrix, RectangleF rect)
        {
            return rect.Contains(GetBounds(matrix));
        }

        /// <summary>
        /// 真正的移动
        /// </summary>
        public virtual void move()
        {
            X += x_add;
            Y += y_add;
            // 清零
            x_add = 0;
            y_add = 0;
        }

        /// <summary>
        /// 真正的改变尺寸
        /// </summary>
        public virtual void resize()
        {
            X += x_add;
            Y += y_add;
            Width += width_add;
            Height += height_add;
            // 清零
            x_add = 0;
            y_add = 0;
            width_add = 0;
            height_add = 0;

        }

        public virtual void setVals(Dictionary<string, string> vars)
        {
            //首先判断是否有这个
            if (vars.ContainsKey(VarName))
            {
                VarName = vars[VarName]; // 这个变量的值
            }
            else
            {
                VarValue = string.Empty;         // 没有是空字符串。
            }
        }

        #region 如下的几个是添加add后的参数
        protected float getX()
        {
            return X + x_add;
        }
        protected float getY()
        {
            return Y + y_add;
        }
        protected float getWidth()
        {
            return Width + width_add;
        }
        protected float getHeight()
        {
            return Height + height_add;
        }


        #endregion

        #endregion
    }
}
