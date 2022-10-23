using Io.Github.Kerwinxu.LibShapes.Core.Command;
using Io.Github.Kerwinxu.LibShapes.Core.Event;
using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/**
  这个画布需要对外联系的是
    1. 各种属性
    2. objectSelected ： 选择的对象更改事件。
 * 
 * **/

namespace Io.Github.Kerwinxu.LibShapes.Core
{



    /// <summary>
    /// 画布
    /// </summary>
    public partial class UserControlCanvas : UserControl
    {

        #region 构造函数
        public UserControlCanvas()
        {
            InitializeComponent();
            // 这里设置默认颜色
            GriddingInterval = 2;                    // 默认网格2mm
            commandRecorder = new CommandRecorder(); // 默认的命令记录器
            shapes = new Shapes();                   // 默认的形状
            state = new  State.StateStandby(this);   // 默认是待机状态。
            // 这里先取得dpi
            var g = this.CreateGraphics();
            var _dpix = g.DpiX;
            var _dpiy = g.DpiY;
            // 一堆的事件
            // 如下的鼠标事件，先将坐标转换成毫米了。
            bool isLeftDown = false; // 左键是否按下。移动的时候需要判断是否按下的。
            // todo 加入键盘事件的处理，暂时只是支持上下左右。
            this.MouseDown += (sender, e) => { if (!IsEdit) return ; isLeftDown = true; if(e.Button == MouseButtons.Left) this.state.LeftMouseDown(PointTransform.pixToMM(_dpix, _dpiy, new PointF(e.X,e.Y))); this.Refresh(); };
            this.MouseMove += (sender, e) => { if (!IsEdit) return;  if (!isLeftDown) return; if (e.Button == MouseButtons.Left) this.state.LeftMouseMove(PointTransform.pixToMM(_dpix, _dpiy, new PointF(e.X, e.Y))); this.Refresh(); };
            this.MouseUp += (sender, e) => { if (!IsEdit) return;  isLeftDown = false; if(e.Button == MouseButtons.Left) this.state.LeftMouseUp(PointTransform.pixToMM(_dpix, _dpiy, new PointF(e.X, e.Y))); this.Refresh(); };
            this.MouseClick += (sender, e) => { if (!IsEdit) return;  if (e.Button == MouseButtons.Right) this.state.RightMouseClick(PointTransform.pixToMM(_dpix, _dpiy, new PointF(e.X, e.Y))); this.Refresh(); };
            // 双缓冲，这里需要打开。
            this.DoubleBuffered = true;
            // 说是推荐如上的这个，
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            // 这里有一个特殊的设置，如果偏移为空的话，这里默认的偏移是有这个刻度尺的
            if (shapes.pointTransform == null || 
                (shapes.pointTransform != null && shapes.pointTransform.OffsetX == 0 && shapes.pointTransform.OffsetY == 0)
                )
            {
                // 稍微多留一点出来。
                shapes.pointTransform.OffsetX = scaleWidth * 1.5f;
                shapes.pointTransform.OffsetY = scaleWidth * 1.5f;
            }
        }

        #endregion

        #region 一堆的常量

        private float scaleWidth = 6;   // 刻度尺的宽度，这个是固定的，单位是mm
        private float scaleLineWidth = 0.1f;  // 刻度尺上的刻度线的宽度。
        private Font  scaleTextFont = new Font("Arial", 6); // 刻度尺上文字的字体。
        private float minIntervalpixel = 5;   // 比如网格，如果小于这个，就不打印了。
        private Color scaleBackColor = Color.Orange;
        private float grid_width = 0.25f;                        // 绘制的网格的宽度
        private Brush grid_brush = new SolidBrush(Color.Black);  // 网格的
        private Pen penSelectShape = new Pen(Color.Pink) {      // 选择框的画笔
            DashStyle=DashStyle.Dash,
            Width = 1f
        };

        #endregion

        #region 属性

        // 如下的很多设置成属性是因为我方便从别的地方设置，这个属性其实是有个set方法的。


        /// <summary>
        /// 是否可以编辑
        /// </summary>
        public bool IsEdit { get; set; }

        /// <summary>
        /// 形状
        /// </summary>
        public Shapes shapes;

        /// <summary>
        /// 显示网格
        /// </summary>
        public bool isDrawDridding { get; set; }

        /// <summary>
        /// 网格间隔
        /// </summary>
        public int GriddingInterval { get; set; } // 默认是1，2，5mm

