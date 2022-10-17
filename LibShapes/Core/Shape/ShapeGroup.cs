using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    public class ShapeGroup : ShapeEle
    {

        private List<ShapeEle> shapes = new List<ShapeEle>();

        public override void Draw(Graphics g, Matrix matrix)
        {
            // 这里首先要注意的是这个可以旋转的
            Matrix matrix1 = matrix.Clone();
            var rect = GetGraphicsPathWithAngle().GetBounds();
            var centerPoint = new PointF() {
                X = rect.X + rect.Width / 2,
                Y = rect.Y + rect.Height / 2
            };
            matrix1.RotateAt(Angle, centerPoint);

            foreach (var item in shapes)
            {
                item.Draw(g, matrix1);
            }
        }

        public override GraphicsPath GetGraphicsPath(Matrix matrix)
        {
            // 这里要先算子形状的所有的路径
            GraphicsPath path = new GraphicsPath();
            foreach (var item in shapes)
            {
                path.AddPath(item.GetGraphicsPath(matrix), false);
            }
            // 然后对这个进行旋转
            var rect = path.GetBounds();
            var centerPoints = new PointF() {
                X = rect.X + rect.Width/2,
                Y = rect.Y + rect.Height/2
            };
            Matrix matrix1 = new Matrix();
            matrix1.RotateAt(Angle, centerPoints);
            path.Transform(matrix1);
            return path;

        }


        /// <summary>
        /// 这个其实是取得了含有角度的。
        /// </summary>
        /// <returns></returns>
        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            // 这个不需要返回什么。
            return  null; 

        }
    }
}
