﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    public class ShapeGroup : ShapeMulti
    {
        /// <summary>
        ///  这个重新是因为这个要真正的绘制，而ShapeMultiSelect不需要，ShapeMultiSelect里保存的只是多个形状的引用。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="matrix"></param>
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



        /// <summary>
        /// 这个其实是取得了含有角度的。
        /// </summary>
        /// <returns></returns>
        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            // 这个不需要返回什么。
            return  null; 

        }

        public override ShapeEle DeepClone()
        {
            // 首先组建一个新的
            ShapeGroup group = new ShapeGroup();
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

        public override bool Equals(object obj)
        {
            var shape = obj as ShapeGroup;
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
