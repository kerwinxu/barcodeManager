using Io.Github.Kerwinxu.LibShapes.Utils;
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

        /// <summary>
        /// 构造函数
        /// </summary>
        public ShapeEle()
        {
            // 提供一些默认的参数
            PenWidth = 1;
            PenColor = Color.Black;

        }


        #region 一堆属性
        #region  设计
        /// <summary>
        /// 唯一的标识符
        /// </summary>
        [CategoryAttribute("设计")]
        public int ID { get; set; }

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
            try
            {
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
            catch (Exception ex)
            {

                //throw;
            }
  
        }


        /// <summary>
        /// 这个返回的是屏幕画布上的真实的坐标体系
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public virtual GraphicsPath GetGraphicsPath(Matrix matrix)
        {
            GraphicsPath path = null;
            if (getWidth() == 0 || getHeight() == 0)
            {
                // 这里表示只是一条线段或者一个点了
                path = new GraphicsPath();
                path.AddLine(new PointF(getX(), getY()), new PointF(getX() + getWidth(), getY() + getHeight()));
            }
            else
            {
                path = GetGraphicsPathWithAngle();
            }

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
        /// 矫正矩形，宽和高都不能是复数
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        protected RectangleF correctRectangle(RectangleF rect)
        {
            RectangleF rect2 = new RectangleF() { 
                X = rect.X,
                Y = rect.Y,
                Width = rect.Width,
                Height = rect.Height,
            };
            if (rect2.Width < 0)
            {
                rect2.X += rect2.Width;
                rect2.Width = -rect2.Width;
            }
            if (rect2.Height < 0)
            {
                rect2.Y += rect2.Height;
                rect2.Height = -rect2.Height;
            }

            return rect2;
        }

        /// <summary>
        /// 返回不包括旋转的路径，且这个是虚拟世界的坐标，不用考虑画布中实际的坐标。
        /// </summary>
        /// <returns></returns>
        public virtual GraphicsPath GetGraphicsPathWithAngle()
        {
            GraphicsPath path = new GraphicsPath();

            var rect = new System.Drawing.RectangleF()
            {
                X = getX(),
                Y = getY(),
                Width = getWidth(),
                Height = getHeight()
            };
            var rect2 = correctRectangle(rect);

            path.AddRectangle(rect2);
            return path;
        }

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
        /// 选择的容忍度
        /// </summary>
        private Pen pen_select_tolerance = new Pen(Color.White) {
            Width = DistanceCalculation.select_tolerance
        };

        /// <summary>
        ///  这个点是否在这个图形的轮廓上
        /// </summary>
        /// <param name="mousePointF"></param>
        /// <returns></returns>
        public virtual bool isOutlineVisible(Matrix matrix, PointF mousePointF)
        {

            var pen = new Pen(new SolidBrush(this.PenColor), this.PenWidth);
            return GetGraphicsPath(matrix).IsOutlineVisible(mousePointF, pen);

            //return GetGraphicsPath(matrix).IsVisible(mousePointF);
        }

        /// <summary>
        ///  这个点是否在这个图形的的内部
        /// </summary>
        /// <param name="mousePointF"></param>
        /// <returns></returns>
        public virtual bool isVisible(Matrix matrix, PointF mousePointF)
        {
            return GetGraphicsPath(matrix).IsVisible(mousePointF);

            //return GetGraphicsPath(matrix).IsVisible(mousePointF);
        }


        /// <summary>
        /// 这个表示是否被包含在这个矩形内
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool isBeContains(Matrix matrix, RectangleF rect)
        {
            var rect2 = GetBounds(matrix);
            var tmp = rect.Contains(rect2);
            return rect.Contains(rect2);
        }

  

        public virtual void setVals(Dictionary<string, string> vars)
        {
            // 什么都不做，子类如果需要，就自己实现。
        }

        /// <summary>
        /// 根据这个矩形更改
        /// </summary>
        /// <param name="rect"></param>
        public  void Change(RectangleF rect)
        {
            x_add = rect.X;
            y_add = rect.Y;
            width_add = rect.Width;
            height_add = rect.Height;
        }


        public virtual void ChangeComplated()
        {
            // 将更改固定下来
            X += x_add;
            Y += y_add;
            Width += width_add;
            Height += height_add;
            // 清零
            x_add = 0;
            y_add = 0;
            width_add = 0;
            height_add = 0;

            // 这里要注意矫正 todo
            if (Width < 0)
            {
                X += Width;
                Width = -Width;
            }
            if (Height < 0)
            {
                Y += Height;
                Height = -Height;
            }

        }

        /// <summary>
        /// 深度拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract ShapeEle DeepClone();

        public override bool Equals(object obj)
        {
            // 首先判断是否是

            var shape = obj as ShapeEle;
            if (shape == null) return false; // 转换失败就是不同啦

            return this.X == shape.X && 
                this.Y == shape.Y &&
                this.Width == shape.Width &&
                this.Height == shape.Height &&
                this.Angle == shape.Angle &&
                this.ID == shape.ID &&
                this.PenColor == shape.PenColor &&
                this.PenWidth == shape.PenWidth &&
                this.PenDashStyle == shape.PenDashStyle &&
                this.IsFill == shape.IsFill &&
                this.FillColor == shape.FillColor;

            //return base.Equals(obj);
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

        public override int GetHashCode()
        {
            return ID * base.GetHashCode();
            //return base.GetHashCode();
        }
    }
}
