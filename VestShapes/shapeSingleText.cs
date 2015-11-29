using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VestShapes
{

    [Serializable]
    public class shapeSingleText : ShapeEle
    {

        protected string _strPrefix;//前缀
        protected string _strSuffix;//后缀
        protected string _strDefaultText;//文字
        protected string _strAllText;//前缀加上后缀加上变量值，就是一个总的
        protected Font _font = new Font("Arial", 6);
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
        [EditorAttribute(typeof(ClsPropertyGridTransfroms), typeof(System.Drawing.Design.UITypeEditor)), DescriptionAttribute("转换"), DisplayName("转换"), CategoryAttribute("文字")]
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
                g.DrawString(_strAllText, _RealFont, new SolidBrush(_FillColor), new PointF(0, 0), sf);
            }
            catch (System.Exception ex)
            {
                ////ClsErrorFile.WriteLine(ex);
            }

            return bitmap;


        }

        public override void Draw(Graphics g, List<Matrix> listMatrix)
        {
            RectangleF rect = getGraphicsPath(listMatrix).GetBounds();
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


        public override void Redim(string strState, PointF startPointf, PointF endPointf)
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
        protected virtual void UpdateWidthHeight()
        {
            UpdateStrAllText();//首先更新

            //这里自动取得字符串的宽度和高度
            Bitmap bitmap = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bitmap);
            //g.PageUnit = GraphicsUnit.Pixel;//因为我所有的都换成毫米单位了。，这里求出来的宽度和高度是以毫米为单位的。
            SizeF textSizef = g.MeasureString(_strAllText, _font);//因为右边用到宽度高度的时候会乘以倍数，所以这里用这个字体判定



            Width = textSizef.Width / g.DpiX * 25.4f;
            Height = textSizef.Height / g.DpiY * 25.4f;

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
        protected string UpdateStrAllText()
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
        /**
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
                    ////ClsErrorFile.WriteLine(ex);
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
                ////ClsErrorFile.WriteLine(ex);
            }

            //如下的这个是恢复原先的，负数.
            g.TranslateTransform(-fltKongX, -fltKongY);
            g.ResetTransform();//恢复原先的坐标系。

            //base.Draw(g, fltKongX, fltKongY);
        }

         * */

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
                    //ClsErrorFile.WriteLine(ex);
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
                //ClsErrorFile.WriteLine(ex);
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
                //ClsErrorFile.WriteLine(ex);
                //throw;
            }

            g.ResetTransform();

        }
         * */




        public override bool updateVarValue(ArrayList arrlistKeyValue)
        {
            bool isChange = base.updateVarValue(arrlistKeyValue);

            if (isChange)
                UpdateWidthHeight();

            return isChange;

            //base.updateVarValue(arrlistKeyValue);
        }

    }

}
