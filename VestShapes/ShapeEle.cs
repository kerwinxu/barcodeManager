using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Xuhengxiao.MyDataStructure;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using com.google.zxing;
using com.google.zxing.qrcode.decoder;
using COMMON = com.google.zxing.common;
using BarcodeLib;
using System.Reflection;
using System.Drawing.Design;
using System.Windows.Forms.Design;
//using ProtoBuf;

namespace VestShapes
{
    /// <summary>
    /// ShapeImage 的变量类型
    /// </summary>
    public enum enumImageVar { 从文件插入,从变量获取文件名 };

    [Serializable]
    //[ProtoContract]
    public abstract class ShapeEle
    {
        //如下的这4个仅仅是用来绘图时实际的坐标，是经过放大后的坐标。
        protected float _X=0f, _Y=0f, _Width=0f, _Height=0f;
        protected double _route=0, _routeAdd=0;

        //如下的这几个是不能暴露给客户的，所以我隐藏了.
        [XmlIgnore]
        public float _XAdd=0f, _YAdd=0f, _WidthAdd=0f, _HeightAdd=0f;

        protected float fltJingDu = 3f;//就是隔多远判断是选中。如果已有改成毫米单位，这个也要改的。已经改成1毫米范围内了

        //这个形状有这个是因为有形状要放大的时候，不能用图片放大，因为那牵涉到失真。
        protected float _Zoom=1f;

        //填充信息
        protected bool _isFill;//是否填充
        //protected SolidBrush _FillBrush ;//填充都是黑色的填充，不能序列化，所以取消了。
        protected Color _FillColor=Color.Black;


        //画笔信息,画笔颜色都是黑色的
        //protected Pen _myPen ;
        protected Color _penColor=Color.Black;
        protected float _penWidth = 1;
        protected DashStyle _dashStyle = DashStyle.Solid;


        //如下是变量信息
        [XmlIgnore]
        public string _strVarName="";
        [XmlIgnore]
        public string _strVarValue="";

        public ShapeEle()
        {
            //_myPen = new Pen(Color.Black, 0.5f);
            //_FillBrush = new SolidBrush(Color.Black);//填充都是黑色的填充
            _FillColor = Color.Black;

        }
        //如下几个属性是为了群组中旋转的
        private bool _isInGroup;
        /// <summary>
        /// 是否在群组中，群组也有旋转的，
        /// </summary>
        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public bool isInGroup
        {
            get { return _isInGroup; }
            set { _isInGroup = value; }
        }


        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public virtual float Zoom
        {
            get
            {
                return _Zoom;
            }
            set
            {
                //因为更改这个放大倍数，很多东西都要更改，首先保存相关信息
                float fltX, fltY, fltW, fltH, fltPW;
                fltX = X;
                fltY = Y;
                fltW = Width;
                fltH = Height;
                fltPW = PenWidth;

                //再设置放大率
                _Zoom = value;

                //再重新计算相关的XYWH。
                X = fltX;
                Y = fltY;
                Width = fltW;
                Height = fltH;
                PenWidth = fltPW;


            }
        }

        [CategoryAttribute("布局")]
        [XmlElement]
        public float X
        {
            get
            {
                return _X / _Zoom;
            }
            set
            {
                _X = value * _Zoom;
            }
        }

        [CategoryAttribute("布局")]
        [XmlElement]
        public float Y
        {
            get
            {
                return _Y / _Zoom;
            }
            set
            {
                _Y = value * _Zoom;
            }
        }
        [DescriptionAttribute("宽度"),DisplayName("宽度"), CategoryAttribute("布局")]
        [XmlElement]
        public virtual  float Width
        {
            get
            {
                return _Width / _Zoom;
            }
            set
            {
                _Width = value * _Zoom;
            }
        }
        [DescriptionAttribute("高度"), DisplayName("高度"), CategoryAttribute("布局")]
        [XmlElement]
        public virtual  float Height
        {
            get
            {
                return _Height / _Zoom;
            }
            set
            {
                _Height = value * _Zoom;
            }
        }



        [DescriptionAttribute("是否填充"), DisplayName("是否填充"), CategoryAttribute("设计")]
        [XmlElement]
        public bool isFill
        {
            get
            {
                return _isFill;
            }
            set
            {
                _isFill = value;
            }
        }
        [DescriptionAttribute("画笔颜色"), DisplayName("画笔颜色"), CategoryAttribute("设计")]
        [XmlIgnore]
        public Color PenColor
        {
            get
            {
                return _penColor;
            }
            set
            {
                _penColor = value;
            }
        }

