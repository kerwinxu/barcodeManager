using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Xml.Serialization;

using Xuhengxiao.MyDataStructure;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
//using ProtoBuf;

namespace VestShapes
{
    /// <summary>
    /// 这个类是用来封装图形的，内部还有一个类的信息是纸张信息，
    /// </summary>
    [Serializable]
    ////[ProtoContract]
    public  class Shapes
    {
        //用这么多来表示这些是可以序列化的，并且被这个支持的。
        [XmlArray]
        [XmlArrayItem(Type=typeof(ShapeLine)),
        XmlArrayItem(Type = typeof(ShapeRect)),
        XmlArrayItem(Type = typeof(ShapeEllipse)),
        XmlArrayItem(Type= typeof(ShapeArc)),
        XmlArrayItem(Type = typeof(ShapePie)),
        XmlArrayItem(Type = typeof(ShapeImage)),
        XmlArrayItem(Type = typeof(ShapeStateText)),
        XmlArrayItem(Type = typeof(shapeVarText)),
        XmlArrayItem(Type = typeof(ShapeEle)),
        XmlArrayItem(Type = typeof(ShapeBarcode)),
        XmlArrayItem(Type = typeof(ShapeRoundRect)),
        XmlArrayItem(Type = typeof(ShapeGroup)),
        XmlArrayItem(Type = typeof(shapeSingleText)),
        XmlArrayItem(Type = typeof(shapeMultiText))]
        public  ArrayList arrlistShapeEle = new ArrayList();

        //[XmlElement(Type = typeof(ClsPageSettings))]
        /// <summary>
        /// 这个是页面的设置
        /// </summary>
        [XmlElement]
        public ClsPageSettings BarcodePageSettings=new ClsPageSettings();

        private List<clsKeyValue> _arrlistKeyValue;
        [XmlArray]
        [XmlArrayItem(Type = typeof(clsKeyValue))]
        public List<clsKeyValue> arrlistKeyValue
        {
            get
            {
                return _arrlistKeyValue;
            }
            set
            {
                if ((value != null))
                {
                    _arrlistKeyValue = value;
                }
            }
        }


        // 如下的取消，因为不支持序列化
        //private Dictionary<string,string> _arrlistKeyValue;

        //public Dictionary<string,string> arrlistKeyValue
        //{
        //    get { return _arrlistKeyValue; }
        //    set { _arrlistKeyValue = value; }
        //}




        protected float _Zoom = 1f;

        [XmlIgnore]
        public float fltCanvasWidth;
        [XmlIgnore]
        public float fltCanvasHeight;

        /**
        [XmlIgnore]
        public  bool isDrawDridding;//是否画网格。
        [XmlIgnore]
        public float fltGriddingInterval ;

         * */

        //这个类好像没有分辨率什么事情，因为画图的单位是毫米。
        //protected float _fltDPIX =96 , _fltDPIY = 96;

        /**
        public float DPIX
        {
            get
            {
                return _fltDPIX;
            }
            set
            {
                _fltDPIX = value;

            }
        }

        public float DPIY
        {
            get
            {
                return _fltDPIY;
            }
            set
            {
                _fltDPIY = value;
            }
        }

         **/
        
        [XmlElement]
        public float Zoom
        {
            get
            {
                return _Zoom;
            }
            set
            {
                _Zoom = value;
                if (arrlistShapeEle != null)
                {
                    foreach (ShapeEle item in arrlistShapeEle)
                    {
                        item.Zoom = _Zoom;
                    }
                }
            }
        }


        [XmlIgnore]
        public static string StrCode;

        
        public Shapes()
        {
            //空白的
            BarcodePageSettings = new ClsPageSettings();
            arrlistKeyValue = new List<clsKeyValue>();
            arrlistShapeEle = new ArrayList();

        }


        /// <summary>
        /// 添加一个形状
        /// </summary>
        /// <param name="shapeEle"></param>
        public void addShapeEle(ShapeEle shapeEle)
        {
            arrlistShapeEle.Add(shapeEle);
            
        }

        public void addShapeEle(ShapeEle shapeEle, int index)
        {
            arrlistShapeEle.Insert(index, shapeEle);
        }

        public int IndexOf(ShapeEle shapeEle)
        {
            return arrlistShapeEle.IndexOf(shapeEle);
        }

        public int Count()
        {
            return arrlistShapeEle.Count;
        }


        /// <summary>
        /// 删除一个形状
        /// </summary>
        /// <param name="shapeEle"></param>
        public void deleteShapeEle(ShapeEle shapeEle)
        {
            arrlistShapeEle.Remove(shapeEle);
        }

