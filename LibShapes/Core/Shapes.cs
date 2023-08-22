using Io.Github.Kerwinxu.LibShapes.Core.Serialize;
using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using Newtonsoft.Json;
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
        #region 保存读取相关

        private static  ISerialize serialize = new JsonSerialize();

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Shapes load(string filename)
        {
            if (serialize != null 
                && ! string.IsNullOrEmpty(filename) 
                && System.IO.File.Exists(filename)) 
            {
                return serialize.DeserializeObject<Shapes>(System.IO.File.ReadAllText(filename));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="filename"></param>
        public void save(string filename)
        {
            // 写入文件
            if (serialize != null)
            {
                System.IO.File.WriteAllText(filename, serialize.SerializeObject(this));
            }
        }


        #endregion


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
        public Paper.Paper Paper { get; set; }

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
            //Paper = new Paper.Paper();
            Vars = new Dictionary<string, string>();
        }

        #endregion
        
        #region 一堆方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="matrix">偏移和放大</param>
        /// <param name="isShowModelBackground">是否打印模板背景</param>
        public void Draw(Graphics g, Matrix matrix, bool isShowModelBackground=true)
        {
            // 1. 先初始化
            initGraphics(g);
            // 2. 绘制模板背景
            if (isShowModelBackground && this.Paper != null && this.Paper.ModelShape != null)
            {
                // 这个纸张的绘制，x，y都是0，width和height是模板的宽和高，不是纸张的。
                this.Paper.ModelShape.X = 0;
                this.Paper.ModelShape.Y = 0;
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
                    if (shape.IsFill)
                    {
                        if (shape.isVisible(pointTransform.GetMatrix(), pointF))
                        {
                            return shape;
                        }
                    }
                    else
                    {
                        if (shape.isOutlineVisible(pointTransform.GetMatrix(), pointF))
                        {
                            return shape;
                        }

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
            // 首先取得所有的id
            var ids = getIds(this.lstShapes);

            if (ids.Count == 0)
            {
                return 1;
            }

            int i = ids.Max() + 1;
            while (ids.Contains(i))
            {
                i++; // 不重复的，
            }
            return i; // 这里简单一点。

        }

        /// <summary>
        /// 取得所有的id
        /// </summary>
        /// <param name="shapeeles"></param>
        /// <returns></returns>
        private List<int>getIds(List<ShapeEle> shapeeles)
        {
            List<int> ids = shapeeles.Where(x => !(x is ShapeGroup)).Select(x => x.ID).ToList();
            foreach (var item in shapeeles.Where(x => x is ShapeGroup))
            {
                ids.AddRange(getIds(((ShapeGroup)item).shapes));
            }
            return ids;
        }

        /// <summary>
        /// 根据某个id取得相应的形状。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ShapeEle getShape(int id)
        {
            return  getShape(lstShapes, id);
        }

        private ShapeEle getShape(List<ShapeEle> shapeeles, int id)
        {
            if (shapeeles!=null)
            {
                foreach (var item in shapeeles)
                {
                    if (item.ID == id)
                    {
                        return item;
                    }

                    if (item is ShapeGroup) // 如果是群组
                    {
                        var tmp = getShape(((ShapeGroup)item).shapes,id);
                        if (tmp != null) return tmp;
                    }
                }

            }

            return null;
        }

        /// <summary>
        /// 将这个id的形状替换了。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shape"></param>
        public void replaceShape(int id, ShapeEle shape)
        {
            replaceShape(lstShapes, id, shape);
        }

        private void replaceShape(List<ShapeEle> shapeeles, int id, ShapeEle shape)
        {
            // 遍历所有的形状
            for (int i = 0; i < shapeeles.Count; i++)
            {
                if (shapeeles[i] is ShapeGroup)
                {
                    replaceShape(((ShapeGroup)shapeeles[i]).shapes, id, shape);
                }

                if (shapeeles[i].ID == id)
                {
                    // 找到了
                    shapeeles.RemoveAt(i);
                    shapeeles.Insert(i, shape);
                    return;
                }
            }
        }


        /// <summary>
        /// 这个是缩放到指定的大小。
        /// </summary>
        /// <param name="dpix"></param>
        /// <param name="dpiy"></param>
        /// <param name="width">像素宽度</param>
        /// <param name="height">像素高度</param>
        /// <param name="spacing">像素间距</param>
        public void zoomTo(float dpix, float dpiy, float width, float height, float spacing)
        {
            // 然后计算放大
            var width2 = (width - spacing * 2) / dpix * 25.4;   // 转成mm
            var height2 = (height - spacing * 2) / dpiy * 25.4; // 转成mm
            // 然后计算所有图形的的宽度和高度
            ShapeGroup group = new ShapeGroup();
            group.shapes = lstShapes.Select(x => x).ToList();// 这里用复制的方式
            // 这里要判断是否有纸张
            if (this.Paper != null && this.Paper.ModelShape != null)
            {
                group.shapes.Add(this.Paper.ModelShape);
            }
            // 如果没有图形，就直接退出
            if (group.shapes.Count == 0) return;
            //
            var rect = group.GetBounds(new Matrix());// 取得不存在放大偏移的情况下，这个的尺寸
            // 这里解方程，思路是，画布的宽度=倍数*（形状总宽度+两边间距的宽度）
            //width/dpix*25.4 = zoom * (rect.Width + spacing*2/dpix*25.4 /zoom)
            // width/dpix*25.4 = zoom * (rect.Width + spacing*2/dpix*25.4
            var scale1 = (width / dpix * 25.4f - spacing * 2 / dpix * 25.4f) / rect.Width;
            var scale2 = (height / dpiy * 25.4f - spacing * 2 / dpiy * 25.4f) / rect.Height;
            // 取得较小值
            this.pointTransform.Zoom = scale1 < scale2 ? scale1 : scale2; // 取得较小值。
            // 然后这里有一个偏移，要算正负两个方向的偏移，要流出两边的spacing，主要留出左边和上边就可以了。
            this.pointTransform.OffsetX = - rect.X + spacing / dpix * 25.4f ;
            this.pointTransform.OffsetY = - rect.Y + spacing / dpix * 25.4f ;
        }

        /// <summary>
        /// 深度复制一个
        /// </summary>
        /// <returns></returns>
        public Shapes DeepClone()
        {
            //这里用json的方式
            JsonSerialize jsonSerialize = new JsonSerialize();
            string json = jsonSerialize.SerializeObject(this);
            // 可能原因是存在自定义对象的集合，所以这里要用复杂的方式去做



            return jsonSerialize.DeserializeObject<Shapes>(json);
        }

        #endregion



    }
}