        /// <summary>
        /// 对齐网格，这个在移动和更改尺寸的时候用到。
        /// </summary>
        public bool isAlignDridding { get; set; }

        /// <summary>
        /// 命令记录的
        /// </summary>
        public ICommandRecorder commandRecorder { get; set; }


        /// <summary>
        /// 是否安装了shift键
        /// </summary>
        public bool isShift { get; set; }

        private State.State _state;
        /// <summary>
        /// 现在的状态
        /// </summary>
        public State.State state { get { return _state; } set { _state = value; onStateChanged(); } }

        /// <summary>
        /// 当前选择的图形
        /// </summary>
        public ShapeEle SelectShape { get; set; }

        #endregion

        #region 事件

        #region 选择更改事件


        // 委托
        public delegate void ObjectSelected(object sender, ObjectSelectEventArgs e);
        // 事件
        public event ObjectSelected objectSelected;

        private void onObjectSelected(Object obj)
        {
            // 首先生成参数
            var args = new ObjectSelectEventArgs(obj);
            // 然后看看是否有监听的。
            if (objectSelected != null)
            {
                objectSelected(this, args);
            }
        }

        /// <summary>
        /// 给外部调用的，可以更改当前的选择。
        /// </summary>
        /// <param name="obj"></param>
        public void changeSelect(Object obj)
        {
            SelectShape = obj as ShapeEle;

            if (SelectShape == null)
            {
                // 这里是设置纸张。
                onObjectSelected(shapes.Paper);
            }else
            {
                onObjectSelected(obj);
            }
        }

        #endregion

        #region 状态更改事件

        // 委托
        public delegate void StateChanged(object sender, StateChangedEventArgs e);
        // 事件
        public event StateChanged stateChanged;

        private void onStateChanged()
        {
            // 首先生成参数
            var args = new StateChangedEventArgs(this.state);
            // 然后看看是否有监听的。
            if (stateChanged != null)
            {
                stateChanged(this, args);
            }
        }

        #endregion


        #endregion

        #region 绘图相关

        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlCanvas_Paint(object sender, PaintEventArgs e)
        {
            // 如下的取消了双缓冲，是因为并不能达到预期，反倒是非常闪烁。
            // 最简单的方式是设置属性 this.DoubleBuffered = true;
            // 这里重新绘图用上了双缓冲
            // 1. 首先在内存中建立Graphics对象
            //BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            //BufferedGraphics myBuffer = currentContext.Allocate(e.Graphics, e.ClipRectangle);
            //Graphics g = myBuffer.Graphics;
            // 1. 取得Graphics对象
            Graphics g = e.Graphics;
            // 全局的偏移和缩放,全局不涉及旋转，因为刻度尺。
            var matrix = shapes.GetMatrix();
            // 背景色，默认用这个控件的背景色填充。
            g.Clear(this.BackColor);
            // 2. 形状绘图
            if (this.shapes != null)
            {
                // 绘图，并且用上了偏移和缩放。
                shapes.Draw(g, matrix); 
            }
            // 3. 绘制网格
            if (isDrawDridding)
            {
                drawGrid(g);
            }
            // 4. 绘制选择框
            drawSelectRect(g);
            // 5. 绘制刻度尺
            drawScale(g);
            // 6. 这里绘制虚线，要选择哪些东西的
            if (this.state is State.ShapeRectSelect)
            {
                ((State.ShapeRectSelect)this.state).Draw(g);
            }

            // 7. 刷新双缓冲
            //myBuffer.Render(e.Graphics);
            //g.Dispose();
            //myBuffer.Dispose();//释放资源
        }


