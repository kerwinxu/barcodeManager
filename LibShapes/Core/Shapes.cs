using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core
{
    /// <summary>
    /// 图形的集合
    /// </summary>
    public  class Shapes
    {
        #region 一堆属性，这些可以被json保存
        // 图形的集合
        public List<ShapeEle> lstShapes { get; set; }
        /// <summary>
        /// 坐标转换
        /// </summary>
        public PointTransform pointTransform { get; set; }

        /// <summary>
        /// 纸张的信息
        /// </summary>
        public Paper Paper { get; set; }

        /// <summary>
        /// 变量信息
        /// </summary>
        private Dictionary<string,string> _vars;

        public Dictionary<string,string> Vars
        {
            get { return _vars; }
            set { 
                _vars = value;
                // 然后这里直接更新吧。
                if (value != null && lstShapes != null && lstShapes.Count > 0)
                {
                    foreach (var item in lstShapes)
                    {
                        item.setVals(value);
                    }
                }
            
            }
        }


        #endregion

        #region 构造函数

        public Shapes()
        {
            lstShapes = new List<ShapeEle>();
            pointTransform = new PointTransform();
            Paper = new Paper();
            Vars = new Dictionary<string, string>();
        }

        #endregion

        #region 一堆方法

        // 绘图的要区分好几个地方,比如是在打印在纸张上还是打印在屏幕的画布上

        public void Draw(Graphics g, Matrix matrix, bool isShowPaperBack=true)
        {
            // 1. 先初始化
            initGraphics(g);
            // 2. 绘制模板背景
            if (isShowPaperBack && this.Paper != null && this.Paper.ModelShape != null )
            {
                // 这个纸张的绘制，x，y都是0，width和height是模板的宽和高，不是纸张的。
                this.Paper.ModelShape.X = 0;
                this.Paper.ModelShape.Y = 0;
                // 
                this.Paper.createModelShape();
                this.Paper.ModelShape.Draw(g, matrix);
            }
            // 3. 显示所有的图形。
            if (lstShapes != null && lstShapes.Count > 0)
            {
                foreach (var item in lstShapes)
                {
                    item.Draw(g, matrix); // 绘图每一个
                }
            }
        }

        /// <summary>
        /// 这个返回偏移和放大后的矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix GetMatrix()
        {
            return pointTransform.GetMatrix();
        }

        /// <summary>
        /// 这个主要是用来初始化的，高精度的
        /// </summary>
        /// <param name="g"></param>
        private void initGraphics(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;     // 高质量
            g.PixelOffsetMode = PixelOffsetMode.HighQuality; // 指定高质量、低速度呈现。
            g.PageUnit = GraphicsUnit.Millimeter;            //将毫米设置为度量单位
        }

        /// <summary>
        /// 鼠标单机选择一个形状
        /// </summary>
        /// <param name="pointF"></param>
        /// <returns></returns>
        public ShapeEle getSelectShape(PointF pointF)
        {
            if (lstShapes != null && lstShapes.Count > 0)
            {
                foreach (var shape in lstShapes)
                {
                    if (shape.isContains(pointTransform.GetMatrix(),pointF))
                    {
                        return shape;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 选择矩形框内的形状
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public List<ShapeEle> getSelectShapes(RectangleF rect)
        {
            var matrix = pointTransform.GetMatrix();
            if (lstShapes!= null && lstShapes.Count > 0)
            {
                return lstShapes.Where(x => x.isBeContains(matrix, rect)).ToList();
            }
            return null;

        }

        public int getNextId()
        {
            // todo
            return 0;
        }

        public ShapeEle getShape(int id)
        {
            // todo
            return null;
        }

        public void forward(ShapeEle shape)
        {
            // todo
        }
        public void forwardToFront(ShapeEle shape)
        {
            // todo
        }

        public void backward(ShapeEle shape)
        {
            // todo
        }
        public void backwardToEnd(ShapeEle shape)
        {
            // todo
        }

        public void addGroup()
        {
            // todo
        }




        #endregion



    }
}
