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
using System.Diagnostics;
using Io.Github.Kerwinxu.LibShapes.Core.State;
using Io.Github.Kerwinxu.LibShapes.Core.Event;
using Io.Github.Kerwinxu.LibShapes.Core.Paper;

namespace 绘图偏移放大
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            init();
            test4();
            //test5();
            test6();
            test7();
            test9(); // 添加文本

            // 
            // this.canvas.state = new StateCreate(this.canvas, new ShapeLine() { PenWidth=1, PenColor=Color.Black});
            // 这里选择对齐模式
            //this.canvas.isAlignDridding = true;
            test8();

            ////我这里测试一下图片
            //this.canvas.state = new StateCreate(
            //    this.canvas,
            //    new ShapeImage()
            //    {
            //        Img = ShapeImage.ImgToBase64String((Bitmap)Image.FromFile(@"E:\kerwin\Pictures\GHIDRA_1.png")),

            //    });

            //// 我这里测试一下条形码
            //this.canvas.state = new StateCreate(
            //    this.canvas,
            //    new ShapeBarcode()
            //    );
            // 测试余下圆弧
            //this.canvas.state = new StateCreate(
            //    this.canvas,
            //    new ShapeArc()

            //    );
            // 测试一下矩形
            this.canvas.state = new StateCreate(
                this.canvas,
                new ShapeRectangle()
                );
            //this.canvas.shapes.pointTransform.Zoom = 1.8f;
            //this.canvas.shapes.pointTransform.OffsetX = 0;
            //this.canvas.shapes.pointTransform.OffsetY = 0;
            //this.Refresh();
            this.canvas.zoomToScreen();

            // 弹出纸张设置的。
            //FrmPaperSetting frmPaperSetting = new FrmPaperSetting();
            //frmPaperSetting.ShowDialog();


        }

        UserControlCanvas canvas;
        private void init()
        {
            canvas = new UserControlCanvas();
            // 显示边框
            canvas.BackColor = Color.Gray;
            canvas.Location = new Point(10, 10);
            canvas.Height = this.Height - 100;
            canvas.Width = this.Width - 250;
            canvas.shapes = new Shapes();
            // 我这里设置一下有偏移的情况
            canvas.shapes.pointTransform.OffsetX = 10;
            canvas.shapes.pointTransform.OffsetY = 10;
            canvas.shapes.pointTransform.Zoom = 1;
            canvas.isDrawDridding = true;// 显示网格
            canvas.GriddingInterval = 5; // 
            this.Controls.Add(canvas);
            canvas.objectSelected += Canvas_objectSelected;
        }

        private void Canvas_objectSelected(object sender, ObjectSelectEventArgs e)
        {
            propertyGrid1.SelectedObject = e.obj;
            //throw new NotImplementedException();
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
            // 然后添加直线
            ShapeLine line = new ShapeLine()
            {
                ID = 1,
                X = 50,
                Y = 10,
                Width = 30,
                Height = 30,
                PenColor = Color.Red,
                PenWidth = 1
            };
            canvas.shapes.lstShapes.Add(line); 
            //canvas.SelectShape = line;
            canvas.Refresh();
        }

        private void test5()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(new PointF(0, 0), new PointF(10, 10));
            var tmp = path.PathPoints;
            // 可以看到这个只是取得了2个点，而不是全部的的点。
        }

        private void test6()
        {
            // 我这里看看这个矩形转换的是否能能张昌
            // 我做一个矩形
            ShapeRectangle shapeRectangle = new ShapeRectangle() {
                ID = 2,
                X = 10,
                Y = 10,
                Width = 30,
                Height = 30,
                PenColor = Color.Red,
                PenWidth = 1,
            };

            Matrix matrix = new Matrix();
            matrix.Scale(3, 1, MatrixOrder.Prepend);
            //shapeRectangle.Matrix = matrix;

            // 添加到控件中
            canvas.shapes.lstShapes.Add(shapeRectangle);

            canvas.Refresh();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // 1. 
            //// Create a path and add a rectangle.
            //GraphicsPath myPath = new GraphicsPath();
            //RectangleF srcRect = new RectangleF(0, 0, 100, 200);
            //myPath.AddRectangle(srcRect);

            //// Draw the source path (rectangle)to the screen.
            //e.Graphics.DrawPath(Pens.Black, myPath);

            //// Create a destination for the warped rectangle.
            //PointF point1 = new PointF(200, 200);
            //PointF point2 = new PointF(400, 250);
            //PointF point3 = new PointF(220, 400);
            //PointF[] destPoints = { point1, point2, point3 };

            //// Create a translation matrix.
            //Matrix translateMatrix = new Matrix();
            //translateMatrix.Translate(100, 0);

            //// Warp the source path (rectangle).
            //myPath.Warp(destPoints, srcRect, translateMatrix,
            //    WarpMode.Perspective, 0.5f);

            //// Draw the warped path (rectangle) to the screen.
            //e.Graphics.DrawPath(new Pen(Color.Red), myPath);

            //2.
            //// Create a path and add two ellipses.
            //GraphicsPath myPath = new GraphicsPath();
            //myPath.AddEllipse(0, 0, 100, 100);
            //myPath.AddEllipse(100, 0, 100, 100);

            //// Draw the original ellipses to the screen in black.
            //e.Graphics.DrawPath(Pens.Black, myPath);

            //// Widen the path.
            //Pen widenPen = new Pen(Color.Black, 10);
            //Matrix widenMatrix = new Matrix();
            //widenMatrix.Translate(50, 50);
            //myPath.Widen(widenPen, widenMatrix, 1.0f);

            //// Draw the widened path to the screen in red.
            //e.Graphics.FillPath(new SolidBrush(Color.Red), myPath);
        }

        private void test7()
        {
            // 这个主要是看看点的顺序是什么样的
            GraphicsPath path = new GraphicsPath();
            path.AddLine(10, 10, 20, 20);
            var rect = path.GetBounds();
            Trace.WriteLine($"边框:{rect}");
            GraphicsPath path2 = new GraphicsPath();
            path2.AddRectangle(rect);
            Trace.WriteLine($"个数:{path2.PointCount}");
            for (int i = 0; i < path2.PointCount; i++)
            {
                Trace.WriteLine(path2.PathPoints[i]);
            }
            // 从这里可以看到第一个点是左上角的点，然后是右上角。
        }

        private void test8()
        {
            // 这个主要看是怎么放大的。
            GraphicsPath path = new GraphicsPath();
            path.AddLine(new PointF(10, 10), new PointF(20, 20));
            //
            Matrix matrix = new Matrix();
            matrix.Translate(10, 10); // 偏移10，10
            matrix.Scale(2, 2);       // 放大两倍
            //
            path.Transform(matrix);
            Trace.WriteLine($"pre的放大后的结果:");
            foreach (var item in path.PathPoints)
            {
                Trace.WriteLine(item);
            }
        }

        private void test9()
        {
            // 这里添加一个文本。
            ShapeText text = new ShapeText()
            {
                ID=3,
                X=40,
                Y=40,
                Width = 50,
                Height = 20,
                Font = new Font("Arial",16),
                StaticText = "hello",
                PenColor=Color.Black,
                FillColor = Color.Red,
                IsFill = true,
                Alignment = StringAlignment.Center,

            };

            // 添加到控件中
            canvas.shapes.lstShapes.Add(text);

        }
    }
}