        /// <summary>
        /// 绘制网格
        /// </summary>
        /// <param name="g"></param>
        private void drawGrid(Graphics g)
        {
            // 我的思路是，首先判断是否小于最小
            if (GriddingInterval / 25.4 * g.DpiX * shapes.pointTransform.Zoom < minIntervalpixel) return;
            // 如下是计算范围
            PointF p1 = new PointF(0, 0);
            PointF p2 = new PointF(this.Width / g.DpiX * 25.4f, this.Height / g.DpiY * 25.4f);
            var point1 = shapes.pointTransform.CanvasToVirtualPoint(p1); // 左上角画布的坐标
            var point2 = shapes.pointTransform.CanvasToVirtualPoint(p2); // 右下角的坐标
            // 画笔
            Pen pen = new Pen(Color.Black);
            pen.Width = 0.1f; // 画笔的用很细的。
            int x_start = (int)point1.X / GriddingInterval * GriddingInterval;
            int y_start = (int)point1.Y / GriddingInterval * GriddingInterval;
            int i_count = (int)(point2.X - point1.X) / GriddingInterval;
            int j_count = (int)(point2.Y - point1.Y) / GriddingInterval;
            // 然后这里绘制
            for (int i = 0; i < i_count; i++)
            {
                for (int j = 0; j < j_count; j++)
                {
                    // 我这里是将坐标转成画布的坐标
                    var point3 = new PointF(
                        x_start + i * GriddingInterval,
                        y_start + j * GriddingInterval);
                    var point4 = shapes.pointTransform.VirtualToCanvasPoint(point3);
                    // 这里进行绘制，这里实际上是绘制一个圆
                    var point5 = new PointF(point4.X - grid_width/2, point4.Y - grid_width/2);
                    var rect = new RectangleF(point5, new SizeF(grid_width, grid_width));
                    //
                    g.FillEllipse(grid_brush, rect);
                }
            }


        }

        /// <summary>
        /// 绘制选择框
        /// </summary>
        /// <param name="g"></param>
        private void drawSelectRect(Graphics g)
        {
            if(this.SelectShape != null)
            {
                // 如果有图形，就取得边框，这个边框的尺寸控件是画布的尺寸控件，已经做过转换了。
                var rect = SelectShape.GetBounds(shapes.GetMatrix());
                // 然后绘制这个边框就是了。
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(rect);
                g.DrawPath(penSelectShape, path);


            }
        }

        /// <summary>
        /// 绘制刻度。
        /// </summary>
        /// <param name="g"></param>
        private void drawScale(Graphics g)
        {
            // 这个刻度值是在上边和左边的。
            // 1 . 首先绘制刻度的背景
            // 如下是计算范围
            var point1 = shapes.pointTransform.CanvasToVirtualPoint(new PointF(0, 0)); // 左上角虚拟的坐标
            var point2 = shapes.pointTransform.CanvasToVirtualPoint(new PointF(this.Width / g.DpiX * 25.4f, this.Height / g.DpiY * 25.4f)); // 右下角的坐标
            // 如下的是计算这个自定义控件的宽度和高度，请注意，这里不算缩放。
            float w = this.Width / g.DpiX * 25.4f;
            float h = this.Height / g.DpiY * 25.4f ;
            // 控件大小不变，这个如下的就是固定的。
            g.FillRectangle(
                new SolidBrush(this.scaleBackColor),
                new RectangleF(0,0, w, scaleWidth)
                );
            g.FillRectangle(
                new SolidBrush(this.scaleBackColor),
                new RectangleF(0, 0, scaleWidth, h)
                );
            // 2.然后是各个刻度，先水平,后竖直
            Pen pen = new Pen(Color.Black);
            pen.Width = scaleLineWidth;
            // 2.1 水平刻度
            horizontal_scale_line(g, pen, point1.X, point2.X, 1, 0.5f);
            horizontal_scale_line(g, pen, point1.X, point2.X, 2, 1);
            horizontal_scale_line(g, pen, point1.X, point2.X, 5, 2);
            horizontal_scale_line(g, pen, point1.X, point2.X, 10, 3);
            horizontal_scale_text(g, new SolidBrush(Color.Black), point1.X, point2.X);
            // 2.2 垂直刻度
            vertical_scale_line(g, pen, point1.Y, point2.Y, 1, 0.5f);
            vertical_scale_line(g, pen, point1.Y, point2.Y, 2, 1);
            vertical_scale_line(g, pen, point1.Y, point2.Y, 5, 2);
            vertical_scale_line(g, pen, point1.Y, point2.Y, 10, 3);
            vertical_scale_text(g, new SolidBrush(Color.Black), point1.Y, point2.Y);

            // 3. 左上角有重复的，这里直接空白出来
            g.FillRectangle(
                new SolidBrush(this.scaleBackColor),
                new RectangleF(0, 0, scaleWidth, scaleWidth)
                );

        }