        /// <summary>
        /// 根据一个点来返回选择的形状
        /// </summary>
        /// <param name="pointF"></param>
        /// <returns></returns>
        public ShapeEle getSelectShapeEle(PointF pointF)
        {


            //判断是否在其中一个的图形内。
            if ((arrlistShapeEle != null) && (arrlistShapeEle.Count > 0))
            {
                for (int i = arrlistShapeEle.Count - 1; i > -1; i--)//从最后的判断
                {
                    //只所以需要判断而不是直接设置是为了以后多选的情况，比如说按ctrl 键，如果在这里直接设置的话容易覆盖原先已经选择的。
                    bool  isS = ((ShapeEle)arrlistShapeEle[i]).isContains(new PointF(pointF.X, pointF.Y));
                    if (isS)
                    {
                        ((ShapeEle)arrlistShapeEle[i]).isSelect = true;
                        return (ShapeEle)arrlistShapeEle[i];//这个点只判断最上边的图形就可以了。
                    }

                }

            }

            return null ;//返回选择的图形，如果没有，里边就没有数据
        }

        /// <summary>
        /// 画图，包含纸张背景
        /// </summary>
        /// <param name="g"></param>
        /// <param name="arrlistMatrix"></param>
        public void Draw(Graphics g, List<Matrix> arrlistMatrix)
        {
            try
            {
                //偏移，绘制画布背景啦，比如说选择的是A4纸张，就绘制A4纸张大小的背景。
                BarcodePageSettings.DrawModelBackground(g, 0, 0, Zoom, arrlistMatrix);//绘制模板的背景

            }
            catch (Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);
                //throw;
            }


            DrawShapes(g, arrlistMatrix);

            /** 
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //如下被认为可以清晰文字。
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
             * */

            //如果需要绘制刻度尺，那么这里就用一个双缓冲来实现。
            //绘制条形码纸范围
            //SolidBrush brush = new SolidBrush(Color.White);



            //如下是画一个条形码模板的底
            //g.FillRectangle(brush, 0f, 0f, barcodePaper.Width * Zoom, barcodePaper.Height * Zoom);


        }

        /// <summary>
        /// 画图，特定的转换中指定设置偏移
        /// </summary>
        /// <param name="g"></param>
        /// <param name="KongX"></param>
        /// <param name="KongY"></param>
        public void Draw(Graphics g, float KongX, float KongY)
        {
            List<Matrix> listTmp = new List<Matrix>();
            System.Drawing.Drawing2D.Matrix m = new Matrix();
            m.Translate(KongX, KongY);
            listTmp.Add(m);
            Draw(g, listTmp);//调用这个进行绘图

        }

        /// <summary>
        /// 初始化Graphics,主要是设置单位和图片清晰之类的，以便有个统一的Graphics对象
        /// </summary>
        /// <param name="g"></param>
        protected  void initGraphics(Graphics g)
        {
            if (g!=null)
            {
                //单位一定要是MM。
                g.PageUnit = GraphicsUnit.Millimeter;
                //如下被认为可以清晰文字。g
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //文字抗锯齿
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                //设置高质量插值法
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            }
        }

        /// <summary>
        /// 绘制图形，不包含背景
        /// </summary>
        /// <param name="g"></param>
        /// <param name="listMatrix"></param>
        public  void DrawShapes(Graphics g, List<Matrix> listMatrix)
        {

            initGraphics(g);
            //
            if (arrlistShapeEle != null)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    //
                    //将变量替换到相应的值后再绘图
                    if ((arrlistKeyValue != null))
                    {
                        item.updateVarValue(arrlistKeyValue);
                    }
                    item.Draw(g, listMatrix);
                }

            }
        }

        /// <summary>
        /// 在一个指定位置绘制所有形状。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="fltKongX"></param>
        /// <param name="fltKongY"></param>
        public void DrawShapes(Graphics g, float fltKongX, float fltKongY)
        {
            List<Matrix> listTmp = new List<Matrix>();

            System.Drawing.Drawing2D.Matrix m = new Matrix();
            m.Translate(fltKongX, fltKongY);
            listTmp.Add(m);
            DrawShapes(g, listTmp);//调用这个进行绘图

        }


        /// <summary>
        /// 根据一个矩形返回被选择的形状
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public ArrayList getSelectShapeEle(RectangleF rect)
        {
            ArrayList arraylistSel = new ArrayList();

            //首先是判断是否有选择到其中的。
            foreach (ShapeEle item in arrlistShapeEle)
            {
                bool isS = item.isContains(rect);
                //如果有被选择的，当然首先是更改状态为选择
                if (isS)
                {
                    item.isSelect = true;//判断位啦,好像这个也没有什么用。
                    arraylistSel.Add(item);

                }

            }

            return arraylistSel;
        }
    }

  
}
