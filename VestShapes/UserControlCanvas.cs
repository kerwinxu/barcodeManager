using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Xuhengxiao.MyDataStructure;
using System.Drawing.Text;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms.VisualStyles;
//using ProtoBuf;

namespace VestShapes
{
    public partial class UserControlCanvas : UserControl
    {

        private float   startX, startY, endX, endY;//鼠标按下弹起坐标 
        private bool isMouseSx;//按下鼠标的标示
        private bool isKeyShift;//是否按下ctrl键

        private string _strOption;//状态
        private string strState;//

        //仅仅需要如下两个就可以了，一个是全部的图形，一个是被选择的图形
        private ShapeSelRect CurrentSelRect=new ShapeSelRect ();//当前的选择框
        public  Shapes myShapes=new Shapes ();//这个是作为图形的集合来用的
        private ShapeEle WillAddShapeEle;//将要添加的形状

        private float _fltZoom = 1f;

        private float _fltDPIX, _fltDPIY;

        private float _fltOffsetNewX=5f, _fltOffsetNewY=5f;
        private float _fltOffsetOldX = 5f, _fltOffsetOldY = 5f;

        private float _fltRulerWidth = 5f;

        private bool _isNeedSaveOperatingRecord;//需要保存激励，主要用在鼠标弹起操作后判断是否要保存的。，在鼠标移动的过程中设置这个。

        //是否显示网格
        [XmlIgnore]
        public  bool isDrawDridding;

        //网格间隔
        [XmlIgnore]
        public static  float GriddingInterval=1f;

        //是否对其网格
        [XmlIgnore]
        public static bool isAlignGridding;


        public float Zoom
        {
            get
            {
                return _fltZoom;
            }
            set
            {
                _fltZoom = value;
                //最大最小是有限度的，
                if (_fltZoom < 0.01)
                    _fltZoom = 0.01f;
                if (_fltZoom > 100)
                    _fltZoom = 100;
                if (myShapes!=null)
                {
                    myShapes.Zoom = _fltZoom;//设置所有形状的放大
                }
                if (CurrentSelRect!=null)
                {
                    CurrentSelRect.SetSelRectXYWH();//重新计算选择框
                    CurrentSelRect.Zoom = _fltZoom;
                }
                if (_shapeEleRectCtrlXC!=null)
                {
                    _shapeEleRectCtrlXC.Zoom = _fltZoom;
                }

                this.Refresh();

            }
        }

        public string Option
        {
            get
            {
                return _strOption;
            }
            set
            {
                _strOption = value;
                //发送事件
                OptionEventArgs optionChanged = new OptionEventArgs(_strOption);
                onOptionChanged(optionChanged);

                if (_strOption == "select")
                {
                    //到这里肯定选择了形状，所以要发送消息
                    PropertyEventArgs selectShapeEleEventArgs = new PropertyEventArgs(CurrentSelRect.arrlistShapeEle);
                    onObjectSelected(selectShapeEleEventArgs);
                    this.Focus();
                }



            }
        }

        public List<clsKeyValue> arrlistKeyValue
        {
            get
            {
                return myShapes.arrlistKeyValue;
            }
            set
            {
                myShapes.arrlistKeyValue = value;
                this.Refresh();
            }

        }


        //定义事件
        public event OptionChanged optionChanged;
        public event ObjectSelected objectSelected;



        private void onOptionChanged(OptionEventArgs e)
        {
            if (optionChanged != null)
                optionChanged(this, e);
        }

        private void onObjectSelected(PropertyEventArgs e)
        {
            if (objectSelected != null)
                objectSelected(this, e);
            

        }

        #region Events & Delegates (Used by the controls vectShape & toolBox )

        public delegate void OptionChanged(object sender, OptionEventArgs e);

        public delegate void ObjectSelected(object sender, PropertyEventArgs e);

        public class PropertyEventArgs : EventArgs
        {
            public object [] arrShapeEleSelect;
            //Constructor.
            public PropertyEventArgs(ArrayList arrlistSel)
            {
                this.arrShapeEleSelect = arrlistSel.ToArray();
            }
        }

        public class OptionEventArgs : EventArgs
        {
            public string option;

            //Constructor.
            //
            public OptionEventArgs(string s)
            {
                this.option = s;
            }
        }

        #endregion



        //虚线选择框的画笔
        Pen penXuxian = new Pen(Color.Black,0.5f);
        
   

        /// <summary>
        /// 这个方法仅仅是将要添加而已，还没有绘制
        /// </summary>
        /// <param name="shapeEle"></param>
        public void addShapeEle(ShapeEle shapeEle)
        {
            WillAddShapeEle = shapeEle;
            WillAddShapeEle.Zoom = _fltZoom;
            Option = "WillAddShape";
        }

        public UserControlCanvas()
        {
            InitializeComponent();
            
            //如下的这个会增强双缓冲的效果。
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.AllPaintingInWmPaint, true);

            penXuxian.DashStyle = DashStyle.Dot;

            Option = "drawRect";//一开始都是虚线的选择

            //设置DPI
            Graphics g = this.CreateGraphics();
            _fltDPIX = g.DpiX;
            _fltDPIY = g.DpiY;

            myShapes = new Shapes();

            saveOperatingRecord();//最开始的状态

            LanguageEncoding.Init();
           
        }

        private void UserControl1_MouseMove(object sender, MouseEventArgs e)
        {
            //因为有时候是从右下角往左上角画或者其他画法，所以这里要判断是否是这样的，如果是，其实，解决方法就是左上角就是最小的，右下角就是最大的。
            //如上的判断净证实不可以，因为不可以减少。

            endX = e.X/_fltDPIX*25.4f-_fltOffsetOldX;
            endY = e.Y/_fltDPIY*25.4f-_fltOffsetOldY;


            //如果对齐的话，这个就要取整数
            if (isAlignGridding)
            {
                endX = (float)Math.Round((endX) / GriddingInterval / Zoom, 0) * GriddingInterval * Zoom;
                endY = (float)Math.Round((endY) / GriddingInterval / Zoom, 0) * GriddingInterval * Zoom;
            }


            if (isMouseSx&&(e.Button==MouseButtons.Left))
            {
                switch (Option)
                {
                    case "select":
                        if (CurrentSelRect != null)
                        {
                            CurrentSelRect.Redim(strState, new PointF(startX, startY), new PointF(endX, endY));
                            onObjectSelected(new PropertyEventArgs(CurrentSelRect.arrlistShapeEle));
                            _isNeedSaveOperatingRecord = true;//要保存的这个。
                        }
                        break;
                    case "Hand":
                        //_fltOffsetNewX = _fltOffsetOldX + (e.X - startMouseX) / 96 * 25.4f;
                       // _fltOffsetNewY = _fltOffsetOldY + (e.Y - startMouseY) / 96 * 25.4f ;

                        _fltOffsetNewX = _fltOffsetOldX + endX-startX;
                        _fltOffsetNewY = _fltOffsetOldY + endY-startY ;

                        break;

                    case "Zoom":

                        break;
                    default:
                        break;
                }


            }

            myRefresh();
        }

