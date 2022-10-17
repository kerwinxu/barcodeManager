using Io.Github.Kerwinxu.LibShapes.Core.Command;
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

namespace Io.Github.Kerwinxu.LibShapes.Core
{
    /// <summary>
    /// 画布
    /// </summary>
    public partial class UserControlCanvas : UserControl
    {
        public UserControlCanvas()
        {
            InitializeComponent();
            // 这里设置默认颜色
            state = new State.StateStandby(this); // 默认情况下是待机状态。
            GriddingInterval = 2;                 // 默认网格2mm
            commandRecorder = new CommandRecorder(); // 默认的命令记录器
            // 一堆的事件
            this.KeyDown += (sender, e) => { this.state.KeyDown(e); };
            this.KeyUp += (sender, e) => { this.state.KeyUp(e); };
            this.MouseDown += (sender, e) => {if (e.Button == MouseButtons.Left) this.state.LeftMouseDown(e); };
            this.MouseMove += (sender, e) => { if (e.Button == MouseButtons.Left) this.state.LeftMouseMove(e); };
            this.MouseUp += (sender, e) => { if (e.Button == MouseButtons.Left) this.state.LeftMouseUp(e); };
            this.MouseClick += (sender, e) => { if (e.Button == MouseButtons.Right) this.state.RightMouseClick(e); };
        }

        #region 一堆的常量

        private float scaleWidth = 6;   // 刻度尺的宽度的像素宽度，这个是固定的。
        private float scaleLineWidth = 0.1f;  // 刻度尺上的刻度线的宽度。
        private Font  scaleTextFont = new Font("Arial", 6); // 刻度尺上文字的字体。
        private float minIntervalpixel = 5;   // 比如网格，如果小于这个，就不打印了。
        private Color scaleBackColor = Color.Orange;
        private float grid_width = 0.25f;                        // 绘制的网格的宽度
        private Brush grid_brush = new SolidBrush(Color.Black);  // 网格的
        private Pen penSelectShape = new Pen(Color.Black) {      // 选择框的画笔
            DashStyle=DashStyle.Dash,
            Width = 0.2f
        };  

        #endregion

        #region 属性

        // 如下的很多设置成属性是因为我方便从别的地方设置，这个属性其实是有个set方法的。

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
        /// 已经选择的图形，也可以多个图形
        /// </summary>
        public ShapeEle SelectShapes { get; set; }

        /// <summary>
        /// 是否安装了shift键
        /// </summary>
        public bool isShift { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public State.State state { get; set; }

        
        /// <summary>
        /// 当前选择的图形
        /// </summary>
        public ShapeEle SelectShape { get; set; }

        #endregion

        private void UserControlCanvas_Paint(object sender, PaintEventArgs e)
        {
            // 这里重新绘图用上了双缓冲
            // 1. 首先在内存中建立Graphics对象
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer = currentContext.Allocate(e.Graphics, e.ClipRectangle);
            Graphics g = myBuffer.Graphics;
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
            myBuffer.Render(e.Graphics);
            g.Dispose();
            myBuffer.Dispose();//释放资源
        }

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


    }
}
