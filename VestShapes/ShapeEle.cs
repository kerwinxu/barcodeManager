using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Xml.Serialization;
using Xuhengxiao.MyDataStructure;
using System.Collections.Generic;

//using ProtoBuf;

namespace VestShapes
{

    /// <summary>
    /// 定义这个类只是为了XML序列化保存字体的相关信息的。
    /// 其实就3项目
    /// 1、字体的名称
    /// 2、字体的style
    /// 3、字体的大小。
    /// </summary>
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


    [Serializable]
    //[ProtoContract]
    public abstract class ShapeEle
    {
        #region 私有的
        //如下的这4个仅仅是用来绘图时实际的坐标，是经过放大后的坐标。
        protected float _X = 0f, _Y = 0f, _Width = 0f, _Height = 0f;
        protected double _route = 0, _routeAdd = 0;

        //如下的这几个是不能暴露给客户的，所以我隐藏了.
        [XmlIgnore]
        public float _XAdd = 0f, _YAdd = 0f, _WidthAdd = 0f, _HeightAdd = 0f;

        protected float fltJingDu = 3f;//就是隔多远判断是选中。如果已有改成毫米单位，这个也要改的。已经改成1毫米范围内了

        //这个形状有这个是因为有形状要放大的时候，不能用图片放大，因为那牵涉到失真。默认为1
        protected float _Zoom = 1f;

        //填充信息
        protected bool _isFill;//是否填充
        //protected SolidBrush _FillBrush ;//填充都是黑色的填充，不能序列化，所以取消了。
        protected Color _FillColor = Color.Black;


        //画笔信息,画笔颜色都是黑色的
        //protected Pen _myPen ;
        protected Color _penColor = Color.Black;
        protected float _penWidth = 1;
        protected DashStyle _dashStyle = DashStyle.Solid;

        //如下几个属性是为了群组中旋转的
        private bool _isInGroup;

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造杉树
        /// </summary>
        public ShapeEle()
        {
            //_myPen = new Pen(Color.Black, 0.5f);
            //_FillBrush = new SolidBrush(Color.Black);//填充都是黑色的填充
            _FillColor = Color.Black;

        }

        #endregion


        #region 因为我这个软件是可以带入变量的，所以这里有2个属性，一个是变量名，一个是现在的变量值。
        //如下是变量信息
        [XmlIgnore]
        public string _strVarName = "";
        [XmlIgnore]
        public string _strVarValue = "";
        #endregion

        #region 属性

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