        /// <summary>
        /// 水平刻度尺
        /// </summary>
        /// <param name="g"></param>
        /// <param name="start_x">开始的x坐标</param>
        /// <param name="end_x">结束的x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="intInterval">间隔</param>
        /// <param name="line_length">线的长度</param>
        private void horizontal_scale_line(Graphics g, Pen pen, float start_x, float end_x, int intInterval, float line_length)
        {
            // 变量
            int x_start = (int)(start_x-1)/ intInterval * intInterval;
            int x_end = (int)(end_x+1)/ intInterval * intInterval;
            int i_count = (x_end - x_start) / intInterval +1;
            // 这里要计算一下是否太密了。
            if (intInterval / 25.4 * g.DpiX * shapes.pointTransform.Zoom < minIntervalpixel) return;
            if (intInterval / 25.4 * g.DpiY * shapes.pointTransform.Zoom < minIntervalpixel) return;
            // 这里是按照画布的坐标绘制的
            // 其y坐标为y，另外向上有line_length的长度
            // 这个首先算出起始的坐标是多少
            for (int i = 0; i < i_count; i++)
            {
                PointF p1 = new PointF(x_start + i * intInterval, 0);        // 虚拟的坐标。
                // 这里计算在画布中的坐标是多少
                var p3 = shapes.pointTransform.VirtualToCanvasPoint(p1);
                p3.Y = scaleWidth;  // 这个是固定的。
                var p4 = new PointF(p3.X, p3.Y - line_length);
                g.DrawLine(pen, p3, p4);
            }
 
        }

        /// <summary>
        /// 这个是绘制文本的
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="start_x"></param>
        /// <param name="end_x"></param>
        private void horizontal_scale_text(Graphics g, Brush brush, float start_x, float end_x)
        {
            int intInterval = 10;
            int x_start = (int)(start_x - 1) / intInterval * intInterval;
            int x_end = (int)(end_x + 1) / intInterval * intInterval;
            int i_count = (x_end - x_start) / intInterval + 1;
            for (int i = 0; i < i_count; i++)
            {
                // 这里写文字
                int j = x_start + i * intInterval ;
                string _text = (j/ intInterval).ToString(); // 显示的是cm，默认的是毫米
                // 然后求x坐标
                var p1 = new PointF(j, 0);
                var p2 = shapes.pointTransform.VirtualToCanvasPoint(p1);
                p2.Y = 0;// 手动更改成0
                // 这个p2坐标就是文字中线的位置，这里还要往左边移动一些。
                var _size = g.MeasureString(_text, scaleTextFont);
                // 然后这个转成
                p2.X -= _size.Width / 2;
                g.DrawString(_text, scaleTextFont, brush, p2);
            }
        }

        /// <summary>
        /// 竖直的刻度尺
        /// </summary>
        /// <param name="g"></param>
        /// <param name="start_y">开始的x坐标</param>
        /// <param name="end_y">结束的x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="intInterval">间隔</param>
        /// <param name="line_length">线的长度</param>
        private void vertical_scale_line(Graphics g, Pen pen, float start_y, float end_y, int intInterval, float line_length)
        {
            // 变量
            int y_start = (int)(start_y - 1) / intInterval * intInterval;
            int y_end = (int)(end_y + 1) / intInterval * intInterval;
            int i_count = (y_end - y_start) / intInterval + 1;
            // 这里要计算一下是否太密了。
            if (intInterval / 25.4 * g.DpiY * shapes.pointTransform.Zoom < minIntervalpixel) return;
            if (intInterval / 25.4 * g.DpiY * shapes.pointTransform.Zoom < minIntervalpixel) return;
            // 这里是按照画布的坐标绘制的
            // 其y坐标为y，另外向上有line_length的长度
            // 这个首先算出起始的坐标是多少
            for (int i = 0; i < i_count; i++)
            {
                PointF p1 = new PointF(0, y_start + i * intInterval);        // 虚拟的坐标。
                // 这里计算在画布中的坐标是多少
                var p3 = shapes.pointTransform.VirtualToCanvasPoint(p1);
                p3.X = scaleWidth;  // 这个是固定的。
                var p4 = new PointF(p3.X - line_length, p3.Y);
                g.DrawLine(pen, p3, p4);
            }

        }

