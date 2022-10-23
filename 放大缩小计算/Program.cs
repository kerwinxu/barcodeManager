using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace 放大缩小计算
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            Console.WriteLine("我这个是计算放大和缩小的情况的");
            //
            var p1 = new PointF(10, 10);
            var p2 = new PointF(20, 20);
            Console.WriteLine($"线段:{p1},{p2}");
            GraphicsPath path1 = new GraphicsPath();
            path1.AddLine(p1, p2);
            // 1. 
            Matrix matrix1 = new Matrix();
            matrix1.Translate(10, 10);
            Console.WriteLine($"1. 只有偏移(10,10)");
            path1.Transform(matrix1);
            var ps1 = path1.PathPoints;// 
            Console.WriteLine($"在画布中的坐标是:");
            foreach (var item in ps1)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();
            // 2
            Console.WriteLine("加上x轴放大2倍");
            GraphicsPath path2 = new GraphicsPath();
            path2.AddLine(p1, p2);
            matrix1.Scale(2, 1);
            path2.Transform(matrix1);
            var ps2 = path2.PathPoints;// 
            Console.WriteLine($"在画布中的坐标是:");
            foreach (var item in ps2)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();
            Console.WriteLine("可以看到这种情况下，画笔的坐标计算方式是：虚拟的坐标*2+固定的偏移10");
            Console.WriteLine("我这里放大后，这个点不变,其实也就是屏幕上的坐标不变");

            // 3.
            GraphicsPath path3 = new GraphicsPath();
            path3.AddLine(p1, p2);
            Matrix matrix3 = new Matrix();
            matrix3.Translate(
                10-10*3, 
                10);
            matrix3.Scale(4, 1);

            path3.Transform(matrix3);
            var ps3 = path3.PathPoints;// 
            Console.WriteLine($"在画布中的坐标是:");
            foreach (var item in ps3)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