        [DescriptionAttribute("填充颜色"), DisplayName("填充颜色"), CategoryAttribute("设计")]
        [XmlIgnore]
        public Color FillColor
        {
            get
            {
                return _FillColor;
            }
            set
            {
                _FillColor = value;
            }
        }

        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]//如下是保存画笔颜色时用的属性
        public int  PenColorSerializer
        {
            get
            {
                return ColorTranslator.ToWin32(_penColor);
                
            }
            set
            {
                _penColor = ColorTranslator.FromWin32(value);
                
            }
        }

        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]//如下是保存画笔颜色时用的属性
        public int FillColorSerializer
        {
            get
            {
                return ColorTranslator.ToWin32(_FillColor);

            }
            set
            {
                _FillColor = ColorTranslator.FromWin32(value);

            }
        }


        [DescriptionAttribute("画笔宽度"), DisplayName("画笔宽度"), CategoryAttribute("设计")]
        [XmlElement]
        public float PenWidth
        {
            get
            {
                return _penWidth/Zoom;
            }
            set
            {
                _penWidth= value*Zoom;
            }
        }
        [DescriptionAttribute("虚线的样式"), DisplayName("虚线的样式"), CategoryAttribute("设计")]
        [XmlElement]
        public DashStyle PenDashStyle
        {
            get
            {
                return _dashStyle;
            }
            set
            {
                _dashStyle = value;
            }

        }
        [DescriptionAttribute("旋转的角度"), DisplayName("旋转的角度"), CategoryAttribute("设计")]
        [XmlElement]
        public double Route
        {
            get
            {
                return _route;
            }
            set
            {
                _route = value;
            }
        }


        

        /**

        // 应用到 DefaultFileName 属性的 TypeConverter 特性。
        [TypeConverter(typeof(VarNameDetails)), DescriptionAttribute("变量名"),
        CategoryAttribute("变量信息")]
        public virtual  string VarName
        {
            get { return _strVarName; }
            set { _strVarName = value; }
        }
        [DescriptionAttribute("变量值"),
        CategoryAttribute("变量信息")]
        public virtual  string VarValue
        {
            get { return _strVarValue; }
            set { _strVarValue = value; }
        }

         * */

        /**

        public float getXAdd()
        {
            return _XAdd;
        }
        public void setXAdd(float fltXAdd)
        {
            _XAdd = fltXAdd;
        }
        public float getYAdd()
        {
            return _YAdd;
        }
        public void setYAdd(float fltYAdd)
        {
            _YAdd = fltYAdd;
        }
        public float getWidthAdd()
        {
            return _WidthAdd;
        }
        public void setWidthAdd(float fltWidthAdd)
        {
            _WidthAdd = fltWidthAdd;
        }
        public float getHeightAdd()
        {
            return _HeightAdd;
        }
        public void setHeightAdd(float fltHeightAdd)
        {
            _HeightAdd = fltHeightAdd;
        }

         * */

        protected  Font ChangeFontSize(Font font, float fontSize)
        {
            Font fontNew = new Font("Arial", 6);//默认值


            if (font != null)
            {
                    try
                    {

                    fontNew = new Font(font.Name, fontSize,
                        font.Style, font.Unit,
                        font.GdiCharSet, font.GdiVerticalFont);
                    }
                    catch (System.Exception ex)
                    {
                        ClsErrorFile.WriteLine(ex);

                    }

            }
            return fontNew;
        }


        //画笔属性

        public   virtual PointF[] getRealPoint()
        {
            //存在旋转的情况
            PointF[] arrPointF = new PointF[4];

            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;

            double dblRoute = _route + _routeAdd;

            //求中心点
            PointF pointCenter = new PointF(_X + _XAdd + (_Width + _WidthAdd) / 2, _Y + _YAdd + (_Height + _HeightAdd) / 2);

            //注意这里是顺时针
            arrPointF[0] = PointRotate(pointCenter, new PointF(fltx, flty), dblRoute);
            arrPointF[1] = PointRotate(pointCenter, new PointF(fltx + fltw, flty), dblRoute);
            arrPointF[2] = PointRotate(pointCenter, new PointF(fltx + fltw, flty + flth), dblRoute);
            arrPointF[3] = PointRotate(pointCenter, new PointF(fltx, flty + flth), dblRoute);

            return arrPointF;

        }

        [XmlIgnore]
        public bool isSelect;//被选择，好像没什么用

        /// <summary>
        /// 这个函数只是获得包含这个形状的矩形而已.因为存在着把图形弄小的情况
        /// </summary>
        /// <returns></returns>
        public   RectangleF getRect()
        {
            RectangleF rect = new RectangleF();

            rect.X = _X + _XAdd;

            if ((_Width + _WidthAdd) < 0)
            {
                rect.Width = Math.Abs(_Width + _WidthAdd);
                rect.X = rect.X - rect.Width;

            }
            else
            {
                rect.Width = _Width + _WidthAdd;
            }

            rect.Y = _Y + _YAdd;

            if ((_Height + _HeightAdd) < 0)
            {
                rect.Height = Math.Abs(_Height + _HeightAdd);
                rect.Y =rect.Y - rect.Height;
            }
            else
            {
                rect.Height = _Height + _HeightAdd;
            }

            return rect;

        }

        /// <summary>
        /// 取得中心点，没有偏移的。
        /// </summary>
        /// <returns></returns>
        public virtual PointF getCentrePoint()
        {
            GraphicsPath path = getGraphicsPathNoOffsetRoute();//取得路径
            RectangleF rect = path.GetBounds();
            return  new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            //return new PointF(_X + _XAdd + (_Width + _WidthAdd) / 2, _Y + _YAdd + (_Height + _HeightAdd) / 2); 

        }


        public virtual void Draw(Graphics g, ArrayList arrlistMatrix)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;


            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            GraphicsPath path = getGraphicsPath(arrlistMatrix);

            //如下这个就是画边界
            try
            {

                g.DrawPath(_myPen, path);


            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //throw;
            }

            //throw new NotImplementedException();
            if (_isFill)
            {
                try
                {

                    g.FillPath(new SolidBrush(_FillColor), path);

                }
                catch (Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                    //throw;
                }

            }

            g.ResetTransform();

        }

        /// <summary>
        /// 这个绘图会加上偏移
        /// </summary>
        /// <param name="g"></param>
        /// <param name="fltKongX"></param>
        /// <param name="fltKongY"></param>
        public virtual void Draw(Graphics g, float fltKongX, float fltKongY)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            GraphicsPath path = getGraphicsPath(fltKongX, fltKongY);

            //如下这个就是画边界
            try
            {

                    g.DrawPath(_myPen, path);


            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //throw;
            }

            //throw new NotImplementedException();
            if (_isFill)
            {
                try
                {

                        g.FillPath(new SolidBrush(_FillColor), path);

                }
                catch (Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                    //throw;
                }

            }

            /**
            //如下的这个是偏移些位置
            g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);

            //如下是先从绘制矩形中的拷贝的，然后再修改
            if (Route != 0)
            {
                PointF pZhongXin = getCentrePoint();
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            //如下这个就是画边界
            try
            {
                using (GraphicsPath path = getGraphicsPath())
                {
                    g.DrawPath(_myPen, path);
                }

            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //throw;
            }

            //throw new NotImplementedException();
            if (_isFill)
            {
                try
                {
                    using (GraphicsPath path = getGraphicsPath())
                    {
                        g.FillPath(new SolidBrush(_FillColor), path);
                    }
                }
                catch (Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                    //throw;
                }

            }

            //如下的这个是恢复原先的，负数.
            g.TranslateTransform(-fltKongX, -fltKongY);
            g.ResetTransform();//恢复原先的坐标系。
             **/
        }

        /// <summary>
        /// 这个绘图会根据形状角度，群组角度和路径自动绘图，只要多态掉路径定义就可以更改绘图了
        /// </summary>
        /// <param name="g"></param>
        public virtual void Draw(Graphics g)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //如下是先从绘制矩形中的拷贝的，然后再修改
            if (Route != 0)
            {
                PointF pZhongXin = new PointF(_X + _XAdd + (_Width + _WidthAdd) / 2, _Y + _YAdd + (_Height + _HeightAdd) / 2);
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }


            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            //如下这个就是画边界
            try
            {
                using (GraphicsPath path = getGraphicsPathNoOffsetRoute())
                {
                    g.DrawPath(_myPen, path);
                }

            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //throw;
            }

            //throw new NotImplementedException();
            if (_isFill)
            {
                try
                {
                    using (GraphicsPath path = getGraphicsPathNoOffsetRoute())
                    {
                        g.FillPath(new SolidBrush(_FillColor), path);
                    }
                }
                catch (Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                    //throw;
                }

            }

            g.ResetTransform();

        }

        /// <summary>
        /// 这个是加上所有前面的变换矩阵得到的路径
        /// </summary>
        /// <param name="arrlistMatrix"></param>
        /// <returns></returns>
        public virtual GraphicsPath getGraphicsPath(ArrayList  arrlistMatrix)
        {

            GraphicsPath path = getGraphicsPath();//首先取得没有偏移但有旋转的路径

            //再反转这个个变换
            arrlistMatrix.Reverse();

            if ((arrlistMatrix!=null)&&(arrlistMatrix.Count>0))//只有数量大于0才能做如下的
            {
                for (int i = 0; i < arrlistMatrix.Count; i++)
                {
                    path.Transform((Matrix)arrlistMatrix[i]);

                }
            }
            return path;

        }



        /// <summary>
        /// 取得文件路径，这个是有偏移和旋转的路径
        /// </summary>
        /// <param name="fltKongX"></param>
        /// <param name="fltKongY"></param>
        /// <returns></returns>
        public virtual GraphicsPath getGraphicsPath(float fltKongX, float fltKongY)
        {
            GraphicsPath path = getGraphicsPath();//首先取得没有偏移但有旋转的路径

            //做一个变换矩阵加上偏移和旋转
            System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
            //g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
            m.Translate(fltKongX, fltKongY);
            
            path.Transform(m);//应用变换矩阵

            return path;
        }

        /// <summary>
        /// 这个取得的是包含旋转,但没有包含偏移的路径
        /// 因为这个旋转是自己的，而偏移是外部的，
        /// </summary>
        /// <returns></returns>
        public virtual  GraphicsPath getGraphicsPath()
        {
            GraphicsPath path =getGraphicsPathNoOffsetRoute();
            //做一个变换矩阵加上偏移和旋转
            System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
            //g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
            m.RotateAt((float)(_route + _routeAdd), getCentrePoint());
            path.Transform(m);//应用变换矩阵

            return path;

        }

        /// <summary>
        /// 生成形状的路径，这个是没有偏移和旋转的路径,我只需要多态这个就可以了
        /// </summary>
        /// <returns></returns>
        public virtual GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(getRect());
            return path;

        }
        /// <summary>
        /// 这个取消掉了。
        /// </summary>
        /// <returns></returns>
        public abstract ShapeEle DeepClone();

        /// <summary>
        /// 这个函数就是根据两个点设置X,Y,W,H。
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public virtual void ShapeInit(PointF p1, PointF p2)
        {
            _X = Math.Min(p1.X, p2.X);
            _Y = Math.Min(p1.Y, p2.Y);
            _Width = Math.Max(p1.X, p2.X) - _X;
            _Height = Math.Max(p1.Y, p2.Y) - _Y;
        }

        /// <summary>
        /// 只是根据两个点返回矩形坐标
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static RectangleF getXYWH(float x1, float y1, float x2, float y2)
        {
            float tempx1, tempx2, tempy1, tempy2;
            tempx1 = Math.Min(x1, x2);
            tempy1 = Math.Min(y1, y2);
            tempx2 = Math.Max(x1, x2);
            tempy2 = Math.Max(y1, y2);

            return new RectangleF(tempx1, tempy1, tempx2 - tempx1, tempy2 - tempy1);
        }

        /// <summary>
        /// 这个方法是根据指定的作为基点（这个基点是不动的），根据鼠标的其实终止坐标来变化图形的，注意返回的是增加或者减少的值。
        /// 这个方法经测试没有达到预期
        /// </summary>
        /// <param name="pointJieDian"></param>
        /// <param name="arrPointF"></param>
        /// <param name="startPointF"></param>
        /// <param name="endPointF"></param>
        /// <returns></returns>
        public  PointF[] DestortShape(PointF pointJieDian, PointF[] arrPointF, PointF startPointF, PointF endPointF)
        {
            ArrayList arrlistAdd = new ArrayList();

            //首先初始化所有增值为0
            for (int i = 0; i < arrPointF.Length; i++)
            {
                PointF pZeo = new PointF();
                arrlistAdd.Add(pZeo);
            }
            //再强制转换成这种数组
            PointF[] arrPointAdd = ((PointF[])(arrlistAdd.ToArray(typeof(PointF))));

            
            /**如下的不可行
             * 

            float fltXBili = (endPointF.X - startPointF.X) / (startPointF.X - pointJieDian.X);//这里好像能出现除0的情况,所以我前面要判断
            for (int i = 0; i < arrPointF.Length; i++)
            {
                arrPointAdd[i].X = (arrPointF[i].X - pointJieDian.X) * fltXBili;
            }

            float fltYBili = (endPointF.Y - startPointF.Y) / (startPointF.Y - pointJieDian.Y);//这里好像能出现除0的情况,所以我前面要判断
            for (int i = 0; i < arrPointF.Length; i++)
            {
                arrPointAdd[i].Y = (arrPointF[i].Y - pointJieDian.Y) * fltYBili;
            }

             * */

            //如果出现如下这种情况，也就是把这些点都压缩到X轴上了，那么增加点就是全部是负数的。
            /**
            if (endPointF.X == pointJieDian.X)
            {
                for (int i = 0; i < arrPointF.Length; i++)
                {
                    arrPointAdd[i].X = pointJieDian.X - arrPointF[i].X;
                }

            }
            else
            {
                float fltXBili = (endPointF.X - startPointF.X) / (startPointF.X- pointJieDian.X);//这里好像能出现除0的情况,所以我前面要判断
                for (int i = 0; i < arrPointF.Length; i++)
                {
                     arrPointAdd[i].X =(arrPointF[i].X-pointJieDian.X)*fltXBili;
                }

            }
            //类似X轴，Y轴也会出现这种情况
            if (endPointF.Y == pointJieDian.Y)
            {
                for (int i = 0; i < arrPointF.Length; i++)
                {
                    arrPointAdd[i].Y = pointJieDian.Y - arrPointF[i].Y;
                }

            }
            else
            {
                float fltYBili = (endPointF.Y - startPointF.Y) / (startPointF.Y - pointJieDian.Y);//这里好像能出现除0的情况,所以我前面要判断
                for (int i = 0; i < arrPointF.Length; i++)
                {
                    arrPointAdd[i].Y = (arrPointF[i].Y - pointJieDian.Y) * fltYBili;
                }

            }
             * */

            return arrPointAdd;
        }



        /// <summary>
        /// 判断是否包含在其中的。
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool isContains(RectangleF rect)
        {
            //只用一句话就可以了。
            return  rect.Contains(getGraphicsPath().GetBounds());

            //如下的是增加别人破解难度的，纯粹没用的
            #region


            ///我实现是这样实现的，
            ///首先取得所有的点，再排序，取得XY的最小最大值，在区间内就是在其中。
            ///
            ArrayList arrX = new ArrayList();
            ArrayList arrY = new ArrayList();

            foreach (PointF item in getRealPoint())
            {
                arrX.Add(item.X);
                arrY.Add(item.Y);
            }

            //排序
            arrX.Sort();
            arrY.Sort();

            //取得XY的极值
            float fltXMin = Convert.ToSingle(arrX[0].ToString());
            float fltXMax = Convert.ToSingle(arrX[arrX.Count - 1].ToString());
            float fltYMin = Convert.ToSingle(arrY[0].ToString());
            float fltYMax = Convert.ToSingle(arrY[arrY.Count - 1].ToString());

            //再判断，根据最小最大的关系。
            if ((fltXMin > rect.X) &&
                (fltXMax < (rect.X + rect.Width)) &&
                (fltYMin > rect.Y) &&
                (fltYMax < (rect.Y + rect.Height)))
                return true;

            return false;
            #endregion
        }

        public virtual void Move(PointF pointF)
        {
            _XAdd = pointF.X;
            _YAdd = pointF.Y;
        }


        /// <summary>
        /// 仅仅设置增加的
        /// </summary>
        /// <param name="rect"></param>
        public virtual void Resize(RectangleF rect)
        {
            _XAdd = rect.X;
            _YAdd = rect.Y;
            _WidthAdd = rect.Width;
            _HeightAdd = rect.Height;

        }

        /// <summary>
        /// 我的移动和更改大小和旋转图形都是增加了一个变量来实现的，当结束有，用这个方法来加上这个变量
        /// </summary>
        public virtual void ReInit()
        {
            _X += _XAdd;

            if ((_Width + _WidthAdd) < 0)
            {
                _Width = Math.Abs(_Width + _WidthAdd);
                _X = _X - _Width;
            }
            else
            {
                _Width += _WidthAdd;
            }

            _Y += _YAdd;

            if ((_Height + _HeightAdd) < 0)
            {
                _Height = Math.Abs(_Height + _HeightAdd);
                _Y = _Y - _Height;
            }
            else
            {
                _Height += _HeightAdd;
            }


            _route += _routeAdd;

            _XAdd = 0;
            _YAdd = 0;
            _WidthAdd = 0;
            _HeightAdd = 0;
            _routeAdd = 0;
        }

        /// <summary>
        /// 对一个坐标点按照一个中心进行旋转
        /// </summary>
        /// <param name="center">中心点</param>
        /// <param name="p1">要旋转的点</param>
        /// <param name="angle">旋转角度，笛卡尔直角坐标</param>
        /// <returns></returns>
        protected PointF PointRotate(PointF center, PointF p1, double angle)
        {
            if (angle == 0)
                return p1;

            PointF tmp = new PointF();
            double angleHude = (360 - angle) * Math.PI / 180;/*角度变成弧度*/
            double x1 = (p1.X - center.X) * Math.Cos(angleHude) + (p1.Y - center.Y) * Math.Sin(angleHude) + center.X;
            double y1 = -(p1.X - center.X) * Math.Sin(angleHude) + (p1.Y - center.Y) * Math.Cos(angleHude) + center.Y;
            tmp.X = (float)x1;
            tmp.Y = (float)y1;
            return tmp;
        }

        /// <summary>
        /// 点到点之间的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private double GetDistance(PointF p1, PointF p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));

        }


        /// <summary>
        /// 判断点是否在graphicspath中，这个已经包含了直线判断和文字判断
        /// </summary>
        /// <param name="arrPointF"></param>
        /// <param name="MousePoint"></param>
        /// <returns></returns>
        public virtual bool isContains(PointF MousePoint)
        {
            //我想用graphicspath来判断这个是否在其中，这样子会精简很多
            //只有一句话了，很精简
            //return getGraphicsPath().IsVisible(MousePoint);

            //首选用路径判断
            GraphicsPath gpath = getGraphicsPath();//取得路径

            if (gpath.IsVisible(MousePoint))
            {
                return true;//返回真就可以了
            }

            //如下还的判断线段
            //GraphicsPath.PathTypes 属性，这个返回的是一个数组
            //值  含义
            // 0  指示此点是图形的起始点。
            // 1  指示此点是线段的两个终结点之一。
            // 3   指示此点是立方贝塞尔样条的终结点或控制点。
            //0x7 对三个低序位（指示点类型）之外的所有位进行掩码。
            //0x20 指定此点是一个标记。
            //0x80 指定此点是闭合子路径（图形）中的最后一点。
            //一个选段总是前面一个0，后边一个1，因为第一个点是图形的起始点

            //取得所有点
            PointF[]pathPointfs=gpath.PathPoints;
            byte[] pathPointType1 = gpath.PathTypes;//获得点的类型

            //线段总是先一个0，再一个1
            for (int i = 0; i < pathPointType1.Length; i++)
            {
                float fltDistance;

                //直线的判定如果这个为1，就判断前一个是否为0
                if ((pathPointType1[i]==1)//如果这个等于1
                    &&(i-1>=0)&&            //并且上一个没有超出边界,实际上这个不太需要
                    (pathPointType1[i-1]==0))//并且上一个等于0
                {
                    fltDistance = getPointLineDistance(pathPointfs[i], pathPointfs[i-1], MousePoint);

                    //如果这个距离小于精度
                    if (fltDistance <= fltJingDu)
                        return true;

                } 

                //为了应对string类型的，我决定每个点都判断距离
                fltDistance = getPointDistance(pathPointfs[i], MousePoint);
                //如果这个距离小于精度
                if (fltDistance <= fltJingDu)
                    return true;


            }

            //到这里肯定返回假了
            return false;
            
            //如下的也没有用，只是为了增加别人破解的难度
            Region region = new Region(getGraphicsPath());
            region.Intersect(new RectangleF(MousePoint, new SizeF(10, 10)));

            Bitmap bitmap=new Bitmap(1000,1000);
            Graphics g=Graphics.FromImage(bitmap);
            g.PageUnit=GraphicsUnit.Millimeter;

            return !region.IsEmpty(g);


            //如下的没有作用，只是为了增加别人破解。
            int intResult = PtInPolygon(MousePoint, getRealPoint());

            if (intResult == -1)
                return false;

            return true;

            /**
            System.Drawing.Drawing2D.GraphicsPath   myGraphicsPath=new System.Drawing.Drawing2D.GraphicsPath();
            Region myRegion=new Region();                        
            myGraphicsPath.Reset();   

            myGraphicsPath.AddPolygon(getRealPoint() );  
            myRegion.MakeEmpty();   
            myRegion.Union(myGraphicsPath);   
            //返回判断点是否在多边形里
            return  myRegion.IsVisible(MousePoint);      
             * */

        }

        /// <summary>
        /// 功能：判断点是否在多边形内 
        /// 方法：求解通过该点的水平线与多边形各边的交点 
        /// 结论：单边交点为奇数，成立! 
        /// 参数： 返回1表示肯定在多边形内；-1肯定不在多边形内；0表示在多边形的边上；
        /// Point p 指定的某个点 
        /// Point[] ptPolygon 多边形的各个顶点坐标（首末点可以不一致） 
        /// int nCount 多边形定点的个数 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ptPolygon"></param>
        /// <returns></returns>
        public int PtInPolygon(PointF p, PointF[] ptPolygon)
        {
            int nCount = ptPolygon.Length;
            bool isBeside = false;// 记录是否在多边形的边上

            #region 矩形外区域
            double maxx;
            double maxy;
            double minx;
            double miny;
            if (nCount > 0)
            {
                maxx = ptPolygon[0].X;
                minx = ptPolygon[0].X;
                maxy = ptPolygon[0].Y;
                miny = ptPolygon[0].Y;

                for (int j = 1; j < nCount; j++)
                {
                    if (ptPolygon[j].X >= maxx)
                        maxx = ptPolygon[j].X;
                    else if (ptPolygon[j].X <= minx)
                        minx = ptPolygon[j].X;

                    if (ptPolygon[j].Y >= maxy)
                        maxy = ptPolygon[j].Y;
                    else if (ptPolygon[j].Y <= miny)
                        miny = ptPolygon[j].Y;
                }

                if ((p.X > maxx) || (p.X < minx) || (p.Y > maxy) || (p.Y < miny))
                    return -1;
            }


            #endregion

            #region 射线法
            int nCross = 0;

            for (int i = 0; i < nCount; i++)
            {
                PointF p1 = ptPolygon[i];
                PointF p2 = ptPolygon[(i + 1) % nCount];

                // 求解 y=p.y 与 p1p2 的交点

                if (p1.Y == p2.Y) // p1p2 与 y=p0.y平行 
                {
                    if (p.Y == p1.Y && p.X >= Math.Min(p1.X, p2.X) && p.X <= Math.Max(p1.X, p2.X))
                    {
                        isBeside = true;
                        continue;
                    }
                }

                if (p.Y < Math.Min(p1.Y, p2.Y) || p.Y > Math.Max(p1.Y, p2.Y)) // 交点在p1p2延长线上 
                    continue;


                // 求交点的 X 坐标 -------------------------------------------------------------- 
                double x = (double)(p.Y - p1.Y) * (double)(p2.X - p1.X) / (double)(p2.Y - p1.Y) + p1.X;

                if (x > p.X)
                    nCross++; // 只统计单边交点 
                else if (x == p.X)
                    isBeside = true;
            }

            if (isBeside)
                return 0;//多边形边上
            else if (nCross % 2 == 1)// 单边交点为偶数，点在多边形之外 --- 
                return 1;//多边形内

            return -1;//多边形外
            #endregion
        }


        //按照角度排序
        protected  PointF[] AngleSort(PointF[] p)
        {
            float xEX = p[0].X;
            float yEX = p[0].X;
            for (int i = 0; i <= p.Length - 1; i++)
            {
                for (int j = 0; j <= p.Length - 1; j++)
                {
                    //if (p[j + 1] < p[j])
                    if (p[j] == p[0])
                    {
                        break;
                    }
                    if (Angle(p[0], p[j + 1]) < Angle(p[0], p[j]))
                    {
                        PointF temp;
                        temp = p[j];
                        p[j] = p[j + 1];
                        p[j + 1] = temp;
                    }
                }
            }
            return p;
        }

        //判断角度
        private double Angle(PointF p0, PointF p1)
        {
            double si;
            double l = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2));
            double l2 = Math.Abs(p1.X - p0.X);
            if (p1.Y < p0.Y)
            {
                si = Math.Acos(l2 / l);
            }
            else
            {
                si = Math.Acos(-l2 / l);
            }
            return si;
        }

        /// <summary>
        /// 移动，更改大小都在这个方法中，以后会添加旋转
        /// </summary>
        /// <param name="strState"></param>
        /// <param name="startPointf"></param>
        /// <param name="endPointf"></param>
        public virtual void Redim(string strState, PointF startPointf, PointF endPointf)
        {
            float startX = startPointf.X;
            float startY = startPointf.Y;
            float endX = endPointf.X;
            float endY = endPointf.Y;

            RectangleF rect = new RectangleF();

            switch (strState)
            {
                case "move":
                    this.Move(new PointF(endX - startX, endY - startY));
                    break;
                case "West":
                    rect.X = endX - startX;
                    rect.Width = startX - endX;
                    this.Resize(rect);
                    break;
                case "East":
                    rect.Width = endX - startX;
                    this.Resize(rect);
                    break;
                case "North":
                    rect.Y = endY - startY;
                    rect.Height = startY - endY;
                    this.Resize(rect);
                    break;
                case "South":
                    rect.Height = endY - startY;
                    this.Resize(rect);
                    break;
                case "NorthEast":
                    rect.Width = endX - startX;
                    rect.Y = endY - startY;
                    rect.Height = startY - endY;
                    this.Resize(rect);
                    break;
                case "SouthWest":
                    rect.X = endX - startX;
                    rect.Width = startX - endX;
                    rect.Height = endY - startY;
                    this.Resize(rect);
                    break;
                case "SouthEast":
                    rect.Width = endX - startX;
                    rect.Height = endY - startY;
                    this.Resize(rect);
                    break;
                case "NorthWest":
                    rect.X = endX - startX;
                    rect.Width = startX - endX;
                    rect.Y = endY - startY;
                    rect.Height = startY - endY;
                    this.Resize(rect);
                    break;

                default:
                    break;
            }


        }

        float getPointDistance(PointF p1, PointF p2)
        {
            return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        protected  float getPointLineDistance(PointF PA, PointF PB, PointF P3)
        {

            //----------图2--------------------
            float a, b, c;
            a = getPointDistance(PB, P3);
            if (a <= 0.00001)
                return 0.0f;
            b = getPointDistance(PA, P3);
            if (b <= 0.00001)
                return 0.0f;
            c = getPointDistance(PA, PB);
            if (c <= 0.00001)
                return a;//如果PA和PB坐标相同，则退出函数，并返回距离
            //------------------------------

            if (a * a >= b * b + c * c)//--------图3--------
                return b;      //如果是钝角返回b
            if (b * b >= a * a + c * c)//--------图4-------
                return a;      //如果是钝角返回a

            //图1
            float l = (a + b + c) / 2;     //周长的一半
            float s = (float)Math.Sqrt(l * (l - a) * (l - b) * (l - c));  //海伦公式求面积，也可以用矢量求
            return 2 * s / c;
        }


        //如下是添加一个方法，这个方法是输入的变量信息来更新变量值，这个主要是某些需要变量的方法来实现这个。
        public virtual bool  updateVarValue(ArrayList arrlistKeyValue)
        {
            //首先设置到空值
            string str1 = _strVarValue;
            _strVarValue = "";


            foreach (clsKeyValue item in arrlistKeyValue)
            {
                if (item.Key == _strVarName)
                {
                    /**不用在这里判断了
                    //是否改变
                    bool isChange = false;
                    if (str1 != item.Value)
                    {
                        isChange = true;
                                              
                    }
                     * */

                    _strVarValue = item.Value;
                    //更新变量值
                    //return isChange;//返回是否更新到不同的值

                }

            }

            if (str1 == _strVarValue)
            {
                return false;

            }
            else
            {
                return true;
            }


        }



    }

    //绘制圆角矩形。
    [Serializable]
    //[ProtoContract]
    public class ShapeRoundRect : ShapeEle
    {
        protected float _fltCornerRadius=2f;

        [DescriptionAttribute("圆角的角度"), DisplayName("圆角的角度"), CategoryAttribute("设计")]
        public float CornerRadius
        {
            get
            {
                return _fltCornerRadius/Zoom;
            }
            set
            {
                if (value > 0)
                {
                    _fltCornerRadius = value*Zoom;
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



        protected  void DrawRoundRectangle(Graphics g, Pen pen, RectangleF rect, float  cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                g.DrawPath(pen, path);
            }
        }
        protected  void FillRoundRectangle(Graphics g, Brush brush, RectangleF rect, float  cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                g.FillPath(brush, path);
            }
        }
        protected  GraphicsPath CreateRoundedRectanglePath(RectangleF rect, float  cornerRadius)
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

    [Serializable]
    //[ProtoContract]
    public class ShapeRect : ShapeEle
    {
        public override ShapeEle DeepClone()
        {
            ShapeRect shapeEle = new ShapeRect();
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

            return shapeEle;
            //throw new NotImplementedException();
        }

        /**
        public override void Draw(Graphics g)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //旋转图形

            if (Route != 0)
            {
                PointF pZhongXin = new PointF(_X + _XAdd + (_Width + _WidthAdd) / 2, _Y + _YAdd + (_Height + _HeightAdd) / 2);
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            RectangleF rect = getRect();

            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;
            g.DrawRectangle(_myPen, rect.X, rect.Y,rect.Width,rect.Height);
            //throw new NotImplementedException();
            if (_isFill)
            {
                g.FillRectangle(new SolidBrush(_FillColor), rect.X, rect.Y, rect.Width, rect.Height);

            }

            g.ResetTransform();

        }
         * */


    }

    [Serializable]
    //[ProtoContract]
    public class ShapeSelRect : ShapeGroup
    {
        //这个类好像不用关心放大倍数问题

        //我将已经选择的形状也放在这里，因为这个形状就是选择框
        /**
        [XmlArray(ElementName = "arrlistShapeEle")]
        [XmlArrayItem(Type = typeof(ShapeLine)),
        XmlArrayItem(Type = typeof(ShapeRect)),
        XmlArrayItem(Type = typeof(ShapeEllipse)),
        XmlArrayItem(Type = typeof(ShapeArc)),
        XmlArrayItem(Type = typeof(ShapePie)),
        XmlArrayItem(Type = typeof(ShapeImage)),
        XmlArrayItem(Type = typeof(ShapeStateText)),
        XmlArrayItem(Type = typeof(shapeVarText)),
        XmlArrayItem(Type = typeof(ShapeEle)),
        XmlArrayItem(Type = typeof(ShapeBarcode))
        ]
        //public ArrayList arrlistSelShapes = new ArrayList();//已经被选择的形状。
         * */

        public ShapeSelRect(ArrayList arrlistShape)
        {
            //这样赋值才避免因为操作这个arrlistShapeEle而引发arrlistShape问题
            arrlistShapeEle = new ArrayList();
            foreach (ShapeEle item in arrlistShape)
            {
                arrlistShapeEle.Add(item);
            }

            SetSelRectXYWH();

        }
        /// <summary>
        /// 这个的选中只是选中最外边的边框就算选中了
        /// </summary>
        /// <param name="MousePoint"></param>
        /// <returns></returns>
        public override bool isContains(PointF MousePoint)
        {
            GraphicsPath path=new GraphicsPath();
            path.AddRectangle(getGraphicsPath().GetBounds());
            return path.IsVisible(MousePoint);
            //return base.isContains(MousePoint);
        }

        /// <summary>
        /// 改变
        /// </summary>
        /// <param name="strState"></param>
        /// <param name="startPointf"></param>
        /// <param name="endPointf"></param>
        public override void Redim(string strState, PointF startPointf, PointF endPointf)
        {
            if (Count() > 0)
            {
                if (!UserControlCanvas.isAlignGridding)
                {
                    //如果不对齐，就仅仅需要如下传递参数就可以了。
                    foreach (ShapeEle item in arrlistShapeEle)
                    {
                        item.Redim(strState, startPointf, endPointf);

                    }
                }
                else
                {
                    //如果对齐，就得判断了

                    //如下是取得原先的XYWH，不加上ADD的那个。
                    float fltStartShapeElesX = 0;
                    float fltStartShapeElesY= 0;
                    float fltStartShapeElesW = 0;
                    float fltStartShapeElesH = 0;

                    if (Count() > 0)
                    {
                        ArrayList arrlistX = new ArrayList();
                        ArrayList arrlistY = new ArrayList();

                        foreach (ShapeEle item in arrlistShapeEle)
                        {
                            arrlistX.Add(item.X);
                            arrlistY.Add(item.Y);
                            arrlistX.Add(item.X + item.Width);
                            arrlistY.Add(item.Y + item.Height);


                        }

                        arrlistX.Sort();
                        arrlistY.Sort();

                        fltStartShapeElesX = Convert.ToSingle(arrlistX[0].ToString())*Zoom;
                        fltStartShapeElesY = Convert.ToSingle(arrlistY[0].ToString())*Zoom;
                        fltStartShapeElesW = (Convert.ToSingle(arrlistX[arrlistX.Count - 1].ToString()) )*Zoom;//这个获得宽度加上X的数值
                        fltStartShapeElesH = (Convert.ToSingle(arrlistY[arrlistY.Count - 1].ToString()) )*Zoom;//这个是获得高度加上Y的数值
                    }

                    float fltStartMouseX = startPointf.X;
                    float fltStartMouseY = startPointf.Y;
                    float fltEndMouseX = endPointf.X;
                    float fltEndMouseY = endPointf.Y;

                    float fx1 = (float)Math.Round((fltStartShapeElesX + fltEndMouseX - fltStartMouseX) / UserControlCanvas.GriddingInterval / Zoom, 0) * UserControlCanvas.GriddingInterval * Zoom - fltStartShapeElesX;
                    float fy1 = (float)Math.Round((fltStartShapeElesY + fltEndMouseY - fltStartMouseY) / UserControlCanvas.GriddingInterval / Zoom, 0) * UserControlCanvas.GriddingInterval * Zoom - fltStartShapeElesY;
                    float fw1 = (float)Math.Round((fltStartShapeElesW + fltEndMouseX - fltStartMouseX) / UserControlCanvas.GriddingInterval / Zoom, 0) * UserControlCanvas.GriddingInterval * Zoom - fltStartShapeElesW;
                    float fh1 = (float)Math.Round((fltStartShapeElesH + fltEndMouseY - fltStartMouseY) / UserControlCanvas.GriddingInterval / Zoom, 0) * UserControlCanvas.GriddingInterval * Zoom - fltStartShapeElesH;


                    RectangleF rect = new RectangleF();

                    switch (strState)
                    {
                        case "move":
                            this.Move(new PointF(fx1, fy1));
                            break;
                        case "West":
                            rect.X = fx1;
                            rect.Width = -fx1;
                            this.Resize(rect);
                            break;
                        case "East":
                            rect.Width = fw1;
                            this.Resize(rect);
                            break;
                        case "North":
                            rect.Y = fy1;
                            rect.Height = -fy1;
                            this.Resize(rect);
                            break;
                        case "South":
                            rect.Height = fh1;
                            this.Resize(rect);
                            break;
                        case "NorthEast":
                            rect.Width = fw1;
                            rect.Y = fy1;
                            rect.Height = -fy1;
                            this.Resize(rect);
                            break;
                        case "SouthWest":
                            rect.X = fx1;
                            rect.Width = -fx1;
                            rect.Height =fy1;
                            this.Resize(rect);
                            break;
                        case "SouthEast":
                            rect.Width = fw1;
                            rect.Height = fh1;
                            this.Resize(rect);
                            break;
                        case "NorthWest":
                            rect.X = fx1;
                            rect.Width =-fx1;
                            rect.Y =fy1;
                            rect.Height = -fy1;
                            this.Resize(rect);
                            break;

                        default:
                            break;
                    }

                    /**备份
                     * 
                    switch (strState)
                    {
                        case "move":
                            this.Move(new PointF(fx1, fy1));
                            break;
                        case "West":
                            rect.X = fltEndMouseX - fltStartMouseX;
                            rect.Width = fltStartMouseX - fltEndMouseX;
                            this.Resize(rect);
                            break;
                        case "East":
                            rect.Width = fltEndMouseX - fltStartMouseX;
                            this.Resize(rect);
                            break;
                        case "North":
                            rect.Y = fltEndMouseY - fltStartMouseY;
                            rect.Height = fltStartMouseY - fltEndMouseY;
                            this.Resize(rect);
                            break;
                        case "South":
                            rect.Height = fltEndMouseY - fltStartMouseY;
                            this.Resize(rect);
                            break;
                        case "NorthEast":
                            rect.Width = fltEndMouseX - fltStartMouseX;
                            rect.Y = fltEndMouseY - fltStartMouseY;
                            rect.Height = fltStartMouseY - fltEndMouseY;
                            this.Resize(rect);
                            break;
                        case "SouthWest":
                            rect.X = fltEndMouseX - fltStartMouseX;
                            rect.Width = fltStartMouseX - fltEndMouseX;
                            rect.Height = fltEndMouseY - fltStartMouseY;
                            this.Resize(rect);
                            break;
                        case "SouthEast":
                            rect.Width = fltEndMouseX - fltStartMouseX;
                            rect.Height = fltEndMouseY - fltStartMouseY;
                            this.Resize(rect);
                            break;
                        case "NorthWest":
                            rect.X = fltEndMouseX - fltStartMouseX;
                            rect.Width = fltStartMouseX - fltEndMouseX;
                            rect.Y = fltEndMouseY - fltStartMouseY;
                            rect.Height = fltStartMouseY - fltEndMouseY;
                            this.Resize(rect);
                            break;

                        default:
                            break;
                     * */




                }

            }
            //base.Redim(strState, startPointf, endPointf);
        }

        /**
        /// <summary>
        /// 移动，需要考虑是否对齐
        /// </summary>
        /// <param name="pointF"></param>
        public override void Move(PointF pointF)
        {
            if (UserControlCanvas.isAlignGridding)
            {
                _XAdd += (X + pointF.X) / UserControlCanvas.GriddingInterval * UserControlCanvas.GriddingInterval -X;
                _YAdd += (Y + pointF.Y) / UserControlCanvas.GriddingInterval * UserControlCanvas.GriddingInterval -Y;

                base.Move(new PointF(_XAdd, _YAdd));

            }
            else  
            {
                //如果没有选择对齐，那么就直接用这个参数调用父类的移动。
                base.Move(pointF);
            }   
        }

         * */


        /// <summary>
        /// 默认的构造函数  //这个的构造函数就是定义画笔的
        /// </summary>
        public ShapeSelRect():base()
        {
            PenColor = Color.Red;
            PenWidth = 1;
            PenDashStyle = DashStyle.Dot;

        }

        public override void Draw(Graphics g, ArrayList arrlistMatrix)
        {

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;
            //定义画笔
            Pen _myPen = new Pen(PenColor, 1f);//这个选择框画笔固定宽度是
            _myPen.DashStyle = PenDashStyle;

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(getGraphicsPath(arrlistMatrix).GetBounds());


            //如下这个就是画边界
            try
            {

                g.DrawPath(_myPen, path);
            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //throw;
            }

            //base.Draw(g, arrlistMatrix);
        }

        /**
        /// <summary>
        /// 设置选择框的四周，因为选择的图形不一定一个，所以需要这个设置，已有如果需要添加选择或者删除选择的时候，再一次用这个函数就可以了。
        /// 
        /// </summary>
        /// <param name="arrlistShape"></param>
        public void SetSelRectXYWH()
        {
            //首先初始化
            _X = 0;
            _Y = 0;
            _Width = 0;
            _Height = 0;

            if (Count() > 0)
            {
                ArrayList arrlistX = new ArrayList();
                ArrayList arrlistY = new ArrayList();

                foreach (ShapeEle item in arrlistSelShapes)
                {
                    //将这个所有点的坐标加到这里边去
                    foreach (PointF p1 in item.getRealPoint())
                    {
                        arrlistX.Add(p1.X);
                        arrlistY.Add(p1.Y);
                    }

                }

                arrlistX.Sort();
                arrlistY.Sort();

                _X = Convert.ToSingle(arrlistX[0].ToString());
                _Y = Convert.ToSingle(arrlistY[0].ToString());
                _Width = Convert.ToSingle(arrlistX[arrlistX.Count - 1].ToString()) - _X;
                _Height = Convert.ToSingle(arrlistY[arrlistY.Count - 1].ToString()) - _Y;
            }

        }

         * */


        public override void Draw(Graphics g)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //首先判断是否有形状，如果没有形状，那么就不用画了
            if (Count() > 0)
            {
                SetSelRectXYWH();
                Pen _myPen = new Pen(PenColor, _penWidth);
                _myPen.DashStyle = DashStyle.Dot;
                _myPen.Width = 1;
                _myPen.Color = Color.Red;
                g.DrawRectangle(_myPen, _X + _XAdd, _Y + _YAdd, _Width + _WidthAdd, _Height + _HeightAdd);

                //画节点，其实就是在这个选择框的四周画四个小矩形
                Pen penJieDian = new Pen(Color.Black);
                penJieDian.Color = Color.Red;
                float fltJieDianWidth = 2;

                g.DrawRectangle(penJieDian, _X + _XAdd - fltJieDianWidth / 2, _Y + _YAdd - fltJieDianWidth / 2, fltJieDianWidth, fltJieDianWidth);
                g.DrawRectangle(penJieDian, _X + _XAdd + _Width + _WidthAdd - fltJieDianWidth / 2, _Y + _YAdd - fltJieDianWidth / 2, fltJieDianWidth, fltJieDianWidth);
                g.DrawRectangle(penJieDian, _X + _XAdd - fltJieDianWidth / 2, _Y + _YAdd + _Height + _HeightAdd - fltJieDianWidth / 2, fltJieDianWidth, fltJieDianWidth);
                g.DrawRectangle(penJieDian, _X + _XAdd + _Width + _WidthAdd - fltJieDianWidth / 2, _Y + _YAdd + _Height + _HeightAdd - fltJieDianWidth / 2, fltJieDianWidth, fltJieDianWidth);
            }
            //throw new NotImplementedException();
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
                    bool isS = ((ShapeEle)arrlistShapeEle[i]).isContains(new PointF(pointF.X, pointF.Y));
                    if (isS)
                    {
                        ((ShapeEle)arrlistShapeEle[i]).isSelect = true;
                        return (ShapeEle)arrlistShapeEle[i];//这个点只判断最上边的图形就可以了。
                    }

                }

            }

            return null;//返回选择的图形，如果没有，里边就没有数据
        }

        /**
        /// <summary>
        /// 这个方法仅仅是返回已经选择的形状的个数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            if (arrlistSelShapes != null)
                return arrlistSelShapes.Count;
            return 0;
        }
         * */
        /**
        public override void ReInit()
        {
            if (arrlistSelShapes != null)
            {
                foreach (ShapeEle item in arrlistSelShapes)
                {
                    item.ReInit();

                }

            }
            //他也要移动
            base.ReInit();

        }

        public void addShapeEle(ShapeEle shapeEle)
        {
            if (arrlistSelShapes != null)
                arrlistSelShapes.Add(shapeEle);

            SetSelRectXYWH();//重新计算
        }

        public void addShapeEles(ArrayList arrlistSel)
        {
            foreach (ShapeEle item in arrlistSel)
            {
                arrlistSelShapes.Add(item);

            }
            SetSelRectXYWH();//重新计算
        }

        public void removeShapeEle(ShapeEle shapeEle)
        {
            if (arrlistSelShapes != null)
                arrlistSelShapes.Remove(shapeEle);

            SetSelRectXYWH();//重新计算
        }

        public void removeShapeEles(ArrayList arrlistSel)
        {
            foreach (ShapeEle item in arrlistSel)
            {
                arrlistSelShapes.Remove(arrlistSel);

            }
            SetSelRectXYWH();//重新计算
        }

         * */


        /// <summary>
        /// 取消所有选择
        /// </summary>

        public void cancelAllSelect()
        {
            if (arrlistShapeEle != null)
                arrlistShapeEle.Clear();
            //还有XYWH清零
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }

        /// <summary>
        /// 这个方法就是当用户鼠标点在这个更改框范围内的时候，判断是在什么位置。
        /// </summary>
        /// <param name="pointF"></param>
        /// <returns></returns>
        public string strOver(PointF pointF)
        {
            //默认是移动
            string strFangxiang = "move";//

            //首先判断是否是正东南西北

            if (Math.Abs((_X + _XAdd) - pointF.X) < fltJingDu)
                strFangxiang = "West";
            if (Math.Abs((_X + _XAdd + _Width + _WidthAdd) - pointF.X) < fltJingDu)
                strFangxiang = "East";
            if (Math.Abs((_Y + _YAdd) - pointF.Y) < fltJingDu)
                strFangxiang = "North";
            if (Math.Abs((_Y + _YAdd + _Height + _HeightAdd) - pointF.Y) < fltJingDu)
                strFangxiang = "South";

            //再判断是不是东南方向之类的
            if (((Math.Abs((_Y + _YAdd) - pointF.Y) < fltJingDu)) && ((Math.Abs((_X + _XAdd + _Width + _WidthAdd) - pointF.X) < fltJingDu)))
                strFangxiang = "NorthEast";
            if (((Math.Abs((_Y + _YAdd) - pointF.Y) < fltJingDu)) && ((Math.Abs((_X + _XAdd) - pointF.X) < fltJingDu)))
                strFangxiang = "NorthWest";
            if (((Math.Abs((_Y + _YAdd + _Height + _HeightAdd) - pointF.Y) < fltJingDu)) && (((Math.Abs(_X + _XAdd + _Width + _WidthAdd) - pointF.X) < fltJingDu)))
                strFangxiang = "SouthEast";
            if (((Math.Abs((_Y + _YAdd + _Height + _HeightAdd) - pointF.Y) < fltJingDu)) && ((Math.Abs((_X + _XAdd) - pointF.X) < fltJingDu)))
                strFangxiang = "SouthWest";

            return strFangxiang;
        }

        //public  enum enumFangXiang { North=1 , South , East , West , NorthEast , SouthEast , SouthWest , NorthWest ,Inside};




    }

    /// <summary>
    /// 这个类会在属性列表中显示类似下拉框的东西
    /// </summary>
    [Serializable]
    //[ProtoContract]
    public class VarNameDetails : StringConverter
    {
        //我现在只能用这种静态的方式来搞定这个了。
        public static string[] arrVarName = {};

        //覆盖 GetStandardValuesSupported 方法并返回 true ，表示此对象支持可以从列表中选取的一组标准值。   
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        /// <summary>
        /// 覆盖 GetStandardValues 方法并返回填充了标准值的 StandardValuesCollection 。
        /// 创建 StandardValuesCollection 的方法之一是在构造函数中提供一个值数组。
        /// 对于选项窗口应用程序，您可以使用填充了建议的默认文件名的 String 数组。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(arrVarName);
        }
        //如下这样就会变成组合框
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }


    }
    /// <summary>
    /// 二维码的语言编码类型 
    /// </summary>
    [Serializable]
    //[ProtoContract]
    public class LanguageEncoding : StringConverter
    {
        //我现在只能用这种静态的方式来搞定这个了。
        public static string[] arrVarName = {};

        public static  void Init()
        {
            ArrayList arrlist = new ArrayList();

            foreach (EncodingInfo item in Encoding.GetEncodings())
            {
                arrlist.Add(item.DisplayName);
            }

            arrVarName = (String[])arrlist.ToArray(typeof(string));
        }

        //覆盖 GetStandardValuesSupported 方法并返回 true ，表示此对象支持可以从列表中选取的一组标准值。   
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        /// <summary>
        /// 覆盖 GetStandardValues 方法并返回填充了标准值的 StandardValuesCollection 。
        /// 创建 StandardValuesCollection 的方法之一是在构造函数中提供一个值数组。
        /// 对于选项窗口应用程序，您可以使用填充了建议的默认文件名的 String 数组。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(arrVarName);
        }
        //如下这样就会变成组合框
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }


    }

    
    /// <summary>
    /// QrCode的容错率
    /// </summary>
    [Serializable]
    //[ProtoContract]
    public class QrCodeErrorLevel : StringConverter
    {
        //我现在只能用这种静态的方式来搞定这个了。
        public static string[] arrVarName = { "容错7%", "容错15%", "容错25%", "容错30%" };

        //覆盖 GetStandardValuesSupported 方法并返回 true ，表示此对象支持可以从列表中选取的一组标准值。   
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        /// <summary>
        /// 覆盖 GetStandardValues 方法并返回填充了标准值的 StandardValuesCollection 。
        /// 创建 StandardValuesCollection 的方法之一是在构造函数中提供一个值数组。
        /// 对于选项窗口应用程序，您可以使用填充了建议的默认文件名的 String 数组。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(arrVarName);
        }
        //如下这样就会变成组合框
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }


    }


    /// <summary>
    /// 显示条形码编码类型
    /// </summary>
    [Serializable]
    //[ProtoContract]
    public class BarcodeEncoding : StringConverter
    {
        //我现在只能用这种静态的方式来搞定这个了。
        public static string[] arrVarName = { "EAN13","EAN8","UPCA","UPCE","UPC_SUPPLEMENTAL_2DIGIT","UPC_SUPPLEMENTAL_5DIGIT",
                                                "CODE39","CODE39Extended","CODE128","CODE128A","CODE128B","CODE128C",
                                                "Codabar","ISBN","Interleaved2of5","Standard2of5","Industrial2of5","PostNet",
                                                "BOOKLAND","JAN13","MSI_Mod10","MSI_2Mod10","MSI_Mod11","MSI_Mod11_Mod10",
                                                "Modified_Plessey","CODE11","USD8","UCC12","UCC13","LOGMARS,","ITF14","CODE93",
                                                "TELEPEN","FIM","QR_CODE"};


        //覆盖 GetStandardValuesSupported 方法并返回 true ，表示此对象支持可以从列表中选取的一组标准值。   
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        /// <summary>
        /// 覆盖 GetStandardValues 方法并返回填充了标准值的 StandardValuesCollection 。
        /// 创建 StandardValuesCollection 的方法之一是在构造函数中提供一个值数组。
        /// 对于选项窗口应用程序，您可以使用填充了建议的默认文件名的 String 数组。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(arrVarName);
        }
        //如下这样就会变成组合框
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }


    }

    [Serializable]
    //[ProtoContract]
    public class ShapeLine : ShapeEle
    {
        protected float _X2, _Y2;//直线是两个点的，这个是另一个点

        protected float _X2Add, _Y2Add;

        [ CategoryAttribute("布局")]
        [XmlElement]
        public float X2
        {
            get
            {
                return _X2 / _Zoom;
            }
            set
            {
                _X2 = value * _Zoom;
            }
        }

        [CategoryAttribute("布局")]
        [XmlElement]
        public float Y2
        {
            get
            {
                return _Y2 / _Zoom;
            }
            set
            {
                _Y2 = value * _Zoom;
            }
        }


        //重新定义宽度和高度
        public override float Width
        {
            get
            {
                return Math.Abs(X-X2);
            }

        }
        public override float Height
        {
            get
            {
                return Math.Abs(Y- Y2);
            }

        }


        [XmlElement]
        public override float Zoom
        {
            get
            {
                return _Zoom;
            }
            set
            {
                //因为更改这个放大倍数，很多东西都要更改，首先保存相关信息
                float fltX, fltY, fltX2, fltY2;

                fltX = X;
                fltY = Y;
                fltX2 = X2;
                fltY2 = Y2;

                //再设置放大率
                base.Zoom = value;


                //再重新计算相关的XYWH。
                X = fltX;
                Y = fltY;
                X2 = fltX2;
                Y2 = fltY2;
                
            }

        }

        public override ShapeEle DeepClone()
        {
            ShapeLine  shapeEle = new ShapeLine();
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
            shapeEle.X2 = X2;
            shapeEle.Y2 = Y2;

            return shapeEle;
            //throw new NotImplementedException();
        }
        
        public override void ShapeInit(PointF p1, PointF p2)
        {
            _X = p1.X;
            _Y = p1.Y;
            _X2 = p2.X;
            _Y2 = p2.Y;
            //base.ShapeInit(p1, p2);
        }

        public override GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            //添加线段
            GraphicsPath path = new GraphicsPath();
            path.AddLine(new  PointF(_X + _XAdd, _Y + _YAdd), new PointF(_X2 + _X2Add, _Y2 + _Y2Add));

            return path;
            //return base.getGraphicsPath();
        }

        public override PointF getCentrePoint()
        {
            return new PointF((_X + _XAdd + _X2 + _X2Add) / 2, (_Y + _YAdd + _Y2 + _Y2Add) / 2);
            //return base.getCentrePoint();
        }

        public override void Draw(Graphics g)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;
            g.DrawLine(_myPen, new PointF(_X + _XAdd, _Y + _YAdd), new PointF(_X2 + _X2Add, _Y2 + _Y2Add));
            //throw new NotImplementedException();
        }

        public override PointF[] getRealPoint()
        {
            PointF[] arrPoint = new PointF[2];

            PointF p1 = new PointF(_X + _XAdd, _Y + _YAdd);
            PointF p2 = new PointF(_X2 + _X2Add, _Y2 + _Y2Add);


            double dblRoute = _route + _routeAdd;

            //求中心点
            PointF pointCenter = getCentrePoint();

            //注意这里是顺时针
            arrPoint[0] = PointRotate(pointCenter, p1, dblRoute);
            arrPoint[1] = PointRotate(pointCenter, p2, dblRoute);

            return arrPoint;
            //return base.getRealPoint();
        }

        public override void Redim(string strState, PointF startPointf, PointF endPointf)
        {
            //也是因为这个是一个线段，所以在选择框中，他肯定占用的是选择框四个节点的两个

            PointF[] arrPAdd = new PointF[2];

            switch (strState)
            {
                case "move":
                    this.Move(new PointF(endPointf.X - startPointf.X, endPointf.Y - startPointf.Y));
                    break;
                case "West":
                    if (_X > _X2)
                    {
                        _X2Add = endPointf.X - startPointf.X;
                    }
                    else
                    {
                        _XAdd = endPointf.X - startPointf.X;
                    }
                    break;
                case "East":
                    if (_X2 > _X)
                    {
                        _X2Add = endPointf.X - startPointf.X;
                    }
                    else
                    {
                        _XAdd = endPointf.X - startPointf.X;
                    }

                    break;
                case "North":
                    if (_Y > _Y2)
                    {
                        _Y2Add = endPointf.Y - startPointf.Y;
                    }
                    else
                    {
                        _YAdd = endPointf.Y - startPointf.Y;
                    }
                    break;
                case "South":
                    if (_Y2 > _Y)
                    {
                        _Y2Add = endPointf.Y - startPointf.Y;
                    }
                    else
                    {
                        _YAdd = endPointf.Y - startPointf.Y;
                    }
                    break;
                case "NorthEast":
                    if (_Y > _Y2)
                    {
                        _Y2Add = endPointf.Y - startPointf.Y;
                    }
                    else
                    {
                        _YAdd = endPointf.Y - startPointf.Y;
                    }
                    if (_X2 > _X)
                    {
                        _X2Add = endPointf.X - startPointf.X;
                    }
                    else
                    {
                        _XAdd = endPointf.X - startPointf.X;
                    }

                    break;
                case "SouthWest":
                    if (_Y2 > _Y)
                    {
                        _Y2Add = endPointf.Y - startPointf.Y;
                    }
                    else
                    {
                        _YAdd = endPointf.Y - startPointf.Y;
                    }
                    if (_X > _X2)
                    {
                        _X2Add = endPointf.X - startPointf.X;
                    }
                    else
                    {
                        _XAdd = endPointf.X - startPointf.X;
                    }

                    break;
                case "SouthEast":
                    if (_Y2 > _Y)
                    {
                        _Y2Add = endPointf.Y - startPointf.Y;
                    }
                    else
                    {
                        _YAdd = endPointf.Y - startPointf.Y;
                    }
                    if (_X2 > _X)
                    {
                        _X2Add = endPointf.X - startPointf.X;
                    }
                    else
                    {
                        _XAdd = endPointf.X - startPointf.X;
                    }

                    break;
                case "NorthWest":
                    if (_Y > _Y2)
                    {
                        _Y2Add = endPointf.Y - startPointf.Y;
                    }
                    else
                    {
                        _YAdd = endPointf.Y - startPointf.Y;
                    }
                    if (_X > _X2)
                    {
                        _X2Add = endPointf.X - startPointf.X;
                    }
                    else
                    {
                        _XAdd = endPointf.X - startPointf.X;
                    }

                    break;

                default:
                    break;
            }




            //base.Redim(strState, startPointf, endPointf);
        }

        /// <summary>
        /// 线段的移动就是两个点同时移动 
        /// </summary>
        /// <param name="pointF"></param>
        public override void Move(PointF pointF)
        {
            _XAdd = pointF.X;
            _X2Add = pointF.X;
            _YAdd = pointF.Y;
            _Y2Add = pointF.Y;
            //base.Move(pointF);
        }

        public override void ReInit()
        {
            _X += _XAdd;
            _Y += _YAdd;
            _X2 += _X2Add;
            _Y2 += _Y2Add;

            _XAdd = 0;
            _YAdd = 0;
            _X2Add = 0;
            _Y2Add = 0;
            //base.ReInit();
        }

        /**

        public override bool isContains(PointF MousePoint)
        {
            //线段判断跟矩形的判断不一样。
            //两个点有变化
            PointF[] pointfReal = getRealPoint();

            //直线的选择是判断点到线段的距离

            float fltDistance = getPointLineDistance(pointfReal[0], pointfReal[1], MousePoint);

            //如果这个距离小于精度
            if (fltDistance <= fltJingDu)
                return true;

            return false;

            //return base.isContains(MousePoint);
        }
         * */


    }//end class


    public class ShapeStateText : shapeSingleText
    {
        [DescriptionAttribute("文字"), DisplayName("文字"), CategoryAttribute("文字")]
        [XmlElement]
        public string Text
        {
            get
            {
                return DefaultText;
            }
            set
            {
                DefaultText = value;
                PreFix = "";
                Suffix = "";
                UpdateWidthHeight();

            }
        }

    }
    public class shapeVarText : shapeSingleText
    {

    }


    /**如下注释掉的是原先的静态变量和动态变量，我用单行文本替代了
    //
    /// <summary>
    /// 这个是静态文字类，这个已经不用了
    /// </summary>
    [Serializable]
    //[ProtoContract]
    public class ShapeStateText : ShapeEle
    {
        [XmlIgnore]
        public  Font _font=new Font ("Arial",6);
        protected Font _RealFont = new Font("Arial", 6);//这个是用在放大中，真实的字体大小。

        protected float _fltStretchWidth = 0f;//拉伸宽度
        protected float _fltStretchHeight = 0f;//拉伸高度
        protected float _fltStretchWidthAdd=0f;//拉伸宽度增值
        protected float _fltStretchHeightAdd=0f;//拉伸高度增值

        public ShapeStateText():base()
        {
            //设置初始字符为静态文字
            Text = "静态文字";

        }

        public override ShapeEle DeepClone()
        {
            ShapeStateText shapeEle = new ShapeStateText();
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
            shapeEle.Text = Text;
            shapeEle.TextFont = TextFont;

            return shapeEle;
            //throw new NotImplementedException();
        }

        //这个变量文字跟静态文字不同点在于，静态文字只有一个文字域，而变量文字有三个。
        protected void UpdateWidthHeight()
        {

            //这里自动取得字符串的宽度和高度
            Bitmap bitmap = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bitmap);
            g.PageUnit = GraphicsUnit.Millimeter;//因为我所有的都换成毫米单位了。，这里求出来的宽度和高度是以毫米为单位的。
            SizeF textSizef = g.MeasureString(Text, _font);//因为右边用到宽度高度的时候会乘以倍数，所以这里用这个字体判定

            Width = textSizef.Width;
            Height = textSizef.Height;

            cancelStretch();
        }

        protected void cancelStretch()
        {
            _fltStretchWidth = 1f;//拉伸宽度
            _fltStretchHeight = 1f;//拉伸高度
            _fltStretchWidthAdd = 0f;//拉伸宽度增值
            _fltStretchHeightAdd = 0f;//拉伸高度增值

        }

        public override void ShapeInit(PointF p1, PointF p2)
        {
            //base.ShapeInit(p1, p2);
            _X = p1.X;
            _Y = p1.Y;

            UpdateWidthHeight();
            
        }

        protected string _strText;

        [DescriptionAttribute("文字"),DisplayName("文字"), CategoryAttribute("文字")]
        [XmlElement]
        public string Text
        {
            get
            {
                return _strText;
            }
            set
            {
                _strText = value;
                UpdateWidthHeight();
                
            }
        }

        [DescriptionAttribute("字体"),DisplayName("字体"), CategoryAttribute("文字")]
        [XmlIgnore ]
        public Font TextFont
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
                _RealFont = ChangeFontSize(_font, _font.Size * _Zoom);//设置真实的字体大小
                UpdateWidthHeight();

            }
        }

        //如下的这个只是在序列化时用到。
        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public myFont SerializerFont
        {
            get
            {
                myFont font2 = new myFont();

                font2.Name = TextFont.Name;
                font2.Size = TextFont.Size;
                font2.Style = TextFont.Style;

                return font2;

            }

            set
            {
                TextFont = new Font(value.Name, value.Size, value.Style);
            }
        }



        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public override float Zoom
        {
            get
            {
                return base.Zoom;
            }
            set
            {
                
                base.Zoom = value;

                TextFont = TextFont;//设置字体
            }
        }

        public override void Draw(Graphics g, float fltKongX, float fltKongY)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //如下的这个是偏移些位置
            g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);

            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;

            if ((_route + _routeAdd) != 0)
            {
                //旋转图形
                PointF pZhongXin = new PointF(fltx + (fltw) / 2, flty + (flth) / 2);
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //如下是拉伸图形。
            g.TranslateTransform(fltx, flty, MatrixOrder.Prepend);
            g.ScaleTransform(_fltStretchWidthAdd + _fltStretchWidth, _fltStretchHeightAdd + _fltStretchHeight); // 
            g.TranslateTransform(-fltx, -flty);

            g.DrawString(Text, _RealFont, new SolidBrush(_FillColor), new PointF(_X + _XAdd, _Y + _YAdd));
            //throw new NotImplementedException();
            //如下的这个是偏移些位置
            g.TranslateTransform(-fltKongX, -fltKongY);

            g.ResetTransform();

            //base.Draw(g, fltKongX, fltKongY);
        }

        public override void Draw(Graphics g)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;
 
            if ((_route + _routeAdd) != 0)
            {
                //旋转图形
                PointF pZhongXin = new PointF(fltx + (fltw) / 2, flty + (flth) / 2);              
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //如下是拉伸图形。
            g.TranslateTransform(fltx, flty, MatrixOrder.Prepend);
            g.ScaleTransform(_fltStretchWidthAdd+_fltStretchWidth, _fltStretchHeightAdd+_fltStretchHeight); // 
            g.TranslateTransform(-fltx, -flty);


            g.DrawString(Text, _RealFont, new SolidBrush(_FillColor), new PointF(_X + _XAdd, _Y + _YAdd));
            //throw new NotImplementedException();

            g.ResetTransform();
        }
        public override void Redim(string strState, PointF startPointf, PointF endPointf)
        {

            float startX = startPointf.X;
            float startY = startPointf.Y;
            float endX = endPointf.X;
            float endY = endPointf.Y;

            switch (strState)
            {
                case "move":
                    this.Move(new PointF(endX - startX, endY - startY));
                    break;
                case "West":
                    //_XAdd = fltEndMouseX - fltStartMouseX;//坐标点向左移动，
                    //计算拉伸了多少
                    //_fltStretchWidthAdd = 1f + _XAdd / _Width;          
                    break;
                case "East":
                    //_fltStretchWidthAdd = 1f + (fltEndMouseX - fltStartMouseX) / _Width;//增量是从0开始的，而不是从一开始的。
                    _fltStretchWidthAdd =  (endX - startX) / _Width;
                    break;
                case "North":
                    //_YAdd = fltEndMouseY - fltStartMouseY;
                    //_fltStretchHeightAdd = 1f + _YAdd / _Height;
                    break;
                case "South":
                    //_fltStretchHeightAdd = 1f + (fltEndMouseY-fltStartMouseY) / _Height;
                    _fltStretchHeightAdd = (endY - startY) / _Height;
                    break;
                case "NorthEast":
                    //_YAdd = fltEndMouseY - fltStartMouseY;
                    //_fltStretchHeightAdd = 1f + _YAdd / _Height;
                    //_fltStretchWidthAdd = 1f + (fltEndMouseX - fltStartMouseX) / _Width;
                    break;
                case "SouthWest":
                    //_fltStretchHeightAdd = 1f + (fltEndMouseY - fltStartMouseY) / _Height;
                    //_XAdd = fltEndMouseX - fltStartMouseX;//坐标点向左移动， 
                    break;
                case "SouthEast":
                    //_fltStretchHeightAdd = 1f + (fltEndMouseY - fltStartMouseY) / _Height;
                    //_fltStretchWidthAdd = 1f + (fltEndMouseX - fltStartMouseX) / _Width;
                    _fltStretchHeightAdd = (endY - startY) / _Height;
                    _fltStretchWidthAdd = (endX - startX) / _Width;
                    break;
                case "NorthWest":
                     //_YAdd = fltEndMouseY - fltStartMouseY;
                    //_fltStretchHeightAdd = 1f + _YAdd / _Height;
                     //_XAdd = fltEndMouseX - fltStartMouseX;//坐标点向左移动，
                    //计算拉伸了多少
                    //_fltStretchWidthAdd = 1f + _XAdd / _Width;     

                    break;

                default:
                    break;
            }

            //base.Redim(strState, startPointf, endPointf);
        }

        public override void ReInit()
        {
            base.ReInit();

            _fltStretchHeight += _fltStretchHeightAdd;
            _fltStretchWidth += _fltStretchWidthAdd;

            _fltStretchWidthAdd = 0;
            _fltStretchHeightAdd = 0;
        }


         public override PointF[] getRealPoint()
         {
             PointF[] arrPoint = new PointF[4];

             float fltx = _X + _XAdd;
             float flty = _Y + _YAdd;
             float fltw = (_Width + _WidthAdd) * (_fltStretchWidth + _fltStretchWidthAdd);
             float flth = (_Height + _HeightAdd) * (_fltStretchHeight + _fltStretchHeightAdd);
             arrPoint[0] = new PointF(fltx, flty);//这个点是不变的，我打算以这个点作为基点。
             arrPoint[1] = new PointF(fltx+ fltw, flty);//
             arrPoint[2] = new PointF(fltx + fltw, flty + flth);
             arrPoint[3] = new PointF(fltx, flty + flth);

             return arrPoint;
             //return base.getRealPoint();
         }

    }
    /// <summary>
    /// 这个已经不用了
    /// </summary>
    [Serializable]
    //[ProtoContract]
    public class shapeVarText : ShapeEle
    {
        protected string _strPrefix;//前缀
        protected string _strSuffix;//后缀
        protected string _strAllText;//前缀加上后缀加上变量值，就是一个总的
        [XmlIgnore]
        public Font _font = new Font("Arial", 6);
        protected Font _RealFone = new Font("Arial", 6);//这个是用在放大中，真实的字体大小。


        protected float _fltStretchWidth = 0f;//拉伸宽度
        protected float _fltStretchHeight = 0f;//拉伸高度
        protected float _fltStretchWidthAdd = 0f;//拉伸宽度增值
        protected float _fltStretchHeightAdd = 0f;//拉伸高度增值

        public shapeVarText():base()
        {
            PreFix = "前缀";
            Suffix = "后缀";
        }
        public override ShapeEle DeepClone()
        {

            shapeVarText shapeEle = new shapeVarText();
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
            shapeEle.VarName = VarName;
            shapeEle.PreFix = PreFix;
            shapeEle.Suffix = Suffix;
            shapeEle.TextFont = TextFont;

            return shapeEle;
            
            //throw new NotImplementedException();
        }

        public override void ShapeInit(PointF p1, PointF p2)
        {
            _X = p1.X;
            _Y = p1.Y;

            UpdateWidthHeight();
            //base.ShapeInit(p1, p2);
        }

        //这个变量文字跟静态文字不同点在于，静态文字只有一个文字域，而变量文字有三个。

        //
        [DescriptionAttribute("前缀"),DisplayName("前缀"), CategoryAttribute("文字")]
        public string PreFix
        {
            get
            {
                return _strPrefix;
            }
            set
            {
                _strPrefix = value;
                UpdateWidthHeight();
            }
        }

        [DescriptionAttribute("后缀"),DisplayName("后缀"), CategoryAttribute("文字")]
        public string Suffix
        {
            get
            {
                return _strSuffix;
            }
            set
            {
                _strSuffix = value;
                UpdateWidthHeight();
            }

        }
        [TypeConverter(typeof(VarNameDetails)), DescriptionAttribute("变量名"),DisplayName("标题名"), CategoryAttribute("文字")]
        [XmlElement]
        public string VarName
        {
            get
            {
                return _strVarName;
            }
            set
            {
                _strVarName = value;

            }


        }

        public override void Draw(Graphics g, float fltKongX, float fltKongY)
        {

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;
            //如下的这个是偏移些位置
            g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);

            //如下是直接拷贝静态文本的，只是修改了显示的

            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;



            if ((_route + _routeAdd) != 0)
            {
                //旋转图形
                PointF pZhongXin = new PointF(fltx + (fltw) / 2, flty + (flth) / 2);
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //如下是拉伸图形。
            g.TranslateTransform(fltx, flty, MatrixOrder.Prepend);
            g.ScaleTransform(_fltStretchWidthAdd + _fltStretchWidth, _fltStretchHeightAdd + _fltStretchHeight); // 
            g.TranslateTransform(-fltx, -flty);

            //跟静态文本唯一的区别是，如下显示的是_strAllText 
            g.DrawString(_strAllText, _RealFone, new SolidBrush(_FillColor), new PointF(_X + _XAdd, _Y + _YAdd));
            //throw new NotImplementedException();

            //如下的这个是偏移些位置
            g.TranslateTransform(-fltKongX, -fltKongY);

            g.ResetTransform();
            //throw new NotImplementedException();

            //base.Draw(g, fltKongX, fltKongY);
        }



        [DescriptionAttribute("字体"),DisplayName("字体"), CategoryAttribute("文字")]
        [XmlIgnore]
        public Font TextFont
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
                _RealFone = ChangeFontSize(_font, _font.Size * _Zoom);//设置真实的字体大小
                UpdateWidthHeight();

            }
        }

        //如下的这个只是在序列化时用到。
        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public myFont SerializerFont
        {
            get
            {
                myFont font2 = new myFont();

                font2.Name = TextFont.Name;
                font2.Size = TextFont.Size;
                font2.Style = TextFont.Style;

                return font2;

            }

            set
            {
                TextFont = new Font(value.Name, value.Size, value.Style);
            }
        }

        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public override float Zoom
        {
            get
            {
                return base.Zoom;
            }
            set
            {

                base.Zoom = value;

                TextFont = TextFont;//设置字体
            }
        }




        public override PointF[] getRealPoint()
        {
            PointF[] arrPoint = new PointF[4];

            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = (_Width + _WidthAdd) * (_fltStretchWidth + _fltStretchWidthAdd);
            float flth = (_Height + _HeightAdd) * (_fltStretchHeight + _fltStretchHeightAdd);
            arrPoint[0] = new PointF(fltx, flty);//这个点是不变的，我打算以这个点作为基点。
            arrPoint[1] = new PointF(fltx + fltw, flty);//
            arrPoint[2] = new PointF(fltx + fltw, flty + flth);
            arrPoint[3] = new PointF(fltx, flty + flth);

            return arrPoint;
            //return base.getRealPoint();
            //return base.getRealPoint();
        }

        public override  void Redim(string strState, PointF startPointf, PointF endPointf)
        {

            float startX = startPointf.X;
            float startY = startPointf.Y;
            float endX = endPointf.X;
            float endY = endPointf.Y;

            switch (strState)
            {
                case "move":
                    this.Move(new PointF(endX - startX, endY - startY));
                    break;
                case "West":
                    //_XAdd = fltEndMouseX - fltStartMouseX;//坐标点向左移动，
                    //计算拉伸了多少
                    //_fltStretchWidthAdd = 1f + _XAdd / _Width;          
                    break;
                case "East":
                    //_fltStretchWidthAdd = 1f + (fltEndMouseX - fltStartMouseX) / _Width;
                    _fltStretchWidthAdd =(endX - startX) / _Width;
                    break;
                case "North":
                    //_YAdd = fltEndMouseY - fltStartMouseY;
                    //_fltStretchHeightAdd = 1f + _YAdd / _Height;
                    break;
                case "South":
                    //_fltStretchHeightAdd = 1f + (fltEndMouseY - fltStartMouseY) / _Height;
                    _fltStretchHeightAdd =(endY - startY) / _Height;
                    break;
                case "NorthEast":
                    //_YAdd = fltEndMouseY - fltStartMouseY;
                    //_fltStretchHeightAdd = 1f + _YAdd / _Height;
                    //_fltStretchWidthAdd = 1f + (fltEndMouseX - fltStartMouseX) / _Width;
                    break;
                case "SouthWest":
                    //_fltStretchHeightAdd = 1f + (fltEndMouseY - fltStartMouseY) / _Height;
                    //_XAdd = fltEndMouseX - fltStartMouseX;//坐标点向左移动， 
                    break;
                case "SouthEast":
                    //_fltStretchHeightAdd = 1f + (fltEndMouseY - fltStartMouseY) / _Height;
                   // _fltStretchWidthAdd = 1f + (fltEndMouseX - fltStartMouseX) / _Width;
                    _fltStretchHeightAdd = (endY - startY) / _Height;
                    _fltStretchWidthAdd =  (endX - startX) / _Width;
                    break;
                case "NorthWest":
                    //_YAdd = fltEndMouseY - fltStartMouseY;
                    //_fltStretchHeightAdd = 1f + _YAdd / _Height;
                    //_XAdd = fltEndMouseX - fltStartMouseX;//坐标点向左移动，
                    //计算拉伸了多少
                    //_fltStretchWidthAdd = 1f + _XAdd / _Width;     

                    break;

                default:
                    break;
            }

            //base.Redim(strState, startPointf, endPointf);
            //base.Redim(strState, startPointf, endPointf);
        }

        //这个变量文字跟静态文字不同点在于，静态文字只有一个文字域，而变量文字有三个。
        protected void UpdateWidthHeight()
        {
            UpdateStrAllText();//首先更新

            //这里自动取得字符串的宽度和高度
            Bitmap bitmap = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bitmap);
            g.PageUnit = GraphicsUnit.Millimeter;//因为我所有的都换成毫米单位了。，这里求出来的宽度和高度是以毫米为单位的。
            SizeF textSizef = g.MeasureString(_strAllText, _font);//因为右边用到宽度高度的时候会乘以倍数，所以这里用这个字体判定

            Width = textSizef.Width;
            Height = textSizef.Height;

            cancelStretch();
        }

        protected void cancelStretch()
        {
            _fltStretchWidth = 1f;//拉伸宽度
            _fltStretchHeight = 1f;//拉伸高度
            _fltStretchWidthAdd = 0f;//拉伸宽度增值
            _fltStretchHeightAdd = 0f;//拉伸高度增值

        }
        /// <summary>
        /// 这个方法没别的事情，只是把字符串连接起来而已，并返回连接后的字符串。
        /// </summary>
        /// <returns></returns>
        protected string UpdateStrAllText()
        {
            _strAllText = _strPrefix + _strVarValue + _strSuffix;
            return _strAllText;

        }


        public override void Draw(Graphics g)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //如下是直接拷贝静态文本的，只是修改了显示的
           
            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;



            if ((_route + _routeAdd) != 0)
            {
                //旋转图形
                PointF pZhongXin = new PointF(fltx + (fltw) / 2, flty + (flth) / 2);
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //如下是拉伸图形。
            g.TranslateTransform(fltx, flty, MatrixOrder.Prepend);
            g.ScaleTransform(_fltStretchWidthAdd + _fltStretchWidth, _fltStretchHeightAdd + _fltStretchHeight); // 
            g.TranslateTransform(-fltx, -flty);

            //跟静态文本唯一的区别是，如下显示的是_strAllText 
            g.DrawString(_strAllText, _RealFone, new SolidBrush(_FillColor), new PointF(_X + _XAdd, _Y + _YAdd));
            //throw new NotImplementedException();

            g.ResetTransform();
            //throw new NotImplementedException();
        }

        public override bool updateVarValue(ArrayList arrlistKeyValue)
        {
            bool isChange = base.updateVarValue(arrlistKeyValue);

            if (isChange)
                UpdateWidthHeight();

            return isChange;

            //base.updateVarValue(arrlistKeyValue);
        }

    }
     * 
     * */


    
    [Serializable]
    public class shapeSingleText : ShapeEle
    {

        protected string _strPrefix;//前缀
        protected string _strSuffix;//后缀
        protected string _strDefaultText;//文字
        protected string _strAllText;//前缀加上后缀加上变量值，就是一个总的
        protected  Font _font = new Font("Arial", 6);
        protected Font _RealFont = new Font("Arial", 6);//这个是用在放大中，真实的字体大小。


        protected float _fltStretchWidth = 1f;//拉伸宽度
        protected float _fltStretchHeight = 1f;//拉伸高度
        protected float _fltStretchWidthAdd = 0f;//拉伸宽度增值
        protected float _fltStretchHeightAdd = 0f;//拉伸高度增值

        private bool isAlreadyInit;//是否已经初始化

        private bool _OppositeGroundColor;
        [DescriptionAttribute("是否反底"), DisplayName("反底"), CategoryAttribute("文字")]
        [XmlElement]
        public bool OppositeGroundColor
        {
            get { return _OppositeGroundColor; }
            set { _OppositeGroundColor = value; }
        }

        private ClsTransforms _clsTransforms;
        [EditorAttribute(typeof(ClsPropertyGridTransfroms), typeof(System.Drawing.Design.UITypeEditor)) , DescriptionAttribute("转换"), DisplayName("转换"), CategoryAttribute("文字")]
        [XmlElement]
        public ClsTransforms Transforms
        {
            get { return _clsTransforms; }
            set { _clsTransforms = value; }
        }

        /**
        /// <summary>
        /// 因为这个字是根据点来判断的，所以需要，这个判断已经在父类中了
        /// </summary>
        /// <param name="MousePoint"></param>
        /// <returns></returns>
        public override bool isContains(PointF MousePoint)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(getGraphicsPathNoOffsetRoute().GetBounds());

            //做一个变换矩阵加上偏移和旋转
            System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
            //g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
            m.RotateAt((float)(_route + _routeAdd), getCentrePoint());
            path.Transform(m);//应用变换矩阵
            return path.IsVisible(MousePoint);

        }
         * */



        //保存拉伸的属性
        /// <summary>
        /// 拉伸的宽度
        /// </summary>
        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public float StretchWidth
        {
            get
            {
                return _fltStretchWidth;
            }
            set
            {
                _fltStretchWidth = value;
            }

        }
        /// <summary>
        /// 拉伸的高度
        /// </summary>
        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public float StretchHeight
        {
            get
            {
                return _fltStretchHeight;
            }
            set
            {
                _fltStretchHeight = value;
            }
        }

        //protected  StringFormat _TextStringFormat=new StringFormat();//字符串格式，我用这个主要用两点，水平对齐方式和垂直对齐方式
        public override ShapeEle DeepClone()
        {
            shapeSingleText shapeEle = new shapeSingleText();
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
            shapeEle.VarName = VarName;
            shapeEle.PreFix = PreFix;
            shapeEle.DefaultText = DefaultText;
            shapeEle.Suffix = Suffix;
            shapeEle.AlignMent = AlignMent;
            shapeEle.LineAlignMent = LineAlignMent;
            shapeEle.TextFont = TextFont;            

            return shapeEle;
            //throw new NotImplementedException();
        }
        public shapeSingleText()
            : base()
        {
            _strPrefix = "前缀";
            _strDefaultText = "默认文本";
            _strSuffix = "后缀";
            IsStretch = true;//可以拉伸
            PenWidth = 0f;//字体宽度为0
            isFill = true;//默认填充
            UpdateWidthHeight();


        }
        
        /**
        public override GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            GraphicsPath path = new GraphicsPath();
            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = (_Width + _WidthAdd) * (_fltStretchWidth + _fltStretchWidthAdd);
            float flth = (_Height + _HeightAdd) * (_fltStretchHeight + _fltStretchHeightAdd);
            path.AddRectangle(new RectangleF(fltx, flty, fltw, flth));
            return path;
            //return base.getGraphicsPathNoOffsetRoute();
        }
        **/
        
       

        

        /**
        public override GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            GraphicsPath path = new GraphicsPath();

            UpdateWidthHeight();

            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = (_Width + _WidthAdd) ;
            float flth = (_Height + _HeightAdd) ;

            if (IsStretch)
            {
                fltw = (_Width + _WidthAdd) * (_fltStretchWidth + _fltStretchWidthAdd);
                flth = (_Height + _HeightAdd) * (_fltStretchHeight + _fltStretchHeightAdd);
            }

            // 字符串格式
            StringFormat sf = new StringFormat();
            sf.Alignment = AlignMent;
            sf.LineAlignment = LineAlignMent;

            float fltZ = 10;//这个是先放大字体，然后再缩小矩阵来提高字体清晰度

            RectangleF rect = getRect();
            rect.Width = rect.Width * fltZ;
            rect.Height = rect.Height * fltZ;
            path.AddString(_strAllText, _RealFont.FontFamily, (int)_RealFont.Style, _RealFont.Size * fltZ, rect, sf);

            System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
            m.Translate(fltx, flty);
            m.Scale(1 / fltZ, 1 / fltZ);
            m.Translate(-fltx, -flty);
            path.Transform(m);//应用变换矩阵

            if (IsStretch)
            {
                System.Drawing.Drawing2D.Matrix m2 = new System.Drawing.Drawing2D.Matrix();
                //g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
                m2.Translate(fltx, flty);
                m2.Scale(_fltStretchWidth + _fltStretchWidthAdd, _fltStretchHeight + _fltStretchHeightAdd);
                m2.Translate(-fltx, -flty);

                path.Transform(m2);//应用变换矩阵
            }
            

            //判断是否有边框
            if (OppositeGroundColor)
            {
                //path.AddRectangle(getRect());
                path.AddRectangle(path.GetBounds());//调用自身的边框
            }

            //闭合
            //path.CloseAllFigures();

            return path;
            //return base.getGraphicsPathNoOffsetRoute();
        }
         * */

        /// <summary>
        /// 这个只是取得在一个画布上画这些字符串的图像
        /// </summary>
        /// <returns></returns>
        protected Bitmap getFontImage()
        {

            Bitmap bitmap = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bitmap);

            //这里绘图。
            // 字符串格式
            StringFormat sf = new StringFormat();
            sf.Alignment = AlignMent;
            sf.LineAlignment = LineAlignMent;
            try
            {
                //只是在原点绘制
                g.DrawString(_strAllText, _RealFont, new SolidBrush(_FillColor), new PointF(0,0), sf);
            }
            catch (System.Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
            }

            return bitmap;

            
        }

        public override void Draw(Graphics g, ArrayList arrlistMatrix)
        {
            RectangleF rect = getGraphicsPath(arrlistMatrix).GetBounds();
            float fltx = rect.X;
            float flty = rect.Y;
            float fltw = rect.Width;
            float flth = rect.Height;

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;
            //如下的这个是偏移些位置


            //如下是先从绘制矩形中的拷贝的，然后再修改
            if (Route != 0)
            {
                PointF pZhongXin = getCentrePoint();
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            //RectangleF rect = getRect();

            g.DrawImage(getFontImage(), new PointF(fltx, flty));

            g.ResetTransform();
            //base.Draw(g, arrlistMatrix);

        }



        public override void ReInit()
        {
            
            base.ReInit();
            if (IsStretch)
            {
                _fltStretchWidth += _fltStretchWidthAdd;
                _fltStretchHeight += _fltStretchHeightAdd;
                _fltStretchWidthAdd = 0;
                _fltStretchHeightAdd = 0;    
                
            }

        }
        public override void ShapeInit(PointF p1, PointF p2)
        {
            _X = p1.X;
            _Y = p1.Y;

            UpdateWidthHeight();
            //base.ShapeInit(p1, p2);
        }

        //这个变量文字跟静态文字不同点在于，静态文字只有一个文字域，而变量文字有三个。

        /// <summary>
        /// 前缀
        /// </summary>
        [DescriptionAttribute("前缀"), DisplayName("前缀"), CategoryAttribute("文字")]
        public string PreFix
        {
            get
            {
                return _strPrefix;
            }
            set
            {
                _strPrefix = value;
                UpdateWidthHeight();
            }
        }
        /// <summary>
        /// 默认文本
        /// </summary>
        [DescriptionAttribute("默认文本"), DisplayName("默认文本"), CategoryAttribute("文字")]
        public string DefaultText
        {
            get
            {
                return _strDefaultText;
            }
            set
            {
                _strDefaultText = value;
                UpdateWidthHeight();
            }
        }
        /// <summary>
        /// 后缀
        /// </summary>
        [DescriptionAttribute("后缀"), DisplayName("后缀"), CategoryAttribute("文字")]
        public string Suffix
        {
            get
            {
                return _strSuffix;
            }
            set
            {
                _strSuffix = value;
                UpdateWidthHeight();
            }

        }
        [TypeConverter(typeof(VarNameDetails)), DescriptionAttribute("就是EXCEL表格的第一行当变量名"), DisplayName("变量名"), CategoryAttribute("变量设置")]
        [XmlElement]
        public string VarName
        {
            get
            {
                return _strVarName;
            }
            set
            {
                _strVarName = value;

            }

        }

        private StringAlignment _stringAlignment;

        [DescriptionAttribute("水平对齐"), DisplayName("水平对齐"), CategoryAttribute("对齐方式")]
        [XmlElement]
        public StringAlignment AlignMent
        {
            get
            {
                return _stringAlignment;
            }
            set
            {
                _stringAlignment = value;
                
            }

        }

        private StringAlignment _LineAlignMent;

        [DescriptionAttribute("垂直对齐"), DisplayName("垂直对齐"), CategoryAttribute("对齐方式")]
        [XmlElement]
        public StringAlignment LineAlignMent
        {
            get
            {
                return _LineAlignMent;
            }
            set
            {
                _LineAlignMent = value;

            }

        }

        /**变量不需要这个变量值，只要变量名就可以了。
        public  string VarValue
        {
            get
            {
                return _strVarValue;
            }
            set
            {
                _strVarValue = value;
                UpdateWidthHeight();
            }
        }
         * */

        [DescriptionAttribute("字体,重新设置字体可以取消拉伸效果，请注意字体大小单位是毫米！"), DisplayName("字体"), CategoryAttribute("文字")]
        [XmlIgnore]
        public Font TextFont
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
                _RealFont = ChangeFontSize(_font, _font.Size * _Zoom);//设置真实的字体大小
                UpdateWidthHeight();

                //只有初始化时肯定运行到这里，所以需要这个
                if (isAlreadyInit)
                {
                    cancelStretch();
                    isAlreadyInit = true;
                }

            }
        }

        //如下的这个只是在序列化时用到。
        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public myFont SerializerFont
        {
            get
            {
                myFont font2 = new myFont();

                font2.Name = TextFont.Name;
                font2.Size = TextFont.Size;
                font2.Style = TextFont.Style;

                return font2;

            }

            set
            {
                TextFont = new Font(value.Name, value.Size, value.Style);
            }
        }

        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public override float Zoom
        {
            get
            {
                return base.Zoom;
            }
            set
            {

                base.Zoom = value;

                TextFont = TextFont;//设置字体
               // _RealFont = ChangeFontSize(_font, _font.Size * _Zoom);//设置真实的字体大小
                //UpdateWidthHeight();
            }
        }

        private bool _isStretch;
        [XmlElement]
        [DescriptionAttribute("是否可以拉伸字体"), DisplayName("拉伸"), CategoryAttribute("文字")]
        public bool IsStretch
        {
            get { return _isStretch; }
            set { _isStretch = value; }
        }


        public override PointF[] getRealPoint()
        {
            PointF[] arrPoint = new PointF[4];

            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = (_Width + _WidthAdd) * (_fltStretchWidth + _fltStretchWidthAdd);
            float flth = (_Height + _HeightAdd) * (_fltStretchHeight + _fltStretchHeightAdd);
            arrPoint[0] = new PointF(fltx, flty);//这个点是不变的，我打算以这个点作为基点。
            arrPoint[1] = new PointF(fltx + fltw, flty);//
            arrPoint[2] = new PointF(fltx + fltw, flty + flth);
            arrPoint[3] = new PointF(fltx, flty + flth);

            return arrPoint;
            //return base.getRealPoint();
            //return base.getRealPoint();
        }

        /// <summary>
        /// 拉伸图形
        /// </summary>
        /// <param name="strState"></param>
        /// <param name="startPointf"></param>
        /// <param name="endPointf"></param>
        public void StretchText(string strState, PointF startPointf, PointF endPointf)
        {

            float startX = startPointf.X;
            float startY = startPointf.Y;
            float endX = endPointf.X;
            float endY = endPointf.Y;

            switch (strState)
            {
                case "move":
                    this.Move(new PointF(endX - startX, endY - startY));
                    break;
                case "West":
                    //_XAdd = fltEndMouseX - fltStartMouseX;//坐标点向左移动，
                    //计算拉伸了多少
                    //_fltStretchWidthAdd = 1f + _XAdd / _Width;          
                    break;
                case "East":
                    //_fltStretchWidthAdd = 1f + (fltEndMouseX - fltStartMouseX) / _Width;
                    _fltStretchWidthAdd = (endX - startX) / _Width;
                    break;
                case "North":
                    //_YAdd = fltEndMouseY - fltStartMouseY;
                    //_fltStretchHeightAdd = 1f + _YAdd / _Height;
                    break;
                case "South":
                    //_fltStretchHeightAdd = 1f + (fltEndMouseY - fltStartMouseY) / _Height;
                    _fltStretchHeightAdd = (endY - startY) / _Height;
                    break;
                case "NorthEast":
                    //_YAdd = fltEndMouseY - fltStartMouseY;
                    //_fltStretchHeightAdd = 1f + _YAdd / _Height;
                    //_fltStretchWidthAdd = 1f + (fltEndMouseX - fltStartMouseX) / _Width;
                    break;
                case "SouthWest":
                    //_fltStretchHeightAdd = 1f + (fltEndMouseY - fltStartMouseY) / _Height;
                    //_XAdd = fltEndMouseX - fltStartMouseX;//坐标点向左移动， 
                    break;
                case "SouthEast":
                    //_fltStretchHeightAdd = 1f + (fltEndMouseY - fltStartMouseY) / _Height;
                    // _fltStretchWidthAdd = 1f + (fltEndMouseX - fltStartMouseX) / _Width;
                    _fltStretchHeightAdd = (endY - startY) / _Height;
                    _fltStretchWidthAdd = (endX - startX) / _Width;
                    break;
                case "NorthWest":
                    //_YAdd = fltEndMouseY - fltStartMouseY;
                    //_fltStretchHeightAdd = 1f + _YAdd / _Height;
                    //_XAdd = fltEndMouseX - fltStartMouseX;//坐标点向左移动，
                    //计算拉伸了多少
                    //_fltStretchWidthAdd = 1f + _XAdd / _Width;     

                    break;

                default:
                    break;
            }

        }


        public override   void Redim(string strState, PointF startPointf, PointF endPointf)
        {
            if (IsStretch)
            {
                StretchText(strState, startPointf, endPointf);

            }
            else
            {
                base.Redim(strState, startPointf, endPointf);
            }

            //base.Redim(strState, startPointf, endPointf);
            //base.Redim(strState, startPointf, endPointf);
        }

        //这个变量文字跟静态文字不同点在于，静态文字只有一个文字域，而变量文字有三个。
        protected virtual  void UpdateWidthHeight()
        {
            UpdateStrAllText();//首先更新

            //这里自动取得字符串的宽度和高度
            Bitmap bitmap = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bitmap);
            //g.PageUnit = GraphicsUnit.Pixel;//因为我所有的都换成毫米单位了。，这里求出来的宽度和高度是以毫米为单位的。
            SizeF textSizef = g.MeasureString(_strAllText, _font);//因为右边用到宽度高度的时候会乘以倍数，所以这里用这个字体判定



            Width = textSizef.Width/g.DpiX*25.4f;
            Height = textSizef.Height/g.DpiY*25.4f;

            g.Dispose();
            bitmap.Dispose();

            //cancelStretch();
        }

        protected void cancelStretch()
        {
            _fltStretchWidth = 1f;//拉伸宽度
            _fltStretchHeight = 1f;//拉伸高度
            _fltStretchWidthAdd = 0f;//拉伸宽度增值
            _fltStretchHeightAdd = 0f;//拉伸高度增值

        }
        /// <summary>
        /// 这个方法没别的事情，只是把字符串连接起来而已，并返回连接后的字符串。
        /// </summary>
        /// <returns></returns>
        protected  string UpdateStrAllText()
        {
            //如果设置了变量，就用变量值
            if (_strVarName != "")
            {
                _strAllText = _strPrefix + _strVarValue + _strSuffix;
            }
            else//如果没有设置变量，就用默认值
            {
                _strAllText = _strPrefix + _strDefaultText + _strSuffix;
            }
            
            return _strAllText;

        }

        public override void Draw(Graphics g, float fltKongX, float fltKongY)
        {
            //如下是直接拷贝静态文本的，只是修改了显示的

            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;


            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;
            //如下的这个是偏移些位置
            g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);

            //如下是先从绘制矩形中的拷贝的，然后再修改
            if (Route != 0)
            {
                PointF pZhongXin = getCentrePoint();
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //如下是拉伸图形。
            g.TranslateTransform(fltx, flty, MatrixOrder.Prepend);
            g.ScaleTransform(_fltStretchWidthAdd + _fltStretchWidth, _fltStretchHeightAdd + _fltStretchHeight); // 
            g.TranslateTransform(-fltx, -flty);

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            //如下这个就是画边界
            if (_OppositeGroundColor)
            {
                try
                {
                    using (GraphicsPath path = getGraphicsPathNoOffsetRoute())
                    {
                        g.DrawPath(_myPen, path);
                    }

                }
                catch (Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                    //throw;
                }
            }



            //跟静态文本唯一的区别是，如下显示的是_strAllText 
            //显示区域
            RectangleF rect = new RectangleF(fltx, flty, fltw, flth);

            // 字符串格式
            StringFormat sf = new StringFormat();
            sf.Alignment = AlignMent;
            sf.LineAlignment = LineAlignMent;

            try
            {
                g.DrawString(_strAllText, _RealFont, new SolidBrush(_FillColor), rect, sf);
            }
            catch (System.Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
            }

            //如下的这个是恢复原先的，负数.
            g.TranslateTransform(-fltKongX, -fltKongY);
            g.ResetTransform();//恢复原先的坐标系。

            //base.Draw(g, fltKongX, fltKongY);
        }


        /**
        public override void Draw(Graphics g, ArrayList arrlistMatrix)
        {
            UpdateWidthHeight();
            //如下是直接拷贝静态文本的，只是修改了显示的
            RectangleF rect = getGraphicsPath(arrlistMatrix).GetBounds();

            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;


            //如下是先从绘制矩形中的拷贝的，然后再修改
            if (Route != 0)
            {
                PointF pZhongXin = getCentrePoint();
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //如下是拉伸图形。
            g.TranslateTransform(rect.X, rect.Y, MatrixOrder.Prepend);
            g.ScaleTransform(_fltStretchWidthAdd + _fltStretchWidth, _fltStretchHeightAdd + _fltStretchHeight); // 
            g.TranslateTransform(-rect.X, -rect.Y);

            GraphicsPath path = getGraphicsPath(arrlistMatrix);
            
            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            //如下这个就是画边界

            if (_isShowFrame)
            {
                try
                {
                    g.DrawPath(_myPen, path);
                }
                catch (Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                    //throw;
                }
            }




            //跟静态文本唯一的区别是，如下显示的是_strAllText 
            //显示区域
            //RectangleF rect = new RectangleF(fltx, flty, fltw, flth);


            // 字符串格式
            StringFormat sf = new StringFormat();
            sf.Alignment = AlignMent;
            sf.LineAlignment = LineAlignMent;

            try
            {
                g.DrawString(_strAllText, _RealFont, new SolidBrush(_FillColor), rect, sf);
            }
            catch (System.Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
            }


            g.ResetTransform();

            //base.Draw(g, arrlistMatrix);
        }
         * */


        /**
        public override  void Draw(Graphics g, ArrayList arrlistMatrix)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            GraphicsPath path = getGraphicsPath(arrlistMatrix);

            //throw new NotImplementedException();
            //文字只用填充就可以了，不用边框
            try
            {

                g.FillPath(new SolidBrush(_FillColor), path);

            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //throw;
            }

            g.ResetTransform();

        }
         * */






        



        public override void Draw(Graphics g)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //如下是直接拷贝静态文本的，只是修改了显示的

            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;



            if ((_route + _routeAdd) != 0)
            {
                //旋转图形
                PointF pZhongXin = new PointF(fltx + (fltw) / 2, flty + (flth) / 2);
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //如下是拉伸图形。
            g.TranslateTransform(fltx, flty, MatrixOrder.Prepend);
            g.ScaleTransform(_fltStretchWidthAdd + _fltStretchWidth, _fltStretchHeightAdd + _fltStretchHeight); // 
            g.TranslateTransform(-fltx, -flty);

            //跟静态文本唯一的区别是，如下显示的是_strAllText 
            //显示区域
            RectangleF rect = new RectangleF(fltx, flty, fltw, flth);

            // 字符串格式
            StringFormat sf = new StringFormat();
            sf.Alignment = AlignMent;
            sf.LineAlignment = LineAlignMent;

            try
            {
                g.DrawString(_strAllText, _RealFont, new SolidBrush(_FillColor), rect,sf);
            }
            catch (System.Exception ex)
            {
            	ClsErrorFile.WriteLine(ex);
            }

            //throw new NotImplementedException();

            g.ResetTransform();
            //throw new NotImplementedException();
        }

        public override bool updateVarValue(ArrayList arrlistKeyValue)
        {
            bool isChange = base.updateVarValue(arrlistKeyValue);

            if (isChange)
                UpdateWidthHeight();

            return isChange;

            //base.updateVarValue(arrlistKeyValue);
        }

    }


    /// <summary>
    /// 如下是多行文本，多行文本跟单行文本的区别是多行文本的宽度是不变的，
    /// </summary>
    [Serializable]
    //[ProtoContract]
    public class shapeMultiText : shapeSingleText
    {
        public shapeMultiText()
            : base()
        {
            IsStretch = false;

        }

        public override void Draw(Graphics g, ArrayList arrlistMatrix)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;


            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            GraphicsPath path = base.getGraphicsPath();//首先取得没有偏移但有旋转的路径

            //再反转这个个变换
            arrlistMatrix.Reverse();

            if ((arrlistMatrix != null) && (arrlistMatrix.Count > 0))//只有数量大于0才能做如下的
            {
                for (int i = 0; i < arrlistMatrix.Count; i++)
                {
                    path.Transform((Matrix)arrlistMatrix[i]);

                }
            }



            //如下这个就是画边界
            try
            {

                g.DrawPath(_myPen, path);


            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //throw;
            }

            //throw new NotImplementedException();
            if (_isFill)
            {
                try
                {

                    g.FillPath(new SolidBrush(_FillColor), path);

                }
                catch (Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                    //throw;
                }

            }

            g.ResetTransform();
            //base.Draw(g, arrlistMatrix);
        }

        /// <summary>
        /// 只所以用这个是因为在这个多行文本中，宽和高是用户画出来的，
        /// 不是根据实际的算的，并且在绘制中要绕开这个，因为这个只是画了一个边框，
        /// 不是实际的文字
        /// </summary>
        /// <returns></returns>
        public override   GraphicsPath getGraphicsPath()
        {
            GraphicsPath path = new GraphicsPath();

            path.AddRectangle(getRect());

            //做一个变换矩阵加上偏移和旋转
            System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
            //g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
            m.RotateAt((float)(_route + _routeAdd), getCentrePoint());
            path.Transform(m);//应用变换矩阵

            return path;
            //return base.getGraphicsPath();
        }




        protected override void UpdateWidthHeight()
        {
            UpdateStrAllText();//首先更新
            /**

            //这里自动取得字符串的宽度和高度
            Bitmap bitmap = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bitmap);
            g.PageUnit = GraphicsUnit.Millimeter;//因为我所有的都换成毫米单位了。，这里求出来的宽度和高度是以毫米为单位的。
            SizeF MaxSizef = new SizeF(Width, Height);//这个最大大小，用宽度固定，来求长度的方式
            SizeF textSizef = g.MeasureString(_strAllText, _font,MaxSizef);//

            Height = MaxSizef.Height;//只有这个需要求。因为宽度是固定的

            // 销毁
            g.Dispose();
            bitmap.Dispose();
            //base.UpdateWidthHeight();
             * */
        }

        

        public override void ShapeInit(PointF p1, PointF p2)
        {
            _X = Math.Min(p1.X, p2.X);
            _Y = Math.Min(p1.Y, p2.Y);
            _Width = Math.Max(p1.X, p2.X) - _X;
            _Height = Math.Max(p1.Y, p2.Y) - _Y;

            UpdateWidthHeight();
            //base.ShapeInit(p1, p2);
        }
        

    }

    [Serializable]
    //[ProtoContract]
    public class ShapeEllipse : ShapeEle
    {
        public override ShapeEle DeepClone()
        {
            ShapeEllipse shapeEle = new ShapeEllipse();
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

            return shapeEle;
            //throw new NotImplementedException();
        }

        public override GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(getRect());
            return path;
            //return base.getGraphicsPath();
        }

    }

    [Serializable]
    //[ProtoContract]
    public class ShapeBarcode : ShapeEle
    {
        //就是一个普通的矩形，但是
        protected   string _BarcodeEncoding;//条形码编码
        protected   bool _isIncludeLabel;//
        protected   string _strBarcodeNumber;
        protected  Font _fontLabelFont=new Font("Microsoft Sans Serif", 10, FontStyle.Bold); //数字的字体。
        protected  Font _RealFont=new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
        private LabelPositions _LabelPosition = LabelPositions.BOTTOMCENTER;//

        private float _fltOldW, _fltOldh;//这两个参数用在条形码中，特别是二维码运算很繁琐，这两个保存旧的宽度和高度，如果这两个都相同，就不用重新计算
        private Image _imageOld;//直接用这个来绘图就可以了。

        /// <summary>
        /// 这里分情况，当出现错误的时候，判断是否也原先的错误一样，如果不一样，就显示，这里已经包含了原先没有错误，为空字符串的情况
        /// 而要重新验证，只需要将这个重置为空值字符串，在两个地方需要重置
        /// 一个是 BarcodeNumber ，当用户输入后当然得重置了。
        /// 另一个是变量更新的时候，updateVarValue ，在这个方法中，如果变量值更新了，就得重置字符串。
        /// </summary>
        private string _strBarcodeErrorMessage;//条形码返回的错误，这个为空字符串时，就会重新验证。

        public  ShapeBarcode() :base()
        {
            _BarcodeEncoding = "EAN13";
            _strBarcodeNumber = "690123456789";
            _isIncludeLabel = true;
            

        }
        /// <summary>
        /// 条形码文字的位置，默认为下边中间。
        /// </summary>
        [DescriptionAttribute("文字位置"),DisplayName("文字位置"), CategoryAttribute("条形码设置")]
        public LabelPositions LabelPosition
        {
            get { return _LabelPosition; }
            set { _LabelPosition = value;
            isChangeed = true;
            }
        }//LabelPosition

        [TypeConverter(typeof(VarNameDetails)), DescriptionAttribute("就是EXCEL表格的第一行当变量名"), DisplayName("变量名"), CategoryAttribute("变量设置")]
        public string VarName
        {
            get
            {
                return _strVarName;
            }
            set
            {
                _strVarName = value;

            }
        }

        [DescriptionAttribute("条形码数字"),DisplayName("默认条形码数字"), CategoryAttribute("条形码设置")]
        public string BarcodeNumber
        {
            get
            {
                return _strBarcodeNumber;
            }
            set
            {
                _strBarcodeNumber = value;
                _strBarcodeErrorMessage = "";//用这个来再次判定是否符合
                isChangeed = true;
            }
        }

        [ DescriptionAttribute("包含数字"),DisplayName("包含文字"), CategoryAttribute("条形码设置")]
        public bool isIncludeLabel
        {
            get
            {
                return _isIncludeLabel;
            }

            set
            {
                _isIncludeLabel = value;
            }

        }

        

        [TypeConverter(typeof(BarcodeEncoding)), DescriptionAttribute("编码"),DisplayName("编码"), CategoryAttribute("条形码设置")]
        public string Encoding
        {
            get
            {
                return _BarcodeEncoding;
            }
            set
            {
                _BarcodeEncoding = value;
                isChangeed = true;
            }
        }

        [ DescriptionAttribute("字体"),DisplayName("字体"), CategoryAttribute("条形码设置")]
        [XmlIgnore]
        public Font  LabelFont
        {
            get
            {
                return _fontLabelFont;
            }
            set
            {
                _fontLabelFont = value;
                _RealFont = ChangeFontSize(_fontLabelFont, _fontLabelFont.Size * _Zoom);//设置真实的字体大小
                isChangeed = true;


            }
        }

        private string _strLanguageEncodingName;

        [TypeConverter(typeof(LanguageEncoding)), DescriptionAttribute("语言编码,中文的话：国外的最好用UTF-8编码，而国内的用GB2312的多"), DisplayName("语言编码"), CategoryAttribute("二维码设置")]
        [XmlElement]
        public string LanguageEncodingDisplayName
        {
            get
            {
                foreach (EncodingInfo item in System.Text.Encoding.GetEncodings())
                {
                    if (item.Name==_strLanguageEncodingName)
                        return item.DisplayName;
                }

                return "";

            }
            set
            {
                isChangeed = true;
                foreach (EncodingInfo item in System.Text.Encoding.GetEncodings())
                {
                    if (item.DisplayName==value)
                    {
                        _strLanguageEncodingName=item.Name;
                        break;
                    }
                }
            }
        }

        private bool isChangeed;//受否改变了，

        private ErrorCorrectionLevel _QRCODEErrorLevel = ErrorCorrectionLevel.L;
        [TypeConverter(typeof(QrCodeErrorLevel)), DescriptionAttribute("容错率"), DisplayName("容错率"), CategoryAttribute("QR Code 设置")]
        [XmlElement]
        public string QrCodeErrorLevel
        {
            get
            {
                if (_QRCODEErrorLevel==ErrorCorrectionLevel.L)
                {
                    return "容错7%";
                } 
                else if (_QRCODEErrorLevel==ErrorCorrectionLevel.M)
                {
                    return "容错15%";
                }
                else if (_QRCODEErrorLevel == ErrorCorrectionLevel.Q)
                {
                    return "容错25%";
                }
                else 
                {
                    return "容错30%";
                }
            }

            set
            {
                isChangeed = true;//不管怎样，都是改变了

                switch (value)
                {
                    case "容错7%":
                        _QRCODEErrorLevel=ErrorCorrectionLevel.L;
                        break;
                    case "容错15%":
                        _QRCODEErrorLevel = ErrorCorrectionLevel.M;
                        break;
                    case "容错25%":
                        _QRCODEErrorLevel = ErrorCorrectionLevel.Q;
                        break;
                    default:
                        _QRCODEErrorLevel = ErrorCorrectionLevel.H;
                        break;
                }
            }
        }


        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public override float Zoom
        {
            get
            {
                return base.Zoom;
            }
            set
            {

                base.Zoom = value;
                isChangeed = true;
                LabelFont = LabelFont;//设置字体
            }
        }


        //如下的这个只是在序列化时用到。
        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public myFont SerializerFont
        {
            get
            {
                myFont font2 = new myFont();

                font2.Name = LabelFont.Name;
                font2.Size = LabelFont.Size;
                font2.Style = LabelFont.Style;

                return font2;

            }

            set
            {
                LabelFont = new Font(value.Name, value.Size, value.Style);
            }
        }
        public override ShapeEle DeepClone()
        {

            ShapeBarcode shapeEle = new ShapeBarcode();
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
            shapeEle.BarcodeNumber = BarcodeNumber;
            shapeEle.Encoding = Encoding;
            shapeEle.isIncludeLabel = isIncludeLabel;
            shapeEle.LabelFont = LabelFont;
            shapeEle.LabelPosition = LabelPosition;
            shapeEle.LanguageEncodingDisplayName = LanguageEncodingDisplayName;
            shapeEle.QrCodeErrorLevel = QrCodeErrorLevel;

            return shapeEle;
            //throw new NotImplementedException();
        }


        public override bool updateVarValue(ArrayList arrlistKeyValue)
        {
            bool isChange = base.updateVarValue(arrlistKeyValue);

            if (isChange)
            {
                _strBarcodeErrorMessage = "";//设置这个为空就能自动验证了。
                isChangeed = true;//这个是类中的变量，设置是否改变
            }

            return isChange;


            /**
            foreach (clsKeyValue item in arrlistKeyValue)
            {
                if (item.Key == _strVarName)
                {
                    //是否改变
                    bool isChange = false;
                    if (_strVarValue != item.Value)
                    {
                        isChange = true;

                        _strBarcodeErrorMessage = "";//设置这个为空就能自动验证了。
                    }
                    //更新变量值
                    _strVarValue = item.Value;
                    return isChange;//返回是否更新到不同的值

                }

            }
            **/
            //return false;
            //return base.updateVarValue(arrlistKeyValue);
        }

        private  Bitmap toBitmap(COMMON.ByteMatrix matrix, float fltDPIX, float fltDPIY)
        {
            int width = matrix.Width;
            int height = matrix.Height;
            Bitmap bmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bmap.SetResolution(fltDPIX, fltDPIY);//设置图片象素
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bmap.SetPixel(x, y, matrix.get_Renamed(x, y) != -1 ? ColorTranslator.FromHtml("0xFF000000") : ColorTranslator.FromHtml("0xFFFFFFFF"));
                }
            }
            return bmap;
        }

        public override void Draw(Graphics g, ArrayList arrlistMatrix)
        {

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;
            //如下的这个是偏移些位置

            //如下是先从绘制矩形中的拷贝的，然后再修改
            if (Route != 0)
            {
                PointF pZhongXin = getCentrePoint();
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            //如下这个就是画边界
            /**
            try
            {
                using (GraphicsPath path = getGraphicsPath())
                {
                    g.DrawPath(_myPen, path);
                }

            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //throw;
            }

            //throw new NotImplementedException();
            if (_isFill)
            {
                try
                {
                    using (GraphicsPath path = getGraphicsPath())
                    {
                        g.FillPath(new SolidBrush(_FillColor), path);
                    }
                }
                catch (Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                    //throw;
                }

            }
             * */
            //如下是拷贝的我原先软件的。
            #region

            //如果两个都是为空值当然先返回啦。
            /**
            if (_strVarValue == "")
                return;
             * */


            string strBarcodeNumber = "";

            //如果设置变量名，就用变量名的对应的变量值。
            if (_strVarName != "")
            {
                strBarcodeNumber = _strVarValue;
                //_strBarcodeNumber = "";

            }
            else//如果没有变量名就用默认的值
            {
                strBarcodeNumber = _strBarcodeNumber;
            }

            if (strBarcodeNumber == "")
                return;


            //条形码可能有异常，比如说位数不符等等
            try
            {

                RectangleF rect = getGraphicsPath(arrlistMatrix).GetBounds();
                float fltx = rect.X;
                float flty = rect.Y;
                float fltw = rect.Width;
                float flth = rect.Height;

                /**
                float fltx = _X + _XAdd;
                float flty = _Y + _YAdd;
                float fltw = _Width + _WidthAdd;
                float flth = _Height + _HeightAdd;
                 * */



                PointF poingStr = new PointF(fltx, flty);//实际位置

                int intBarCodeWidth = (int)(fltw * g.DpiX / 25.4);
                int intBarCodeHeight = (int)(flth * g.DpiY / 25.4);
                ;
                string strEncoding = _BarcodeEncoding; 
                BarcodeLib.TYPE myType = BarcodeLib.TYPE.EAN13;
                switch (_BarcodeEncoding)
                {
                    case "EAN13":
                        myType = BarcodeLib.TYPE.EAN13;
                        break;
                    case "EAN8":
                        myType = BarcodeLib.TYPE.EAN8;
                        break;
                    case "FIM":
                        myType = BarcodeLib.TYPE.FIM;
                        break;
                    case "Codabar":
                        myType = BarcodeLib.TYPE.Codabar;
                        break;
                    case "UPCA":
                        myType = BarcodeLib.TYPE.UPCA;
                        break;
                    case "UPCE":
                        myType = BarcodeLib.TYPE.UPCE;
                        break;
                    case "UPC_SUPPLEMENTAL_2DIGIT":
                        myType = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_2DIGIT;
                        break;
                    case "UPC_SUPPLEMENTAL_5DIGIT":
                        myType = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_5DIGIT;
                        break;
                    case "CODE39":
                        myType = BarcodeLib.TYPE.CODE39;
                        break;
                    case "CODE39Extended":
                        myType = BarcodeLib.TYPE.CODE39Extended;
                        break;
                    case "CODE128":
                        myType = BarcodeLib.TYPE.CODE128;
                        break;
                    case "CODE128A":
                        myType = BarcodeLib.TYPE.CODE128A;
                        break;
                    case "CODE128B":
                        myType = BarcodeLib.TYPE.CODE128B;
                        break;
                    case "CODE128C":
                        myType = BarcodeLib.TYPE.CODE128C;
                        break;
                    case "ISBN":
                        myType = BarcodeLib.TYPE.ISBN;
                        break;
                    case "Interleaved2of5":
                        myType = BarcodeLib.TYPE.Interleaved2of5;
                        break;
                    case "Standard2of5":
                        myType = BarcodeLib.TYPE.Standard2of5;
                        break;
                    case "Industrial2of5":
                        myType = BarcodeLib.TYPE.Industrial2of5;
                        break;
                    case "PostNet":
                        myType = BarcodeLib.TYPE.PostNet;
                        break;
                    case "BOOKLAND":
                        myType = BarcodeLib.TYPE.BOOKLAND;
                        break;
                    case "JAN13":
                        myType = BarcodeLib.TYPE.JAN13;
                        break;
                    case "MSI_Mod10":
                        myType = BarcodeLib.TYPE.MSI_Mod10;
                        break;
                    case "MSI_2Mod10":
                        myType = BarcodeLib.TYPE.MSI_2Mod10;
                        break;
                    case "MSI_Mod11":
                        myType = BarcodeLib.TYPE.MSI_Mod11;
                        break;
                    case "MSI_Mod11_Mod10":
                        myType = BarcodeLib.TYPE.MSI_Mod11_Mod10;
                        break;
                    case "Modified_Plessey":
                        myType = BarcodeLib.TYPE.Modified_Plessey;
                        break;
                    case "CODE11":
                        myType = BarcodeLib.TYPE.CODE11;
                        break;
                    case "USD8":
                        myType = BarcodeLib.TYPE.USD8;
                        break;
                    case "UCC12":
                        myType = BarcodeLib.TYPE.UCC12;
                        break;
                    case "UCC13":
                        myType = BarcodeLib.TYPE.UCC13;
                        break;
                    case "LOGMARS":
                        myType = BarcodeLib.TYPE.LOGMARS;
                        break;
                    case "ITF14":
                        myType = BarcodeLib.TYPE.ITF14;
                        break;
                    case "TELEPEN":
                        myType = BarcodeLib.TYPE.TELEPEN;
                        break;
                    case "QR_CODE":
                        //如下得判断长度和宽度是否可以显示,我得茶皂他们最短需要多少。
                        if ((intBarCodeWidth < 21) || (intBarCodeHeight < 21))
                        {
                            g.DrawString("图像太小显示不了", new Font("Arial", 6), new SolidBrush(Color.Black), poingStr);
                            g.DrawRectangle(new Pen(Color.Black, 0.5f), fltx, flty, fltw, flth);
                        }
                        else
                        {
                            //只有在这两个中有一个不相等的情况下才需要更新。
                            if ((_fltOldW != _Width) || (_fltOldh != _Height) || isChangeed)
                            {
                                isChangeed = false;//重新设置成没有更新。

                                _fltOldW = _Width;
                                _fltOldh = _Height;

                                Hashtable hints = new Hashtable();
                                //hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);//容错能力

                                hints.Add(EncodeHintType.ERROR_CORRECTION, _QRCODEErrorLevel);//设置容错率

                                //如下是读取编码，只有在这个不为空的时候才选择
                                if (_strLanguageEncodingName != "")
                                {
                                    hints.Add(EncodeHintType.CHARACTER_SET, _strLanguageEncodingName);//字符集
                                }


                                COMMON.ByteMatrix byteMatrix = new MultiFormatWriter().encode(strBarcodeNumber, BarcodeFormat.QR_CODE, intBarCodeWidth, intBarCodeHeight, hints);
                                _imageOld = toBitmap(byteMatrix, g.DpiX, g.DpiY);
                                g.DrawImage(_imageOld, rect);

                            }
                            else//如果没有更新，就直接绘图就可以了。
                            {
                                g.DrawImage(_imageOld, rect);
                            }


                        }
                        return;//这个是直接返回的。因为下边的是调用的一维码的程序
                }



                BarcodeLib.Barcode bar = new BarcodeLib.Barcode();


                bar.IncludeLabel = _isIncludeLabel;
                bar.LabelFont = _RealFont;
                bar.LabelPosition = LabelPosition;//条形码文字的位置


                //不能少于这个大小
                /**因为跟放大缩小相冲突，所以注释掉了，如果图像小，就显示显示不了。
                if (intBarCodeWidth < 100)
                {
                    intBarCodeWidth = 100;
                    _Width = intBarCodeWidth / g.DpiX * 25.4f;
                    _WidthAdd = 0;
                }
                if (intBarCodeHeight < 30)
                {
                    intBarCodeHeight = 30;
                    _Height = intBarCodeHeight / g.DpiY * 25.4f;
                    _HeightAdd = 0;
                }
                 * */

                if ((intBarCodeWidth < 100) || (intBarCodeHeight < 30))
                {
                    g.DrawString("图像太小显示不了", new Font("Arial", 6), new SolidBrush(Color.Black), poingStr);
                    g.DrawRectangle(new Pen(Color.Black, 0.5f), fltx, flty, fltw, flth);
                }
                else
                {

                    Image myImage = bar.Encode(myType, strBarcodeNumber, intBarCodeWidth, intBarCodeHeight, g.DpiX, g.DpiY);
                    //将最新的宽度更新。好像不用更新。
                    g.DrawImage(myImage, rect);

                }

                bar.Dispose();


            }
            catch (Exception e)
            {
                //因为这个Draw是持续刷新的，为了在条形码数字出错是只提示依次，在此需要这个，而我在条形码数字更改的时候，重新设置那个为空了
                if (_strBarcodeErrorMessage != e.Message)
                {
                    _strBarcodeErrorMessage = e.Message;
                    MessageBox.Show(e.Message);
                    return;
                }

            }

            #endregion

            //throw new NotImplementedException();

            g.ResetTransform();//恢复原先的坐标系。

            //base.Draw(g, arrlistMatrix);
        }

        public override void Draw(Graphics g, float fltKongX, float fltKongY)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;
            //如下的这个是偏移些位置
            g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);

            //如下是先从绘制矩形中的拷贝的，然后再修改
            if (Route != 0)
            {
                PointF pZhongXin = getCentrePoint();
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            //如下这个就是画边界
            /**
            try
            {
                using (GraphicsPath path = getGraphicsPath())
                {
                    g.DrawPath(_myPen, path);
                }

            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //throw;
            }

            //throw new NotImplementedException();
            if (_isFill)
            {
                try
                {
                    using (GraphicsPath path = getGraphicsPath())
                    {
                        g.FillPath(new SolidBrush(_FillColor), path);
                    }
                }
                catch (Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                    //throw;
                }

            }
             * */
            //如下是拷贝的我原先软件的。
            #region

            //如果两个都是为空值当然先返回啦。
            /**
            if (_strVarValue == "")
                return;
             * */


            string strBarcodeNumber = "";

            //如果设置变量名，就用变量名的对应的变量值。
            if (_strVarName != "")
            {
                strBarcodeNumber = _strVarValue;
                //_strBarcodeNumber = "";

            }
            else//如果没有变量名就用默认的值
            {
                strBarcodeNumber = _strBarcodeNumber;
            }

            if (strBarcodeNumber == "")
                return;


            //条形码可能有异常，比如说位数不符等等
            try
            {

                float fltx = _X + _XAdd;
                float flty = _Y + _YAdd;
                float fltw = _Width + _WidthAdd;
                float flth = _Height + _HeightAdd;

                PointF poingStr = new PointF(fltx, flty);//实际位置

                int intBarCodeWidth = (int)(fltw * g.DpiX / 25.4);
                int intBarCodeHeight = (int)(flth * g.DpiY / 25.4);
                ;
                string strEncoding = _BarcodeEncoding;
                BarcodeLib.TYPE myType = BarcodeLib.TYPE.EAN13;
                switch (_BarcodeEncoding)
                {
                    case "EAN13":
                        myType = BarcodeLib.TYPE.EAN13;
                        break;
                    case "EAN8":
                        myType = BarcodeLib.TYPE.EAN8;
                        break;
                    case "FIM":
                        myType = BarcodeLib.TYPE.FIM;
                        break;
                    case "Codabar":
                        myType = BarcodeLib.TYPE.Codabar;
                        break;
                    case "UPCA":
                        myType = BarcodeLib.TYPE.UPCA;
                        break;
                    case "UPCE":
                        myType = BarcodeLib.TYPE.UPCE;
                        break;
                    case "UPC_SUPPLEMENTAL_2DIGIT":
                        myType = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_2DIGIT;
                        break;
                    case "UPC_SUPPLEMENTAL_5DIGIT":
                        myType = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_5DIGIT;
                        break;
                    case "CODE39":
                        myType = BarcodeLib.TYPE.CODE39;
                        break;
                    case "CODE39Extended":
                        myType = BarcodeLib.TYPE.CODE39Extended;
                        break;
                    case "CODE128":
                        myType = BarcodeLib.TYPE.CODE128;
                        break;
                    case "CODE128A":
                        myType = BarcodeLib.TYPE.CODE128A;
                        break;
                    case "CODE128B":
                        myType = BarcodeLib.TYPE.CODE128B;
                        break;
                    case "CODE128C":
                        myType = BarcodeLib.TYPE.CODE128C;
                        break;
                    case "ISBN":
                        myType = BarcodeLib.TYPE.ISBN;
                        break;
                    case "Interleaved2of5":
                        myType = BarcodeLib.TYPE.Interleaved2of5;
                        break;
                    case "Standard2of5":
                        myType = BarcodeLib.TYPE.Standard2of5;
                        break;
                    case "Industrial2of5":
                        myType = BarcodeLib.TYPE.Industrial2of5;
                        break;
                    case "PostNet":
                        myType = BarcodeLib.TYPE.PostNet;
                        break;
                    case "BOOKLAND":
                        myType = BarcodeLib.TYPE.BOOKLAND;
                        break;
                    case "JAN13":
                        myType = BarcodeLib.TYPE.JAN13;
                        break;
                    case "MSI_Mod10":
                        myType = BarcodeLib.TYPE.MSI_Mod10;
                        break;
                    case "MSI_2Mod10":
                        myType = BarcodeLib.TYPE.MSI_2Mod10;
                        break;
                    case "MSI_Mod11":
                        myType = BarcodeLib.TYPE.MSI_Mod11;
                        break;
                    case "MSI_Mod11_Mod10":
                        myType = BarcodeLib.TYPE.MSI_Mod11_Mod10;
                        break;
                    case "Modified_Plessey":
                        myType = BarcodeLib.TYPE.Modified_Plessey;
                        break;
                    case "CODE11":
                        myType = BarcodeLib.TYPE.CODE11;
                        break;
                    case "USD8":
                        myType = BarcodeLib.TYPE.USD8;
                        break;
                    case "UCC12":
                        myType = BarcodeLib.TYPE.UCC12;
                        break;
                    case "UCC13":
                        myType = BarcodeLib.TYPE.UCC13;
                        break;
                    case "LOGMARS":
                        myType = BarcodeLib.TYPE.LOGMARS;
                        break;
                    case "ITF14":
                        myType = BarcodeLib.TYPE.ITF14;
                        break;
                    case "TELEPEN":
                        myType = BarcodeLib.TYPE.TELEPEN;
                        break;
                    case "QR_CODE":
                        //如下得判断长度和宽度是否可以显示,我得茶皂他们最短需要多少。
                        if ((intBarCodeWidth < 21) || (intBarCodeHeight < 21))
                        {
                            g.DrawString("图像太小显示不了", new Font("Arial", 6), new SolidBrush(Color.Black), poingStr);
                            g.DrawRectangle(new Pen(Color.Black, 0.5f), fltx, flty, fltw, flth);
                        }
                        else
                        {
                            //只有在这两个中有一个不相等的情况下才需要更新。
                            if ((_fltOldW != _Width) || (_fltOldh != _Height) || isChangeed)
                            {
                                isChangeed = false;//重新设置成没有更新。

                                _fltOldW = _Width;
                                _fltOldh = _Height;

                                Hashtable hints = new Hashtable();
                                //hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);//容错能力

                                hints.Add(EncodeHintType.ERROR_CORRECTION, _QRCODEErrorLevel);//设置容错率

                                //如下是读取编码，只有在这个不为空的时候才选择
                                if (_strLanguageEncodingName != "")
                                {
                                    hints.Add(EncodeHintType.CHARACTER_SET, _strLanguageEncodingName);//字符集
                                }

                                COMMON.ByteMatrix byteMatrix = new MultiFormatWriter().encode(strBarcodeNumber, BarcodeFormat.QR_CODE, intBarCodeWidth, intBarCodeHeight, hints);
                                _imageOld = toBitmap(byteMatrix, g.DpiX, g.DpiY);
                                g.DrawImage(_imageOld, poingStr);

                            }
                            else//如果没有更新，就直接绘图就可以了。
                            {
                                g.DrawImage(_imageOld, poingStr);
                            }


                        }
                        return;//这个是直接返回的。因为下边的是调用的一维码的程序
                }



                BarcodeLib.Barcode bar = new BarcodeLib.Barcode();


                bar.IncludeLabel = _isIncludeLabel;
                bar.LabelFont = _RealFont;
                bar.LabelPosition = LabelPosition;//条形码文字的位置





                //不能少于这个大小
                /**因为跟放大缩小相冲突，所以注释掉了，如果图像小，就显示显示不了。
                if (intBarCodeWidth < 100)
                {
                    intBarCodeWidth = 100;
                    _Width = intBarCodeWidth / g.DpiX * 25.4f;
                    _WidthAdd = 0;
                }
                if (intBarCodeHeight < 30)
                {
                    intBarCodeHeight = 30;
                    _Height = intBarCodeHeight / g.DpiY * 25.4f;
                    _HeightAdd = 0;
                }
                 * */

                if ((intBarCodeWidth < 100) || (intBarCodeHeight < 30))
                {
                    g.DrawString("图像太小显示不了", new Font("Arial", 6), new SolidBrush(Color.Black), poingStr);
                    g.DrawRectangle(new Pen(Color.Black, 0.5f), fltx, flty, fltw, flth);
                }
                else
                {
                    Image myImage = bar.Encode(myType, strBarcodeNumber, intBarCodeWidth, intBarCodeHeight, g.DpiX, g.DpiY);
                    //将最新的宽度更新。好像不用更新。
                    g.DrawImage(myImage, poingStr);

                }

                bar.Dispose();


            }
            catch (Exception e)
            {
                //因为这个Draw是持续刷新的，为了在条形码数字出错是只提示依次，在此需要这个，而我在条形码数字更改的时候，重新设置那个为空了
                if (_strBarcodeErrorMessage != e.Message)
                {
                    _strBarcodeErrorMessage = e.Message;
                    MessageBox.Show(e.Message);
                    return;
                }

            }

            #endregion

            //throw new NotImplementedException();

            //如下的这个是恢复原先的，负数.
            g.TranslateTransform(-fltKongX, -fltKongY);
            g.ResetTransform();//恢复原先的坐标系。
            //base.Draw(g, fltKongX, fltKongY);
        }


        public override void Draw(Graphics g)
        {

            //如下是拷贝的我原先软件的。
            #region

      //如果两个都是为空值当然先返回啦。
            /**
            if (_strVarValue == "")
                return;
             * */


            string strBarcodeNumber = "";

            //如果设置变量名，就用变量名的对应的变量值。
            if (_strVarName != "")
            {
                strBarcodeNumber = _strVarValue;
                //_strBarcodeNumber = "";
                
            }
            else//如果没有变量名就用默认的值
            {
                strBarcodeNumber = _strBarcodeNumber;
            }

            if (strBarcodeNumber == "")
                return;


            //条形码可能有异常，比如说位数不符等等
            try
            {

                float fltx = _X + _XAdd;
                float flty = _Y + _YAdd;
                float fltw = _Width + _WidthAdd;
                float flth = _Height + _HeightAdd;

                PointF poingStr = new PointF(fltx, flty);//实际位置

                int intBarCodeWidth = (int)(fltw * g.DpiX / 25.4);
                int intBarCodeHeight = (int)(flth * g.DpiY / 25.4);
                ;
                string strEncoding = _BarcodeEncoding;
                BarcodeLib.TYPE myType = BarcodeLib.TYPE.EAN13;
                switch (_BarcodeEncoding)
                {
                    case "EAN13":
                        myType = BarcodeLib.TYPE.EAN13;
                        break;
                    case "EAN8":
                        myType = BarcodeLib.TYPE.EAN8;
                        break;
                    case "FIM":
                        myType = BarcodeLib.TYPE.FIM;
                        break;
                    case "Codabar":
                        myType = BarcodeLib.TYPE.Codabar;
                        break;
                    case "UPCA":
                        myType = BarcodeLib.TYPE.UPCA;
                        break;
                    case "UPCE":
                        myType = BarcodeLib.TYPE.UPCE;
                        break;
                    case "UPC_SUPPLEMENTAL_2DIGIT":
                        myType = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_2DIGIT;
                        break;
                    case "UPC_SUPPLEMENTAL_5DIGIT":
                        myType = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_5DIGIT;
                        break;
                    case "CODE39":
                        myType = BarcodeLib.TYPE.CODE39;
                        break;
                    case "CODE39Extended":
                        myType = BarcodeLib.TYPE.CODE39Extended;
                        break;
                    case "CODE128":
                        myType = BarcodeLib.TYPE.CODE128;
                        break;
                    case "CODE128A":
                        myType = BarcodeLib.TYPE.CODE128A;
                        break;
                    case "CODE128B":
                        myType = BarcodeLib.TYPE.CODE128B;
                        break;
                    case "CODE128C":
                        myType = BarcodeLib.TYPE.CODE128C;
                        break;
                    case "ISBN":
                        myType = BarcodeLib.TYPE.ISBN;
                        break;
                    case "Interleaved2of5":
                        myType = BarcodeLib.TYPE.Interleaved2of5;
                        break;
                    case "Standard2of5":
                        myType = BarcodeLib.TYPE.Standard2of5;
                        break;
                    case "Industrial2of5":
                        myType = BarcodeLib.TYPE.Industrial2of5;
                        break;
                    case "PostNet":
                        myType = BarcodeLib.TYPE.PostNet;
                        break;
                    case "BOOKLAND":
                        myType = BarcodeLib.TYPE.BOOKLAND;
                        break;
                    case "JAN13":
                        myType = BarcodeLib.TYPE.JAN13;
                        break;
                    case "MSI_Mod10":
                        myType = BarcodeLib.TYPE.MSI_Mod10;
                        break;
                    case "MSI_2Mod10":
                        myType = BarcodeLib.TYPE.MSI_2Mod10;
                        break;
                    case "MSI_Mod11":
                        myType = BarcodeLib.TYPE.MSI_Mod11;
                        break;
                    case "MSI_Mod11_Mod10":
                        myType = BarcodeLib.TYPE.MSI_Mod11_Mod10;
                        break;
                    case "Modified_Plessey":
                        myType = BarcodeLib.TYPE.Modified_Plessey;
                        break;
                    case "CODE11":
                        myType = BarcodeLib.TYPE.CODE11;
                        break;
                    case "USD8":
                        myType = BarcodeLib.TYPE.USD8;
                        break;
                    case "UCC12":
                        myType = BarcodeLib.TYPE.UCC12;
                        break;
                    case "UCC13":
                        myType = BarcodeLib.TYPE.UCC13;
                        break;
                    case "LOGMARS":
                        myType = BarcodeLib.TYPE.LOGMARS;
                        break;
                    case "ITF14":
                        myType = BarcodeLib.TYPE.ITF14;
                        break;
                    case "TELEPEN":
                        myType = BarcodeLib.TYPE.TELEPEN;
                        break;
                    case "QR_CODE":
                        //如下得判断长度和宽度是否可以显示,我得茶皂他们最短需要多少。
                        if ((intBarCodeWidth < 21) || (intBarCodeHeight < 21))
                        {
                            g.DrawString("图像太小显示不了", new Font("Arial", 6), new SolidBrush(Color.Black), poingStr);
                            g.DrawRectangle(new Pen(Color.Black, 0.5f), fltx, flty, fltw, flth);
                        }
                        else
                        {
                            //只有在这两个中有一个不相等的情况下才需要更新。
                            if ((_fltOldW != _Width) || (_fltOldh != _Height)||isChangeed)
                            {
                                isChangeed = false;//重新设置成没有更新。

                                _fltOldW=_Width;
                                _fltOldh=_Height;

                                Hashtable hints = new Hashtable();
                                //hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);//容错能力

                                hints.Add(EncodeHintType.ERROR_CORRECTION, _QRCODEErrorLevel);//设置容错率

                                //如下是读取编码，只有在这个不为空的时候才选择
                                if (_strLanguageEncodingName != "")
                                {
                                    hints.Add(EncodeHintType.CHARACTER_SET, _strLanguageEncodingName);//字符集
                                }

                                COMMON.ByteMatrix byteMatrix = new MultiFormatWriter().encode(strBarcodeNumber, BarcodeFormat.QR_CODE, intBarCodeWidth, intBarCodeHeight,hints);
                                _imageOld=toBitmap(byteMatrix, g.DpiX, g.DpiY);
                                g.DrawImage(_imageOld,poingStr);

                            }
                            else//如果没有更新，就直接绘图就可以了。
                            {
                                g.DrawImage(_imageOld, poingStr);
                            }


                        }
                        return;//这个是直接返回的。因为下边的是调用的一维码的程序
                }



                BarcodeLib.Barcode bar = new BarcodeLib.Barcode();
                

                bar.IncludeLabel = _isIncludeLabel;
                bar.LabelFont = _RealFont;
                bar.LabelPosition = LabelPosition;//条形码文字的位置

               



                //不能少于这个大小
                /**因为跟放大缩小相冲突，所以注释掉了，如果图像小，就显示显示不了。
                if (intBarCodeWidth < 100)
                {
                    intBarCodeWidth = 100;
                    _Width = intBarCodeWidth / g.DpiX * 25.4f;
                    _WidthAdd = 0;
                }
                if (intBarCodeHeight < 30)
                {
                    intBarCodeHeight = 30;
                    _Height = intBarCodeHeight / g.DpiY * 25.4f;
                    _HeightAdd = 0;
                }
                 * */

                if ((intBarCodeWidth < 100) || (intBarCodeHeight < 30))
                {
                    g.DrawString("图像太小显示不了", new Font("Arial", 6), new SolidBrush(Color.Black), poingStr);
                    g.DrawRectangle(new Pen(Color.Black, 0.5f), fltx, flty, fltw, flth);
                }
                else
                {
                    Image myImage = bar.Encode(myType, strBarcodeNumber, intBarCodeWidth, intBarCodeHeight, g.DpiX, g.DpiY);

                    //将最新的宽度更新。好像不用更新。
                    g.DrawImage(myImage, poingStr);
                   
                }

                bar.Dispose();


            }
            catch (Exception e)
            {
                //因为这个Draw是持续刷新的，为了在条形码数字出错是只提示依次，在此需要这个，而我在条形码数字更改的时候，重新设置那个为空了
                if  (_strBarcodeErrorMessage != e.Message)
                {
                    _strBarcodeErrorMessage = e.Message;
                    MessageBox.Show(e.Message);
                    return;
                }

            }

            #endregion

            //throw new NotImplementedException();
        }

    }//end class ShapeBarcode

    [Serializable]
    //[ProtoContract]
    public class ShapeImage : ShapeEle
    {

        private   Bitmap _image=new Bitmap (10,10);//预防没有初始化吧
        private bool _isOriginalSize = false;//原始大小
        private bool _isScaled = true;//按比例缩放。

        private bool isChangeed;//受否改变了，

        /**
        [TypeConverter(typeof(VarNameDetails)), DescriptionAttribute("这个加载图片默认是将字段的值当成图片文件路径"), DisplayName("变量名"), CategoryAttribute("变量设置")]
        [XmlElement]
        public string VarName
        {
            get
            {
                return _strVarName;
            }
            set
            {
                _strVarName = value;

            }

        }
         * */

        [DescriptionAttribute("图片"),DisplayName("图片"), CategoryAttribute("图片设置")]
        [XmlIgnore]
        public Bitmap Image
        {
            get
            {
                return _image;
            }
            set
            {
                //_image = new Bitmap(value);

                try
                {
                    using (System.IO.MemoryStream memory3 = new System.IO.MemoryStream())
                    {

                        (new Bitmap(value)).Save(memory3, System.Drawing.Imaging.ImageFormat.Jpeg);
                        Bitmap bitmapTemp = new Bitmap(memory3);
                        //这里强制最大的图像宽度是800*600，等比例缩放。
                        //条件是宽度和高度大于
                        // 俺比例缩放首先要求出图像的宽和高

                        if ((bitmapTemp.Width>800)||
                            (bitmapTemp.Height>600))
                        {
                            float fltx = 0;
                            float flty = 0;
                            float fltw = 800;
                            float flth = 600;


                            float fltImageWidth = bitmapTemp.Width;
                            float fltImageHeight = bitmapTemp.Height;//因为只需要比例，所以不需要放大率和分辨率之类的东西换算



                            if ((fltImageWidth / fltImageHeight) > (fltw / flth))
                            {
                                //如果图片的宽高比例大于用户的选择框的宽高比例，那么也就是说得按照图片的宽等比例放大到用户的款。
                                //图片的X坐标也在选择框的Y上。

                                float fltImageRealHeight = fltw * fltImageHeight / fltImageWidth;//图片实际的高，

                                Bitmap bitmap2 = new Bitmap((int)fltw, (int)fltImageRealHeight);

                                Graphics g = Graphics.FromImage(bitmap2);


                                //如下就是画图了
                                g.DrawImage(bitmapTemp, fltx, flty, fltw, fltImageRealHeight);//

                                _image = new Bitmap(bitmap2);

                            }
                            else
                            {
                                //到这里可以算是图片的高等比例到选择框的高
                                float fltImageRealWidth = fltImageWidth * flth / fltImageHeight;

                                Bitmap bitmap2 = new Bitmap((int)fltImageRealWidth, (int)flth);

                                Graphics g = Graphics.FromImage(bitmap2);


                                //如下就是画图了
                                g.DrawImage(bitmapTemp, fltx, flty, fltImageRealWidth, flth);

                                _image = new Bitmap(bitmap2);

                            }
                            
                        }else
                        {
                            _image = bitmapTemp;//如果小于那个尺寸就用原先的啦。
                        }





                    }
                }
                catch (Exception exception)
                {
                    ClsErrorFile.WriteLine(exception);
                    //Console.Error.Write(exception.ToString());
                }

            }
        }

        [DescriptionAttribute("原始图片大小"),DisplayName("原始图片大小"), CategoryAttribute("图片设置")]
        [XmlElement]
        public bool OriginalSize
        {
            get
            {
                return _isOriginalSize;
            }

            set
            {
                _isOriginalSize = value;
            }

        }
        [DescriptionAttribute("按比例缩放"), DisplayName("按比例缩放"), CategoryAttribute("图片设置")]
        [XmlElement]
        public bool Scaled
        {
            get
            {
                return _isScaled;
            }
            set
            {
                _isScaled = value;

                if (_isScaled == true)
                {
                    _isOriginalSize = false;
                }
            }
        }


        public override ShapeEle DeepClone()
        {
            ShapeImage shapeEle = new ShapeImage();
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
            shapeEle.Image = new Bitmap(Image);
            shapeEle.OriginalSize = OriginalSize;
            shapeEle.Scaled = Scaled;

            return shapeEle;
            //throw new NotImplementedException();
        }

        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]//如下是单独序列化保存图片时用的属性
        public string SerializerBitmap
        {
            get
            {
                return ImgToBase64String(_image);
            }
            set
            {
                _image = Base64StringToImage(value);
            }

        }

        private string ImgToBase64String(Bitmap bmp)
        {
            try
            {
                //如下是为了预防GDI一般性错误而深度复制
                Bitmap bmp2 = new Bitmap(bmp);

                MemoryStream ms = new MemoryStream();
                bmp2.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);

                bmp2.Dispose();

                return strbaser64;

            }
            catch (Exception ex)
            {
                //MessageBox.Show("ImgToBase64String 转换失败/nException:" + ex.Message);
                //ClsErrorFile.WriteLine("ImgToBase64String 转换失败/nException:" + ex.Message);
                ClsErrorFile.WriteLine("ImgToBase64String 转换失败/nException:", ex);
                //Console.Error.WriteLine("ImgToBase64String 转换失败/nException:" + ex.Message);
                return "";
            }
        }

        //base64编码的文本 转为图片
        private Bitmap Base64StringToImage(string strbaser64)
        {
            try
            {

                byte[] arr = Convert.FromBase64String(strbaser64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();

                return bmp;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Base64StringToImage 转换失败/nException：" + ex.Message);
                //ClsErrorFile.WriteLine("Base64StringToImage 转换失败/nException：" + ex.Message);
                ClsErrorFile.WriteLine("Base64StringToImage 转换失败/nException：" , ex);
                //Console.Error.WriteLine("Base64StringToImage 转换失败/nException：" + ex.Message);
                return new Bitmap(10, 10);
            }
        }

        public override void Draw(Graphics g, ArrayList arrlistMatrix)
        {
            RectangleF rect = getGraphicsPath(arrlistMatrix).GetBounds();
            float fltx = rect.X;
            float flty = rect.Y;
            float fltw = rect.Width;
            float flth = rect.Height;
            
            /**
            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;
             * */

            //如果没有图片，那么就直接跳过了。
            if (_image == null)
                return;

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;
            //如下的这个是偏移些位置


            //如下是先从绘制矩形中的拷贝的，然后再修改
            if (Route != 0)
            {
                PointF pZhongXin = getCentrePoint();
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            //RectangleF rect = getRect();


            if (_isOriginalSize)
            {
                g.DrawImage(_image, new PointF(fltx, flty));
            }
            else if (_isScaled)//按比例缩放
            {
                // 俺比例缩放首先要求出图像的宽和高
                float fltImageWidth = _image.Width;
                float fltImageHeight = _image.Height;//因为只需要比例，所以不需要放大率和分辨率之类的东西换算

                if ((fltImageWidth / fltImageHeight) > (fltw / flth))
                {
                    //如果图片的宽高比例大于用户的选择框的宽高比例，那么也就是说得按照图片的宽等比例放大到用户的款。
                    //图片的X坐标也在选择框的Y上。

                    float fltImageRealHeight = fltw * fltImageHeight / fltImageWidth;//图片实际的高，

                    //我想要把图片放在正中的位置,那么图片的X点就是用户的fltx, 图片的Y点计算如下，

                    float fltImageRealY = flty + (flth - fltImageRealHeight) / 2;

                    //如下就是画图了
                    g.DrawImage(_image, fltx, fltImageRealY, fltw, fltImageRealHeight);//

                }
                else
                {
                    //到这里可以算是图片的高等比例到选择框的高
                    float fltImageRealWidth = fltImageWidth * flth / fltImageHeight;

                    //我同样想把这个图片放在正中的位置。
                    float fltImageRealX = fltx + (fltw - fltImageRealWidth) / 2;

                    //如下就是画图了
                    g.DrawImage(_image, fltImageRealX, flty, fltImageRealWidth, flth);

                }

            }
            else//不按比例缩放。
            {
                g.DrawImage(_image, fltx, flty, fltw, flth);

            }


            g.ResetTransform();
            //base.Draw(g, arrlistMatrix);

        }

        public override void Draw(Graphics g, float fltKongX, float fltKongY)
        {
            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;

            //如果没有图片，那么就直接跳过了。
            if (_image == null)
                return;

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;
            //如下的这个是偏移些位置
            g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);

            //如下是先从绘制矩形中的拷贝的，然后再修改
            if (Route != 0)
            {
                PointF pZhongXin = getCentrePoint();
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            RectangleF rect = getRect();

            
            if (_isOriginalSize)
            {
                g.DrawImage(_image, new PointF(fltx, flty));
            }
            else if (_isScaled)//按比例缩放
            {
                // 俺比例缩放首先要求出图像的宽和高
                float fltImageWidth = _image.Width;
                float fltImageHeight = _image.Height;//因为只需要比例，所以不需要放大率和分辨率之类的东西换算

                if ((fltImageWidth / fltImageHeight) > (fltw / flth))
                {
                    //如果图片的宽高比例大于用户的选择框的宽高比例，那么也就是说得按照图片的宽等比例放大到用户的款。
                    //图片的X坐标也在选择框的Y上。

                    float fltImageRealHeight = fltw * fltImageHeight / fltImageWidth;//图片实际的高，

                    //我想要把图片放在正中的位置,那么图片的X点就是用户的fltx, 图片的Y点计算如下，

                    float fltImageRealY = flty + (flth - fltImageRealHeight) / 2;

                    //如下就是画图了
                    g.DrawImage(_image, fltx, fltImageRealY, fltw, fltImageRealHeight);//

                }
                else
                {
                    //到这里可以算是图片的高等比例到选择框的高
                    float fltImageRealWidth = fltImageWidth * flth / fltImageHeight;

                    //我同样想把这个图片放在正中的位置。
                    float fltImageRealX = fltx + (fltw - fltImageRealWidth) / 2;

                    //如下就是画图了
                    g.DrawImage(_image, fltImageRealX, flty, fltImageRealWidth, flth);

                }

            }
            else//不按比例缩放。
            {
                g.DrawImage(_image, fltx, flty, fltw, flth);

            }

            //如下的这个是恢复原先的，负数.
            g.TranslateTransform(-fltKongX, -fltKongY);
            g.ResetTransform();//恢复原先的坐标系。

        }


        public override void Draw(Graphics g)
        {
            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;


            //如果没有图片，那么就直接跳过了。
            if (_image == null)
                return;

            //旋转图形
            if (Route != 0)
            {
                PointF pZhongXin = new PointF(fltx + (fltw) / 2, flty + (flth) / 2);
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            RectangleF rect = getRect();

            //首先判断是否是原始图片大小
            if (_isOriginalSize)
            {
                g.DrawImage(_image, new PointF(fltx,flty));
            }
            else if (_isScaled)//按比例缩放
            {
                // 俺比例缩放首先要求出图像的宽和高
                float fltImageWidth = _image.Width;
                float fltImageHeight = _image.Height;//因为只需要比例，所以不需要放大率和分辨率之类的东西换算

                if ((fltImageWidth / fltImageHeight) > (fltw / flth))
                {
                    //如果图片的宽高比例大于用户的选择框的宽高比例，那么也就是说得按照图片的宽等比例放大到用户的款。
                    //图片的X坐标也在选择框的Y上。

                    float fltImageRealHeight = fltw * fltImageHeight / fltImageWidth;//图片实际的高，
                    
                    //我想要把图片放在正中的位置,那么图片的X点就是用户的fltx, 图片的Y点计算如下，

                    float fltImageRealY = flty + (flth - fltImageRealHeight) / 2;

                    //如下就是画图了
                    g.DrawImage(_image, fltx, fltImageRealY, fltw, fltImageRealHeight);//


                }
                else
                {
                    //到这里可以算是图片的高等比例到选择框的高
                    float fltImageRealWidth = fltImageWidth * flth / fltImageHeight;

                    //我同样想把这个图片放在正中的位置。
                    float fltImageRealX = fltx + (fltw - fltImageRealWidth) / 2;

                    //如下就是画图了
                    g.DrawImage(_image, fltImageRealX, flty, fltImageRealWidth, flth);

                }

            }
            else//不按比例缩放。
            {
                g.DrawImage(_image, fltx, flty, fltw, flth);

            }
            g.ResetTransform();
            //throw new NotImplementedException();
        }

    }

    [Serializable]
    //[ProtoContract]
    public class ShapeArc : ShapeEle
    {
        //这个圆弧是根据椭圆画的，只是添加了两个监督角度。

        private float _startAngle=0f;
        private float _endAngle=90f;

        [DescriptionAttribute("开始角度"), DisplayName("开始角度"),CategoryAttribute("布局")]
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
        [DescriptionAttribute("结束角度"),DisplayName("结束角度"), CategoryAttribute("布局")]
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
                ClsErrorFile.WriteLine("这里是一个圆弧出现参数错误，异常处理是构造一个默认宽和高都是10，角度为0和90的扇形",ex);
                //throw;
            }
            
            return path;
            //return base.getGraphicsPath();
        }



    }

    //扇形跟圆弧差不多。
    [Serializable]
    //[ProtoContract]
    public class ShapePie : ShapeArc
    {
        public override GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            GraphicsPath path = new GraphicsPath();
            RectangleF rect = getRect();
            try
            {
                path.AddPie(rect.X, rect.Y, rect.Width, rect.Height, StartAngle, EndAngle);
            }
            catch (Exception ex)
            {

                rect.X = _X + _XAdd;
                rect.Y = _Y + _YAdd;
                rect.Width = 10;
                rect.Height = 10;
                path.AddPie(rect.X, rect.Y, rect.Width, rect.Height, StartAngle, EndAngle);
                ClsErrorFile.WriteLine("这里是一个扇形出现参数错误，异常处理是构造一个默认宽和高都是10，角度为0和90的扇形", ex);
                //throw;
                
                //throw;
            }
            
            return path;
            //return base.getGraphicsPath();
        }

    }


    /// <summary>
    /// 这个组合图形实质上就是将几个图形组合起来。但这个类只是保存信息的，操作的部分是归画布的。
    /// 从各方面来将，这个继承自 ShapeSelRect 比较好些。省了拷贝一大堆代码，但这个是不显示边框的，所以多态掉Draw。
    /// </summary>
    [Serializable]
    //[ProtoContract]
    public class ShapeGroup : ShapeEle
    {
        [XmlArray]
        [XmlArrayItem(Type = typeof(ShapeLine)),
        XmlArrayItem(Type = typeof(ShapeRect)),
        XmlArrayItem(Type = typeof(ShapeEllipse)),
        XmlArrayItem(Type = typeof(ShapeArc)),
        XmlArrayItem(Type = typeof(ShapePie)),
        XmlArrayItem(Type = typeof(ShapeImage)),
        XmlArrayItem(Type = typeof(ShapeStateText)),
        XmlArrayItem(Type = typeof(shapeVarText)),
        XmlArrayItem(Type = typeof(ShapeEle)),
        XmlArrayItem(Type = typeof(ShapeBarcode)),
        XmlArrayItem(Type = typeof(ShapeRoundRect)),
        XmlArrayItem(Type = typeof(ShapeGroup)),
        XmlArrayItem(Type = typeof(shapeSingleText)),
        XmlArrayItem(Type = typeof(shapeMultiText))

        ]
        public ArrayList arrlistShapeEle = new ArrayList();


        public ShapeGroup()
        {

        }

        /**如下会出现错误，因为在群组中，也是有旋转的
        public override bool isContains(PointF MousePoint)
        {
            if (Count() > 0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    if (item.isContains(MousePoint))
                    {
                        return true;//只要有一个返回真就可以了。
                    }
                }

            }

            return false;
            //return base.isContains(MousePoint);
        }
         * */
        public override GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            GraphicsPath path = new GraphicsPath();
            if (Count() > 0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    try
                    {
                        path.AddPath(item.getGraphicsPath(), true);//这个群组不便宜旋转的是内部旋转和偏移的。 
                    }
                    catch (Exception ex)
                    {
                        ClsErrorFile.WriteLine("这个不明原因",ex);
                        //throw;
                    }
                    
                }

            }
            return path;
            //return base.getGraphicsPathNoOffsetRoute();
        }



        public ShapeGroup(ArrayList arrlist)
        {
            foreach (ShapeEle item in arrlist)
            {
                arrlistShapeEle.Add(item);

            }
            SetSelRectXYWH();
        }

        public override bool updateVarValue(ArrayList arrlistKeyValue)
        {
            bool isS = false;
            if (Count()>0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    if (item.updateVarValue(arrlistKeyValue))
                    {
                        isS = true;
                    }
                    
                }
                
            }
            return isS;
            //return base.updateVarValue(arrlistKeyValue);
        }

        /**
        public override GraphicsPath getGraphicsPath()
        {
            GraphicsPath path = new GraphicsPath();

            if (Count()>0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    try
                    {
                        path.AddPath(item.getGraphicsPath(), false);
                    }
                    catch (Exception ex)
                    {
                        ClsErrorFile.WriteLine(ex);
                        //throw;
                    }
                    
                }

                //做一个变换矩阵加上偏移和旋转
                System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
                m.RotateAt((float)(_route + _routeAdd), getCentrePoint());
                path.Transform(m);//应用变换矩阵
                
            }

            return path;
        }

         * */

        public override void Draw(Graphics g, ArrayList arrlistMatrix)
        {

            if (Count() > 0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    ArrayList arr2 = new ArrayList();

                    for (int i = 0; i < arrlistMatrix.Count; i++)
                    {
                        arr2.Add(((System.Drawing.Drawing2D.Matrix)arrlistMatrix[i]).Clone());
                    }

                    //加上这个群组的旋转
                    System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
                    //g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
                    m.RotateAt((float)(_route + _routeAdd), getCentrePoint());
                    arr2.Add(m);//加上群组的旋转
                    item.Draw(g, arr2);
                }

                //如果这个有填充就填充
                if (isFill)
                {
                    try
                    {
                        GraphicsPath path = getGraphicsPath();
                        path.CloseAllFigures();

                        for (int i = 0; i < arrlistMatrix.Count; i++)
                        {
                            path.Transform(((System.Drawing.Drawing2D.Matrix)arrlistMatrix[i]));
                        }

                        g.FillPath(new SolidBrush(_FillColor), path);

                    }
                    catch (Exception ex)
                    {
                        ClsErrorFile.WriteLine(ex);
                        //throw;
                    }

                }

            }

            //base.Draw(g, arrlistMatrix);
        }

        public override void Draw(Graphics g, float fltKongX, float fltKongY)
        {

            if (Count()>0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {

                    //如下是群组的角度
                    if (Route != 0)
                    {

                        RectangleF rect = getRect();
                        //如下的这个是偏移些位置
                        g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
                        g.TranslateTransform(rect.X + rect.Width / 2, rect.Y + rect.Height / 2, MatrixOrder.Prepend);
                        g.RotateTransform((float)Route);
                        g.TranslateTransform(-rect.X, -rect.Y);
                        g.TranslateTransform(-fltKongX - rect.Width / 2, -fltKongY - rect.Height / 2);
                    }
                    item.Draw(g, fltKongX, fltKongY);
                    //g.ResetTransform();//恢复原先的坐标系。
                }
               
                
            }
            SetSelRectXYWH();//仅仅计算就可以了。

        }

        public override ShapeEle DeepClone()
        {
            ArrayList arrlist = new ArrayList();

            foreach (ShapeEle item in arrlistShapeEle)
            {
                arrlist.Add(item.DeepClone());
            }


            ShapeGroup shapeEle = new ShapeGroup(arrlist);

            return shapeEle;

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 改变
        /// </summary>
        /// <param name="strState"></param>
        /// <param name="startPointf"></param>
        /// <param name="endPointf"></param>
        public override void Redim(string strState, PointF startPointf, PointF endPointf)
        {
            if (Count() > 0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    item.Redim(strState, startPointf, endPointf);

                }

            }
            //base.Redim(strState, startPointf, endPointf);
        }


        /// <summary>
        /// 改变大小
        /// </summary>
        /// <param name="rect"></param>
        public override void Resize(RectangleF rect)
        {
            //首先判断是否已经选择图形

            if (Count() > 0)
            {

                foreach (ShapeEle item in arrlistShapeEle)
                {
                    item.Resize(rect);
                }

            }

            SetSelRectXYWH();
            //base.Resize(rect);
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="pointF"></param>
        public override void Move(PointF pointF)
        {
            //首先判断是否已经选择图形

            if (Count() > 0)
            {

                foreach (ShapeEle item in arrlistShapeEle)
                {
                    item.Move(pointF);
                }

            }

            SetSelRectXYWH();

            //base.Move(pointF);
        }




        /// <summary>
        /// 设置选择框的四周，因为选择的图形不一定一个，所以需要这个设置，已有如果需要添加选择或者删除选择的时候，再一次用这个函数就可以了。
        /// 
        /// </summary>
        /// <param name="arrlistShape"></param>
        public void SetSelRectXYWH()
        {

            RectangleF rect = getGraphicsPath().GetBounds();//取得边框
            _X = rect.X;
            _Y = rect.Y;
            _Width = rect.Width;
            _Height = rect.Height;
            return;

            #region 如下的不需要了，仅仅为了增加破解难度


            //首先初始化
            _X = 0;
            _Y = 0;
            _Width = 0;
            _Height = 0;

            if (Count() > 0)
            {
                ArrayList arrlistX = new ArrayList();
                ArrayList arrlistY = new ArrayList();

                foreach (ShapeEle item in arrlistShapeEle)
                {
                    //将这个所有点的坐标加到这里边去
                    foreach (PointF p1 in item.getRealPoint())
                    {
                        arrlistX.Add(p1.X);
                        arrlistY.Add(p1.Y);
                    }

                    /**
                    arrlistX.Add(item.getRealX() + item.getXAdd());
                    arrlistX.Add(item.getRealX() + item.getXAdd() + item.getRealWidth() + item.getWidthAdd());
                    arrlistY.Add(item.getRealY() + item.getYAdd());
                    arrlistY.Add(item.getRealY() + item.getYAdd() + item.getRealHeight() + item.getHeightAdd());
                     * */
                }

                arrlistX.Sort();
                arrlistY.Sort();

                _X = Convert.ToSingle(arrlistX[0].ToString());
                _Y = Convert.ToSingle(arrlistY[0].ToString());
                _Width = Convert.ToSingle(arrlistX[arrlistX.Count - 1].ToString()) - _X;
                _Height = Convert.ToSingle(arrlistY[arrlistY.Count - 1].ToString()) - _Y;
            }

            #endregion

        }


        public override void ReInit()
        {
            if (arrlistShapeEle != null)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    item.ReInit();

                }

            }
            //他也要移动
            base.ReInit();

            SetSelRectXYWH();
        }

        public void addShapeEle(ShapeEle shapeEle)
        {
            if (arrlistShapeEle != null)
                arrlistShapeEle.Add(shapeEle);

            SetSelRectXYWH();//重新计算
        }

        public void addShapeEles(ArrayList arrlistSel)
        {
            foreach (ShapeEle item in arrlistSel)
            {
                arrlistShapeEle.Add(item);

            }
            SetSelRectXYWH();//重新计算
        }

        public void removeShapeEle(ShapeEle shapeEle)
        {
            if (arrlistShapeEle != null)
                arrlistShapeEle.Remove(shapeEle);

            SetSelRectXYWH();//重新计算
        }

        public void removeShapeEles(ArrayList arrlistSel)
        {
            foreach (ShapeEle item in arrlistSel)
            {
                arrlistShapeEle.Remove(arrlistSel);

            }
            SetSelRectXYWH();//重新计算
        }


        /// <summary>
        /// 这个方法仅仅是返回已经选择的形状的个数
        /// </summary>
        /// <returns></returns>
        /// 
        public int Count()
        {
            if (arrlistShapeEle != null)
                return arrlistShapeEle.Count;
            return 0;
        }

        [XmlElement]
        public override float Zoom
        {
            get
            {
                return base.Zoom;
            }
            set
            {
                //要迭代这个群组内的所有形状。如果这个群组中包含群组，那么这个群组也会迭代的。
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    item.Zoom = value;
                    
                }

                base.Zoom = value;
            }
        }

    }

    

    [Serializable]
    //[ProtoContract]
    public class myFont
    {
        public myFont()
        {
        }
        public string Name;
        public FontStyle Style;//这个可以包括其他几项。
        public float Size;
    }

    /// <summary>
    /// BaseObject类是一个用来继承的抽象类。 
    /// 每一个由此类继承而来的类将自动支持克隆方法。
    /// 该类实现了Icloneable接口，并且每个从该对象继承而来的对象都将同样地
    /// 支持Icloneable接口。 
    /// </summary> 
    public abstract class BaseObject : ICloneable
    {
        /// <summary>    
        /// 克隆对象，并返回一个已克隆对象的引用    
        /// </summary>    
        /// <returns>引用新的克隆对象</returns>     
        public object Clone()
        {
            //首先我们建立指定类型的一个实例         
            object newObject = Activator.CreateInstance(this.GetType());
            //我们取得新的类型实例的字段数组。         
            FieldInfo[] fields = newObject.GetType().GetFields();
            int i = 0;
            foreach (FieldInfo fi in this.GetType().GetFields())
            {
                //我们判断字段是否支持ICloneable接口。             
                Type ICloneType = fi.FieldType.GetInterface("ICloneable", true);
                if (ICloneType != null)
                {
                    //取得对象的Icloneable接口。                 
                    ICloneable IClone = (ICloneable)fi.GetValue(this);
                    //我们使用克隆方法给字段设定新值。                
                    fields[i].SetValue(newObject, IClone.Clone());
                }
                else
                {
                    // 如果该字段部支持Icloneable接口，直接设置即可。                 
                    fields[i].SetValue(newObject, fi.GetValue(this));
                }
                //现在我们检查该对象是否支持IEnumerable接口，如果支持，             
                //我们还需要枚举其所有项并检查他们是否支持IList 或 IDictionary 接口。            
                Type IEnumerableType = fi.FieldType.GetInterface("IEnumerable", true);
                if (IEnumerableType != null)
                {
                    //取得该字段的IEnumerable接口                
                    IEnumerable IEnum = (IEnumerable)fi.GetValue(this);
                    Type IListType = fields[i].FieldType.GetInterface("IList", true);
                    Type IDicType = fields[i].FieldType.GetInterface("IDictionary", true);
                    int j = 0;
                    if (IListType != null)
                    {
                        //取得IList接口。                     
                        IList list = (IList)fields[i].GetValue(newObject);
                        foreach (object obj in IEnum)
                        {
                            //查看当前项是否支持支持ICloneable 接口。                         
                            ICloneType = obj.GetType().GetInterface("ICloneable", true);
                            if (ICloneType != null)
                            {
                                //如果支持ICloneable 接口，			 
                                //我们用它李设置列表中的对象的克隆			 
                                ICloneable clone = (ICloneable)obj;
                                list[j] = clone.Clone();
                            }
                            //注意：如果列表中的项不支持ICloneable接口，那么                      
                            //在克隆列表的项将与原列表对应项相同                      
                            //（只要该类型是引用类型）                        
                            j++;
                        }
                    }
                    else if (IDicType != null)
                    {
                        //取得IDictionary 接口                    
                        IDictionary dic = (IDictionary)fields[i].GetValue(newObject);
                        j = 0;
                        foreach (DictionaryEntry de in IEnum)
                        {
                            //查看当前项是否支持支持ICloneable 接口。                         
                            ICloneType = de.Value.GetType().
                                GetInterface("ICloneable", true);
                            if (ICloneType != null)
                            {
                                ICloneable clone = (ICloneable)de.Value;
                                dic[de.Key] = clone.Clone();
                            }
                            j++;
                        }
                    }
                }
                i++;
            }
            return newObject;
        }
    }

    /// <summary>
    /// 这个是弹出转换的窗体来设置转换的
    /// </summary>
    internal class ClsPropertyGridTransfroms : UITypeEditor
    {
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        /// <summary>
        /// 编辑属性
        /// </summary>
        /// <param name="context">可用于获取附加上下文信息的 ITypeDescriptorContext。 </param>
        /// <param name="provider">IServiceProvider ，此编辑器可用其来获取服务。</param>
        /// <param name="value">要编辑的对象。 </param>
        /// <returns>新的对象值。 如果对象的值尚未更改，则它返回的对象应与传递给它的对象相同。 </returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                FrmTransforms f = new FrmTransforms();
                // your setting here
                edSvc.ShowDialog(f);
            }
            return value;//返回原先的值
        }
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return false;
        }
    }


}