        [DescriptionAttribute("宽度"), DisplayName("宽度"), CategoryAttribute("布局")]
        [XmlElement]
        public virtual float Width
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
        public virtual float Height
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
        public int PenColorSerializer
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
                return _penWidth / Zoom;
            }
            set
            {
                _penWidth = value * Zoom;
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

        #endregion


        /// <summary>
        /// 更改字体大小，这个主要是为了放大缩小字体来的。
        /// </summary>
        /// <param name="font"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        protected Font ChangeFontSize(Font font, float fontSize)
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
                    //ClsErrorFile.WriteLine(ex);

                }

            }
            return fontNew;
        }


        //画笔属性
        /// <summary>
        /// 取得真实的路径
        /// </summary>
        /// <returns></returns>
        public virtual PointF[] getRealPoint()
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
        public RectangleF getRect()
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
                rect.Y = rect.Y - rect.Height;
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
            return new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            //return new PointF(_X + _XAdd + (_Width + _WidthAdd) / 2, _Y + _YAdd + (_Height + _HeightAdd) / 2); 

        }

        /// <summary>
        /// 这个是这个类的核心方法，画图，在Graphics绘图，并且加上各种变换。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="listMatrix"></param>
        public virtual void Draw(Graphics g, List<Matrix> listMatrix)
        {

            //单位一定要是MM。
            //在前面已经设置了，这里就不用设置了吧，并且
            //客户自己会选择单位的吧。
            //g.PageUnit = GraphicsUnit.Millimeter;

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            GraphicsPath path = getGraphicsPath(listMatrix);

            //如下这个就是画边界
            try
            {
                g.DrawPath(_myPen, path);
            }
            catch (Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);
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
                    //ClsErrorFile.WriteLine(ex);
                    //throw;
                }

            }

            g.ResetTransform();

        }

        /// <summary>
        /// 这个绘图会加上偏移，这个方法也是可以去掉的。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="fltKongX"></param>
        /// <param name="fltKongY"></param>
        public virtual void Draw(Graphics g, float fltKongX, float fltKongY)
        {

            List<Matrix> listTmp = new List<Matrix>();
            System.Drawing.Drawing2D.Matrix m = new Matrix();
            m.Translate(fltKongX, fltKongY);
            listTmp.Add(m);

            Draw(g, listTmp);

            #region 如下是原先的版本，已经注释掉了。
            /** 
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
                //ClsErrorFile.WriteLine(ex);
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
                    //ClsErrorFile.WriteLine(ex);
                    //throw;
                }

            }
             * */
            #endregion

        }

       

        /// <summary>
        /// 这个是加上所有前面的变换矩阵得到的路径
        /// </summary>
        /// <param name="listMatrix"></param>
        /// <returns></returns>
        public virtual GraphicsPath getGraphicsPath(List<Matrix> listMatrix)
        {
           
            GraphicsPath path = getGraphicsPath();//首先取得没有偏移但有旋转的路径

            //再反转这个个变换
            listMatrix.Reverse();

            if ((listMatrix != null) && (listMatrix.Count > 0))//只有数量大于0才能做如下的
            {
                for (int i = 0; i < listMatrix.Count; i++)
                {
                    path.Transform(listMatrix[i]);

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

            List<Matrix> listTmp = new List<Matrix>();
            System.Drawing.Drawing2D.Matrix m = new Matrix();
            m.Translate(fltKongX, fltKongY);
            listTmp.Add(m);
            return getGraphicsPath(listTmp);

            /** 如下是不调用另一个重载函数的版本。
            GraphicsPath path = getGraphicsPath();//首先取得没有偏移但有旋转的路径
            //做一个变换矩阵加上偏移和旋转
            System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
            //g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
            m.Translate(fltKongX, fltKongY);
            path.Transform(m);//应用变换矩阵
            return path;
             * */
        }

        /// <summary>
        /// 这个取得的是包含旋转,但没有包含偏移的路径
        /// 因为这个旋转是自己的，而偏移是外部的，
        /// </summary>
        /// <returns></returns>
        public virtual GraphicsPath getGraphicsPath()
        {
            GraphicsPath path = getGraphicsPathNoOffsetRoute();
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
        public PointF[] DestortShape(PointF pointJieDian, PointF[] arrPointF, PointF startPointF, PointF endPointF)
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
            return rect.Contains(getGraphicsPath().GetBounds());

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
        protected double GetDistance(PointF p1, PointF p2)
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
            PointF[] pathPointfs = gpath.PathPoints;
            byte[] pathPointType1 = gpath.PathTypes;//获得点的类型

            //线段总是先一个0，再一个1
            for (int i = 0; i < pathPointType1.Length; i++)
            {
                float fltDistance;

                //直线的判定如果这个为1，就判断前一个是否为0
                if ((pathPointType1[i] == 1)//如果这个等于1
                    && (i - 1 >= 0) &&            //并且上一个没有超出边界,实际上这个不太需要
                    (pathPointType1[i - 1] == 0))//并且上一个等于0
                {
                    fltDistance = getPointLineDistance(pathPointfs[i], pathPointfs[i - 1], MousePoint);

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

            Bitmap bitmap = new Bitmap(1000, 1000);
            Graphics g = Graphics.FromImage(bitmap);
            g.PageUnit = GraphicsUnit.Millimeter;

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
        protected PointF[] AngleSort(PointF[] p)
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
        protected float getPointLineDistance(PointF PA, PointF PB, PointF P3)
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
        public virtual bool updateVarValue(ArrayList arrlistKeyValue)
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


}