        private void DrawGridding(Graphics g)
        {
            float fx = -(GriddingInterval - _fltOffsetNewX / Zoom % GriddingInterval);
            float fy = -(GriddingInterval - _fltOffsetNewY / Zoom % GriddingInterval);

            //这个网点就是一个实心圆.
            float fltRadius = 0.1f;

            float fltRealDius = fltRadius * Zoom;

            if (fltRealDius > 0.5)
                fltRealDius = 0.5f;

            if ((isDrawDridding) && (UserControlCanvas.GriddingInterval > 0))
            {
                for (float  i = 0; i < this.Width / 96f * 25.4 / GriddingInterval /Zoom+2; i++)
                {
                    for (float  j = 0; j < this.Height / 96f * 25.4 /GriddingInterval / Zoom+2; j++)
                    {
                        RectangleF rect = new RectangleF((fx + i * GriddingInterval) * Zoom - fltRealDius, (fy + j * GriddingInterval) * Zoom - fltRealDius, fltRealDius * 2, fltRealDius * 2 );

                        g.FillEllipse(new SolidBrush(Color.Black), rect);
                        //g.DrawEllipse(new Pen(Color.Black), rect); 

                        //这个网格只是填充一个1个小圆
                        //g.FillEllipse(new SolidBrush(Color.Black), i * (UserControlCanvas.GriddingInterval * Zoom+fx), j * (UserControlCanvas.GriddingInterval * Zoom+fy), 0.5f, 0.5f);
                    }

                }
            }

        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            #region 创建双缓冲区等设置。
            //内存上创建Graphics对象：
            Rectangle rect = e.ClipRectangle;//取得绘制区域的矩形
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer = currentContext.Allocate(e.Graphics, e.ClipRectangle);
            Graphics g = myBuffer.Graphics;
            //如上这部分是取得双缓冲的标配了。


            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            g.Clear(Color.Pink);
            g.PageUnit = GraphicsUnit.Millimeter;//将毫米设置为度量单位
            #endregion

            # region 初始化转换矩阵
            //我的那些矩阵转换等数据都是保存在  中的。
            List<Matrix> listTmp = new List<Matrix>();
            System.Drawing.Drawing2D.Matrix m = new Matrix();
            m.Translate(_fltOffsetNewX, _fltOffsetNewY);
            listTmp.Add(m);
            #endregion

            #region  绘制图形
            // 绘制形状图片用如下的这个，好处是可以有偏移。
            try
            {
                myShapes.fltCanvasWidth = this.Width;
                myShapes.fltCanvasHeight = this.Height;

                myShapes.Draw(g, listTmp);
            }
            catch (System.Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);
            }
            #endregion

            //绘制网格
            DrawGridding(g);



            #region  绘制将要新建的图形，就是你那个正在画的图形啦。
            //
            if ((Option == "WillAddShape") && isMouseSx && (WillAddShapeEle != null)) 
            {
   
                    WillAddShapeEle.ShapeInit(new PointF(startX, startY), new PointF(endX, endY));

                    WillAddShapeEle.Draw(g, listTmp);
            }
            #endregion

            #region 绘制当前选择图形外边的边框
            //判断是否有当前选择的图形
            if (CurrentSelRect != null)
            {
                CurrentSelRect.SetSelRectXYWH();
                CurrentSelRect.Draw(g, listTmp);
            }


            //g.TranslateTransform(-_fltOffsetNewX, -_fltOffsetNewY);
            g.ResetTransform();


            g.TranslateTransform(_fltOffsetNewX, _fltOffsetNewY, MatrixOrder.Prepend);

            //如下是的是画一个虚线选择框,画图形的时候也要画这个选择框
            if (((Option == "drawRect")||(Option == "WillAddShape")) && isMouseSx)
            {
                RectangleF rectf = ShapeEle.getXYWH(startX, startY, endX, endY);
                g.DrawRectangle(penXuxian, rectf.X, rectf.Y, rectf.Width, rectf.Height);

            }

            g.TranslateTransform(-_fltOffsetNewX, -_fltOffsetNewY);
            g.ResetTransform();

            #endregion

            #region 如下是绘制刻度尺

