using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Io.Github.Kerwinxu.LibShapes.Core;
using Io.Github.Kerwinxu.LibShapes.Core.Shape;

namespace 绘图偏移放大
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            test4();
        }

      

        private void panel1_Paint(object sender, PaintEventArgs e)
        {



        }

        private void test1(PaintEventArgs e)
        {
            // 这里测试
            Pen penBlue = new Pen(Color.Blue, 1);
            Pen penRed = new Pen(Color.Red, 1);
            Pen penGreen = new Pen(Color.Green, 1);
            Pen penBlock = new Pen(Color.Black, 1);

            //GraphicsPath path = new GraphicsPath();
            //path.AddRectangle(new Rectangle(50, 50, 50, 50));

            //// 这个是没有偏移，没有放大的。
            //e.Graphics.DrawPath(penBlue, path);

            ////
            //Matrix matrix1 = new Matrix();
            //matrix1.Translate(50, 50);
            //e.Graphics.Transform = matrix1;
            //e.Graphics.DrawPath(penRed, path);
            //// 然后放大
            //matrix1.Scale(2, 2, MatrixOrder.Prepend);
            //e.Graphics.Transform = matrix1;
            //e.Graphics.DrawPath(penGreen, path);

            //// 这里偏移跟图形都同步的放大了。比较好计算
            //Matrix matrix2 = new Matrix();
            //matrix2.Translate(50, 50);
            //matrix2.Scale(2, 2, MatrixOrder.Append);
            //e.Graphics.Transform = matrix2;
            //e.Graphics.DrawPath(penBlock, path);

            // 这里测试一下字体是否会拉伸
            GraphicsPath path2 = new GraphicsPath();
            Matrix matrix3 = new Matrix();
            var rect2 = path2.GetBounds();
            matrix3.RotateAt(10, new PointF((rect2.X + rect2.Width) / 2, (rect2.Y + rect2.Height) * 5 / 2));
            matrix3.Scale(1, 5);

            path2.AddString("Hello World", new FontFamily("Arial"), (int)FontStyle.Bold, 26, new PointF(100, 20), StringFormat.GenericDefault);
            path2.Transform(matrix3);

            e.Graphics.FillPath(Brushes.Black, path2);

            // 这里测试一下这个的区间
            var rect = path2.GetBounds();
            GraphicsPath path3 = new GraphicsPath();
            path3.AddRectangle(rect);
            path3.AddEllipse(new RectangleF(60, 40, 5, 5));
            e.Graphics.DrawPath(penRed, path3);

            //// 这里判断范围
            //if (path2.IsVisible(60, 40))
            //{
            //    MessageBox.Show("IsVisible=True");
            //}

            //if (path3.IsOutlineVisible(60, 22, penRed))
            //{
            //    MessageBox.Show("IsOutlineVisible=True");
            //}

            // 这里试试是否可以自定义文字的范围
            // 经过测试，这个只是在这个矩形的范围内写字
            GraphicsPath path4 = new GraphicsPath();
            path4.AddString("你好啊，世界",
                new FontFamily("Arial"),
                0,
                16,
                new RectangleF(100, 100, 50, 20),
                StringFormat.GenericDefault);
            e.Graphics.FillPath(Brushes.Black, path4);

        }

        private void test2(PaintEventArgs e)
        {
            // 这里看看是否可以显示图形
            Shapes shapes = new Shapes();
            // 然后添加直线
            ShapeLine line = new ShapeLine()
            {
                X = 0,
                Y = 0,
                Width = 50,
                Height = 50,
                PenColor = Color.Red,
                PenWidth = 1
            };
            shapes.lstShapes.Add(line);
            // 这里设置偏移
            Matrix matrix = new Matrix();
            matrix.Translate(50, 50);
            // 最后绘图
            shapes.Draw(e.Graphics, matrix);
            // 可以看到，这个偏移如果是正数，表示往右下角偏移。
        }
        private void test3(PaintEventArgs e)
        {
            // 这里看看是否可以显示图形
            Shapes shapes = new Shapes();
            // 然后添加直线
            ShapeLine line = new ShapeLine()
            {
                X = 0,
                Y = 0,
                Width = 50,
                Height = 50,
                PenColor = Color.Red,
                PenWidth = 1
            };
            shapes.lstShapes.Add(line);
            // 这里设置偏移
            Matrix matrix = new Matrix();
            matrix.Translate(50, 50);
            matrix.Scale(3, 1,MatrixOrder.Append); // 如果是这个
            // 最后绘图
            shapes.Draw(e.Graphics, matrix);
            // 可以看到，这个偏移如果是正数，表示往右下角偏移。
        }

        private void test4()
        {
            // 这个看看自定义控件是否可以画矩形
            UserControlCanvas canvas = new UserControlCanvas();
            // 显示边框
            canvas.BackColor = Color.Gray;
            canvas.Location = new Point(10, 10);
            canvas.Height = this.Height - 100;
            canvas.Width = this.Width - 50;
            canvas.shapes = new Shapes();
            canvas.shapes.Paper.ModelWidth = 100;
            canvas.shapes.Paper.ModelHeight = 50;
            
            canvas.shapes.Paper.createModelShape();
            // 然后添加直线
            ShapeLine line = new ShapeLine()
            {
                X = 10,
                Y = 10,
                Width = 30,
                Height = 30,
                PenColor = Color.Red,
                PenWidth = 1
            };
            canvas.shapes.lstShapes.Add(line);
            this.Controls.Add(canvas);
            // 我这里设置一下有偏移的情况
            canvas.shapes.pointTransform.OffsetX = 10;
            canvas.shapes.pointTransform.OffsetY = 10;
            canvas.shapes.pointTransform.Zoom = 1;
            canvas.isDrawDridding = true;// 显示网格
            canvas.GriddingInterval = 5; // 
            canvas.SelectShape = line;
            canvas.Refresh();
        }
    }
}
