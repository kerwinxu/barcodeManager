using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Xuhengxiao.MyDataStructure;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using System.Collections;


namespace VestShapes
{
    [Serializable]
    public  class ClsPageSettings
    {
        public string[] arrModelShapes = {"方形","圆角矩形","椭圆形","CD" };

        public string ModelShapes = "圆角矩形";//模板的形状

        /**这个类好像不用放大
        protected float _Zoom;

        public float Zoom
        {
            get
            {
                return _Zoom;
            }
            set
            {
                _Zoom = value;
            }
        }
         * */

        //条形码纸的布局
        [XmlElement]
        public ClsPaperLayout BarcodePaperLayout=new ClsPaperLayout();


        /// <summary>
        /// 如下的这个只是在一个picture上绘制条形码纸张的布局的，只是预览窗口需要这个，其他的都不需要的。
        /// </summary>
        /// <param name="pic"></param>
        public void   DrawModelsBackgroundOnPaper(Graphics g , float fW , float fH)
        {
            //纸张左上角的边距,只是这个不能太靠边而已，没有其他的作用
            float fltBianJuX =0/ 25.4f * g.DpiX;
            float fltBianJuY = 0 / 25.4f * g.DpiY;

            float fwzoom = (fW- 2*fltBianJuX) / (BarcodePaperLayout.PaperWidth / 25.4f * g.DpiX);
            float fhzoom = (fH- 2*fltBianJuY) / (BarcodePaperLayout.PaperHeight / 25.4f * g.DpiY);

            float fzoom = fwzoom > fhzoom ? fhzoom : fwzoom;

            float fltPenWidth = 0.5f / fzoom;//绘制纸张边距的画笔宽度

            try
            {
                g.Clear(Color.Beige);//清空

                //如下被认为可以清晰文字。
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;

                g.PageUnit = GraphicsUnit.Millimeter;//毫米单位


                //绘制纸张的背景
                ShapeRect shapeRect = new ShapeRect();
                shapeRect.X = fltBianJuX;
                shapeRect.Y = fltBianJuY;
                shapeRect.Width = BarcodePaperLayout.PaperWidth;
                shapeRect.Height = BarcodePaperLayout.PaperHeight;
                shapeRect.Zoom = fzoom;
                shapeRect.PenWidth = fltPenWidth;
                shapeRect.Draw(g);

                //如下是绘制各个模板的背景
                for (int i = 0; i < BarcodePaperLayout.NumberOfColumn; i++)
                {
                    for (int j =0; j < BarcodePaperLayout.NumberOfLine; j++)
                    {
                        //计算每个模板的左上角的坐标
                        float fx = BarcodePaperLayout.Left+fltBianJuX +(i)*BarcodePaperLayout.HorizontalInterval+ i * BarcodePaperLayout.ModelWidth;
                        float fy = BarcodePaperLayout.Top+fltBianJuY +(j)*BarcodePaperLayout.VerticalInterval+ j * BarcodePaperLayout.ModelHeight;
                        DrawModelBackground(g, fx, fy, fzoom);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("预览不成功，原因是： "+ex.Message);
                ClsErrorFile.WriteLine("预览不成功，原因是： ",ex);
                //throw;
            }

        }


        public void DrawModelBackground(Graphics g, float fx, float fy, float Zoom, ArrayList arrlistMatrix)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //如下被认为可以清晰文字。
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;


            float fltPenWidth = 0.5f / Zoom;//绘制纸张边距的画笔宽度

            //基本上只是一个调用ShapeEle来绘图的方法

            switch (ModelShapes)
            {
                case "方形":
                    ShapeRect shapeRect = new ShapeRect();
                    shapeRect.X = fx ;
                    shapeRect.Y = fy ;
                    shapeRect.Width = BarcodePaperLayout.ModelWidth ;
                    shapeRect.Height = BarcodePaperLayout.ModelHeight ;
                    shapeRect.Zoom = Zoom;
                    shapeRect.FillColor = Color.White;
                    shapeRect.isFill = true;
                    shapeRect.PenWidth = fltPenWidth;
                    shapeRect.Draw(g,arrlistMatrix);
                    break;
                case "圆角矩形":
                     ShapeRoundRect  shapeRouneRect = new ShapeRoundRect();
                     shapeRouneRect.X = fx ;
                     shapeRouneRect.Y = fy ;
                     shapeRouneRect.Width = BarcodePaperLayout.ModelWidth ;
                     shapeRouneRect.Height = BarcodePaperLayout.ModelHeight ;
                     shapeRouneRect.Zoom = Zoom;
                     shapeRouneRect.CornerRadius = BarcodePaperLayout.CornerRadius;
                     shapeRouneRect.FillColor = Color.White;
                     shapeRouneRect.isFill = true;
                     shapeRouneRect.PenWidth = fltPenWidth;
                     shapeRouneRect.Draw(g,arrlistMatrix);
                    break;
                case "椭圆形":
                    ShapeEllipse shapeEllipse = new ShapeEllipse();
                    shapeEllipse.X = fx ;
                    shapeEllipse.Y = fy ;
                    shapeEllipse.Width = BarcodePaperLayout.ModelWidth ;
                    shapeEllipse.Height = BarcodePaperLayout.ModelHeight ;
                    shapeEllipse.Zoom = Zoom;
                    shapeEllipse.FillColor = Color.White;
                    shapeEllipse.isFill = true;
                    shapeEllipse.PenWidth = fltPenWidth;
                    shapeEllipse.Draw(g,arrlistMatrix);
                    break;
                case "CD":
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 如下的这个方法只是绘制一张模板的大小背景的。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="Zoom"></param>
        public void DrawModelBackground(Graphics g,float fx ,float fy , float Zoom)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //如下被认为可以清晰文字。
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;


            float fltPenWidth = 0.5f / Zoom;//绘制纸张边距的画笔宽度

            //基本上只是一个调用ShapeEle来绘图的方法

            switch (ModelShapes)
            {
                case "方形":
                    ShapeRect shapeRect = new ShapeRect();
                    shapeRect.X = fx ;
                    shapeRect.Y = fy ;
                    shapeRect.Width = BarcodePaperLayout.ModelWidth ;
                    shapeRect.Height = BarcodePaperLayout.ModelHeight ;
                    shapeRect.Zoom = Zoom;
                    shapeRect.FillColor = Color.White;
                    shapeRect.isFill = true;
                    shapeRect.PenWidth = fltPenWidth;
                    shapeRect.Draw(g);
                    break;
                case "圆角矩形":
                     ShapeRoundRect  shapeRouneRect = new ShapeRoundRect();
                     shapeRouneRect.X = fx ;
                     shapeRouneRect.Y = fy ;
                     shapeRouneRect.Width = BarcodePaperLayout.ModelWidth ;
                     shapeRouneRect.Height = BarcodePaperLayout.ModelHeight ;
                     shapeRouneRect.Zoom = Zoom;
                     shapeRouneRect.CornerRadius = BarcodePaperLayout.CornerRadius;
                     shapeRouneRect.FillColor = Color.White;
                     shapeRouneRect.isFill = true;
                     shapeRouneRect.PenWidth = fltPenWidth;
                     shapeRouneRect.Draw(g);
                    break;
                case "椭圆形":
                    ShapeEllipse shapeEllipse = new ShapeEllipse();
                    shapeEllipse.X = fx ;
                    shapeEllipse.Y = fy ;
                    shapeEllipse.Width = BarcodePaperLayout.ModelWidth ;
                    shapeEllipse.Height = BarcodePaperLayout.ModelHeight ;
                    shapeEllipse.Zoom = Zoom;
                    shapeEllipse.FillColor = Color.White;
                    shapeEllipse.isFill = true;
                    shapeEllipse.PenWidth = fltPenWidth;
                    shapeEllipse.Draw(g);
                    break;
                case "CD":
                    break;
                default:
                    break;
            }

        }
    }


    [Serializable]
    ///条形码纸的布局
    public class ClsPaperLayout
    {
        private PaperSize _BarcodePaperSize=new PaperSize ();//保存纸张尺寸的

        private float _fltPaperWidth;
        private float _fltPaperHeight;

        private int _intNumberOfLine=2;//行数
        private int _intNumberOfColumn=2;//列数

        //如下是边距Left, Top, Right, and Bottom
        private float _fltLeft=2f;
        private float _fltTop=2f;
        private float _fltRight=2f;
        private float _fltBottom=2f;

        //模板的宽度和高度
        private float _fltModelWidth;
        private float _fltModelHeight;
        private bool _isCustomModelSize;//自定义模板大小

        //如下是间距
        private  float _fltHorizontalInterval=2f;//水平间距,默认是2毫米
        private  float _fltVerticalInterval;//垂直间距
        private bool _isCustomDistance=true;//自定义间距,默认是可以自定义的。

        //在圆角矩形中还有个是角度的问题。
        protected float _fltCornerRadius = 2f;

        //在CD中还有个是孔的问题

        public string strError;//错误信息，通常是超出边界之类的。


        //如下是改变事件。

        public delegate void PaperLayoutChangedEventHandler(object sender, PaperLayoutChangedEventArgs e);//定义委托
        public event PaperLayoutChangedEventHandler PaperLayoutChanged;// 定义事件
        public virtual void  OnPaperLayoutChanged(PaperLayoutChangedEventArgs e)
        {
            if (PaperLayoutChanged != null)//如果有对象注册，就调用。
                PaperLayoutChanged(this, e);

        }
        //有了如上这个事件，只要有变动就通知。

        //如下是各种属性
        /// <summary>
        /// 纸张大小属性
        /// </summary>
        /// 
        /// 
        [DescriptionAttribute("纸张尺寸"), DisplayName("纸张尺寸"), CategoryAttribute("纸张")]
        [XmlElement]
        public PaperSize BarcodePaperSize
        {
            get
            {
                return _BarcodePaperSize;
            }
            set
            {
                //如果不相等才赋值，且
                //if (! _BarcodePaperSize.Equals(value))
                {
                    _BarcodePaperSize = value;

                    //计算纸张的宽度和高度，以毫米为单位
                    //还得看看是否是横向,横向就是宽和高互换
                    if (LandScape)
                    {
                        _fltPaperHeight = (float)Math.Round(_BarcodePaperSize.Width * 0.254, 0);
                        _fltPaperWidth = (float)Math.Round(_BarcodePaperSize.Height * 0.254, 0);
                    }
                    else
                    {
                        _fltPaperWidth = (float)Math.Round(_BarcodePaperSize.Width * 0.254, 0);
                        _fltPaperHeight = (float)Math.Round(_BarcodePaperSize.Height * 0.254, 0);

                    }

                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }

        }
        /// <summary>
        /// 纸张宽度
        /// </summary>
        [DescriptionAttribute("宽度"), DisplayName("纸张宽"), CategoryAttribute("纸张")]
        [XmlElement]
        public float PaperWidth
        {
            get
            {
                return _fltPaperWidth;
            }

        }

        /// <summary>
        /// 纸张高度
        /// </summary>
        [DescriptionAttribute("纸张高度"), DisplayName("纸张高"), CategoryAttribute("纸张")]
        [XmlElement]
        public float PaperHeight
        {
            get
            {
                return _fltPaperHeight;
            }
        }

        /// <summary>
        /// 行数
        /// </summary>
        [DescriptionAttribute("行数"), DisplayName("行数"), CategoryAttribute("布局")]
        [XmlElement]
        public int NumberOfLine
        {
            get
            {
                return _intNumberOfLine;
            }
            set
            {
                if (_intNumberOfLine != value)
                {
                    _intNumberOfLine = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }

        }
        /// <summary>
        /// 列数
        /// </summary>
        [DescriptionAttribute("列数"), DisplayName("列数"), CategoryAttribute("布局")]
        [XmlElement]
        public int NumberOfColumn
        {
            get
            {
                return _intNumberOfColumn;
            }
            set
            {
                if (_intNumberOfColumn != value)
                {
                    _intNumberOfColumn = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }
        }

        /// <summary>
        /// 边距：上
        /// </summary>
        [DescriptionAttribute("上"), DisplayName("上边距"), CategoryAttribute("边距")]
        [XmlElement]
        public float Top
        {
            get
            {
                return _fltTop;
            }
            set
            {
                if (_fltTop != value)
                {
                    _fltTop = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }

        }
        /// <summary>
        /// 边距：左
        /// </summary>
        [DescriptionAttribute("左"), DisplayName("左边距"), CategoryAttribute("边距")]
        [XmlElement]
        public float Left
        {
            get
            {
                return _fltLeft;
            }
            set
            {
                if (_fltLeft != value)
                {
                    _fltLeft = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }

        }
        /// <summary>
        /// 边距：右
        /// </summary>
        [DescriptionAttribute("右"), DisplayName("右边距"), CategoryAttribute("边距")]
        [XmlElement]
        public float Right
        {
            get
            {
                return _fltRight;
            }
            set
            {
                if (_fltRight != value)
                {
                    _fltRight = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }

        }
        /// <summary>
        /// 边距：下
        /// </summary>
        [DescriptionAttribute("下"), DisplayName("下边距"), CategoryAttribute("边距")]
        [XmlElement]
        public float Bottom
        {
            get
            {
                return _fltBottom;
            }
            set
            {
                if (_fltBottom != value)
                {
                    _fltBottom = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }

        }

        /// <summary>
        /// 模板宽度
        /// </summary>
        [DescriptionAttribute("模板宽度"), DisplayName("模板宽"), CategoryAttribute("模板尺寸")]
        [XmlElement]
        public float ModelWidth
        {
            get
            {
                return _fltModelWidth;
            }
            set
            {
                if (_fltModelWidth != value)
                {
                    _fltModelWidth = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }

        }
        /// <summary>
        /// 模板高度
        /// </summary>
        [DescriptionAttribute("模板高度"), DisplayName("模板高"), CategoryAttribute("模板尺寸")]
        [XmlElement]
        public float ModelHeight
        {
            get
            {
                return _fltModelHeight;
            }
            set
            {
                if (_fltModelHeight != value)
                {
                    _fltModelHeight = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }

        }
        /// <summary>
        /// 自定义模板尺寸
        /// </summary>
        [DescriptionAttribute("是否自定义模板尺寸"), DisplayName("自定义模板尺寸"), CategoryAttribute("模板尺寸")]
        [XmlElement]
        public bool CustomModelSize
        {
            get
            {
                return _isCustomModelSize;
            }
            set
            {
                if (_isCustomModelSize != value)
                {
                    _isCustomModelSize = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }
        }

        /// <summary>
        /// 水平间距
        /// </summary>
        [DescriptionAttribute("水平间距"), DisplayName("水平间距"), CategoryAttribute("间距")]
        [XmlElement]
        public float  HorizontalInterval
        {
            get
            {
                return _fltHorizontalInterval;
            }
            set
            {
                if (_fltHorizontalInterval != value)
                {
                    _fltHorizontalInterval = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }
        }
        /// <summary>
        /// 垂直间距
        /// </summary>
        [DescriptionAttribute("垂直间距"), DisplayName("垂直间距"), CategoryAttribute("间距")]
        [XmlElement]
        public float  VerticalInterval
        {
            get
            {
                return _fltVerticalInterval;
            }
            set
            {
                if (_fltVerticalInterval != value)
                {
                    _fltVerticalInterval = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }
        }
        

        /// <summary>
        /// 圆角矩形的角度
        /// </summary>
        [DescriptionAttribute("圆角矩形的角度"), DisplayName("圆角的角度"),]
        [XmlElement]
        public float CornerRadius
        {
            get
            {
                return _fltCornerRadius;
            }
            set
            {
                if (_fltCornerRadius != value)
                {
                    _fltCornerRadius = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }
        }

        /// <summary>
        /// 自定义间距
        /// </summary>
        [DescriptionAttribute("是否自定义间距"), DisplayName("自定义间距"), CategoryAttribute("间距")]
        [XmlElement]
        public bool CustomDistance
        {
            get
            {
                return _isCustomDistance;
            }
            set
            {
                if (_isCustomDistance != value)
                {
                    _isCustomDistance = value;
                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));
                }
            }

        }

        private bool _LandScape = false;
        /// <summary>
        /// 是否横向
        /// </summary>
        /// 
        [XmlElement]
        public bool LandScape
        {
            get { return _LandScape; }
            set
            {

                    _LandScape = value;

                    //横向后还的直接改变了
                    if (value)
                    {
                        _fltPaperHeight = (float)Math.Round(_BarcodePaperSize.Width * 0.254, 0);
                        _fltPaperWidth = (float)Math.Round(_BarcodePaperSize.Height * 0.254, 0);
                    }
                    else
                    {
                        _fltPaperWidth = (float)Math.Round(_BarcodePaperSize.Width * 0.254, 0);
                        _fltPaperHeight = (float)Math.Round(_BarcodePaperSize.Height * 0.254, 0);

                    }

                    Compute();
                    OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));


            }
        }

        /// <summary>
        /// 设置的话得用这个设置
        /// </summary>
        /// <param name="isLandScape"></param>
        public void TurnLandScape(bool isLandScape)
        {
            //不相同才需要更改
            if (_LandScape!=isLandScape)
            {
                //如下是只要更改横向或者纵向就一样改变的
                _LandScape = isLandScape;

                Swap(ref _fltPaperWidth, ref _fltPaperHeight);//纸张宽和高交换
                Swap(ref _fltHorizontalInterval, ref  _fltVerticalInterval);//水平间距和垂直间距交换
                Swap(ref _fltModelWidth, ref  _fltModelHeight);//模板的宽和高交换

                //行数和列数交换
                int intTemp = _intNumberOfLine;
                _intNumberOfLine = _intNumberOfColumn;
                _intNumberOfColumn = intTemp;

                //如下是更改成横向和纵向不相同的部分
                //如果改成横向.表示原先是纵向
                if (isLandScape == true)
                {
                    //如下的本来要横向是90度还是270度的，因为跟打印库存在循环引用，所以这里暂时设置是270度。解决方面是把这个放在打印库中
                    // 如下是上下左右顺时针旋转270度，也就是逆时针旋转90度。
                    float fTemp = _fltTop;
                    _fltTop = _fltRight;
                    _fltRight = _fltBottom;
                    _fltBottom = _fltLeft;
                    _fltLeft = fTemp;

                    //Compute();

                }
                else
                {
                    //如下是改成纵向，表示是从横向改的.
                    float fTemp = _fltTop;
                    _fltTop = _fltLeft;
                    _fltLeft = _fltBottom;
                    _fltBottom = _fltRight;
                    _fltRight = fTemp;

                }

                OnPaperLayoutChanged(new PaperLayoutChangedEventArgs(this));//触发事件
            }


        }
        
        /// <summary>
        /// 只是交换数据的
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        private void Swap(ref float f1, ref float f2)
        {
            float fTemp = f1;
            f1 = f2;
            f2 = fTemp;
        }



        /// <summary>
        /// 根据输入的数据计算相关属性
        /// </summary>
        public void Compute()
        {
            strError = "";//清空这个错误信息

            //如下是分成四种情况，
            if (CustomModelSize && CustomDistance)
            {
                //都是自定义，那么就直接计算是否超边界就可以了

                //计算实际的宽度，再和纸张宽度做比较
                float fw = Left + Right + (NumberOfColumn - 1) * HorizontalInterval;
                if (fw > PaperWidth)
                    strError = "您设置的纸张左边距，右边距，水平间距，模板宽度加起来已经超出纸张宽度\n";
                //计算实际的高度，再和纸张高度做比较
                float fh = Top + Bottom + (NumberOfLine  - 1) * VerticalInterval;
                if (fh > PaperHeight)
                    strError += "您设置的纸张上边距，下边距，垂直间距，模板高度加起来已经超出纸张高度\n";

            }
            else if (CustomDistance)
            {
                //自定义间距，那就需要求模板尺寸了。
                if (NumberOfColumn >= 1)
                {
                    float fw = (PaperWidth - Left - Right - (NumberOfColumn - 1) * HorizontalInterval) / NumberOfColumn;
                    if (fw <= 0)
                    {
                        strError = "经计算，您设置的参数使得模板宽度小于0\n";
                    }
                    else
                    {
                        _fltModelWidth = fw;
                    }
                }
                else
                {
                    strError = "行数必须大于等于1";
                }
                //得判断列数是否大于0
                if (NumberOfLine >=1)
                {
                    float fh = (PaperHeight - Top - Bottom - (NumberOfLine - 1) * VerticalInterval) / NumberOfLine;
                    if (fh <= 0)
                    {
                        strError = "经计算，您设置的参数使得模板高度小于0\n";
                    }
                    else
                    {
                        _fltModelHeight = fh;
                    }
                }
                else
                {
                    strError = "列数必须大于等于1";
                }
            }
            else if (CustomModelSize)
            {
                //自定义模板尺寸，那就需要求间距了。
                if (NumberOfColumn == 1)
                {
                    //如果行数为1，那么事实上就得判断是否超出边界
                    float fw = (PaperWidth - Left - Right -  ModelWidth);
                    if (fw<0)
                        strError = "经计算，你的左边距+右边距+模板宽度超出了纸张宽度\n，";

                }
                else if (NumberOfColumn > 1)
                {
                    float fw = (PaperWidth - Left - Right - (NumberOfColumn) * ModelWidth) / (NumberOfColumn - 1);
                    if (fw < 0)
                    {
                        strError = "经计算，您设置的参数使得间距小于0\n";
                    }
                    else
                    {
                        _fltHorizontalInterval = fw;
                    }
                }
                else
                {
                    strError = "行数必须大于等于1";
                }

                //得判断列数是否大于0
                if (NumberOfLine == 1)
                {
                    //如果行数为1，那么事实上就得判断是否超出边界
                    float fh = (PaperHeight - Top - Bottom - ModelHeight);
                    if (fh < 0)
                        strError += "经计算，你的上边距+下边距+模板高度超出了纸张高度\n";

                }
                else if (NumberOfLine > 1)
                {
                    float fh = (PaperHeight - Top - Bottom - (NumberOfLine) * ModelHeight) / (NumberOfLine - 1);
                    if (fh < 0)
                    {
                        strError += "经计算，您设置的参数使得模板高度小于0\n";
                    }
                    else
                    {
                        _fltModelHeight = fh;
                    }
                }
                else
                {
                    strError += "列数必须大于等于1";
                }

            }
            else
            {
                //这里是没有选择的，我就假设水平和垂直间距都是2毫米计算。
                _fltHorizontalInterval = 2f;
                _fltVerticalInterval = 2f;

                //如下就是直接拷贝的是自定义水平间距的了
                //自定义间距，那就需要求模板尺寸了。
                if (NumberOfColumn >= 1)
                {
                    float fw = (PaperWidth - Left - Right - (NumberOfColumn - 1) * HorizontalInterval) / NumberOfColumn;
                    if (fw <= 0)
                    {
                        strError = "经计算，您设置的参数使得模板宽度小于0\n";
                    }
                    else
                    {
                        _fltModelWidth = fw;
                    }
                }
                else
                {
                    strError = "行数必须大于等于1";
                }
                //得判断列数是否大于0
                if (NumberOfLine >= 1)
                {
                    float fh = (PaperHeight - Top - Bottom - (NumberOfLine - 1) * VerticalInterval) / NumberOfLine;
                    if (fh <= 0)
                    {
                        strError = "经计算，您设置的参数使得模板高度小于0\n";
                    }
                    else
                    {
                        _fltModelHeight = fh;
                    }
                }
                else
                {
                    strError = "列数必须大于等于1";
                }
            }        

        }

    }

    public class PaperLayoutChangedEventArgs : EventArgs //定义事件参数
    {
        public ClsPaperLayout BarcodePaperLayout;

        public PaperLayoutChangedEventArgs(ClsPaperLayout paperLayout)
        {
            BarcodePaperLayout = paperLayout;
        }
    }


}