            //如下是绘制水平刻度
            //首先要填充出一块空白
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, _fltRulerWidth, this.Height);

            ChuizhiKuDu(g, 1, _fltRulerWidth, 1);
            ChuizhiKuDu(g, 2, _fltRulerWidth, 2);
            ChuizhiKuDu(g, 5, _fltRulerWidth, 3);
            ChuizhiKuDu(g, 10, _fltRulerWidth, 5);

            ChuizhiKuDuShuzi(g);

            //如下是绘制垂直刻度
            //首先要填充出一块空白
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, this.Width, _fltRulerWidth);

            ShuiPingKuDu(g, 1, _fltRulerWidth, 1);
            ShuiPingKuDu(g, 2, _fltRulerWidth, 2);
            ShuiPingKuDu(g, 5, _fltRulerWidth, 3);
            ShuiPingKuDu(g, 10, _fltRulerWidth, 5);

            shuiPingKuDuShuzi(g);

            //将左上角空白，只是不好看而已。
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, _fltRulerWidth, _fltRulerWidth);
            g.DrawRectangle(new Pen(Color.Black, 0.1f), 0, 0, _fltRulerWidth, _fltRulerWidth);

            #endregion

            #region 刷新双缓冲
            myBuffer.Render(e.Graphics);
            g.Dispose();
            myBuffer.Dispose();//释放资源
            #endregion

        }

        /// <summary>
        /// 绘制标尺
        /// </summary>
        /// <param name="g"></param>
        private void DrawScalePlate(Graphics g)
        {
            //绘制标尺，标尺就是线段

            //下面绘制水平的标尺
            ShuiPingScalePlate(g, 1f, 1f);
            ShuiPingScalePlate(g, 2f, 2f);
            ShuiPingScalePlate(g, 5f, 3f);
            ShuiPingScalePlate(g, 10f, 5f);

            //会煮竖直的标尺
            ShuZhiScalePlate(g, 1f, 1f);
            ShuZhiScalePlate(g, 2f, 2f);
            ShuZhiScalePlate(g, 5f, 3f);
            ShuZhiScalePlate(g, 10f, 5f);

            

        }

        private void ShuiPingScalePlate(Graphics g, float fltDanWei, float fltLength)
        {
            Pen penBiaoChi = new Pen(Color.Black, 0.3f);

            //首先求这个画布需要画多少个
            for (int i = 0; i < this.Width/_fltDPIX*25.4/fltDanWei/Zoom; i++)
            {
                PointF p1 = new PointF(i * fltDanWei * Zoom+_fltOffsetNewX, 0);
                PointF p2 = new PointF(i * fltDanWei * Zoom + _fltOffsetNewX, fltLength);
                g.DrawLine(penBiaoChi, p1, p2);
                
            }
        }
        private void ShuZhiScalePlate(Graphics g, float fltDanWei, float fltLength)
        {
            Pen penBiaoChi = new Pen(Color.Black, 0.3f);

            //首先求这个画布需要画多少个
            for (int i = 0; i < this.Width / _fltDPIX * 25.4 / fltDanWei / Zoom; i++)
            {
                PointF p1 = new PointF(0, i * fltDanWei * Zoom + _fltOffsetNewY);
                PointF p2 = new PointF(fltLength, i * fltDanWei * Zoom + _fltOffsetNewY);
                g.DrawLine(penBiaoChi, p1, p2);

            }
        }


        /// <summary>
        /// 因为有时候画图的时候不一定用左上角往下画，这个方法用来返回
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>


        private void label1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private float startMouseX, startMouseY;



        private void UserControl1_MouseDown(object sender, MouseEventArgs e)
        {
            startMouseX = e.X;
            startMouseY = e.Y;

            //设置按下坐标。
            startX = e.X / _fltDPIX * 25.4f-_fltOffsetOldX;
            startY = e.Y/_fltDPIY*25.4f-_fltOffsetOldY;

            endX = e.X / _fltDPIX * 25.4f - _fltOffsetOldX;
            endY = e.Y / _fltDPIY * 25.4f - _fltOffsetOldY;

            isMouseSx = true;

            //如果对齐的话，这个就要取整数
            if (isAlignGridding)
            {
                startX = (float)Math.Round((startX) / GriddingInterval / Zoom, 0) * GriddingInterval * Zoom;
                startY = (float)Math.Round((startY) / GriddingInterval / Zoom, 0) * GriddingInterval * Zoom;
                endX = (float)Math.Round((endX) / GriddingInterval / Zoom, 0) * GriddingInterval * Zoom;
                endY = (float)Math.Round((endY) / GriddingInterval / Zoom, 0) * GriddingInterval * Zoom;
            }


            /**如下的转移到鼠标单击事件中了
            //鼠标右键
            if (e.Button == MouseButtons.Right)
            {
                switch (Option)
                {
                    case "Zoom":
                        float fltzoomTemp = Zoom /2;
                         _fltOffsetNewX = this.Width / 2 / 96f * 25.4f - (e.X / 96f * 25.4f - _fltOffsetOldX) / Zoom * fltzoomTemp;
                         _fltOffsetNewY = this.Height /2 / 96f * 25.4f - (e.Y / 96f * 25.4f - _fltOffsetOldY) / Zoom * fltzoomTemp;
                        Zoom = fltzoomTemp;
                        return;
                    default:
                        break;
                }

            }
           **/

            if (e.Button == MouseButtons.Left)
            {
                switch (Option)
                {
                    case "WillAddShape":
                        //cancelAllSelect();
                        CurrentSelRect.cancelAllSelect();//注销这个

                        //因为有时间他根本没有移动，所以得需要如下的先初始化。
                        WillAddShapeEle.ShapeInit(new PointF(startX, startY), new PointF(startX+5, startY+5));
                        return;
                        //break;
                    case "select":
                        //首先判断是否在其中的
                        #region
                        strState = "";

                        if (CurrentSelRect.isContains(new PointF(startX, startY)))
                        {

                            //判断这个点是否是有形状的，如果有就除去的

                            ShapeEle se=CurrentSelRect.getSelectShapeEle(new PointF(startX,startY));
                            if ((se != null)&&isKeyShift)//只有选中且是按住shift的前提下才需要这个的
                            {
                                CurrentSelRect.removeShapeEle(se);

                            }
                            else
                            {
                                strState = CurrentSelRect.strOver(new PointF(startX, startY));
                                //以后添加判断上下左右，以便有更改大小功能，现在只是做移动功能。
                                changeCursor();
                            }
                            return;//直接返回
                        }
                        else//如果不在其中，那么标示在外边点击了
                        {
                            //cancelAllSelect();

                        }
#endregion

                         break;

                    case "Zoom":
                        /**
                         float fltzoomTemp = Zoom*2;

                         _fltOffsetNewX = this.Width / 2 / 96f * 25.4f - (e.X / 96f * 25.4f - _fltOffsetOldX) / Zoom * fltzoomTemp;
                         _fltOffsetNewY = this.Height /2 / 96f * 25.4f - (e.Y / 96f * 25.4f - _fltOffsetOldY) / Zoom * fltzoomTemp;

                        Zoom = fltzoomTemp;
                         * 
                         * */
                         return;

                         break;

                    case "Hand":

                         this.Cursor = Cursors.Hand;
                        return ;
                        break;
                        
                    default:




                        /**
                         bool isS2 = false;

                         //判断是否在其中一个的图形内。
                         if ((arrlistShapes != null) && (arrlistShapes.Count > 0))
                         {
                             for (int i = arrlistShapes.Count - 1; i > -1; i--)//从最后的判断
                             {
                                 isS2 = ((ShapeEle)arrlistShapes[i]).isContains(new PointF(e.X, e.Y));
                                 if (isS2)
                                 {
                                     ((ShapeEle)arrlistShapes[i]).isSelect = true;
                                     _strOption = "select";
                                     strState = "move";
                                     arrlistSelShapes.Clear();
                                     arrlistSelShapes.Add(((ShapeEle)arrlistShapes[i]));
                                     CurrentSelRect = new ShapeSelRect(arrlistSelShapes);

                                     break;//这个点只判断最上边的图形就可以了。
                                 }

                             }

                         }
                         * */
                        break;
                }
               

                //运行到这里，就说明状态既不是将要添加图形，也不是已经选择了图形。

                //首先取消所有的选择
                //cancelAllSelect();

                //判断是否在其中一个的图形内。

                ShapeEle se2=myShapes.getSelectShapeEle(new PointF(startX, startY));

                if ( se2!= null)
                {
                    if (!isKeyShift)
                    {
                        cancelAllSelect();
                    }

                    CurrentSelRect.addShapeEle(se2);
                    strState = CurrentSelRect.strOver(new PointF(startX, startY));
                    changeCursor();
                    Option = "select";

                    return;

                }

                Option = "drawRect";

                //将纸张设为
                ArrayList arrlist = new ArrayList();
                arrlist.Add(myShapes.BarcodePageSettings.BarcodePaperLayout);
                PropertyEventArgs background = new PropertyEventArgs(arrlist);
                onObjectSelected(background);  

            }

            
        }

        
 
        /// <summary>
        /// 根据方向更改鼠标
        /// </summary>
        private void changeCursor()
        {
            switch (strState)
            {
                case "move":
                    this.Cursor = Cursors.Hand;//手型鼠标
                    break;
                case "West":
                    this.Cursor = Cursors.SizeWE;
                    break;
                case "East":
                    this.Cursor = Cursors.SizeWE;
                    break;
                case "North":
                    this.Cursor = Cursors.SizeNS;
                    break;
                case "South":
                    this.Cursor = Cursors.SizeNS;
                    break;
                case "NorthEast":
                    this.Cursor = Cursors.SizeNESW;
                    break;
                case "SouthWest":
                    this.Cursor = Cursors.SizeNESW;
                    break;
                case "SouthEast":
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                case "NorthWest":
                    this.Cursor = Cursors.SizeNWSE;
                    break;

                default:
                    break;
            }
                            

        }


        private void UserControl1_MouseUp(object sender, MouseEventArgs e)
        {
            //Rectangle rect = new Rectangle(fltStartMouseX, fltStartMouseY, fltEndMouseX - fltStartMouseX, fltEndMouseY - fltStartMouseY);
            //arrlistShapes.Add(rect);

            /**转移到鼠标单击事件中了
            _fltOffsetOldX = _fltOffsetNewX;
            _fltOffsetOldY = _fltOffsetNewY;
             * */
            if (isMouseSx && (e.Button == MouseButtons.Left))
            {
                switch (Option)
                {
                    case "WillAddShape":

                        if (WillAddShapeEle != null)
                        {
                            cancelAllSelect();//这个是新建，当然要取消原先的
                            myShapes.addShapeEle(WillAddShapeEle);
                            CurrentSelRect.cancelAllSelect();
                            CurrentSelRect.addShapeEle(WillAddShapeEle);
                            Option = "select";

                            _isNeedSaveOperatingRecord = true;//要保存的这个。

                        }
                        break;

                    case "drawRect":

                        if (!isKeyShift)
                        {
                            cancelAllSelect();
                        }

                        //取得被选择的图形
                        ArrayList arrlist = myShapes.getSelectShapeEle(ShapeEle.getXYWH(startX, startY, endX, endY));

                        if (arrlist!=null)
                        {
                            foreach (ShapeEle item in arrlist)
                            {
                                //原先在里边的就是反选
                                if (CurrentSelRect.arrlistShapeEle.Contains(item))
                                {
                                    CurrentSelRect.removeShapeEle(item);
                                }else
                                {
                                    CurrentSelRect.addShapeEle(item);
                                }
                            }

                            if (CurrentSelRect.Count()>0)
                            {
                                Option = "select";
                            }
                        }
                        else
                        {
                            cancelAllSelect();
                        }

                        break;
                    default:
                        break;
                }
            }

            /**
             * 
           if ((Option == "WillAddShape") && isMouseSx )
           {
               if (WillAddShapeEle != null)
               {
                   myShapes.addShapeEle(WillAddShapeEle);
                   CurrentSelRect.cancelAllSelect();
                   CurrentSelRect.addShapeEle(WillAddShapeEle);
                   Option = "select";
                                        
               }

           }
           if (Option == "drawRect")
           {

               if ((myShapes.getSelectShapeEle(ShapeEle.getXYWH(startX, startY, endX, endY))) != null)
               {
                   CurrentSelRect.addShapeEles(myShapes.getSelectShapeEle(ShapeEle.getXYWH(startX, startY, endX, endY)));
                   Option = "select";

                   //发出选择图形的消息

               }

               
                
               //首先是判断是否有选择到其中的。
               foreach (ShapeEle item in arrlistShapes)
               {
                   bool isS = item.isContains(ShapeEle.getXYWH(fltStartMouseX, fltStartMouseY, fltEndMouseX , fltEndMouseY));
                   //如果有被选择的，当然首先是更改状态为选择
                   if (isS)
                   {
                       _strOption = "select";
                       //arrlistSelShapes.Add(item);
                       if (CurrentSelRect != null)
                       {
                           CurrentSelRect.addShapeEle(item);
                       }

                   }
                    
               }
                * */

                /**因为我已经
                //判断是否有被选择的
                if ((arrlistSelShapes != null) && (arrlistSelShapes.Count > 0))
                {
                    CurrentSelRect = new ShapeSelRect(arrlistSelShapes);
                }
                

            } 
                 * * */
 

            if (CurrentSelRect.Count() == 0)
            {
                //将纸张设为
                ArrayList arrlist = new ArrayList();
                arrlist.Add(myShapes.BarcodePageSettings.BarcodePaperLayout);
                PropertyEventArgs background = new PropertyEventArgs(arrlist);
                onObjectSelected(background);
            }



            //如下的这个跟如上的判断背景的冲突。
            /**
            if ((Option == "select") && isMouseSx)
            {

                Option = "select";//重新设置就会发出属性该表的消息了。


            }
             * */
             


            //重新初始化选择框，其实不管是什么情况，都需要重新初始化。
            CurrentSelRect.ReInit();

            isMouseSx = false;

            this.Refresh();

            //我做省事，每次鼠标操作都保存一次
            if (_isNeedSaveOperatingRecord)
            {
                saveOperatingRecord();
            }


            this.Cursor = Cursors.Default;//恢复默认鼠标


        }

        private void cancelAllSelect()
        {
            /**
            if (arrlistShapes != null)
            {
                foreach (ShapeEle item in arrlistShapes)
                {
                    item.isSelect = false;
                    
                }
            }
             * */

            //arrlistSelShapes.Clear();

            CurrentSelRect.cancelAllSelect();//注销这个

            strState = "";

        }


        public void setArrKeyValue(List<clsKeyValue> arrlistKeyValue)
        {
            if ((arrlistKeyValue != null) && (arrlistKeyValue.Count > 0))
            {
                myShapes.arrlistKeyValue = arrlistKeyValue;

                //设置属性面板中变量下拉框信息。
                //因为是不定长的，所以需要用ArrayList，然后再转换成字符串数组
                List<string> arrlistVarName = new List<string>();

                foreach (var item in arrlistKeyValue)
                {
                    arrlistVarName.Add(item.Key);

                }
                //转换成字符串数组并赋值给
                VarNameDetails.arrVarName = arrlistVarName.ToArray();


            }

        }


        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="strFileName"></param>
        public string  Loader(String strFileName)
        {
         
            try
            {
                using (Stream stream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {

                    XmlSerializer   formatter = new XmlSerializer (typeof (Shapes));

                    myShapes = formatter.Deserialize(stream) as Shapes ;

                    myShapes.BarcodePageSettings.BarcodePaperLayout.Compute();

                    Zoom = myShapes.Zoom;//提取放大率，很重要。因为有时候保存的放大倍数不为1，则提取的时候是按照1的。

                    initOperatingRecord();

                    //并且要设置背景为已选择对象
                    //将纸张设为
                    ArrayList arrlist = new ArrayList();
                    arrlist.Add(myShapes.BarcodePageSettings.BarcodePaperLayout);
                    PropertyEventArgs background = new PropertyEventArgs(arrlist);
                    onObjectSelected(background);

                    _isNeedSave=false;// 刚开始是不需要保存的。
                }

            }
            catch (Exception exception)
            {
                //ClsErrorFile.WriteLine("加载不成功，原因是" , exception);
                //MessageBox.Show("加载不成功，原因是" + exception.Message);
                return "";
            }
            finally
            {

            }

            CurrentSelRect = new ShapeSelRect();// 重置选择框
            this.Refresh();

            return strFileName;

        }

        public string Loader(Shapes shapes)
        {
            myShapes = shapes;
            
            //如下还得加上若干清空的
            CurrentSelRect.cancelAllSelect();

            initOperatingRecord();

            _isNeedSave=false;//刚开始是不需要保存的

            //并且要设置背景为已选择对象
            //将纸张设为
            ArrayList arrlist = new ArrayList();
            arrlist.Add(myShapes.BarcodePageSettings.BarcodePaperLayout);
            PropertyEventArgs background = new PropertyEventArgs(arrlist);
            onObjectSelected(background);


            return "";

        }

        /// <summary>
        /// 初始化历史记录
        /// </summary>
        private void initOperatingRecord()
        {
            if (arrlistOperatingRecord == null)

                arrlistOperatingRecord = new ArrayList();

            arrlistOperatingRecord.Clear();//清空历史
            intOperatingItem = -1;
            saveOperatingRecord();//重新保存

        }

        /// <summary>
        /// 不带参数的加载，它会调用打开文件。
        /// </summary>
        public string Loader()
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.DefaultExt = "xml";
                openFileDialog1.Title = "加载图形";
                openFileDialog1.Filter = "模板文件 (*.barcode)|*.barcode|All files (*.*)|*.*";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Loader(openFileDialog1.FileName);
                    this.Refresh();
                    return openFileDialog1.FileName;
                }

            }
            catch (Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);
               // Console.Error.WriteLine(ex.Message);
            }

            return "";

        }

        /// <summary>
        /// 保存形状到图片
        /// </summary>
        /// <param name="strFileName"></param>
        public string  Saver(string strFileName)
        {

            //就是保存myShapes这个
            try
            {
                /**
                using (Stream stream = new FileStream(strFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    SoapFormatter  formatter = new SoapFormatter ();
                    formatter.Serialize(stream, myShapes);
                    
                    return strFileName;

                }
                 * */

                using (Stream stream = new FileStream(strFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer xmls = new XmlSerializer(typeof(Shapes));
                    xmls.Serialize(stream, myShapes);

                    _isNeedSave=false;//已经保存了

                    return strFileName;

                }


            }
            catch (Exception exception)
            {
                //ClsErrorFile.WriteLine("保存不成功，原因是" , exception);
                //MessageBox.Show("保存不成功，原因是" + exception.Message);

                if (exception.InnerException != null)
                    ClsErrorFile.WriteLine("模板文件保存不成功"+exception.InnerException.Message);
                    //MessageBox.Show(exception.InnerException.Message);
            }
            finally
            {

            }

            
            //
            this.Refresh();

            return "";//返回空值说明没有保存

        }

        /// <summary>
        /// 不带参数的保存，它会调用保存的对话框
        /// </summary>
        public string  Saver()
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.DefaultExt = "xml";
                saveFileDialog1.Title = "保存图形";
                saveFileDialog1.Filter = "模板文件 (*.barcode)|*.barcode|All files (*.*)|*.*";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (Saver(saveFileDialog1.FileName) != "")
                        return saveFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);
                //Console.Error.WriteLine(ex.Message);
            }

            return "";//返回空值说明没有保存。

        }

        /// <summary>
        /// 这个方法会被打印方法调用，直接在打印方法上绘制条形码。
        /// </summary>
        /// <param name="g"></param>
        public void DrawBarcode(Graphics g)
        {
            float fltzoomOld = Zoom;//保存放大率
            Zoom = 1;//条形码打印需要的是1倍放大率

            //因为打印条形码会有每行有多个条形码，还有其他的间距问题。这里不用考虑页面边距之类的问题，因为这个已经在打印类中考虑了。
            for (int i = 0; i < myShapes.BarcodePageSettings.BarcodePaperLayout.NumberOfColumn; i++)
            {
                for (int j = 0; j < myShapes.BarcodePageSettings.BarcodePaperLayout.NumberOfLine; j++)
                {
                    //我将页面边距加到这里来了
                    float fx=myShapes.BarcodePageSettings.BarcodePaperLayout.Left+i * (myShapes.BarcodePageSettings.BarcodePaperLayout.ModelWidth + myShapes.BarcodePageSettings.BarcodePaperLayout.HorizontalInterval);
                    float fy=myShapes.BarcodePageSettings.BarcodePaperLayout.Top+j*(myShapes.BarcodePageSettings.BarcodePaperLayout.ModelHeight+myShapes.BarcodePageSettings.BarcodePaperLayout.VerticalInterval);
                    DrawShapes(g,fx,fy);
                }
               
            }
            
            Zoom = fltzoomOld;//恢复原先的放大率
        }

        public void DrawShapes(Graphics g, float fltKongX, float fltKongY)
        {
            myShapes.DrawShapes(g, fltKongX, fltKongY);
        }


        //这个方法只是封装了打印形状。
        public void Draw(Graphics g, float KongX, float KongY)
        {
            List<Matrix> listTmp = new List<Matrix>();
            
            System.Drawing.Drawing2D.Matrix m = new Matrix();
            m.Translate(KongX, KongY);

            listTmp.Add(m);

            myShapes.fltCanvasWidth = this.Width;
            myShapes.fltCanvasHeight = this.Height;

            myShapes.Draw(g, listTmp);
        }

        /// <summary>
        /// 删除已经选择的形状
        /// </summary>
        public void deleteSelectShapeEle()
        {
            //迭代删除所有已经选择的
            foreach (ShapeEle item in CurrentSelRect.arrlistShapeEle)
            {
                myShapes.deleteShapeEle(item);
                
            }


            //删除后肯定是没有选择了，所以选择的要重置
            CurrentSelRect = new ShapeSelRect();

            saveOperatingRecord();

            this.Refresh();

        }

        private void UserControlCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            //判断是否是对其网格，就移动网格距离，如果不是对其网格，就只移动0.1毫米，
            //关于连续按键问题，只能是定时器的方式解决。按下后启动定时器，谈起后关闭计时器。
            
            switch (e.KeyData)
            {
                case Keys.Add://直接放大
                    Zoom = Zoom * 2;
                    break;
                case Keys.Subtract://缩小操作。
                    Zoom = Zoom / 2;
                    break;
                case Keys.ShiftKey:
                    isKeyShift = false;//
                    break;
                default:
                    break;
            }



            //每一次的键盘操作都保存一次
            

        }

        /// <summary>
        /// 将选中的形状向前移动一位
        /// </summary>
        public void forward()
        {
            //首先取得选中形状的索引，再在myshapes中删除它，再在索引+1处插入它。        
            if (CurrentSelRect.Count() > 0)
            {
                foreach (ShapeEle item in CurrentSelRect.arrlistShapeEle)
                {
                    int intIndex = myShapes.IndexOf(item);
                    if (intIndex < myShapes.Count() - 1)
                        intIndex++;
                    myShapes.deleteShapeEle(item);

                    myShapes.addShapeEle(item, intIndex);
                }

            }

            this.Refresh();
        }

        /// <summary>
        /// 将选中的移动到最前。
        /// </summary>
        public void forward2()
        {
            ///移动到最前的简便方法是，对选中的形状用两次迭代，第一次在myshapes中删除，再一次重新加入。
            ///

            if (CurrentSelRect.Count() > 0)
            {
                foreach (ShapeEle item in CurrentSelRect.arrlistShapeEle)
                {
                    myShapes.deleteShapeEle(item);
                    myShapes.addShapeEle(item);
                }
                
            }

            this.Refresh();

        }

        /// <summary>
        /// 将选中的后一位
        /// </summary>
        public void backward()
        {
            //首先取得选中形状的索引，再在myshapes中删除它，再在索引-1处插入它。        
            if (CurrentSelRect.Count() > 0)
            {
                CurrentSelRect.arrlistShapeEle.Reverse();//翻转
                foreach (ShapeEle item in CurrentSelRect.arrlistShapeEle)
                {
                    int intIndex = myShapes.IndexOf(item);
                    if (intIndex >0)
                        intIndex--;
                    myShapes.deleteShapeEle(item);
                    myShapes.addShapeEle(item, intIndex);
                }
                CurrentSelRect.arrlistShapeEle.Reverse();//再次翻转即反正了。

            }

            this.Refresh();

        }

        /// <summary>
        /// 将选中的移动到最后
        /// </summary>
        public void backward2()
        {
            //同移动到最前，用两次迭代.
            if (CurrentSelRect.Count() > 0)
            {
                CurrentSelRect.arrlistShapeEle.Reverse();//翻转
                foreach (ShapeEle item in CurrentSelRect.arrlistShapeEle)
                {
                    myShapes.deleteShapeEle(item);
                    myShapes.addShapeEle(item,0);//跟移动到最前的区别是，这个一直是插入到最后的一位。
                }
                CurrentSelRect.arrlistShapeEle.Reverse();//再次翻转即反正了。

            }

            this.Refresh();

        }

        /// <summary>
        /// 这个方法讲已经选择的图形组成一个群组。
        /// </summary>
        public void doGroup()
        {
            //首先生成群组对象
            ShapeGroup group = new ShapeGroup(CurrentSelRect.arrlistShapeEle);

            CurrentSelRect.cancelAllSelect();//取消所有选择

            CurrentSelRect.addShapeEle(group);//再选择这个。

            //如下再在myshapes 中删除那些图形，并加上这一个。

            int intIndex=0;

            foreach (ShapeEle item in group.arrlistShapeEle)
            {
                intIndex = myShapes.IndexOf(item);
                myShapes.deleteShapeEle(item);
                               
            }

            if (intIndex < 0)
                intIndex = 0;

            myShapes.addShapeEle(group, intIndex);

            //将这个作为已选择对象
            CurrentSelRect = new ShapeSelRect();
            CurrentSelRect.addShapeEle(group);
            Option = "select";

            saveOperatingRecord();

        }

        /// <summary>
        /// 解散群组
        /// </summary>
        public void DeGroup()
        {
            //只有选择的只有一个组合的时候才调用
            if ((CurrentSelRect.Count() == 1) && (CurrentSelRect.arrlistShapeEle[0].GetType().Name == "ShapeGroup"))
            {
                int intIndex = myShapes.IndexOf((ShapeEle)CurrentSelRect.arrlistShapeEle[0]);

                //就以这个intIndex将数组解放。

                foreach (ShapeEle item in ((ShapeGroup)CurrentSelRect.arrlistShapeEle[0]).arrlistShapeEle)
                {
                    myShapes.addShapeEle(item, intIndex++);
                    
                }

                myShapes.deleteShapeEle((ShapeGroup)CurrentSelRect.arrlistShapeEle[0]);

                saveOperatingRecord();

            }

        }

        /// <summary>
        /// 所选对象与主对象垂直方向的对齐方式
        /// </summary>
        /// <param name="alignment"></param>
        public void doVerticalAlignWithMainObject(System.Windows.Forms.VisualStyles.VerticalAlignment alignment)
        {
            

            if ((CurrentSelRect != null) && (CurrentSelRect.arrlistShapeEle.Count > 1))
            {
                float fx = ((ShapeEle)CurrentSelRect.arrlistShapeEle[0]).X;
                float fy = ((ShapeEle)CurrentSelRect.arrlistShapeEle[0]).Y;
                float fw = ((ShapeEle)CurrentSelRect.arrlistShapeEle[0]).Width;
                float fh = ((ShapeEle)CurrentSelRect.arrlistShapeEle[0]).Height;
                //主对象是第一个对象
                for (int i = 1; i < CurrentSelRect.arrlistShapeEle.Count; i++)
                {
                    float fx2 = ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).X;
                    float fy2 = ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Y;
                    float fw2 = ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Width;
                    float fh2 = ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Height;
                    switch (alignment)
                    {
                        case VerticalAlignment.Center:
                            ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Y = fy + fh/ 2 - fh2 / 2;
                            break;
                        case VerticalAlignment.Top:
                            ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Y = fy;
                            break;
                        case VerticalAlignment.Bottom:
                            ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Y = fy + fh - fh2;
                            break;
                        default:
                            break;
                    }

                    Refresh();

                }

            }

        }


        /// <summary>
        /// 所选对象与主对象水平方向的对齐方式
        /// </summary>
        /// <param name="alignment"></param>
        public void doHorizontalAlignWithMainObject(System.Windows.Forms.VisualStyles.HorizontalAlign alignment)
        {
            

            if ((CurrentSelRect != null) && (CurrentSelRect.arrlistShapeEle.Count > 1))
            {
                float fx = ((ShapeEle)CurrentSelRect.arrlistShapeEle[0]).X;
                float fy = ((ShapeEle)CurrentSelRect.arrlistShapeEle[0]).Y;
                float fw = ((ShapeEle)CurrentSelRect.arrlistShapeEle[0]).Width;
                float fh = ((ShapeEle)CurrentSelRect.arrlistShapeEle[0]).Height;
                //主对象是第一个对象
                for (int i = 1; i < CurrentSelRect.arrlistShapeEle.Count; i++)
                {
                    float fx2 = ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).X;
                    float fy2 = ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Y;
                    float fw2 = ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Width;
                    float fh2 = ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Height;
                    switch (alignment)
                    {
                        case HorizontalAlign.Center:
                            ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).X = fx+fw/2-fw2/2;
                            break;
                        case HorizontalAlign.Left:
                            ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).X = fx;
                            break;
                        case HorizontalAlign.Right:
                            ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).X = fx + fw - fw2;
                            break;
                        default:
                            break;
                    }

                    Refresh();

                }

            }

        }


       

        /// <summary>
        /// 所选对象与模板水平方向的对齐方式
        /// </summary>
        /// <param name="alignment"></param>
        public void doHorizontalAlignWithModel(System.Windows.Forms.VisualStyles.HorizontalAlign alignment)
        {
            //模板的宽度和高度如下
            float fw=0;
            try
            {
                fw = myShapes.BarcodePageSettings.BarcodePaperLayout.ModelWidth;
            }
            catch (Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);
                
                //throw;
            }
             

            if ((CurrentSelRect != null) && (CurrentSelRect.arrlistShapeEle.Count > 0))
            {

                for (int i = 0; i < CurrentSelRect.arrlistShapeEle.Count; i++)
                {


                    switch (alignment)
                    {
                        case HorizontalAlign.Center:
                            ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).X = fw / 2 - ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Width/2;
                            break;
                        case HorizontalAlign.Left:
                            ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).X = 0;
                            break;
                        case HorizontalAlign.Right:
                            ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).X = fw - ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Width;
                            break;
                        default:
                            break;
                    }

                    Refresh();

                }

            }

        }

        /// <summary>
        /// 所选对象与主对象垂直方向的对齐方式
        /// </summary>
        /// <param name="alignment"></param>
        public void doVerticalAlignWithModel(System.Windows.Forms.VisualStyles.VerticalAlignment alignment)
        {
            //模板的宽度和高度如下
            float fh = 0;
            try
            {
                fh = myShapes.BarcodePageSettings.BarcodePaperLayout.ModelHeight;
            }
            catch (Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);

                //throw;
            }



            if ((CurrentSelRect != null) && (CurrentSelRect.arrlistShapeEle.Count > 0))
            {

                //主对象是第一个对象
                for (int i = 0; i < CurrentSelRect.arrlistShapeEle.Count; i++)
                {

                    switch (alignment)
                    {
                        case VerticalAlignment.Center:
                            ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Y = fh / 2 - ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Height/2;
                            break;
                        case VerticalAlignment.Top:
                            ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Y = 0;
                            break;
                        case VerticalAlignment.Bottom:
                            ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Y = fh - ((ShapeEle)CurrentSelRect.arrlistShapeEle[i]).Height;
                            break;
                        default:
                            break;
                    }

                    Refresh();

                }

            }

        }



       
        private void UserControlCanvas_Resize(object sender, EventArgs e)
        {
            myShapes.fltCanvasWidth = this.Width;
            myShapes.fltCanvasHeight = this.Height;           

        }

        /// <summary>
        /// 放到纸张到屏幕
        /// </summary>
        public void ZoomPaperToScreen()
        {
            try
            {
                _fltOffsetNewX = 5;
                _fltOffsetNewY = 5;
                _fltOffsetOldX = 5;
                _fltOffsetOldY = 5;


                float f1 =(float ) (Math.Round(((this.Width-30) / (myShapes.BarcodePageSettings.BarcodePaperLayout.ModelWidth/25.4*96f)),3));
                float f2 = (float)(Math.Round((this.Height-30) / (myShapes.BarcodePageSettings.BarcodePaperLayout.ModelHeight / 25.4 *96f), 3));

                if (f1 < f2)
                {
                    Zoom = f1;
                }
                else
                {
                    Zoom = f2;
                }

                if (Zoom ==0)
                    Zoom = 1;

                this.Refresh();

            }
            catch (Exception ex)
            {
                //ClsErrorFile.WriteLine("在放到纸张到屏幕中出现异常", ex);
                Zoom = 1;
                
                //Console.Error.WriteLine(ex.Message);
                //throw;
            }


        }

        private void myRefresh()
        {
            this.Refresh();

          

        }
        private void UserControlCanvas_Layout(object sender, LayoutEventArgs e)
        {
        }


        private void ShuiPingKuDu(Graphics g, int intInterval, float  intDi, float  fltLength)
        {
            g.PageUnit = GraphicsUnit.Millimeter;//设置单位

            float f1 = -(intInterval - _fltOffsetNewX / Zoom % intInterval);

            for (int i = 0; i < this.Width / 96f * 25.4f / intInterval / Zoom; i++)
            {
                g.DrawLine(new Pen(Color.Black, 0.1f), new PointF((f1 + i * intInterval) * Zoom, intDi), new PointF((f1 + i * intInterval) * Zoom, intDi - fltLength));

            }

        }

        private void shuiPingKuDuShuzi(Graphics g)
        {
            g.PageUnit = GraphicsUnit.Millimeter;//设置单位

            float f1 = -(10 - _fltOffsetNewX / Zoom % 10);
            //只所以加2是补偿前面算法精度
            for (int i = 0; i < this.Width / 96f * 25.4f / 10f / Zoom+2; i++)
            {
                string str = (Math.Round(f1 + i * 10 - _fltOffsetNewX / Zoom,0)).ToString();

                g.DrawString(str, new Font("Arial",10), new SolidBrush(Color.Black), new PointF((f1 + i * 10) * Zoom - 6, 0));

            }


        }


        private void ChuizhiKuDu(Graphics g, int intInterval, float intDi, int intLength)
        {
            float fy = -(intInterval - _fltOffsetNewY / Zoom % intInterval) ;

            for (int i = 0; i < this.Height / 96f * 25.4f / intInterval / Zoom; i++)
            {
                g.DrawLine(new Pen(Color.Black, 0.1f), new PointF(intDi, (fy + i * intInterval) * Zoom), new PointF(intDi - intLength, (fy + i * intInterval) * Zoom));

            }

        }

        private void ChuizhiKuDuShuzi(Graphics g)
        {
            float fy = -(10 - _fltOffsetNewY / Zoom % 10);

            //只所以加2是补偿前面算法精度

            for (int i = 0; i < this.Height / 96f * 25.4f / 10 / Zoom+2; i++)
            {
                string str = (Math.Round(fy + i * 10 - _fltOffsetNewY / Zoom,0  )).ToString();

                g.DrawString(str, new Font("Arial", 8), new SolidBrush(Color.Black), new PointF(0, (fy + i * 10) * Zoom - 5));

            }


        }

        private void DrawGridding(Graphics g, int intInterval)
        {
            float fx = -(intInterval - _fltOffsetNewX / Zoom % intInterval);
            float fy = -(intInterval - _fltOffsetNewY / Zoom % intInterval);

            //这个网点就是一个实心圆.
            float fltRadius = 0.1f;

            double di = this.Width / 96f * 25.4f / intInterval / Zoom;
            double dj = this.Height / 96f * 25.4f / intInterval / Zoom;

            for (int i = 0; i < di; i++)
            {
                for (int j = 0; j < dj; j++)
                {
                    RectangleF rect = new RectangleF((fx + i * intInterval - fltRadius) * Zoom, (fy + j * intInterval - fltRadius) * Zoom, fltRadius * 2, fltRadius * 2);

                    g.FillEllipse(new SolidBrush(Color.Black), rect);
                    g.DrawEllipse(new Pen(Color.Black), rect);

                }

            }


        }

        public virtual  void UserControlCanvas_Click(object sender, EventArgs e)
        {

            switch (Option)
            {
                case "Zoom":

                    MouseEventArgs mouseEvent = (MouseEventArgs)e;

                    float fltzoomTemp = 1;//默认为1倍

                    if (mouseEvent.Button == MouseButtons.Left)//如果是左键就放大
                    {
                        fltzoomTemp = Zoom * 2;
                    }
                    else if (mouseEvent.Button==MouseButtons.Right)//如果是右键就缩小
                    {
                        fltzoomTemp = Zoom /2;
                    }

                    _fltOffsetNewX = this.Width / 2 / 96f * 25.4f - (mouseEvent.X / 96f * 25.4f - _fltOffsetOldX) / Zoom * fltzoomTemp;
                    _fltOffsetNewY = this.Height / 2 / 96f * 25.4f - (mouseEvent.Y / 96f * 25.4f - _fltOffsetOldY) / Zoom * fltzoomTemp;

                    Zoom = fltzoomTemp;

                    break;

                default:
                    break;
            }

            //这个得更新的。
            _fltOffsetOldX = _fltOffsetNewX;
            _fltOffsetOldY = _fltOffsetNewY;

        }

        public virtual  void UserControlCanvas_DoubleClick(object sender, EventArgs e)
        {
            switch (Option)
            {
                case "Zoom":
                    ZoomPaperToScreen();

                    break;
                default:
                    break;
            }
        }

        //如下是剪切，复制，粘贴部分
        private ShapeSelRect _shapeEleRectCtrlXC;//剪切复制操作拷贝出来的东西
        private string _strOptionCtrlXC;//剪切复制操作的状态

        /// <summary>
        /// 剪切操作
        /// </summary>
        public void CtrlX()
        {

            CtrlC();//调用复制操作

            _strOptionCtrlXC = "CtrlX";//设置状态为剪切

            //再在形状中删除已选择对象
            foreach (ShapeEle item in CurrentSelRect.arrlistShapeEle)
            {
                myShapes.deleteShapeEle(item);
            }

            //因为剪切操作，已经选择对象就被剪切到剪切对象中了，所以剪一选择对象要清空了
            CurrentSelRect.cancelAllSelect();//

            //并且要设置背景为已选择对象
            //将纸张设为
            ArrayList arrlist = new ArrayList();
            arrlist.Add(myShapes.BarcodePageSettings.BarcodePaperLayout);
            PropertyEventArgs background = new PropertyEventArgs(arrlist);
            onObjectSelected(background);

            Option = "";//设置状态为空。

            this.Refresh();//刷新

        }
        /// <summary>
        /// 复制操作,复制操作只是将已选择对象序列化深度拷贝
        /// </summary>
        public void CtrlC()
        {
            _strOptionCtrlXC = "CtrlC";//设置状态为复制

            //CurrentSelRect对象的类是继承自shapeGroup，这个类是可以序列化的
            //如下是用内存流来序列化
            try
            {
                using (MemoryStream memory1 = new MemoryStream())
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ShapeSelRect));

                    xs.Serialize(memory1, CurrentSelRect);//序列化
                    memory1.Seek(0, SeekOrigin.Begin);//移动到开头

                    //反序列化
                    XmlSerializer xs2 = new XmlSerializer(typeof(ShapeSelRect));
                    _shapeEleRectCtrlXC = xs2.Deserialize(memory1) as ShapeSelRect;//这样就深度拷贝到_shapeEleRectCtrlXC中了

                    //Clipboard.SetDataObject(_shapeEleRectCtrlXC);//复制到剪切板中

                    memory1.Close();//销毁

                    //添加自定义对象到剪切板
                    MemoryStream stream = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, _shapeEleRectCtrlXC);
                    Clipboard.SetData(DataFormats.Serializable, stream);
                    Clipboard.SetAudio(stream);

                    //保存记录
                    saveOperatingRecord();
                }
            }
            catch (Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);

                if (ex.InnerException != null)
                    ClsErrorFile.WriteLine(ex.InnerException.Message);
            }

            
        }

        /// <summary>
        /// 粘贴操作
        /// </summary>
        public void CtrlV()
        {


            //粘贴操作是在剪贴和复制后的，首先判断_shapeEleRectCtrlXC是否为空
            try
            {

                //if (_shapeEleRectCtrlXC != null)
                {
                    //首先向右下角移动5毫米
                    _shapeEleRectCtrlXC.Redim("move", new PointF(0, 0), new PointF(5 * Zoom, 5 * Zoom));
                    _shapeEleRectCtrlXC.Zoom = Zoom;//设置放大率
                    _shapeEleRectCtrlXC.ReInit();


                    //从剪切板中取得图形
                    MemoryStream stream = Clipboard.GetAudioStream() as MemoryStream;
                    BinaryFormatter formatter = new BinaryFormatter();
                    CurrentSelRect = formatter.Deserialize(stream) as ShapeSelRect;
                    //首先向右下角移动5毫米
                    CurrentSelRect.Redim("move", new PointF(0, 0), new PointF(5 * Zoom, 5 * Zoom));
                    CurrentSelRect.Zoom = Zoom;//设置放大率
                    CurrentSelRect.ReInit();


                    //粘贴操作首先将这个对象深度拷贝到图形中
                    /**
                    try
                    {
                        using (MemoryStream memory2 = new MemoryStream())
                        {
                            XmlSerializer xs = new XmlSerializer(typeof(ShapeSelRect));

                            xs.Serialize(memory2, _shapeEleRectCtrlXC);//序列化
                            memory2.Seek(0, SeekOrigin.Begin);//移动到开头

                            //反序列化
                            XmlSerializer xs2 = new XmlSerializer(typeof(ShapeSelRect));
                            CurrentSelRect = xs2.Deserialize(memory2) as ShapeSelRect;//这样就深度拷贝到CurrentSelRect中了

                            memory2.Close();//销毁
                        }
                    }
                    catch (Exception ex)
                    {
                        //ClsErrorFile.WriteLine(ex);

                        if (ex.InnerException != null)
                            //ClsErrorFile.WriteLine(ex.InnerException.Message);
                    }

                     * */


                    //然后将这个添加到图形中
                    foreach (ShapeEle item in CurrentSelRect.arrlistShapeEle)
                    {
                        myShapes.addShapeEle(item);
                    }

                    //设置新粘贴出来的对象为已选择
                    PropertyEventArgs background = new PropertyEventArgs(CurrentSelRect.arrlistShapeEle);
                    onObjectSelected(background);
                    Option = "select";


                    switch (_strOptionCtrlXC)
                    {
                        case "CtrlX":
                            //如果是剪切操作，那么_shapeEleRectCtrlXC就没有了
                            _shapeEleRectCtrlXC = null;
                            //Clipboard.Clear();//清空
                            break;
                        case "CtrlC":
                            //如果是复制操作，就再一次执行复制操作，这样子每次都是往右下角移动的。
                            CtrlC();
                            break;

                        default:
                            break;
                    }

                    saveOperatingRecord();

                    //CurrentSelRect.Redim("move", new PointF(0, 0), new PointF(5 * Zoom, 5 * Zoom));

                    this.Refresh();//刷新
                }


            }
            catch (Exception ex2)
            {
                //ClsErrorFile.WriteLine(ex2);
                //throw;
            }
            
        }

        /// <summary>
        /// 撤销操作
        /// </summary>
        public void CtrlZ()
        {

            if (arrlistOperatingRecord.Count>0)
            {
                intOperatingItem--;//后退一位 

                if (intOperatingItem < 0)
                    intOperatingItem = 0;//最低只能0
                myShapes = ((OperatingRecordItem)(arrlistOperatingRecord[intOperatingItem])).shapes;
                Zoom = Zoom;//设置放大率
                CurrentSelRect = ((OperatingRecordItem)(arrlistOperatingRecord[intOperatingItem])).shapeSellectRect;
                if (CurrentSelRect!=null)
                {
                    CurrentSelRect.SetSelRectXYWH();
                    onObjectSelected(new PropertyEventArgs(CurrentSelRect.arrlistShapeEle));
                }

                this.Refresh();
            }

        }
        /// <summary>
        /// 重复操作
        /// </summary>
        public void CtrlY()
        {
            if (arrlistOperatingRecord.Count>0)
            {
                intOperatingItem++;//后退一位 

                if (intOperatingItem > arrlistOperatingRecord.Count - 1)
                    intOperatingItem = arrlistOperatingRecord.Count - 1;//

                myShapes = ((OperatingRecordItem)(arrlistOperatingRecord[intOperatingItem])).shapes;
                Zoom = Zoom;//设置放大率
                CurrentSelRect = ((OperatingRecordItem)(arrlistOperatingRecord[intOperatingItem])).shapeSellectRect;
                if (CurrentSelRect!=null)
                {
                    CurrentSelRect.SetSelRectXYWH();

                    onObjectSelected(new PropertyEventArgs(CurrentSelRect.arrlistShapeEle));
                }

                this.Refresh();
            }
        }

        /// <summary>
        /// 全选操作
        /// </summary>
        public void CtrlA()
        {
            if (myShapes.arrlistShapeEle.Count>0)
            {
                 CurrentSelRect = new ShapeSelRect(myShapes.arrlistShapeEle);
            }
        }

        ArrayList arrlistOperatingRecord=new ArrayList();//记录操作历史的
        int intOperatingItem=-1;//当前选择项
        /// <summary>
        /// 保存操作历史的
        /// </summary>
        public void saveOperatingRecord()
        {
            if (true)
            {
                _isNeedSaveOperatingRecord = false;//设置已经操作了

                //我这个用迭代的方式来深度复制出一个
                OperatingRecordItem operatingRecordItem = new OperatingRecordItem();

                bool isStopWatch = false;//是否显示耗费时间

                Stopwatch sw = new Stopwatch();
                if (isStopWatch)
                {
                    sw.Start();
                }


                //首先深度复制出一个shapes对象来
                try
                {


                    //如下的用序列化平均不到100毫秒，完全可以接受,
                    if (true)
                    {
                        using (MemoryStream memory3 = new MemoryStream())
                        {
                            XmlSerializer xs = new XmlSerializer(typeof(OperatingRecordItem));

                            OperatingRecordItem op1 = new OperatingRecordItem();
                            op1.shapes = myShapes;
                            op1.shapeSellectRect = CurrentSelRect;

                            xs.Serialize(memory3, op1);//序列化
                            memory3.Seek(0, SeekOrigin.Begin);//移动到开头

                            //反序列化
                            XmlSerializer xs2 = new XmlSerializer(typeof(OperatingRecordItem));
                            operatingRecordItem = xs2.Deserialize(memory3) as OperatingRecordItem;//这样就深度拷贝了

                            memory3.Close();//销毁
                        }
                    }

#region 如下的用二进制以及谷歌序列化和DeepClone方法，但不符合要求


                    //而用如下的方式二进制序列化，时间仅仅是1-10毫秒，这个完全能够接受。但出现一个情况，就是图片等引用类型会造成问题
                    if (false)
                    {
                        using (MemoryStream memory3 = new MemoryStream())
                        {
                            BinaryFormatter xs = new BinaryFormatter();

                            OperatingRecordItem op1 = new OperatingRecordItem();
                            op1.shapes = myShapes;
                            op1.shapeSellectRect = CurrentSelRect;


                            xs.Serialize(memory3, op1);//序列化
                            memory3.Seek(0, SeekOrigin.Begin);//移动到开头

                            //反序列化
                            BinaryFormatter xs2 = new BinaryFormatter();

                            operatingRecordItem = xs2.Deserialize(memory3) as OperatingRecordItem;//这样就深度拷贝了

                            memory3.Close();//销毁
                        }
                    }


  
                    //用谷歌上的protobuf，这个最大的问题是，这个序列化不知道序列成什么了
                    /**
                    if (false)
                    {
                        using (MemoryStream memory3 = new MemoryStream())
                        {
                            OperatingRecordItem op1 = new OperatingRecordItem();
                            op1.shapes = myShapes;
                            op1.shapeSellectRect = CurrentSelRect;

                            Serializer.Serialize(memory3, op1);

                            memory3.Seek(0, SeekOrigin.Begin);//移动到开头

                            operatingRecordItem = Serializer.Deserialize<OperatingRecordItem>(memory3);//这样就深度拷贝了

                            memory3.Close();//销毁
                        }


                    }
                     * */


                    //如下方法耗费了我很长时间,但我已经搞定了XML序列化了，这个不需要这么繁琐了，
                    //因为存在我可能添加了属性，而没有修改DeepClone方法
                    if (false)
                    {
                        Shapes s1 = new Shapes();
                        ArrayList arr = new ArrayList();

                        foreach (ShapeEle item in myShapes.arrlistShapeEle)
                        {
                            ShapeEle se1 = (ShapeEle)item.DeepClone();//创建副本

                            s1.addShapeEle(se1);//加到形状类中

                            //如果是被选中的，还要更新到已选择中
                            if (CurrentSelRect.arrlistShapeEle.Contains(item))
                                arr.Add(se1);
                        }
                        operatingRecordItem.shapes = s1;//复制
                        operatingRecordItem.shapeSellectRect = new ShapeSelRect(arr);
                    }
#endregion

                    //接下来就是保存了
                    //如果有撤销操作就把后边的操作清空。
                    if (intOperatingItem < arrlistOperatingRecord.Count - 1)
                    {
                        arrlistOperatingRecord.RemoveRange(intOperatingItem, arrlistOperatingRecord.Count - 1 - intOperatingItem);
                    }

                    arrlistOperatingRecord.Add(operatingRecordItem);
                    intOperatingItem++;//指针加一

                    //默认保存100个记录
                    while (arrlistOperatingRecord.Count > 100)
                    {
                        arrlistOperatingRecord.RemoveAt(0);
                        intOperatingItem--;//指针减1
                    }
                    if (isStopWatch)
                    {
                        MessageBox.Show(sw.ElapsedMilliseconds.ToString());
                    }

                    _isNeedSave = true;
                }
                catch (Exception ex)
                {
                    //ClsErrorFile.WriteLine(ex);

                    if (ex.InnerException != null)
                        ClsErrorFile.WriteLine(ex.InnerException.Message);
                }

            }

        }

        private bool _isNeedSave = false;

        /// <summary>
        /// 判断是否要保存的
        /// </summary>
        /// <returns></returns>
        public bool isNeedSave()
        {
            //判断方法很简单，就是intOperatingItem和元素个数的判断
            return _isNeedSave;

        }



       
        /// <summary>
        /// 这个方法只是启动定时器
        /// </summary>
        private void startTimerLianXuAnJian()
        {
            timerLianXuAnJian.Start();
            timerLianXuAnJian.Interval = 100;//设置时间为0.1秒钟
            this.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


        }

        private void KeyMove(PointF pointf)
        {
            if ((pointf != null) && (CurrentSelRect != null))
            {
                CurrentSelRect.Redim("move", new PointF(0, 0), pointf);
                CurrentSelRect.ReInit();
                //saveOperatingRecord();这个连续按键就不需要保存历史记录了吧
                this.Refresh();
            }

        }



        private void UserControlCanvas_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //设置移动的数据
            PointF pointFKeyMove;
            //判断是否是对其网格，就移动网格距离，如果不是对其网格，就只移动0.1毫米，
            //关于连续按键问题，只能是定时器的方式解决。按下后启动定时器，谈起后关闭计时器。
            float fltMove = 1f;
            if (UserControlCanvas.isAlignGridding)
            {
                fltMove = Zoom * GriddingInterval;
            }
            else
            {
                fltMove = 0.1f * Zoom;
            }


            switch (e.KeyCode)
            {
                case Keys.Up:
                    pointFKeyMove = new PointF(0, -fltMove);
                    KeyMove(pointFKeyMove);
                    break;
                case Keys.Down:
                    pointFKeyMove = new PointF(0, fltMove);
                    KeyMove(pointFKeyMove);
                    break;
                case Keys.Right:
                    pointFKeyMove = new PointF(fltMove, 0);
                    KeyMove(pointFKeyMove);
                    break;
                case Keys.Left:
                    pointFKeyMove = new PointF(-fltMove, 0);
                    KeyMove(pointFKeyMove);
                    break;

                case Keys.Shift:
                    isKeyShift = true;
                    break;
                default:
                    break;
            }



            //每一次的键盘操作都保存一次


        }


        private void UserControlCanvas_KeyDown(object sender, KeyEventArgs e)
        {

            //如下的是判断键盘了
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    isKeyShift = true;
                    break;
                default:
                    break;
            }


            //每一次的键盘操作都保存一次

        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //设置移动的数据
            PointF pointFKeyMove;
            //判断是否是对其网格，就移动网格距离，如果不是对其网格，就只移动0.1毫米，
            //关于连续按键问题，只能是定时器的方式解决。按下后启动定时器，谈起后关闭计时器。
            float fltMove = 1f;
            if (UserControlCanvas.isAlignGridding)
            {
                fltMove = Zoom * GriddingInterval;
            }
            else
            {
                fltMove = 0.1f * Zoom;
            }


            switch (keyData)
            {
                case Keys.Up:
                    pointFKeyMove = new PointF(0, -fltMove);
                    KeyMove(pointFKeyMove);
                    return true;
                    break;
                case Keys.Down:
                    pointFKeyMove = new PointF(0, fltMove);
                    KeyMove(pointFKeyMove);
                    return true;
                    break;
                case Keys.Right:
                    pointFKeyMove = new PointF(fltMove, 0);
                    KeyMove(pointFKeyMove);
                    return true;
                    break;
                case Keys.Left:
                    pointFKeyMove = new PointF(-fltMove, 0);
                    KeyMove(pointFKeyMove);
                    return true;
                    break;

                default:
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        /// <summary>
        /// 这个是闭合图形
        /// </summary>
        public void CloseAllFigures()
        {
            //闭合图形就是判断图形中每个点,隔着最近的一个设置为相同坐标


        }
    }

    /// <summary>
    /// 这个类保存的是操作记录
    /// </summary>
    [Serializable]
    //[ProtoContract]
    public  class OperatingRecordItem
    {
        //如下是保存的项目，一个是所有形状，一个是被选择的形状。
        public Shapes shapes;
        public ShapeSelRect shapeSellectRect;

    }
}