        /// <summary>
        /// 这个是绘制文本的
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="start_y"></param>
        /// <param name="end_y"></param>
        private void vertical_scale_text(Graphics g, Brush brush, float start_y, float end_y)
        {
            int intInterval = 10;
            int y_start = (int)(start_y - 1) / intInterval * intInterval;
            int y_end = (int)(end_y + 1) / intInterval * intInterval;
            int i_count = (y_end - y_start) / intInterval + 1;
            for (int i = 0; i < i_count; i++)
            {
                // 这里写文字
                int j = y_start + i * intInterval;
                string _text = (j / intInterval).ToString(); // 显示的是cm，默认的是毫米
                // 然后求x坐标
                var p1 = new PointF(0, j);
                var p2 = shapes.pointTransform.VirtualToCanvasPoint(p1);
                p2.X = 0;// 手动更改成0
                // 这个p2坐标就是文字中线的位置，这里还要往左边移动一些。
                var _size = g.MeasureString(_text, scaleTextFont);
                // 然后这个转成
                p2.Y -= _size.Height / 2;
                g.DrawString(_text, scaleTextFont, brush, p2);
            }
        }

        #endregion

        #region 请注意，如下的几个操作都只是处理顶层的

        /// <summary>
        /// 以某个点为中心放大
        /// </summary>
        /// <param name="point"></param>
        public void zoom(PointF point)
        {
            // 1. 取得这个点在虚拟画布上的地址
            var point2 = this.shapes.pointTransform.CanvasToVirtualPoint(point);
            // 2.  更新偏移
            this.shapes.pointTransform.OffsetX -= point2.X * (this.shapes.pointTransform.Zoom );
            this.shapes.pointTransform.OffsetY -= point2.Y * (this.shapes.pointTransform.Zoom );
            // 3 放大
            this.shapes.pointTransform.Zoom *= 2;
            // 4. 刷新
            this.Refresh();

        }

        /// <summary>
        /// 以某个点为中心缩小。
        /// </summary>
        /// <param name="point"></param>
        public void reduce(PointF point)
        {
            // 1. 取得这个点在虚拟画布上的地址
            var point2 = this.shapes.pointTransform.CanvasToVirtualPoint(point);
            // 2.  更新偏移
            this.shapes.pointTransform.OffsetX += point2.X * (this.shapes.pointTransform.Zoom/2);
            this.shapes.pointTransform.OffsetY += point2.Y * (this.shapes.pointTransform.Zoom/2);
            // 3 放大
            this.shapes.pointTransform.Zoom /= 2;
            // 4. 刷新
            this.Refresh();
        }

        /// <summary>
        /// 放大到屏幕
        /// </summary>
        public void zoomToScreen()
        {
            if (shapes != null)
            {
                var g = this.CreateGraphics();
                shapes.zoomTo(
                    g.DpiX, 
                    g.DpiY, 
                    this.Width, 
                    this.Height,
                    scaleWidth/25.4f*g.DpiX* 1.5f    // scaleWidth/25.4f*g.DpiX表示刻度尺实际的像素宽度，
                                                     // 而*1.5，表示两边流出1.5倍数的距离，这样，图形也不会太靠近边界。
                    );
                this.Refresh();
            }
        }


        /// <summary>
        /// 添加一个新的形状
        /// </summary>
        /// <param name="shape"></param>
        public void addShape(ShapeEle shape)
        {
            // 只是在顶层添加。
            shapes.lstShapes.Add(shape);
            changeSelect(shape); // 添加后是当作当前选择的。
        }

        /// <summary>
        /// 删除，
        /// </summary>
        public void deleteShapes(ShapeEle shape)
        {
            //返回的是第几个项目删除的。
            var index = shapes.lstShapes.IndexOf(shape);
            if (index == -1) return;

            shapes.lstShapes.Remove(shape);// 请注意，这里只是删除顶层的。
            // 这里添加command
            CommandDelete commandDelete = new CommandDelete() {
                canvas=this,           // 画布
                NewShape = shape,      // 删除这个形状
                index = index          // 从这个位置删除的。
            };
            commandRecorder.addCommand(commandDelete);
        }

        /// <summary>
        /// 删除已经选中的形状
        /// </summary>
        public void deleteShapes()
        {
            if (SelectShape != null) deleteShapes(SelectShape);
        }


        public void forward(int id)
        {
            // todo 添加命令。

            var shape = shapes.getShape(id);
            int index = shapes.lstShapes.IndexOf(shape);// 取得下标
            if (index > 0)
            {
                // 如果不是最前面的
                shapes.lstShapes.Remove(shape);
                shapes.lstShapes.Insert(index - 1, shape);
            }
        }
        public void forwardToFront(int id)
        {
            // todo 添加命令。
            var shape = shapes.getShape(id);
            int index = shapes.lstShapes.IndexOf(shape);// 取得下标
            if (index > 0)
            {
                // 如果不是最前面的
                shapes.lstShapes.Remove(shape);
                shapes.lstShapes.Insert(0, shape);
            }
        }

