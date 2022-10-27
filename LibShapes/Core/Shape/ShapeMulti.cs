using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    /// 多个形状的集合
    /// </summary>
    public  class ShapeMulti:ShapeEle
    {
        public List<ShapeEle> shapes = new List<ShapeEle>();

        public override ShapeEle DeepClone()
        {
            // 首先组建一个新的
            ShapeMulti group = new ShapeMulti();
            if (shapes != null)
            {
                foreach (var item in shapes)
                {
                    group.shapes.Add(item.DeepClone());
                }
            }
            return group;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
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
            var centerPoints = new PointF()
            {
                X = rect.X + rect.Width / 2,
                Y = rect.Y + rect.Height / 2
            };
            Matrix matrix1 = new Matrix();
            matrix1.RotateAt(Angle, centerPoints);
            path.Transform(matrix1);
            return path;

        }

        public override bool Equals(object obj)
        {
            var shape = obj as ShapeMulti;
            if (shape == null) return false; // 转换失败就是不同啦
            // 群组，需要判断每一个是否相同。
            if (shapes.Count != shape.shapes.Count) return false;
            for (int i = 0; i < shapes.Count; i++)
            {
                if (shapes[i] != shape.shapes[i])
                {
                    return false;
                }
            }
            return true;
            //return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.ID;
            return base.GetHashCode();
        }
    }
}