        public void backward(int id)
        {
            // todo 添加命令。
            var shape = shapes.getShape(id);
            int index = shapes.lstShapes.IndexOf(shape);// 取得下标
            if (index >=  0 && index < shapes.lstShapes.Count-1)
            {
                // 如果不是最前面的
                shapes.lstShapes.Remove(shape);
                shapes.lstShapes.Insert(index + 1, shape);
            }
        }
        public void backwardToEnd(int id)
        {
            // todo 添加命令。
            var shape = shapes.getShape(id);
            int index = shapes.lstShapes.IndexOf(shape);// 取得下标
            if (index >= 0 && index < shapes.lstShapes.Count - 1)
            {
                // 如果不是最前面的
                shapes.lstShapes.Remove(shape);
                shapes.lstShapes.Insert(shapes.lstShapes.Count-1, shape);
            }
        }


        public void forward()
        {
            //这里用当前的
            if (SelectShape != null)
            {
                forward(SelectShape.ID);
            }
        }
        public void forwardToFront()
        {
            if (SelectShape != null)
            {
                forwardToFront(SelectShape.ID);
            }
        }
        public void backward()
        {
            //这里用当前的
            if (SelectShape != null)
            {
                backward(SelectShape.ID);
            }
        }
        public void backwardToEnd()
        {
            //这里用当前的
            if (SelectShape != null)
            {
                backwardToEnd(SelectShape.ID);
            }
        }



        /// <summary>
        /// 组成群组
        /// </summary>
        /// <param name="shapes"></param>
        public ShapeGroup mergeGroup(List<ShapeEle> shapes)
        {
            // todo 添加命令。
            ShapeGroup group = new ShapeGroup();
            group.shapes.AddRange(shapes);
            group.ID = this.shapes.getNextId();
            foreach (var item in shapes)
            {
                this.shapes.lstShapes.Remove(item); // 从旧的里边删除。
            }
            addShape(group);
            return group;
        }

        /// <summary>
        /// 取消这个群组
        /// </summary>
        /// <param name="group"></param>
        public void cancelGroup(ShapeGroup group)
        {
            // todo 添加命令。
            this.shapes.lstShapes.Remove(group);
            foreach (var item in group.shapes)
            {
                this.shapes.lstShapes.Add(item);
            }
        }

        /// <summary>
        /// 将选择的组成群组
        /// </summary>
        public void mergeGroup()
        {
            if (SelectShape is ShapeGroup)
            {
                var group =  mergeGroup(((ShapeGroup)SelectShape).shapes);
                changeSelect(group);
            }
            
        }

        /// <summary>
        /// 将选择的取消群组
        /// </summary>
        public void cancelGroup()
        {
            if (SelectShape is ShapeGroup)
            {
                cancelGroup((ShapeGroup)SelectShape);
                changeSelect(null);// 取消选择
            }
        }

        #region 各种对齐

        // todo 实现如下的对齐
        
        /// <summary>
        /// 上对齐
        /// </summary>
        public void align_top() { }

        /// <summary>
        /// 下对齐
        /// </summary>
        public void align_bottom() {  }


        /// <summary>
        /// 左对齐
        /// </summary>
        public void align_left() { }

        /// <summary>
        /// 右对齐
        /// </summary>
        public void align_right() { }

        public void align_center() { }


        public void align_midele() { }

        #endregion

        #endregion



        #region 小函数

        /// <summary>
        /// 对齐网格后的坐标，这个主要是提供给更改尺寸的。
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public PointF gridAlign(PointF point)
        {
            // 这里首先判断是否需要对齐网格
            if (isAlignDridding)
            {
                // 这里需要转换坐标
                var p1 = this.shapes.pointTransform.CanvasToVirtualPoint(point); // 先转成虚拟的坐标
                var p2 = new PointF()
                {
                    X = ((int)Math.Round(p1.X / GriddingInterval, 0)) * GriddingInterval,     // 对齐,四舍五入五
                    Y = ((int)Math.Round(p1.Y / GriddingInterval, 0)) * GriddingInterval,
                };
                return this.shapes.pointTransform.VirtualToCanvasPoint(p2);      // 转成画布的坐标

            }
            else
            {
                return point; // 不用对齐，直接返回原先的。
            }
        }

        #endregion

    }
}
